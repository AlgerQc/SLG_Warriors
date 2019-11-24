using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SLGame
{
    public class ConfigReader
    {
        
        public static Dictionary<int, SkillConfigInfo> skillInfoDic = new Dictionary<int, SkillConfigInfo>();
        public static Dictionary<int, ComboConfigInfo> comboInfoDic = new Dictionary<int, ComboConfigInfo>();
        public static GridUnit[] teamALocations;
        public static GridUnit[] teamBLocations;

        public static void Init()
        {
            skillInfoDic = ReadSkillConfig.ReadSkillConfigFromXml("Config/SkillCfg");
            comboInfoDic = ReadSkillComboConfig.ReadSkillComboConfigFromXml("Config/ComboSkill");
            

        }

    }
}
