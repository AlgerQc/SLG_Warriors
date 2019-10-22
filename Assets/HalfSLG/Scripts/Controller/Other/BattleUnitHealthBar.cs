using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SLGame
{
    public class BattleUnitHealthBar
        : BaseBehaviour
    {
        [SerializeField] private GameObject objHealthBarNode;
        [SerializeField] private SpriteRenderer srHealthBarGreen;
        [SerializeField] private TextMeshPro tmpLabelHealth;
        [SerializeField] private SortingOrderHelper sortingOrderHelper;
        //[SerializeField] private SpriteRenderer srEnergyBarYellow;
        [SerializeField] private GameObject objBPBarNode;
        [SerializeField] private SpriteRenderer srBPBarGreen;
        [SerializeField] private TextMeshPro tmpLabelBP;

        public void Init()
        {
            //刷新层级
            sortingOrderHelper.RefreshOrder(EGameConstL.SortingLayer_Battle_Bar, 0);
        }

        public void UpdateHealth(int current, int max)
        {
            current = Mathf.Clamp(current, 0, max);
            //设置值
            tmpLabelHealth.text = string.Format("{0}/{1}", current, max);
            //设置长度
            srHealthBarGreen.transform.localScale = new Vector3(current * 1f / max, 1f, 1f);
        }

        public void UpdateEnergy(int current, int max)
        {
            //current = Mathf.Clamp(current, 0, max);
            //设置长度
            //srEnergyBarYellow.transform.localScale = new Vector3(current * 1f / max, 1f, 1f);
        }

        public void UpdateBP(int current, int max)
        {
            current = Mathf.Clamp(current, 0, max);
            //设置值
            tmpLabelBP.text = string.Format("{0}/{1}", current, max);
            //设置长度
            srBPBarGreen.transform.localScale = new Vector3(current * 6.997f / max, 1f, 1f);
        }
    }
}