﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLGame.BattleBehaviourSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLGame
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SO_BattleUnitAttribute))]
    [CanEditMultipleObjects]
    public class SO_BattleUnitAttributeCustomEditor
        :Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Reset name."))
            {
                SO_BattleUnitAttribute instance = (SO_BattleUnitAttribute)target;
                instance.battleUnitName = instance.name;
            }
        }
    }
#endif

    public class PreCombo
    {
        public PreCombo(int stg, string sk1, string sk2, string sk3)
        {
            stage = stg;
            skill1 = sk1;
            skill2 = sk2;
            skill3 = sk3;
        }

        public int stage;
        public string skill1;
        public string skill2;
        public string skill3;
    }

    class ComboBuff
    {
        public int effectID;
        public int roundCount;
    }

    [CreateAssetMenu(menuName = "ScriptableObject/Battle unit attributes")]
    public class SO_BattleUnitAttribute 
        : ScriptableObject
    {
        public bool manualOperation;      //手动操作
        public string battleUnitName;

        public int hp;
        public int maxHp;

        public int BP = 0;
        private int useBPNum = 0;
        private bool usingBPAction = false;

        public AIBattleStrategy battleStrategy;

        //连击记录
        private Queue<int> skillCombo = new Queue<int>();

        //期待连击
        private PreCombo m_precombo = new PreCombo( 0, "", "", "" );

        //连击效果使用次数
        private List<ComboBuff> comboBuffRoundList = new List<ComboBuff>();

        public float springPower = 1.0f; //跳跃力
        public float volume = 5.0f;      //体积

        public int energy;
        public int maxEnergy;

        public int mobility;
        public int stopDistance = 1;

        public int baseAtk;
        public int atkRandRange;
        private int atk;

        public int baseDef;
        public int defRandRange;
        private int def;

        private int accuracy = 0;
        private int evade = 0;
        private int critical = 0;

        public float power = 5.0f;       //力量值
        public float speed = 4.0f;       //速度值
        public float technic = 4.0f;     //技巧值
        private float atkUpRatio = 1.0f;  //攻击力提升比率，供combo使用

        public float AtkUpRatio
        {
            get
            {
                foreach (ComboBuff comboBuff in comboBuffRoundList)
                {
                    if (comboBuff.effectID == 300) return atkUpRatio + 0.5f;
                    if (comboBuff.effectID == 210) return atkUpRatio + 0.2f;
                    if (comboBuff.effectID == 201) return atkUpRatio + 0.2f;
                }
                return atkUpRatio;
            }
        }

        public float GetTargetAvoidRatio(BattleUnit target)
        {
            return Mathf.Clamp((target.battleUnitAttribute.speed / speed) * 0.5f, 0.0f, 1.0f);      //待修改的公式
        }

        public float GetTargetCritRatio(BattleUnit target)
        {
            return Mathf.Clamp((technic / target.battleUnitAttribute.technic) * 0.2f, 0.0f, 1.0f);  //待修改的公式
        }

        public float GetDamageTypeModulus(BattleUnit target)
        {
            float result = 1.0f;
            BattleSkillDamageType selfType = battleSkills[0] ? battleSkills[0].damageType : BattleSkillDamageType.None;
            BattleSkillDamageType targetType = target.battleUnitAttribute.battleSkills[0] ? target.battleUnitAttribute.battleSkills[0].damageType : BattleSkillDamageType.None;
            if (selfType == BattleSkillDamageType.Physical && targetType == BattleSkillDamageType.Skill)
            {
                result += EGameConstL.DamageTypeModulus;
            }
            else if (selfType == BattleSkillDamageType.Physical && targetType == BattleSkillDamageType.Move)
            {
                result -= EGameConstL.DamageTypeModulus;
            }
            else if (selfType == BattleSkillDamageType.Move && targetType == BattleSkillDamageType.Physical)
            {
                result += EGameConstL.DamageTypeModulus;
            }
            else if (selfType == BattleSkillDamageType.Move && targetType == BattleSkillDamageType.Skill)
            {
                result -= EGameConstL.DamageTypeModulus;
            }
            else if (selfType == BattleSkillDamageType.Skill && targetType == BattleSkillDamageType.Move)
            {
                result += EGameConstL.DamageTypeModulus;
            }
            else if (selfType == BattleSkillDamageType.Skill && targetType == BattleSkillDamageType.Physical)
            {
                result -= EGameConstL.DamageTypeModulus;
            }
            return result;
        }

        public int Atk
        {
            get
            {
                if (hostBattleUnit.mapGrid.gridUnitBuff != null && hostBattleUnit.mapGrid.gridUnitBuff.buffType == GridUnitBuffType.Atk)
                    return atk + hostBattleUnit.mapGrid.gridUnitBuff.addition;

                return atk;
            }
        }

        public int Def
        {
            get
            {
                if (hostBattleUnit.mapGrid.gridUnitBuff != null && hostBattleUnit.mapGrid.gridUnitBuff.buffType == GridUnitBuffType.Def)
                    return def + hostBattleUnit.mapGrid.gridUnitBuff.addition;

                return def;
            }
        }
        
        public SO_BattleSkill[] battleSkills;

        public BattleUnit hostBattleUnit;
        public BattleBehaviourSystem.BattleBehaviourSystem battleBehaviourSystem;

        public void Reset()
        {
            hp = maxHp;
            energy = maxEnergy;
        }

        public void RandomAttributes()
        {
            atk = baseAtk + Random.Range(0, atkRandRange);
            def = baseDef + Random.Range(0, defRandRange);
        }

        public void UpdateAtk(int value)
        {
            atk = atk + value;
            if (atk < 0)
                atk = 0;
        }

        public void UpdateAtkSlim(int value)
        {
            UpdateAtk(value / 3);
        }

        public void UpdateBP(int value)
        {
            BP += value;
            if (BP > EGameConstL.BPMax)
                BP = EGameConstL.BPMax;
            if (BP < 0)
                BP = 0;
        }

        public void UpdateBPSlim(int value)
        {
            UpdateBP(value / 3);
        }

        public void UpdateAccuracy(int value)
        {
            accuracy += value;
            if (accuracy < 0)
                accuracy = 0;
        }

        public void UpdateAccuracySlim(int value)
        {
            UpdateAccuracy(value / 3);
        }

        public void UpdateEvade(int value)
        {
            evade += value;
            if (evade < 0)
                evade = 0;
        }

        public void UpdateEvadeSlim(int value)
        {
            UpdateEvade(value / 3);
        }

        public void UpdateCritical(int value)
        {
            critical += value;
            if (critical < 0)
                critical = 0;
        }

        public void UpdateCriticalSlim(int value)
        {
            UpdateCritical(value / 3);
        }

        public void IncreaseAbility()
        {
            UpdateCritical(EGameConstL.CriticalIncrease);
            UpdateEvade(EGameConstL.EvadeIncrease);
            UpdateAccuracy(EGameConstL.AccuracyIncrease);
        }

        public void DecreaseAbility()
        {
            UpdateCritical(EGameConstL.CriticalDecrease);
            UpdateEvade(EGameConstL.EvadeDecrease);
            UpdateAccuracy(EGameConstL.AccuracyDecrease);
        }

        public void IncreaseAbilitySlim()
        {
            UpdateCriticalSlim(EGameConstL.CriticalIncrease);
            UpdateEvadeSlim(EGameConstL.EvadeIncrease);
            UpdateAccuracySlim(EGameConstL.AccuracyIncrease);
        }

        public void DecreaseAbilitySlim()
        {
            UpdateCriticalSlim(EGameConstL.CriticalDecrease);
            UpdateEvadeSlim(EGameConstL.EvadeDecrease);
            UpdateAccuracySlim(EGameConstL.AccuracyDecrease);
        }

        public void RemoveBuff(int buffID)
        {
            switch (buffID)
            {
                case 300:
                    UpdateAtk(EGameConstL.AtkDecrease);
                    UtilityHelper.Log("Combo Attack Power * 3 Remove!");
                    break;

                case 30:
                    UpdateBP(EGameConstL.BPDecrease);
                    UtilityHelper.Log("Combo Attack Move * 3 Remove!");
                    break;

                case 3:
                    DecreaseAbility();
                    UtilityHelper.Log("Combo Attack Skill * 3 Remove!");
                    break;

                case 201:
                    
                    UtilityHelper.Log("Combo Attack 2 Power 1 Skill Remove!");
                    break;

                case 210:
                    UtilityHelper.Log("Combo Attack 2 Power 1 Move Remove!");
                    break;

                case 102:
                    UtilityHelper.Log("Combo Attack 1 Power 2 Skill Remove!");
                    break;

                case 12:
                    UtilityHelper.Log("Combo Attack 1 Move 2 Skill Remove!");
                    break;

                case 120:
                    UtilityHelper.Log("Combo Attack 1 Power 2 Move Remove!");
                    break;

                case 21:
                    UtilityHelper.Log("Combo Attack 2 Move 1 Skill Remove!");
                    break;
            }
            return;
        }

        public bool HeroUsingBP()
        {
            return useBPNum > 0;
        }

        public void BPUsing()
        {
            BP--;
            useBPNum++;
            UtilityHelper.LogFormat("left BP = {0}, using BP = {1}", BP, useBPNum);
        }

        public bool BPUsed()
        {
            GetInBPAction();

            if (useBPNum > 1)
            {
                useBPNum--;
                return true;
            }
            else
            {
                useBPNum = 0;
                return false;
            }
        }

        public void BPUsed(int num)
        {
            useBPNum -= num;
            OutOfBPAction();
        }

        public int BPUseNum()
        {
            return useBPNum;
        }

        public void GetInBPAction()
        {
            usingBPAction = true;
        }

        public void OutOfBPAction()
        {
            usingBPAction = false;
        }

        public bool BPUsingStage()
        {
            return usingBPAction;
        }

        public bool BPCanUse()
        {
            if (BP > 0 && useBPNum < EGameConstL.BPUseMax && usingBPAction == false)
                return true;
            return false;
        }

        public int ComboCount
        {
            get
            {
                return skillCombo.Count;
            }
        }

        public PreCombo ComboStatus
        {
            get
            {
                return m_precombo;
            }
        }

        public int comboJudge(int skillType)
        {
            skillCombo.Enqueue(skillType);

            if (skillCombo.Count == EGameConstL.ComboCount)
            {
                int id = Check.checkCombo(skillCombo);
                //UpdatePrecombo(id);
                if ( id >= 0)
                {
                    return id;
                }
                else
                {
                    return -1;
                }
            }
            else 
            {
                int id = Check.checkComboEarly(skillCombo);
                if (id >= 0)
                {
                    //UpdatePrecombo(id);
                    UtilityHelper.LogFormat("Wait for {0} more skills to fill combo", EGameConstL.ComboCount - skillCombo.Count);
                }
                else
                {
                    ClearPrecombo();
                    skillCombo.Clear();
                    UtilityHelper.Log("Combo error, start from first skill again");
                }

                return -1;
            }
        }

        public void AddComboBuffEffect(int id)
        {
            //对BUFF类，延迟类combo效果需要计算持续时间
            ComboBuff tmp = new ComboBuff();
            tmp.effectID = id;
            tmp.roundCount = 0;
            comboBuffRoundList.Add(tmp);
        }

        public void skillComboActiveCheck()
        {
            for (int i = 0; i < comboBuffRoundList.Count; i++)
            {
                if (comboBuffRoundList[i].roundCount < EGameConstL.ComboEffectRoundCount)
                {
                    comboBuffRoundList[i].roundCount++;
                    UtilityHelper.LogFormat("buff id = {0}, left round = {1}", i, EGameConstL.ComboEffectRoundCount - comboBuffRoundList[i].roundCount);
                }
                else
                {
                    UtilityHelper.LogFormat("remove buff {0}", i);
                    RemoveBuff(comboBuffRoundList[i].effectID);
                    comboBuffRoundList.RemoveAt(i);
                }
            }
        }

        private void UpdatePrecombo(int id)
        {
            m_precombo.stage = skillCombo.Count;
            int id1 = ConfigReader.comboInfoDic[id].skill1;
            int id2 = ConfigReader.comboInfoDic[id].skill2;
            int id3 = ConfigReader.comboInfoDic[id].skill3;

            m_precombo.skill1 = ConfigReader.skillInfoDic[id1].name;
            m_precombo.skill2 = ConfigReader.skillInfoDic[id2].name;
            m_precombo.skill3 = ConfigReader.skillInfoDic[id3].name;
        }

        private void ClearPrecombo()
        {
            m_precombo.stage = 0;
            m_precombo.skill1 = "";
            m_precombo.skill2 = "";
            m_precombo.skill3 = "";
        }

    }
}