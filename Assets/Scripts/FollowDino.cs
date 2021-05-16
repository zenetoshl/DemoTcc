using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDino : MonoBehaviour
{
    public Transform dino;
    
    void FixedUpdate()
    {
        if(dino.position.x > this.transform.position.x){
            this.transform.position = new Vector3(dino.position.x,  this.transform.position.y, -10);
        }
    }
}
