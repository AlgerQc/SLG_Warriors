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
            int x = 0, y = 0;
            //GridUnit tmpGrid = new GridUnit(map, 0, 0);
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
                                x = Convert.ToInt32(xEle.InnerText);
                            }
                            break;

                        case "column":
                            {
                                y = Convert.ToInt32(xEle.InnerText);
                            }
                            break;                     
                    }
                }

                //tmpGrid.localPosition = new Vector3((tmpGrid.column + 1)* EGameConstL.Map_GridWidth, -(tmpGrid.row + 1)* EGameConstL.Map_GridOffsetY, 0);
                //Debug.LogFormat("i = {0}, row = {1}, column = {2}, position = {3}", i, tmpGrid.row, tmpGrid.column, tmpGrid.localPosition);
                Debug.LogFormat("i = {0}, row = {1}, column = {2}, position = {3}", i, x, y, map.mapGrids[x, y].localPosition);
                if (teamKind == 0)
                {
                    bornGrids[i] = map.mapGrids[x, y];
                }
                else if (teamKind == 1)
                {
                    bornGrids[i - 1] = map.mapGrids[x, y];
                }

            }
            return bornGrids;
        }
    }
}



