using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SLGame.AI
{
    public abstract class ConditionDescriptorBase
    {
        [SerializeField]
        protected CustomParamSet customParamSet;


        public abstract bool JudgeCondition(Brain brain);

    }
}
