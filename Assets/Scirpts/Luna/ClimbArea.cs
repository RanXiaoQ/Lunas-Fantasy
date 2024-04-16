using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ≈ ≈¿≈ˆ◊≤ºÏ≤‚
/// </summary>
public class ClimbArea : MonoBehaviour
{
    public LunaController m_LunaController;
    private void Start()
    {
        if (m_LunaController == null)
            m_LunaController = FindObjectOfType<LunaController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            m_LunaController.Climb(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_LunaController.Climb(false);
        }
    }
}
