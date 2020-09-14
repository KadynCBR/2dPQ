using Mirror;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Multiplayer.Lobby
{
    public class NetworkManagerLobby : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;
        [Scene] [SerializeField] private string menuScene = string.Empty;

        [Header("Room")]
        [SerializeField] private RoomPlayerLobby roomPlayerPrefab = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        public List<RoomPlayerLobby> RoomPlayers { get; } = new List<RoomPlayerLobby>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }
            if (SceneManager.GetActiveScene().name != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if (SceneManager.GetActiveScene().name == menuScene)
            {
                bool isLeader = RoomPlayers.Count == 0;
                RoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.IsLeader = isLeader;
                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<RoomPlayerLobby>();

                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }
            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        private bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) { return false; }
            foreach (var player in RoomPlayers)
            {
                if (!player.IsReady) { return false; }
            }
            return true;
        }
    }
}