using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public bool m_Vertical;  //�������
    private float m_Speed;
    private Rigidbody2D m_Rigidbody2D;
    private int m_Direction;  //�������
    private float m_ChangeTime;  //����ı�ʱ����
    private float m_Timer;  //��ʱ��
    private Animator m_Animator;  //����������
    public bool m_MonsterMove = true;

    void Start()
    {
        m_Speed = 5;
        m_Direction = 1;
        m_ChangeTime = 5;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Timer = m_ChangeTime;
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        ChangeTimer();
    }
    private void FixedUpdate()
    {
        MonstreMove();
    }


    /// <summary>
    /// ����Monster���ƶ�
    /// </summary>
    public void MonstreMove()
    {
        if(m_MonsterMove == true)
        {
            Vector3 pos = m_Rigidbody2D.position;
            if (m_Vertical)  //��ֱ�������ƶ�
            {
                m_Animator.SetFloat("LookX", 0);
                m_Animator.SetFloat("LookY", m_Direction);
                pos.y = pos.y + m_Speed * m_Direction * Time.fixedDeltaTime;
            }
            else  //ˮƽ�����ƶ�
            {
                m_Animator.SetFloat("LookX", m_Direction);
                m_Animator.SetFloat("LookY", 0);
                pos.x = pos.x + m_Speed * m_Direction * Time.fixedDeltaTime;
            }
            m_Rigidbody2D.MovePosition(pos);
        }     
    }

    /// <summary>
    /// ʱ����Ʒ����ƶ�
    /// </summary>
    public void ChangeTimer()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer < 0)
        {
            m_Direction = -m_Direction;
            m_Timer = m_ChangeTime;
        }
    }

    /// <summary>
    /// �������˽���ս��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GameManager.m_Instance.EnterOrExitBattle();
            GameManager.m_Instance.SetMonster(gameObject);
            m_MonsterMove = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            m_MonsterMove = true;
        }
    }
}
