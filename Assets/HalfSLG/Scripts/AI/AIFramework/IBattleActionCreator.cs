using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame.AI
{
    public interface IBattleActionCreator
    {
        BattleFieldEvent CreateBattleAction();
    }
}
