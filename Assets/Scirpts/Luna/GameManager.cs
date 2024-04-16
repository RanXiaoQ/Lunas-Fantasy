using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;
    public GameObject m_BattleGo;  //ս������
    //Luna����
    public int m_LunaHP;  //�������ֵ
    public float m_LunaCurrentHealth;  //Luna������ֵ
    public int m_LunaMP;  //�������
    public float m_LunaCurrentMP;  //Luna������
    //public int MaxHealth { get { return m_LunaHP; } }
    //public int CurrentHealth { get { return m_LunaCurrentHealth; } }
    //Monster����
    public int m_MonsterHP;  //�������ֵ
    public int m_MonsterCurrentHP;  //Luna������ֵ
    //�Ի�
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
    /// ����Ѫ���ı�
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHeath(int amount)
    {
        m_LunaCurrentHealth = Mathf.Clamp(m_LunaCurrentHealth + amount, 0, m_LunaHP);
        Debug.Log(m_LunaCurrentHealth + "/" + m_LunaHP);
    }

    /// <summary>
    /// ����ս��״̬
    /// </summary>
    /// <param name="enter"></param>
    /// <param name="addKillNum"></param>
    public void EnterOrExitBattle(bool enter = true,int addKillNum = 0)
    {
        UIManager.m_Instance.ShowBattlePanle(enter);
        m_BattleGo.SetActive(enter);
        if (!enter)  //��ս��״̬������˵ս������
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

    #region Luna�����Ա仯
    /// <summary>
    /// LunaѪ���ĸı�
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
    /// Luna�����ĸı�
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
    /// Luna�Ƿ����ʹ�ü���
    /// </summary>
    public bool CanUseLunaSkill(int value)
    {
        return m_LunaCurrentMP >= value;
    }
    #endregion

    #region Monster�����Ա仯
    /// <summary>
    /// MonsterѪ���ĸı�
    /// </summary>
    /// <param name="Value"></param>
    public int AddOrDecreaseMonsterHP(int Value)
    {
        m_MonsterCurrentHP += Value;
        return m_MonsterCurrentHP;
        //UIManager.m_Instance.SetHPValue((float)m_MonsterCurrentHP / m_MonsterHP);
    }

    /// <summary>
    /// ��ʾ����
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
    /// ���������������
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
