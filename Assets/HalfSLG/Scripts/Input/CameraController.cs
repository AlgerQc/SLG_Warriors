using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float m_Horizontal = 0.0f;
    float m_Vertical = 0.0f;
    public float m_MoveSpeed = 1.0f;

    void Update()
    {
        m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        transform.Translate(m_Horizontal * m_MoveSpeed, m_Vertical * m_MoveSpeed, 0.0f);
    }
}
