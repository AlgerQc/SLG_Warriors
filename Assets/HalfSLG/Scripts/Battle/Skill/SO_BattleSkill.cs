using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{
    public enum BattleSkillDamageType
    {
        Physical = 1,   //物理伤害
        Move,           //位移类
        Heal,           //恢复&buff类
    }

    public enum BattleSkillTargetType
    {
        BattleUnit, //对某一个战斗单位
        GridUnit,   //对某一个地图格子(范围)
        Self,       //以自身为中心的
    }

    [CreateAssetMenu(menuName = "ScriptableObject/Battle skill")]
    public class SO_BattleSkill
        : ScriptableObject
    {
        public int skillID;                //技能id
        public string skillName;           //技能名字
        [SerializeField] private int releaseRadius;          //技能释放半径
        public int effectRadius;            //技能影响半径idu
        public BattleSkillDamageType damageType;    //伤害类型
        public BattleSkillTargetType targetType;    //目标类型
        public int mainValue;              //造成的伤害
        public int energyCost = 0;        //体力扣减
        public float hatredMultiple = 1f;   //仇恨倍数
        public float rageLevel = 0f;        //愤怒增加
        public string action;               //对应动画
        public string sound;                //技能音效
        public string info;                 //技能信息文本

        public int GetReleaseRadius(GridUnit gridUnit)
        {
            int addition = 0;
            if (gridUnit != null && gridUnit.gridUnitBuff != null && gridUnit.gridUnitBuff.buffType == GridUnitBuffType.Range)
                addition = gridUnit.gridUnitBuff.addition;

            return releaseRadius + addition;
        }

        //用于计算的释放半径(最大，考虑效果范围)
        public int GetMaxReleaseRadiusForCalculate(GridUnit gridUnit)
        {
            int addition = 0;
            if (gridUnit != null && gridUnit.gridUnitBuff != null && gridUnit.gridUnitBuff.buffType == GridUnitBuffType.Range)
                addition = gridUnit.gridUnitBuff.addition;

            switch (targetType)
            {
                case BattleSkillTargetType.BattleUnit:
                    return releaseRadius + addition;
                case BattleSkillTargetType.GridUnit:
                    return releaseRadius + effectRadius + addition;
                case BattleSkillTargetType.Self:
                    return effectRadius + addition;
                default:
                    return 0;
            }
        }
    }
}