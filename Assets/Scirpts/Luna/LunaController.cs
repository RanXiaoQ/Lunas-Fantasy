using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaController : MonoBehaviour
{
    public float m_Speed = 1;
    private Rigidbody2D m_Rig2D;
    
    private Animator m_Animator;
    private Vector2 m_LookDirection = new Vector2(1,0);
    private float m_MoveScale;
    public PolygonCollider2D m_MapPolygonCollider2D;
    private MonsterController m_MonsterController;




    private void Start()
    {
        //设置帧率为10，默认为60
        //Application.targetFrameRate = 10;
        m_Rig2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponentInChildren<Animator>();
        if (m_MonsterController == null)
            m_MonsterController = FindObjectOfType<MonsterController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Talk();
        }
    }

    private void FixedUpdate()
    {
        LunaMove();
        Expedite();
    }

    /// <summary>
    /// Luna的移动控制和移动动画
    /// </summary>
    private void LunaMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(h, v).normalized;  //将输入向量归一化，以便获取方向
        // 计算总速度
        float totalSpeed = Mathf.Abs(h) + Mathf.Abs(v);
        Vector2 move = new Vector2(h, v);
        Vector2 pos = transform.position;
        m_Animator.SetFloat("MoveValue", 0);
        // 如果按下了多个方向键，将总速度限制在单个方向键情况下的速度
        if (totalSpeed > 1)
        {
            totalSpeed = 1;
        }
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            m_LookDirection.Set(move.x, move.y);
            m_LookDirection.Normalize();
            m_Animator.SetFloat("MoveValue", 1);
        }
        m_Animator.SetFloat("LookX", m_LookDirection.x);
        m_Animator.SetFloat("LookY", m_LookDirection.y);      
        pos += direction * m_Speed * totalSpeed * Time.deltaTime;
        m_Rig2D.MovePosition(pos);  //使用之后优化人物跟碰撞体产生的抖动
    }

    /// <summary>
    /// 加速
    /// </summary>
    private void Expedite()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(h, v);
        m_MoveScale = move.magnitude;
        if (move.magnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_MoveScale = 2;
                m_Speed = 2.5f;
            }
            else
            {
                m_MoveScale = 1;
                m_Speed = 1;
            }
        }
        m_Animator.SetFloat("MoveValue", m_MoveScale);
    }

    /// <summary>
    /// 攀爬
    /// </summary>
    /// <param name="Star"></param>
    public void Climb(bool Star)
    {
        m_Animator.SetBool("Climb",Star);
    }  

    /// <summary>
    /// 跳跃
    /// </summary>
    /// <param name="Star"></param>
    public void Jump(bool Star)
    {
        m_Animator.SetBool("Jump", Star);
        m_MapPolygonCollider2D.enabled = !Star;
    }

    

    /// <summary>
    /// 对话
    /// </summary>
    public void Talk()
    {
        Collider2D collider = Physics2D.OverlapCircle(m_Rig2D.position,
            0.5f, LayerMask.GetMask("NPC"));
        if (collider != null)
        {
            if (collider.name == "Nala")
            {
                GameManager.m_Instance.m_CanControlLuna = false;
                collider.GetComponent<NPCDialog>().DisplayDialog();
            }
            else if (collider.name == "Dog"
                && !GameManager.m_Instance.m_HasPetTheDog &&
                GameManager.m_Instance.m_DialogInfoIndex == 2)
            {
                PetTheDog();
                GameManager.m_Instance.m_CanControlLuna = false;
                collider.GetComponent<Dog>().BeHappy();
            }
        }
    }

    /// <summary>
    /// 摸狗动画
    /// </summary>
    private void PetTheDog()
    {
        m_Animator.CrossFade("Pet", 0);
        transform.position = new Vector3(-0.55f, -7.83f, 0);
    }
}
