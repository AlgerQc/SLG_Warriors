using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{

    public class BattleMapManager
        : SingDicMgr<BattleMapManager, BattleMapData>, SLGBase
    {
        public string Desc()
        {
            return string.Empty;
        }

        public void Init(params object[] args)
        {
            Helper.Log("Battle map manager inited.");
        }

        public BattleMapData CreateMap(int width, int height, int obstacleCount, int obstacleGap)
        {
            BattleMapData battleMapData = null;
            int mapID = 0;
            base.Create(out battleMapData, out mapID);
            if (battleMapData != null)
            {
                battleMapData.mapID = mapID;
                battleMapData.Generate(width, height, obstacleCount, obstacleGap);
            }
            else
            {
                Helper.LogError(string.Format("Create map failed->width:{0},height:{1}",
                    width, height));
            }
            return battleMapData;
        }
    }
}