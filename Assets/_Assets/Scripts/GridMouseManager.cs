using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMouseManager : MonoBehaviour
{
    public Camera camera;
    public GameObject mapSelector;
    // Start is called before the first frame update
    void Start()
    {
        
    
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
            mapSelector.transform.position = new Vector3(Mathf.Round(hit.point.x),mapSelector.transform.position.y,Mathf.Round(hit.point.z));
            print(hit.point);
        } else mapSelector.transform.position = new Vector3(999,mapSelector.transform.position.y,999);
    }
}
