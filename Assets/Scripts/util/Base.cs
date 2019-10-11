using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{
    public interface SLGBase
    {
        void Init(params System.Object[] args);
        string Desc();
    }
}