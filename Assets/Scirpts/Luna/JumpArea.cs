using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 跳跃碰撞检测
/// </summary>
public class JumpArea : MonoBehaviour
{
    private LunaController m_LunaController;
    public Transform m_JumpPointA;
    public Transform m_JumpPointB;
    public Transform m_LunaLocalTrans;
    private void Start()
    {
        if (m_LunaController == null)
            m_LunaController = FindObjectOfType<LunaController>();
        if (m_LunaLocalTrans == null)
            m_LunaLocalTrans = m_LunaController.transform.GetChild(0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            m_LunaController.Jump(true);
            float DisA =  Vector3.Distance(m_LunaController.transform.position, m_JumpPointA.position);
            float DisB = Vector3.Distance(m_LunaController.transform.position, m_JumpPointB.position);
            Transform tragetTrans;
            tragetTrans = DisA > DisB ? m_JumpPointA : m_JumpPointB;
            m_LunaController.transform.DOMove(tragetTrans.position, 0.5f).SetEase(Ease.Linear);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(m_LunaLocalTrans.DOLocalMoveY(1, 0.25f)).SetEase(Ease.InOutSine);
            sequence.Append(m_LunaLocalTrans.DOLocalMoveY(0.5f,0.25f)).SetEase(Ease.InOutSine);
            sequence.Play();
            
            
            //if(DisA > DisB)
            //{
            //    //离A的距离大于离B的距离，所以现在是离B近，那么需要跳跃到A点
            //    tragetTrans = m_JumpPointA;
            //}
            //else
            //{
            //    //离B的距离大于离A的距离，所以现在是离A近，那么需要跳跃到B点
            //    tragetTrans = m_JumpPointB;
            //}

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_LunaController.Jump(false);
        }
    }
}
