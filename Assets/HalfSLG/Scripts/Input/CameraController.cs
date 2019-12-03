using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{
    public class CameraController : MonoBehaviour
    {
        float m_Horizontal = 0.0f;
        float m_Vertical = 0.0f;
        public float m_MoveSpeed = 1.0f;
        private BattleFieldRenderer m_BattleFieldRenderer;
        
        private void Start()
        {
            m_BattleFieldRenderer = transform.GetComponentInParent<BattleFieldRenderer>();
        }

        void Update()
        {
            m_Horizontal = Input.GetAxis("Horizontal");
            m_Vertical = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            //transform.Translate(m_Horizontal * m_MoveSpeed, m_Vertical * m_MoveSpeed, 0.0f);
            if (m_BattleFieldRenderer.hotPointGridUnit != null)
            {
                Vector3 position = new Vector3
                {
                    z = transform.localPosition.z
                };
                if (m_BattleFieldRenderer.hotPointBattleUnit != null)
                {
                    position.x = m_BattleFieldRenderer.hotPointBattleUnit.battleUnitRenderer.transform.localPosition.x;
                    position.y = m_BattleFieldRenderer.hotPointBattleUnit.battleUnitRenderer.transform.localPosition.y;
                }
                else
                {
                    position.x = m_BattleFieldRenderer.hotPointGridUnit.localPosition.x;
                    position.y = m_BattleFieldRenderer.hotPointGridUnit.localPosition.y;
                }
                transform.localPosition = position;
            }
        }
    }
}
