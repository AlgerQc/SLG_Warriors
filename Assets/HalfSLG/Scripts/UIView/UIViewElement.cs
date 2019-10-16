using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{
    public abstract class UIViewElement 
        : MonoBehaviour
    {
        protected abstract void UpdateElement();
    }
}