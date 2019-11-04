using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using SLGame.Resource;
using Object = UnityEngine.Object;

namespace SLGame
{
    public class SkillConfigInfo : System.Object
    {
        public enum BattleSkillDamageType
        {
            Physical = 1,   //物理伤害
            Move,           //位移类
            Heal,           //恢复&buff类
        }
        
        public enum BattleSkillTargetType
        {
            BattleUnit = 1, //对某一个战斗单位
            GridUnit,   //对某一个地图格子(范围)
            Self,       //以自身为中心的
        }

        public uint id;
        public string name;
        public string icon;
        public int releaseRadius;          //技能释放半径
        public int effectRadius;            //技能影响半径
        public int damageType;    //伤害类型
        public int targetType;    //目标类型
        public int mainValue;              //造成的伤害
        public string action;               //对应动画
        public string sound;                //技能音效
        public string info;                 //技能信息文本
    }

    class ReadSkillConfig
    {
        XmlDocument xmlDoc = null;

        public ReadSkillConfig()
        {
        }

        public ReadSkillConfig(string xmlFilePath)
        {
            //TextAsset xmlfile = Resources.Load(xmlFilePath) as TextAsset;

            //UtilityHelper.LogFormat("begin reading skill xml file from {0}", xmlFilePath);
            Object asset = Resources.Load(xmlFilePath);
            ResourceUnit xmlfileUnit = new ResourceUnit(null, 0, asset, null, ResourceType.ASSET);
            TextAsset xmlfile = xmlfileUnit.Asset as TextAsset;

            if (!xmlfile)
            {
                return;
            }
            UtilityHelper.Log("read skill xml successful");

            xmlDoc = new XmlDocument(); 
            xmlDoc.LoadXml(xmlfile.text);
            XmlNodeList infoNodeList = xmlDoc.SelectSingleNode("SkillCfg").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {//(XmlNode xNode in infoNodeList)
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null)
                    continue;

                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                //Debug.LogError(typeName);
                SkillConfigInfo skillInfo = new SkillConfigInfo();
                skillInfo.id = (uint)Convert.ToUInt32(typeName);
                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    #region 搜索
                    switch (xEle.Name)
                    {
                        case "Name":
                            {
                                skillInfo.name = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "SkillIcon":
                            {
                                skillInfo.icon = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "ReleaseRadius":
                            {
                                skillInfo.releaseRadius = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "EffectRadius":
                            {
                                skillInfo.effectRadius = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "DamageType":
                            {
                                skillInfo.damageType = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "TargetType":
                            {
                                skillInfo.targetType = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "mainValue":
                            {
                                skillInfo.mainValue = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "ReleaseAction":
                            {
                                skillInfo.action = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "ReleaseSound":
                            {
                                skillInfo.sound = Convert.ToString(xEle.InnerText);
                            }
                            break;

                        case "info":
                            skillInfo.info = Convert.ToString(xEle.InnerText);
                            break;
                    }

                    #endregion
                }
                ConfigReader.skillInfoDic.Add(skillInfo.id, skillInfo);
                //Debug.LogError("add buff" + buffInfo.BuffID);
            }
        }
    }
}


