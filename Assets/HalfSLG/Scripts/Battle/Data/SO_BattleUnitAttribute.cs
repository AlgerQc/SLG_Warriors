using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //连击记录
        private Queue<int> skillCombo = new Queue<int>();

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
        }

        public void RemoveBuff(int buffID)
        {
            switch (buffID)
            {
                case (int)ComboEffectType.DamageEffect:
                    UtilityHelper.Log("Combo Attack DamageEffect remove!");
                    UpdateAtk(EGameConstL.AtkDecrease);
                    break;

                case (int)ComboEffectType.HealEffect:
                    UtilityHelper.Log("Combo Attack HealEffect remove!");
                    break;

                case (int)ComboEffectType.PushEffect:
                    UtilityHelper.Log("Combo Attack PushEffect remove!");
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

        public int comboJudge(int skillID)
        {
            skillCombo.Enqueue(skillID);
            if (skillCombo.Count == EGameConstL.ComboCount)
            {
                int id = ConfigReader.checkCombo(skillCombo);
                if ( id > 0)
                {
                    return id;
                }
                else
                {
                    return 0;
                }
            }
            else 
            {
                if (ConfigReader.checkComboEarly(skillCombo))
                {
                    UtilityHelper.LogFormat("Wait for {0} more skills to fill combo", EGameConstL.ComboCount - skillCombo.Count);
                }
                else
                {
                    skillCombo.Clear();
                    UtilityHelper.Log("Combo error, start from first skill again");
                }

                return 0;
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
    }
}