using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using SLGame.Resource;
using Object = UnityEngine.Object;

namespace SLGame
{
    class ReadScene1Location
    {
        public ReadScene1Location()
        {
        }

        public static GridUnit[] ReadScene1LocationFromXml(string xmlFilePath, int teamKind)
        {
            GridUnit[] bornGrids = null;
            XmlDocument xmlDoc = null;
            //TextAsset xmlfile = Resources.Load(xmlFilePath) as TextAsset;

            //UtilityHelper.LogFormat("begin reading skill xml file from {0}", xmlFilePath);
            Object asset = Resources.Load(xmlFilePath);
            ResourceUnit xmlfileUnit = new ResourceUnit(null, 0, asset, null, ResourceType.ASSET);
            TextAsset xmlfile = xmlfileUnit.Asset as TextAsset;

            if (!xmlfile)
            {
                return bornGrids;
            }
            UtilityHelper.Log("read scene 1 location xml successful");

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlfile.text);
            XmlNodeList infoNodeList = xmlDoc.SelectSingleNode("SkillCfg").ChildNodes;
            for (int i = 0; i < infoNodeList.Count; i++)
            {//(XmlNode xNode in infoNodeList)
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null)
                    continue;

                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                int id = (int)Convert.ToUInt32(typeName);

                if (id != teamKind)
                    continue;

                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    switch (xEle.Name)
                    {
                        case "row":
                            {
                                
                            }
                            break;

                        case "column":
                            {
                                //skillInfo.icon = Convert.ToString(xEle.InnerText);
                            }
                            break;                     
                    }
                }
            }
            return bornGrids;
        }
    }
}



