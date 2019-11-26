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

        public static GridUnit[] ReadScene1LocationFromXml(BattleMap map, string xmlFilePath, int teamKind, int gridCount)
        {
            GridUnit[] bornGrids = new GridUnit[gridCount];
            GridUnit tmpGrid = new GridUnit(map, 0, 0);
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
            XmlNodeList infoNodeList = xmlDoc.SelectSingleNode("Locations").ChildNodes;
          
            for (int i = 0; i < infoNodeList.Count; i++)
            {//(XmlNode xNode in infoNodeList)
                if ((infoNodeList[i] as XmlElement).GetAttributeNode("un32ID") == null)
                    continue;

                string typeName = (infoNodeList[i] as XmlElement).GetAttributeNode("un32ID").InnerText;
                int id = (int)Convert.ToUInt32(typeName);

                if (id != teamKind)
                {                    
                    continue;
                }

                foreach (XmlElement xEle in infoNodeList[i].ChildNodes)
                {
                    switch (xEle.Name)
                    {
                        case "row":
                            {
                                tmpGrid.row = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "column":
                            {
                                tmpGrid.column = Convert.ToInt32(xEle.InnerText);
                            }
                            break;                     
                    }
                }
                if (teamKind == 0)
                {
                    bornGrids[i] = tmpGrid;
                }
                else if (teamKind == 1)
                {
                    bornGrids[i - 1] = tmpGrid;
                }

            }
            return bornGrids;
        }
    }
}



