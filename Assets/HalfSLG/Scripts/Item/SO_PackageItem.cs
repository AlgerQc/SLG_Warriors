using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{
    public enum PackageItemType
    {
        Recover,
    }

    [CreateAssetMenu(menuName = "Item")]
    public class SO_PackageItem
        : ScriptableObject
    {
        public int itemID;
        public string itemName;
        public Sprite icon;
        public PackageItemType itemType;
        public int hpRecovery;
        public int energyRecovery;
    }
}