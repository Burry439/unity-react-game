using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utility.Attributes;
using UnitySocketIO;
using UnitySocketIO.Events;
namespace Project.Networking {

 
    public class NetworkIdentity : MonoBehaviour
    {
        [Header("Helpful Values")]
        [SerializeField]
        [GreyOut]
        private string id;
        [SerializeField]
        [GreyOut]
        private bool isControlling;

        private SocketIOController socket;
        // Start is called before the first frame update
        void Awake()
        {
            isControlling = false;
        }

        // Update is called once per frame
        public void SetControllerId(string Id)
        {
            id = Id;
            isControlling = (NetworkClient.ClientId == Id) ? true : false;
        }

        public void SetSocketRefrence(SocketIOController Socket){
            socket = Socket;
        }

        public string GetId(){
            return id;
        }

        public bool IsControlling(){
            return isControlling;
        }

        public SocketIOController GetSocket(){
            return socket;
        }
    }
}