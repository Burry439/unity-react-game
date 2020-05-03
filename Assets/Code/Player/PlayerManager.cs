using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Networking;

namespace Project.Player{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Data handaling")]
        [SerializeField]
        private float speed = 4;

        [Header("Class Refrences")]
        [SerializeField]
        private NetworkIdentity networkIdentity;

        private void checkMovement(){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            transform.position += new Vector3(horizontal,vertical,0) * speed * Time.deltaTime;
        }

        void Update()
        {
            if(networkIdentity.IsControlling()){
                checkMovement();
            }
        }
    }
}

