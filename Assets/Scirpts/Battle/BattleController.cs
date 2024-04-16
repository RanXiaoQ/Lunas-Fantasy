using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleController : MonoBehaviour
{
    public Animator m_LunaAnimator;
    public Animator m_MonsterAnimator;
    public Transform m_LunaTranform;
    public Transform m_MonsterTranform;
    private Vector3 m_LunaInitPos;
    private Vector3 m_MonsterInitPos;
    public SpriteRenderer m_LunaSprite;
    public SpriteRenderer m_MonsterSprite;
    public GameObject m_LunaSkillEffect;
    public GameObject m_LunaRecoverHPEffect;
    //音效
    public AudioClip m_AttackSound;
    public AudioClip m_LunaAttackSound;
    public AudioClip m_MonsterAttackSound;
    public AudioClip m_MonsterHitSound;
    public AudioClip m_LunaSkillSound;
    public AudioClip m_LunaRecoverHPSound;
    public AudioClip m_LunaHitSound;
    public AudioClip m_LunaDieSound;

    private void Awake()
    {
        m_LunaInitPos = m_LunaTranform.localPosition;
        m_MonsterInitPos = m_MonsterTranform.localPosition;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
    private void OnEnable()
    {
        m_MonsterSprite.DOFade(1, 0.01f);
        m_LunaSprite.DOFade(1, 0.01f);
        m_LunaTranform.localPosition = m_LunaInitPos;
        m_MonsterTranform.localPosition = m_MonsterInitPos;
    }

    #region Luna行动方式调用
    public void LunaAttack()
    {
        StartCoroutine(PerformAttack());
    }

    public void LunaDefend()
    {
        StartCoroutine(PerformDefend());
    }

    public void LunaUseSkill()
    {
        if (!GameManager.m_Instance.CanUseLunaSkill(30))
        {
            return;
        }
        StartCoroutine(PerformSkill());
    }

    public void LunaRecoverHP()
    {
        if(!GameManager.m_Instance.CanUseLunaSkill(50))
        {
            return;
        }
        StartCoroutine(PerformRecoverHP());
    }

    public void LunaEscape()
    {
        PerformEscape();
    }
    #endregion

    #region Luna行动方法
    /// <summary>
    /// Luna的攻击
    /// </summary>
    /// <returns></returns>
    IEnumerator PerformAttack()
    {
        UIManager.m_Instance.ShowBattlePanle(false);
        m_LunaAnimator.SetBool("MoveState", true);
        m_LunaAnimator.SetFloat("MoveValue", -1);
        m_LunaTranform.DOLocalMove(m_MonsterInitPos + new Vector3(1.3f,0,0),1.2f).OnComplete
            (
                () => 
                {
                    GameManager.m_Instance.PlaySound(m_AttackSound);
                    GameManager.m_Instance.PlaySound(m_LunaAttackSound);
                    m_LunaAnimator.SetBool("MoveState", false);
                    m_LunaAnimator.SetFloat("MoveValue", 0);
                    m_LunaAnimator.CrossFade("Attack", 0);
                    m_MonsterSprite.DOColor(Color.red, 1.5f);
                    m_MonsterSprite.DOFade(0.3f, 1.5f).OnComplete
                        (
                            () =>
                            {
                                MonsterHp(-20);
                                GameManager.m_Instance.PlaySound(m_MonsterHitSound);
                            }
                        );
                }
            );
        yield return new WaitForSeconds(2f);
        m_LunaAnimator.SetBool("MoveState", true);
        m_LunaAnimator.SetFloat("MoveValue", 1);
        m_LunaTranform.DOLocalMove(m_LunaInitPos, 1.2f).OnComplete
            (
                () =>
                {
                    m_LunaAnimator.SetBool("MoveState", false);
                }
            );
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(MonsterAttack());
    }

    /// <summary>
    /// Luna的防御
    /// </summary>
    /// <returns></returns>
    IEnumerator PerformDefend()
    {
        UIManager.m_Instance.ShowBattlePanle(false);
        m_LunaAnimator.SetBool("Defend",true);
        m_MonsterTranform.DOLocalMove(m_LunaInitPos + new Vector3(-1.3f, 0, 0), 0.7f);
        yield return new WaitForSeconds(0.5f);
        m_MonsterTranform.DOLocalMove(m_LunaInitPos,0.2f).OnComplete
            (
                () =>
                {
                    m_MonsterTranform.DOLocalMove(m_LunaInitPos + new Vector3(-1.3f, 0, 0), 0.2f);
                    m_LunaTranform.DOLocalMove(m_LunaInitPos + new Vector3(1, 0, 0), 0.2f).OnComplete
                        (
                            () =>
                            {
                                m_LunaTranform.DOLocalMove(m_LunaInitPos, 0.2f);
                            }
                        );

                }
            );
        yield return new WaitForSeconds(2f);
        m_MonsterTranform.DOLocalMove(m_MonsterInitPos, 1.2f).OnComplete
            (
                () =>
                {
                    UIManager.m_Instance.ShowBattlePanle(true);
                    GameManager.m_Instance.PlaySound(m_MonsterAttackSound);
                    m_LunaAnimator.SetBool("Defend", false);
                }
            );
    }

    /// <summary>
    /// Luna的技能使用方法
    /// </summary>
    /// <returns></returns>
    IEnumerator PerformSkill()
    {
        UIManager.m_Instance.ShowBattlePanle(false);
        m_LunaAnimator.CrossFade("Skill", 0);
        GameManager.m_Instance.AddOrDecreaseLunaMP(-30);
        //yield return new WaitForSeconds(1f);
        GameObject SkillEffect =  Instantiate(m_LunaSkillEffect,m_MonsterTranform);
        SkillEffect.transform.localPosition = Vector3.zero;
        GameManager.m_Instance.PlaySound(m_AttackSound); 
        GameManager.m_Instance.PlaySound(m_LunaSkillSound);
        yield return new WaitForSeconds(0.4f);
        m_MonsterSprite.DOFade(0.3f, 0.2f).OnComplete
            (
                () =>
                {
                    MonsterHp(-40);
                    GameManager.m_Instance.PlaySound(m_MonsterHitSound);
                }
            );
        yield return new WaitForSeconds(2f);
        StartCoroutine(MonsterAttack());
    }

    /// <summary>
    /// Luna的回血方法
    /// </summary>
    /// <returns></returns>
    IEnumerator PerformRecoverHP()
    {
        UIManager.m_Instance.ShowBattlePanle(false);
        m_LunaAnimator.CrossFade("RecoverHP", 0);
        GameManager.m_Instance.AddOrDecreaseLunaMP(-50);
        GameManager.m_Instance.PlaySound(m_AttackSound);
        GameManager.m_Instance.PlaySound(m_LunaSkillSound);
        GameManager.m_Instance.PlaySound(m_LunaRecoverHPSound);
        yield return new WaitForSeconds(0.4f);
        GameObject RecoverHP = Instantiate(m_LunaRecoverHPEffect, m_LunaTranform);
        RecoverHP.transform.localPosition = Vector3.zero;
        GameManager.m_Instance.AddOrDecreaseLunaHP(40);
        yield return new WaitForSeconds(2f);
        StartCoroutine(MonsterAttack());
    }

    public void PerformEscape()
    {
        UIManager.m_Instance.ShowBattlePanle(false);
        m_LunaTranform.DOLocalMove(m_LunaInitPos + new Vector3(5, 0, 0), 0.5f).OnComplete
            (
                () =>
                {
                    GameManager.m_Instance.EnterOrExitBattle(false);
                }
            );
        m_LunaAnimator.SetBool("MoveState", true);
        m_LunaAnimator.SetFloat("MoveValue", 1);
    }
    #endregion

    #region Monster的攻击方法
    /// <summary>
    /// Monster的攻击
    /// </summary>
    /// <returns></returns>
    IEnumerator MonsterAttack()
    {
        m_MonsterTranform.DOLocalMove(m_LunaInitPos + new Vector3(-1.3f, 0, 0), 0.7f).OnComplete
            (
                () =>
                {
                    GameManager.m_Instance.PlaySound(m_MonsterAttackSound);
                    m_LunaAnimator.CrossFade("Hit", 0);
                    GameManager.m_Instance.PlaySound(m_LunaHitSound);
                    m_LunaSprite.DOColor(Color.red,1.5f);
                    m_LunaSprite.DOFade(0.3f, 1.5f).OnComplete
                        (
                            () =>
                            {
                                LunaHp(-30);
                            }
                        );
                }
            );    
        yield return new WaitForSeconds(1f);
        m_MonsterTranform.DOLocalMove(m_MonsterInitPos, 0.5f).OnComplete
            (
                () =>
                {
                    UIManager.m_Instance.ShowBattlePanle(true);
                }
            );
        
    }
    #endregion


    /// <summary>
    /// Luna扣血的变化
    /// </summary>
    /// <param name="value"></param>
    private void LunaHp(int value)
    {
        GameManager.m_Instance.AddOrDecreaseLunaHP(value);
        m_LunaSprite.DOFade(1, 1.5f);
        m_LunaSprite.DOColor(Color.white, 1.5f);
        if(GameManager.m_Instance.m_LunaCurrentHealth <= 0)
        {
            m_LunaAnimator.CrossFade("Die", 0);
            GameManager.m_Instance.PlaySound(m_LunaDieSound);
            m_LunaSprite.DOFade(0, 0.8f).OnComplete
                (
                    () =>
                    {
                        GameManager.m_Instance.EnterOrExitBattle(false);
                    }
                 );
        }
    }

    /// <summary>
    /// Monster扣血的变化
    /// </summary>
    /// <param name="value"></param>
    private void MonsterHp(int value)
    {
        if(GameManager.m_Instance.AddOrDecreaseMonsterHP(value) <= 0)
        {
            m_MonsterSprite.DOFade(0, 1.5f).OnComplete
                (
                    () =>
                    {
                        GameManager.m_Instance.EnterOrExitBattle(false,1);
                    }
                 ); 
            
        }
        else
        {
            m_MonsterSprite.DOColor(Color.white, 1.5f);
            m_MonsterSprite.DOFade(1, 1.5f);            
        }    
    }
   
}
