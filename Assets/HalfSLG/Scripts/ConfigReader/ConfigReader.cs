using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SLGame
{
    public class ConfigReader
    {
        #region read skill.xml
        public static Dictionary<uint, SkillConfigInfo> skillInfoDic = new Dictionary<uint, SkillConfigInfo>();

        public static Dictionary<uint, SkillConfigInfo> SkillInfoDic
        {
            get
            {
                if (skillInfoDic.Count == 0)
                {
                    ReadSkillConfig spConfig = new ReadSkillConfig("Config/SkillCfg");
                }
                return skillInfoDic;
            }
        }

        public static SkillConfigInfo GetSkillConfig(uint id)
        {
            if (SkillInfoDic.ContainsKey(id))
            {
                return SkillInfoDic[id];
            }
            return null;
        }
        #endregion

        #region read ComboSkill.xml
        public static Dictionary<uint, ComboConfigInfo> comboInfoDic = new Dictionary<uint, ComboConfigInfo>();

        public static Dictionary<uint, ComboConfigInfo> ComboInfoDic
        {
            get
            {
                if (comboInfoDic.Count == 0)
                {
                    ReadSkillComboConfig spConfig = new ReadSkillComboConfig("Config/ComboSkill");
                }
                return comboInfoDic;
            }
        }

        public static ComboConfigInfo GetComboConfig(uint id)
        {
            if (ComboInfoDic.ContainsKey(id))
            {
                return ComboInfoDic[id];
            }
            return null;
        }

        #endregion

        public static void Init()
        {
            Dictionary<uint, SkillConfigInfo> skillDic = SkillInfoDic;
            Dictionary<uint, ComboConfigInfo> comboDic = ComboInfoDic;
        }

        public static int checkCombo(Queue<int> skillCombo)
        {
            Queue<int> tmp = skillCombo;
            int skill1 = tmp.Dequeue();
            int skill2 = tmp.Dequeue();
            int skill3 = tmp.Dequeue();
            for (uint i = 0; i < comboInfoDic.Count; i++)
            {
                if (comboInfoDic[i].skill1 == skill1 && comboInfoDic[i].skill2 == skill2 &&
                    comboInfoDic[i].skill3 == skill3)
                {
                    return comboInfoDic[i].effect;
                }
            }

            return 0;
        }

        public static bool checkComboEarly(Queue<int> skillCombo)
        {
            if (skillCombo.Count == 2)
            {
                int skill1 = skillCombo.Dequeue();
                int skill2 = skillCombo.Dequeue();
                for (uint i = 0; i < comboInfoDic.Count; i++)
                {
                    if (comboInfoDic[i].skill1 == skill1 && comboInfoDic[i].skill2 == skill2)
                    {
                        UtilityHelper.Log("We are in combo stage 2");
                        skillCombo.Enqueue(skill1);
                        skillCombo.Enqueue(skill2);
                        return true;
                    }
                }
            }
            else if (skillCombo.Count == 1)
            {
                int skill1 = skillCombo.Dequeue();
                for (uint i = 0; i < comboInfoDic.Count; i++)
                {
                    if (comboInfoDic[i].skill1 == skill1)
                    {
                        UtilityHelper.LogFormat("We are in combo stage 1");
                        skillCombo.Enqueue(skill1);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
