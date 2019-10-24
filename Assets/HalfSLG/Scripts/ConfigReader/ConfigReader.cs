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
                if (SkillInfoDic.Count == 0)
                {
                    ReadSkillConfig spConfig = new ReadSkillConfig("Config/SkillCfg");
                }
                return SkillInfoDic;
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

        public static void Init()
        {
            Dictionary<uint, SkillConfigInfo> sp = skillInfoDic;
        }
    }
}
