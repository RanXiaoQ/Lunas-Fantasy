using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI����
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager m_Instance;
    public Image m_HP;
    public Image m_MP;
    public float m_HPSize;   //LunaHP��ʼHP
    public float m_MPSize;   //LunaMP��ʼMP 
    public GameObject m_BattlePanel;
    public GameObject m_TaklPanel;
    public Image m_CharacterImage;
    public Sprite[] m_CharacterSprtes;
    public TextMeshProUGUI m_NameText;
    public TextMeshProUGUI m_ContentText;


    private void Awake()
    {
        m_Instance = this;
        m_HPSize = m_HP.rectTransform.rect.width;
        m_MPSize = m_MP.rectTransform.rect.width;
    }
    private void Start()
    {
        SetHPValue(1);
        SetHPValue(1);
    }

    /// <summary>
    /// ����Ѫ��UI�����ʾ
    /// </summary>
    /// <param name="size">���ٷֱ�</param>
    public void SetHPValue(float size)
    {
        m_HP.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size * m_HPSize);
    }

    /// <summary>
    /// ��������UI�����ʾ
    /// </summary>
    /// <param name="size">���ٷֱ�</param>
    public void SetMPValue(float size)
    {
        m_HP.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * m_MPSize);
    }

    /// <summary>
    /// ս���������
    /// </summary>
    /// <param name="Start"></param>
    public void ShowBattlePanle(bool Start)
    {
        m_BattlePanel.SetActive(Start);
    }

    /// <summary>
    /// ��ʾ�Ի����ݣ�����������л������ֵĸ������Ի����ݵĸ�����
    /// </summary>
    /// <param name="content"></param>
    /// <param name="name"></param>
    public void ShowDialog(string content = null, string name = null)
    {
        //�ر�
        if (content == null)
        {
            m_TaklPanel.SetActive(false);
        }
        else
        {
            m_TaklPanel.SetActive(true);
            if (name != null)
            {
                if (name == "Luna")
                {
                    m_CharacterImage.sprite = m_CharacterSprtes[0];
                }
                else
                {
                    m_CharacterImage.sprite = m_CharacterSprtes[1];
                }
                m_CharacterImage.SetNativeSize();
            }
            m_NameText.text = content;
            m_ContentText.text = name;
        }
    }
}
