using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLGame
{
    public class lobbyScene : MonoBehaviourSingleton<lobbyScene>
    {
        [SerializeField] private Button startBtn;

        public void Start()
        {
            UtilityHelper.Log("go into lobby awake");
            InitUIObjects();
        }

        public void InitUIObjects()
        {
            startBtn.gameObject.SetActive(true);
            startBtn.onClick.AddListener(ClickStart);
        }

        private void ClickStart()
        {
            EventManager.Instance.Run(EGameConstL.EVENT_LOBBY_CLICK_GAME_START, null);
        }
    }
}
