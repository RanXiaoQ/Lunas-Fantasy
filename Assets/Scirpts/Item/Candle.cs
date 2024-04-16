using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public GameObject m_StarsEffect;
    public AudioClip m_PickClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.m_Instance.m_CandleNum++;          
            Instantiate(m_StarsEffect, transform.position, Quaternion.identity);
            if(GameManager.m_Instance.m_CandleNum >= 5)
            {
                GameManager.m_Instance.SetContentIndex();
            }
            GameManager.m_Instance.PlaySound(m_PickClip);
            Destroy(gameObject);
        }
    }
}
