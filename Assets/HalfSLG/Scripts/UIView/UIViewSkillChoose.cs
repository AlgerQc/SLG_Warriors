﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SLGame
{
    public class UIViewSkillChoose
        : UIViewBase
    {
        //控制显示位置
        [Header("Skill"), Space]
        [SerializeField] private RectTransform rtSkillLayout;    //技能按钮组
        [SerializeField] private RectTransform skillContent;     //滚动窗口
        [SerializeField] private List<Button> skillBtns;
        [SerializeField] private RectTransform chooseLayout;     //选中技能按钮组
        [SerializeField] private List<Button> skillChosenBtns;
        [SerializeField] private Button AckBtn;

        private List<BattleUnit> heros;         //操纵的英雄
        private Dictionary<int, SkillConfigInfo> chosedSkill = new Dictionary<int, SkillConfigInfo>();    //选中的技能组

        protected override void UpdateArguments(params object[] args)
        {
            if (args.Length >= 0)
                heros = args[0] as List<BattleUnit>;
        }

        protected override void InitUIObjects()
        {
            base.InitUIObjects();
            
            //获取技能按钮
            rtSkillLayout.GetComponentsInChildren<Button>(true, skillBtns);

            //动态获取，保证顺序
            if (skillBtns == null || skillBtns.Count == 0)
            {
                UtilityHelper.LogError("Init UIViewSkillChoose failed. Not found skill btn item.");
                return;
            }

            //绑定技能按钮回调
            for (int i = 0; i < skillBtns.Count; ++i)
            {
                skillBtns[i].name = string.Format("{0}{1}", EGameConstL.STR_SkillBtn, i);
                skillBtns[i].onClick.AddListener(OnClickedSkillItem);
            }
            

        }

        public override void OnShow()
        {
            base.OnShow();

            if (heros == null)
            {
                UtilityHelper.LogError("Show view error: UIViewSkillChoose");
                Close();
                return;
            }

            //设置位置
            /*
            var anchoredPosition = UIViewManager.Instance.ConvertWorldPositionToRootCanvasPosition(heros[0].mapGrid.localPosition);
            var relativePos = UIViewManager.Instance.GetRelativePosition(anchoredPosition);
            rtSkillLayout.ResetPivot(relativePos, 0f, 0f);
            rtSkillLayout.anchoredPosition = anchoredPosition;
            */

            //显示技能列表
            ShowSkillPanel();
            AckBtn.gameObject.SetActive(true);
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnExit()
        {
            base.OnExit();
            heros = null;
        }

        //点击了按钮组的触发器
        private void OnClickedOptionLayoutTrigger()
        {
            HideSkillNode();
        }

        //点击了技能按钮，则选中该技能
        private void OnClickedSkillItem()
        {
            //获取当前点击对象
            string btnName = EventSystem.current.currentSelectedGameObject.name;
            int skillIdx = -1;
            if (int.TryParse(btnName.Replace(EGameConstL.STR_SkillBtn, string.Empty), out skillIdx))
            {
                SkillConfigInfo skill = ConfigReader.skillInfoDic[skillIdx];
                if (chosedSkill.ContainsKey(skill.id))
                {
                    UtilityHelper.Log("Already choose that skill");
                }
                else
                {
                    chosedSkill.Add(skill.id, skill);
                    ShowChosedSkillPanle();
                }
            }
            else
            {
                UtilityHelper.LogError("Skill item name error ->" + btnName);
            }
        }

        //点击了已选择的按钮，则取消选择
        private void onClickChosedSkillItem()
        {

        }

        //显示技能节点
        private void ShowSkillPanel()
        {
            rtSkillLayout.gameObject.SetActive(true);

            //从ConfigReader中获取技能列表
            int i = 0;
            foreach (KeyValuePair<int, SkillConfigInfo> skill in ConfigReader.skillInfoDic)
            {
                //创建新按钮
                Button btn = Instantiate<Button>(skillBtns[0], skillContent);

                //设置新的按钮
                btn.name = string.Format("{0}{1}", EGameConstL.STR_SkillBtn, skill.Key);
                btn.onClick.AddListener(OnClickedSkillItem);
                skillBtns.Add(btn);

                //设置技能名字
                var label = skillBtns[i].transform.Find("Label").GetComponent<TextMeshProUGUI>();
                label.text = string.Format("{0}", skill.Value.name);
                label.color = EGameConstL.Color_labelWhite;
                i++;
            }

            //设置按钮状态
            for (i = 0; i < skillBtns.Count; ++i)
            {
                skillBtns[i].gameObject.SetActive(i < heros[0].battleUnitAttribute.battleSkills.Length);
            }
        }

        //显示选中的技能节点
        private void ShowChosedSkillPanle()
        {
            chooseLayout.gameObject.SetActive(true);

            //从ConfigReader中获取技能列表
            int i = 0;
            foreach (KeyValuePair<int, SkillConfigInfo> skill in chosedSkill)
            {
                //创建新按钮
                Button btn = Instantiate<Button>(skillChosenBtns[0], chooseLayout);

                //设置新的按钮
                btn.name = string.Format("{0}{1}", EGameConstL.STR_ChosedSkillBtn, skill.Key);
                btn.onClick.AddListener(onClickChosedSkillItem);
                skillChosenBtns.Add(btn);

                //设置技能名字
                var label = skillChosenBtns[i].transform.Find("Label").GetComponent<TextMeshProUGUI>();
                label.text = string.Format("{0}", skill.Value.name);
                label.color = EGameConstL.Color_labelWhite;
                i++;
            }

            //设置按钮状态
            for (i = 0; i < skillChosenBtns.Count; ++i)
            {
                skillChosenBtns[i].gameObject.SetActive(i < chosedSkill.Count);
            }
        }

        //隐藏技能节点
        private void HideSkillNode()
        {
            rtSkillLayout.gameObject.SetActive(false);
        }
    }
}
