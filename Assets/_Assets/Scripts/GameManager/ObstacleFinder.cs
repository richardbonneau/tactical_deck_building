using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFinder : MonoBehaviour
{
    void FixedUpdate(){
        print("a "+transform.position);
       

    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col){
        print(transform.position);
        print("col "+col);
        print("-----------");
    }
}
