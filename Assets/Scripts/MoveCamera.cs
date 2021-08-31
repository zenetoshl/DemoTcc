using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCamera : MonoBehaviour
{
    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;

    public float outerLeft = -10f;
    public float outerRight = 5f;
    public float outerUp = 5f;
    public float outerDown = 4f;
    void Start () {
        ResetCamera = Camera.main.transform.position;
    }
    void FixedUpdate () {
        if (!EventSystem.current.IsPointerOverGameObject(Input.touchCount > 0 ? Input.touches[0].fingerId : -1)) {
            if (Input.GetMouseButton (0)) {
                Diference = (Camera.main.ScreenToWorldPoint (Input.mousePosition)) - Camera.main.transform.position;
                if (Drag == false) {
                    Drag = true;
                    Origin = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                }
            } else {
                Drag = false;
            }
            Vector3 newPos = Origin - Diference;
            
            if (Drag == true) {
                Camera.main.transform.position = new Vector3 (
                    Mathf.Clamp (newPos.x, outerLeft, outerRight),
                    Mathf.Clamp (newPos.y, outerDown, outerUp), -10);
            }
             
            //RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
            if (Input.GetMouseButton (1)) {
                Camera.main.transform.position = ResetCamera;
            }
        }
    }
}
