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
                foreach (XmlElement xmlElement in mapMsgList)
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
                GridUnit gridUnit = battleMap.mapGrids[posX, posY];
                gridUnit.column = posX;
                gridUnit.row = posY;
                gridUnit.m_GridAttribute = gridAttribute;
            }

            return battleMap;
        }
    }
}
