using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 targetPosition;
   
    // Update is called once per frame
    void Update () {
 
        if(Input.GetKeyDown(KeyCode.Mouse0))
           {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
 
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
    }
}
