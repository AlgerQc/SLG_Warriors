﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using SLGame.Resource;
using Object = UnityEngine.Object;

namespace SLGame
{
    public class ComboConfigInfo : System.Object
    {
        public int id;
        public string name;
        public int skill1;
        public int skill2;
        public int skill3;
        public int effect;
    }

    class ReadSkillComboConfig
    {
        public ReadSkillComboConfig()
        {
        }

        public static Dictionary<int, ComboConfigInfo> ReadSkillComboConfigFromXml(string xmlFilePath)
        {
            //UtilityHelper.Log("begin reading combo xml file");
            //TextAsset xmlfile = Resources.Load(xmlFilePath) as TextAsset;

            XmlDocument xmlDoc = null;
            Dictionary<int, ComboConfigInfo> comboInfoDic = new Dictionary<int, ComboConfigInfo>();
            Object asset = Resources.Load(xmlFilePath);
            ResourceUnit xmlfileUnit = new ResourceUnit(null, 0, asset, null, ResourceType.ASSET);
            TextAsset xmlfile = xmlfileUnit.Asset as TextAsset;

            if (!xmlfile)
            {
                return comboInfoDic;
            }
            UtilityHelper.Log("read combo xml successful");
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlfile.text);
            XmlNodeList infoNodeList = xmlDoc.SelectSingleNode("ComboSkill").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {//(XmlNode xNode in infoNodeList)
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null)
                    continue;

                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                //Debug.LogError(typeName);
                ComboConfigInfo comboInfo = new ComboConfigInfo();
                comboInfo.id = (int)Convert.ToUInt32(typeName);
                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    #region 搜索
                    switch (xEle.Name)
                    {
                        case "Name":
                            {
                                comboInfo.name = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "skill1":
                            {
                                comboInfo.skill1 = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "skill2":
                            {
                                comboInfo.skill2 = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "skill3":
                            {
                                comboInfo.skill3 = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "effect":
                            {
                                comboInfo.effect = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                    }

                    #endregion
                }
                comboInfoDic.Add(i, comboInfo);
                //UtilityHelper.LogFormat("add {0} with skill1 = {1}, skill2 = {2}, skill3 = {3}", comboInfo.id, comboInfo.skill1,
                    //comboInfo.skill2, comboInfo.skill3);
            }

            return comboInfoDic;
        }
    }
}


