using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ��Ծ��ײ���
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
            //    //��A�ľ��������B�ľ��룬������������B������ô��Ҫ��Ծ��A��
            //    tragetTrans = m_JumpPointA;
            //}
            //else
            //{
            //    //��B�ľ��������A�ľ��룬������������A������ô��Ҫ��Ծ��B��
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
