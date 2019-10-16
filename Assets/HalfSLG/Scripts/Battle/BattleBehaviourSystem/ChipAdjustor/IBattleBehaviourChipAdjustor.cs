//用于调节chip的结果
using System.Collections.Generic;

namespace SLGame.BattleBehaviourSystem
{
    public interface IBattleBehaviourChipAdjustor 
    {
        void AdjustBehaviourItem(List<BattleBehaviourItem> behaviourList);
    }
}