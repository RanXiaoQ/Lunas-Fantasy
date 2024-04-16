using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ѫƿʹ��
/// </summary>
public class RecoverHP : MonoBehaviour
{
    public GameManager m_GameManager;
    public GameObject m_StarsEffect;
    public AudioClip m_PickSound;

    private void Start()
    {
        if(m_GameManager == null)
            m_GameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// ���Ѫƿ�Ƿ���ײPlayer
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(m_GameManager.m_LunaCurrentHealth < m_GameManager.m_LunaHP)
            {
                Instantiate(m_StarsEffect,transform.position,Quaternion.identity);
                m_GameManager.AddOrDecreaseLunaHP(40);
                GameManager.m_Instance.PlaySound(m_PickSound);
                //m_GameManager.ChangeHeath(1);
                //Debug.Log("1");
                Destroy(gameObject);
            }           
        }
    }


}
