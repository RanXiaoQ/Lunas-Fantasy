using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;
    public GameObject m_BattleGo;  //战斗场景
    //Luna属性
    public int m_LunaHP;  //最大生命值
    public float m_LunaCurrentHealth;  //Luna的生命值
    public int m_LunaMP;  //最大蓝量
    public float m_LunaCurrentMP;  //Luna的蓝量
    //public int MaxHealth { get { return m_LunaHP; } }
    //public int CurrentHealth { get { return m_LunaCurrentHealth; } }
    //Monster属性
    public int m_MonsterHP;  //最大生命值
    public int m_MonsterCurrentHP;  //Luna的生命值
    //对话
    public int m_DialogInfoIndex;
    public bool m_CanControlLuna;
    public bool m_HasPetTheDog;
    public int m_CandleNum;
    public int m_KillNum;
    public GameObject m_MonstersGo;
    public NPCDialog m_NPC;
    public bool m_EnterBattle;
    public GameObject m_BattleMonsterGo;
    public AudioSource m_AudioSource;
    public AudioClip m_NormalClip;
    public AudioClip m_BattleClip;



    private void Awake()
    {
        m_Instance = this;
        m_LunaHP = 100;
        m_LunaCurrentHealth = 100;
        m_LunaMP = 100;
        m_LunaCurrentMP = 100;
        m_MonsterHP = m_MonsterCurrentHP = 100;
    }

    private void Update()
    {
        if (!m_EnterBattle)
        {
            if (m_LunaCurrentHealth <= 100)
            {
                AddOrDecreaseLunaMP(Time.deltaTime);
            }
            if (m_LunaCurrentHealth <= 100)
            {
                AddOrDecreaseLunaHP(Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 人物血量改变
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHeath(int amount)
    {
        m_LunaCurrentHealth = Mathf.Clamp(m_LunaCurrentHealth + amount, 0, m_LunaHP);
        Debug.Log(m_LunaCurrentHealth + "/" + m_LunaHP);
    }

    /// <summary>
    /// 进入战斗状态
    /// </summary>
    /// <param name="enter"></param>
    /// <param name="addKillNum"></param>
    public void EnterOrExitBattle(bool enter = true,int addKillNum = 0)
    {
        UIManager.m_Instance.ShowBattlePanle(enter);
        m_BattleGo.SetActive(enter);
        if (!enter)  //非战斗状态，或者说战斗结束
        {
            m_KillNum += addKillNum;
            if (addKillNum > 0)
            {
                DestoryMonster();
            }
            m_MonsterCurrentHP = 50;
            PlayMusic(m_NormalClip);
            if (m_LunaCurrentHealth <= 0)
            {
                m_LunaCurrentHealth = 100;
                m_LunaCurrentMP = 0;
                m_BattleMonsterGo.transform.position += new Vector3(0, 2, 0);
            }
        }
        else
        {
            PlayMusic(m_BattleClip);
        }
        m_EnterBattle = enter;
    }

    public void DestoryMonster()
    {
        Destroy(m_BattleMonsterGo);
    }

    public void SetMonster(GameObject go)
    {
        m_BattleMonsterGo = go;
    }

    #region Luna的属性变化
    /// <summary>
    /// Luna血量的改变
    /// </summary>
    /// <param name="Value"></param>
    public void AddOrDecreaseLunaHP(float Value)
    {
        m_LunaCurrentHealth += Value;
        if(m_LunaCurrentHealth >= m_LunaHP)
        {
            m_LunaCurrentHealth = m_LunaHP;
        }
        if(m_LunaCurrentHealth <= 0)
        {
            m_LunaCurrentHealth = 0;
        }
        UIManager.m_Instance.SetHPValue((float)m_LunaCurrentHealth / m_LunaHP);
    }

    /// <summary>
    /// Luna蓝量的改变
    /// </summary>
    /// <param name="Value"></param>
    public void AddOrDecreaseLunaMP(float Value)
    {
        m_LunaCurrentMP += Value;
        if (m_LunaCurrentMP >= m_LunaMP)
        {
            m_LunaCurrentMP = m_LunaMP;
        }
        if (m_LunaCurrentMP <= 0)
        {
            m_LunaCurrentMP = 0;
        }
        UIManager.m_Instance.SetHPValue((float)m_LunaCurrentMP / m_LunaMP);
    }

    /// <summary>
    /// Luna是否可以使用技能
    /// </summary>
    public bool CanUseLunaSkill(int value)
    {
        return m_LunaCurrentMP >= value;
    }
    #endregion

    #region Monster的属性变化
    /// <summary>
    /// Monster血量的改变
    /// </summary>
    /// <param name="Value"></param>
    public int AddOrDecreaseMonsterHP(int Value)
    {
        m_MonsterCurrentHP += Value;
        return m_MonsterCurrentHP;
        //UIManager.m_Instance.SetHPValue((float)m_MonsterCurrentHP / m_MonsterHP);
    }

    /// <summary>
    /// 显示怪物
    /// </summary>
    public void ShowMonsters()
    {
        if (!m_MonstersGo.activeSelf)
        {
            m_MonstersGo.SetActive(true);
        }
    }
    #endregion

    /// <summary>
    /// 任务完成设置索引
    /// </summary>
    public void SetContentIndex()
    {
        m_NPC.SetContentIndex();
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (m_AudioSource.clip != audioClip)
        {
            m_AudioSource.clip = audioClip;
            m_AudioSource.Play();
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip)
        {
            m_AudioSource.PlayOneShot(audioClip);
        }
    }
}
