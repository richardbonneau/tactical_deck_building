using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameras : MonoBehaviour
{
    GameObject player;

    void Awake(){
            player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        transform.LookAt(player.transform);
    }
}
