using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.Lobby
{
    public class MultiplayerMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;

        public void HostLobby()
        {
            networkManager.StartHost();
            landingPagePanel.SetActive(false); 
        }
    }
}