using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float m_DestoryTime;
    private void Start()
    {
        if(m_DestoryTime != -1)
        {
            Destroy(gameObject, m_DestoryTime);
        }
    }
}
