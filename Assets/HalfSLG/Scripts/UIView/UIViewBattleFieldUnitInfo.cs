﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SLGame
{
    public class UIViewBattleFieldUnitInfo 
        : UIViewBase
    {
        [SerializeField] TextMeshProUGUI tmpBattleUnitInfo;
        [SerializeField] TextMeshProUGUI tmpGridUnitInfo;
        [SerializeField] TextMeshProUGUI BattleName;
        [SerializeField] GameObject separateLine;
        [SerializeField] ViewElementPackage package;

        private GridUnit focusGridUnit;
        private BattleUnit focusBattleUnit;

        private BattleUnit FocusBattleUnit
        {
            set
            {
                if (focusBattleUnit != null && focusBattleUnit.battleUnitRenderer) 
                    focusBattleUnit.battleUnitRenderer.linkedUnitInfoView = null;

                focusBattleUnit = value;
                if (focusBattleUnit != null && focusBattleUnit.battleUnitRenderer) 
                    focusBattleUnit.battleUnitRenderer.linkedUnitInfoView = this;
            }
        }

        protected override void InitUIObjects()
        {
            base.InitUIObjects();
        }

        protected override void UpdateArguments(params object[] args)
        {
            if (args.Length == 0)
            {
                ErrorClose("Args error");
                return;
            }

            focusGridUnit = args[0] as GridUnit;
            FocusBattleUnit = args[1] as BattleUnit;

            if (focusGridUnit == null && focusBattleUnit == null)
            {
                Close();
                return;
            }
        }

        private string GetGridUnitBuffStr(GridUnitBuff buff, GridUnitBuffType convertInfoType)
        {
            if (buff == null)
                return string.Empty;
            else if (buff.buffType != convertInfoType)
                return string.Empty;
            else
            {
                switch (buff.buffType)
                {
                    case GridUnitBuffType.Atk:
                        return string.Format("<color=#FF0000>(+{0})</color>", buff.addition);
                    case GridUnitBuffType.Def:
                        return string.Format("<color=#0000FF>(+{0})</color>", buff.addition);
                    default:
                        return string.Empty;
                }
            }
        }

        private void UpdateBattleUnitPackage()
        {
            if (focusBattleUnit != null)
            {
                package.UpdateBattleUnitPackage(focusBattleUnit.package, false);
                package.gameObject.SetActive(true);
            }
            else
            {
                package.UpdateBattleUnitPackage(null, false);
                package.gameObject.SetActive(false);
            }
        }

        public override void UpdateView()
        {
            base.UpdateView();

            if (focusGridUnit != null)
                tmpGridUnitInfo.text = string.Format("Type : {0}", focusGridUnit.GridType);

            if (focusBattleUnit != null)
                tmpBattleUnitInfo.text = string.Format("Hp:{0}/{1}\nAtk:{2}{3}\nDef:{4}{5}\nMobility:{6}{7}",
                    focusBattleUnit.battleUnitAttribute.hp, focusBattleUnit.battleUnitAttribute.maxHp,
                    focusBattleUnit.battleUnitAttribute.Atk, GetGridUnitBuffStr(focusBattleUnit.mapGrid.gridUnitBuff, GridUnitBuffType.Atk),
                    focusBattleUnit.battleUnitAttribute.Def, GetGridUnitBuffStr(focusBattleUnit.mapGrid.gridUnitBuff, GridUnitBuffType.Def),
                    focusBattleUnit.battleUnitAttribute.mobility, GetGridUnitBuffStr(focusBattleUnit.mapGrid.gridUnitBuff, GridUnitBuffType.Range)
                    );

            if (focusBattleUnit != null)
                BattleName.text = string.Format("{0}", focusBattleUnit.battleUnitAttribute.name);

            //设置显示
            separateLine.SetActive(focusGridUnit != null && focusBattleUnit != null);
            tmpGridUnitInfo.gameObject.SetActive(focusGridUnit != null);
            tmpBattleUnitInfo.gameObject.SetActive(focusBattleUnit != null);
            BattleName.gameObject.SetActive(focusBattleUnit != null);

            UpdateBattleUnitPackage();
        }

        private void OnBattleUnitManualStateChanged(IGameEvent e)
        {
            if (!e.Name.Equals(EGameConstL.EVENT_BATTLE_UNIT_MANUAL_STATE_CHANGED))
                return;

            if (focusBattleUnit != null && focusBattleUnit.Equals(e.Body))
            {
                UpdateBattleUnitPackage();
            }
        }

        public override void OnPush()
        {
            base.OnPush();
            RegisterEventListener(EGameConstL.EVENT_BATTLE_UNIT_MANUAL_STATE_CHANGED, OnBattleUnitManualStateChanged);
        }

        public override void OnPopup()
        {
            base.OnPopup();
            RemoveAllEventListeners();
        }
    }
}