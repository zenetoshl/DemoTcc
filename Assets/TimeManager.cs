using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float fixedDeltaTime;
    public float timescale;
    // Start is called before the first frame update
    private void Awake() {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }
    void Start()
    {
        Time.timeScale = timescale;
        Time.fixedDeltaTime = Time.fixedDeltaTime / timescale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
