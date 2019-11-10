using System.Collections;
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
        [SerializeField] private List<Button> skillBtns;

        private BattleUnit battleUnit;

        protected override void UpdateArguments(params object[] args)
        {
            if (args.Length >= 0)
                battleUnit = args[0] as BattleUnit;
        }

        protected override void InitUIObjects()
        {
            base.InitUIObjects();

            //获取技能按钮
            rtSkillLayout.GetComponentsInChildren<Button>(true, skillBtns);
            //动态获取，保证顺序
            if (skillBtns == null || skillBtns.Count == 0)
            {
                UtilityHelper.LogError("Init BattleFieldPlayerActOption failed.Not found skill btn item.");
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

            if (battleUnit == null)
            {
                UtilityHelper.LogError("Show view error: UIViewBattleFieldPlayerActOption");
                Close();
                return;
            }
            //设置位置
            var anchoredPosition = UIViewManager.Instance.ConvertWorldPositionToRootCanvasPosition(battleUnit.mapGrid.localPosition);
            var relativePos = UIViewManager.Instance.GetRelativePosition(anchoredPosition);
            rtSkillLayout.ResetPivot(relativePos, 0f, 0f);
            rtSkillLayout.anchoredPosition = anchoredPosition;
            //初始时隐藏技能节点
            HideSkillNode();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnExit()
        {
            base.OnExit();
            battleUnit = null;
        }

        //点击了按钮组的触发器
        private void OnClickedOptionLayoutTrigger()
        {
            HideSkillNode();
        }

        //点击了技能按钮
        private void OnClickedSkillItem()
        {
            //获取当前点击对象
            string btnName = EventSystem.current.currentSelectedGameObject.name;
            int skillIdx = -1;
            if (int.TryParse(btnName.Replace(EGameConstL.STR_SkillBtn, string.Empty), out skillIdx))
            {
                SO_BattleSkill skill = battleUnit.battleUnitAttribute.battleSkills[skillIdx];
                if (skill != null)
                {
                    if (battleUnit.battleUnitAttribute.energy >= skill.energyCost && BattleFieldRenderer.Instance != null)
                    {
                        BattleFieldRenderer.Instance.BattleUnitUseSkill(battleUnit, skill);
                    }
                    else
                    {
                        UtilityHelper.LogWarning(string.Format("能量不足:{0}/{1}", battleUnit.battleUnitAttribute.energy, skill.energyCost));
                    }
                }
                else
                    UtilityHelper.LogError("Skill item error ->" + btnName);
            }
            else
            {
                UtilityHelper.LogError("Skill item name error ->" + btnName);
            }
        }

        //显示技能节点
        private void ShowSkillPanel()
        {
            rtSkillLayout.gameObject.SetActive(true);

            //获取技能
            for (int i = 0; i < battleUnit.battleUnitAttribute.battleSkills.Length; ++i)
            {
                if (i >= skillBtns.Count && skillBtns.Count > 0)
                {
                    //创建新按钮
                    Button btn = Instantiate<Button>(skillBtns[0], rtSkillLayout);
                    //设置新的按钮
                    btn.name = string.Format("{0}{1}", EGameConstL.STR_SkillBtn, i);
                    btn.onClick.AddListener(OnClickedSkillItem);
                    skillBtns.Add(btn);
                }
                //设置技能名字
                var label = skillBtns[i].transform.Find("Label").GetComponent<TextMeshProUGUI>();
                int battleUnitEnergy = battleUnit.battleUnitAttribute.energy;
                int energyCost = battleUnit.battleUnitAttribute.battleSkills[i].energyCost;
                label.text = string.Format("{0}({1}/{2})", battleUnit.battleUnitAttribute.battleSkills[i].skillName, battleUnitEnergy, energyCost);
                //判断能量是否足够
                label.color = battleUnitEnergy >= energyCost ? EGameConstL.Color_labelWhite : EGameConstL.Color_labelRed;
            }

            //设置按钮状态
            for (int i = 0; i < skillBtns.Count; ++i)
            {
                skillBtns[i].gameObject.SetActive(i < battleUnit.battleUnitAttribute.battleSkills.Length);
            }
        }

        //隐藏技能节点
        private void HideSkillNode()
        {
            rtSkillLayout.gameObject.SetActive(false);
        }
    }
}
