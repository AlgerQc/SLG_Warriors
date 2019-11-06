using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SLGame
{
    public class ConfigReader
    {
        
        public static Dictionary<uint, SkillConfigInfo> skillInfoDic = new Dictionary<uint, SkillConfigInfo>();
        public static Dictionary<uint, ComboConfigInfo> comboInfoDic = new Dictionary<uint, ComboConfigInfo>();

        public static void Init()
        {
            skillInfoDic = ReadSkillConfig.ReadSkillConfigFromXml("Config/SkillCfg");
            comboInfoDic = ReadSkillComboConfig.ReadSkillComboConfigFromXml("Config/ComboSkill");
        }

    }
}
