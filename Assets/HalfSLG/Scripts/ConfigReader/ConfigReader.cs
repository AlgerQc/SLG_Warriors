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
        public static Dictionary<uint, SkillConfigInfo> SkillPassiveInfoDic
        {
            get
            {
                if (SkillPassiveInfoDic.Count == 0)
                {
                    ReadSkillConfig spConfig = new ReadSkillConfig("Config/SkillCfg");
                }
                return SkillPassiveInfoDic;
            }
        }
        public static SkillConfigInfo GetSkillPassiveConfig(uint id)
        {
            if (SkillPassiveInfoDic.ContainsKey(id))
            {
                return SkillPassiveInfoDic[id];
            }
            return null;
        }
        #endregion


        public static void Init()
        {
            Dictionary<uint, SkillConfigInfo> sp = skillInfoDic;
        }
    }
}
