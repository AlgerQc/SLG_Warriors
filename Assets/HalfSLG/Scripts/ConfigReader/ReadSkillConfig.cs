using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

namespace SLGame
{
    public class SkillConfigInfo : System.Object
    {
        public uint id;
        public string name;
        public string icon;
        public string action;
        public string effect;
        public string sound;
        public string info;
    }

    class ReadSkillPassiveConfig
    {
        XmlDocument xmlDoc = null;
        public ReadSkillPassiveConfig()
        {
        }
        public ReadSkillPassiveConfig(string xmlFilePath)
        {
            //TextAsset xmlfile = Resources.Load(xmlFilePath) as TextAsset;
            ResourceUnit xmlfileUnit = ResourcesManager.Instance.loadImmediate(xmlFilePath, ResourceType.ASSET);
            TextAsset xmlfile = xmlfileUnit.Asset as TextAsset;

            if (!xmlfile)
            {
                return;
            }

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlfile.text);
            XmlNodeList infoNodeList = xmlDoc.SelectSingleNode("SkillCfg_passitive ").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {//(XmlNode xNode in infoNodeList)
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null)
                    continue;

                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                //Debug.LogError(typeName);
                SkillConfigInfo passiveInfo = new SkillConfigInfo();
                passiveInfo.id = (uint)Convert.ToUInt32(typeName);
                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    #region 搜索
                    switch (xEle.Name)
                    {
                        case "szName":
                            {
                                passiveInfo.name = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "SkillIcon":
                            {
                                passiveInfo.icon = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "n32ReleaseAction":
                            {
                                passiveInfo.action = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "n32ReleaseSound":
                            {
                                passiveInfo.sound = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "ReleaseEffect":
                            {
                                passiveInfo.effect = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "info":
                            passiveInfo.info = Convert.ToString(xEle.InnerText);
                            break;
                    }

                    #endregion
                }
                ConfigReader.skillPassiveInfoDic.Add(passiveInfo.id, passiveInfo);
                //Debug.LogError("add buff" + buffInfo.BuffID);
            }
        }
    }
}


