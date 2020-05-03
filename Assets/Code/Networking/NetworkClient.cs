using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utility;
using UnityEngine.UI;
using UnitySocketIO;
using UnitySocketIO.Events;
using System.Runtime.InteropServices;
using System;
using UnityEngine.SceneManagement;

namespace Project.Networking {

    //ws://127.0.0.1:52300/socket.io/?EIO=4&transport=websocket
    public class NetworkClient : MonoBehaviour
    {

        public SocketIOController io;


        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;
        [SerializeField]
        private GameObject playerPrefab;
        public static string ClientId;
        private Dictionary<string, NetworkIdentity> serverObjects;


        public void leaveGame(){
            Debug.Log("in leave game");
            io.Close();
        }

        // Start is called before the first frame update
        public void Start()
        {
            initialize();
            setupEvents();
        }

        private void initialize(){
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }

        // Update is called once per frame
        public  void Update()
        {
           // base.Update();
        }

        // Import the JSLib as following. Make sure the
        // names match with the JSLib file we've just created.

        private void setupEvents()
        {
            io.On("connect", (e) => {       
                io.Emit("addUnityUser");   
            }); 

            io.On("register", (e) => {
                Player player = JsonUtility.FromJson<Player>(e.data);
                ClientId = player.id;
                Debug.LogFormat("pur client Id", ClientId);
            });

            io.On("spawn", (SocketIOEvent e) => {
                Player newPlayer = JsonUtility.FromJson<Player>(e.data);          
                GameObject go  = Instantiate(playerPrefab,networkContainer);
                go.name = string.Format("Player ({0})", newPlayer.id);
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                ni.SetControllerId(newPlayer.id);
                ni.SetSocketRefrence(io);
                serverObjects.Add(newPlayer.id, ni);
              
            });

            io.On("updatePosition",(e) =>{
                Player player = JsonUtility.FromJson<Player>(e.data);
                string id = player.id;
                float x = player.position.x;
                float y = player.position.y;
                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector3(x,y,0);
            });

            // gets called if someone is playing on a difrent tab or browser
            io.On("duplicatePlayer", (e) =>{
                Debug.Log("UNITY : duplicatePlayer");
                io.Close();              
                SceneManager.LoadScene("test");
             });

            // gets called if someone signout of the react client
            io.On("disconnectFromReact", (e) =>{
                 Debug.Log("UNITY : disconnectFromReact");              
                 Player player = JsonUtility.FromJson<Player>(e.data);
                 Debug.Log(player);
                 string id = player.id; 
                 GameObject go = serverObjects[id].gameObject;
                 Destroy(go);
                 serverObjects.Remove(id);
             });

            //gets called if someonecloses a tab
            io.On("disconnected", (e) =>{
               Debug.Log("UNITY : disconnect");
                Player player = JsonUtility.FromJson<Player>(e.data);
                Debug.Log(e.data);
                string id = player.id;
                
                GameObject go = serverObjects[id].gameObject;
                Destroy(go);
                serverObjects.Remove(id);
            });

             io.Connect(); 
        }
    }

    [Serializable]
    public class Player {
        public string id;
        public string username;
        public Position position;
    }
    [Serializable]
    public class Position {
        public float x;
        public float y;
    }
}
