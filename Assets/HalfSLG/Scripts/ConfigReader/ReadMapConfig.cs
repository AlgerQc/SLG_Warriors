using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace SLGame
{
    class ReadMapConfig
    {
        public ReadMapConfig() { }

        public static BattleMap ReadBattleMapFromXML(string xmlFilePath)
        {
            TextAsset xmlfile = Resources.Load(xmlFilePath) as TextAsset;
            if (!xmlfile)
            {
                UtilityHelper.LogFormat("Load {0} xml file failed!", xmlFilePath);
                return null;
            }

            int mapWidth = 0;
            int mapHeight = 0;
            GridUnit[,] gridMap = null;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlfile.text);
            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("MapBaseMsg").ChildNodes;
            XmlNodeList mapMsgList = null;
            foreach (XmlElement xmlElement in xmlNodeList)
            {
                switch (xmlElement.Name)
                {
                    case ("Width"):
                        mapWidth = Convert.ToInt32(xmlElement.InnerText);
                        break;
                    case ("Height"):
                        mapHeight = Convert.ToInt32(xmlElement.InnerText);
                        break;
                    case ("GridMsg"):
                        mapMsgList = xmlElement.ChildNodes;
                        break;
                }
            }

            BattleMap battleMap = new BattleMap(mapWidth, mapHeight);

            foreach (XmlElement gridUnitInfo in mapMsgList)
            {
                int posX = 0, posY = 0;
                GridAttribute gridAttribute = new GridAttribute();
                foreach (XmlElement xmlElement in gridUnitInfo.ChildNodes)
                {
                    switch (xmlElement.Name)
                    {
                        case ("PosX"):
                            posX = Convert.ToInt32(xmlElement.InnerText);
                            break;
                        case ("PosY"):
                            posY = Convert.ToInt32(xmlElement.InnerText);
                            break;
                        case ("Name"):
                            gridAttribute.m_Name = Convert.ToString(xmlElement.InnerText);
                            break;
                        case ("Avoid"):
                            gridAttribute.m_Avoid = Convert.ToSingle(xmlElement.InnerText);
                            break;
                        case ("Defense"):
                            gridAttribute.m_Defense = Convert.ToSingle(xmlElement.InnerText);
                            break;
                        case ("Height"):
                            gridAttribute.m_Height = Convert.ToSingle(xmlElement.InnerText);
                            break;
                        case ("MaxPassVolume"):
                            gridAttribute.m_MaxPassVolume = Convert.ToSingle(xmlElement.InnerText);
                            break;
                        case ("CrossCost"):
                            gridAttribute.m_CrossCost = Convert.ToSingle(xmlElement.InnerText);
                            break;
                        case ("GridType"):
                            gridAttribute.SetGridType(Convert.ToString(xmlElement.InnerText));
                            break;
                    }
                }
                GridUnit gridUnit = battleMap.mapGrids[posX - 1, posY - 1];
                if (gridUnit == null)
                {
                    gridUnit = new GridUnit(battleMap, posY - 1, posX - 1);
                    battleMap.mapGrids[posX - 1, posY - 1] = gridUnit;
                }
                gridUnit.column = posX - 1;
                gridUnit.row = posY - 1;
                gridUnit.m_GridAttribute = gridAttribute;
                gridUnit.localPosition = new Vector3(posX * EGameConstL.Map_GridWidth, -posY * EGameConstL.Map_GridOffsetY, 0);

                if (gridUnit.m_GridAttribute.m_GridType == GridType.Normal)
                {
                    battleMap.normalGrids.Add(gridUnit);
                }
                else if (gridUnit.m_GridAttribute.m_GridType == GridType.Obstacle)
                {
                    battleMap.obstacleGrids.Add(gridUnit);
                }
            }

            return battleMap;
        }
    }
}
