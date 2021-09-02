using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipController : MonoBehaviour
{
    public static int dnaLength = 300;
    public bool winner = false;
    public int turnAngleLimit = 45;
    public int maxFrameLimit = 9; //plus 1
    public float timeLimit = 15f;
    public float timeAlive = 0.0f;
    public float distanceWalked = 0.0f;
    public static int fitnessOpt = 1;
    public int maxSpeed = 25; //divide por 100 portanto é 0.3f
    public DNA dna;
    public float bornTime = 0.0f;
    bool alive = true;
    public List<int> goalsId; 
    int i = 0;
    int frame = 0;
    bool canJump = false;
    bool jump = false;
    public float fitness = 0.0f; 
    Vector3 initPos;
    public Animator animator;

    public float speed
    {
        get { return (float) ((dna.genes[dnaLength] + 5) / (turnAngleLimit/maxSpeed)) / 100; }
    }

    public float frameLimit
    {
        get { return (float) (dna.genes[dnaLength + 1] / (turnAngleLimit/maxFrameLimit)) + 1; }
    }

    public void Init () {
        ToggleBrain(false);
        goalsId = new List<int>();
        initPos = transform.position;
        dna = new DNA (dnaLength, turnAngleLimit, maxSpeed, maxFrameLimit);
        timeAlive = 0.0f;
        frame = 0;
        i = 0;
        distanceWalked = 0.0f;
        winner = false;
    }

    public void Init (DNA Elitedna, float fitness) {
        //DNA Sequence
        //0 - frente
        //1 - pulo
        ToggleBrain(false);
        Debug.Log("init elite: " + fitness);
        initPos = transform.position;
        dna = new DNA(Elitedna);
        goalsId = new List<int>();
        timeAlive = 0.0f;
        frame = 0;
        i = 0;
        distanceWalked = 0.0f;
        winner = false;
    }

    private void Die(){
        alive = false;
        fitness = CalculateFitness1();
        PopulationManagerShip.populationDead++;
        if(PopulationManagerShip.instance.minOrbs <= goalsId.Count){
            winner = true;
        }
        if(winner){
            PopulationManagerShip.noPenalty++;
        }
    }

    public void ToggleBrain(bool b){
        alive = b;
        this.transform.GetComponent<SpriteRenderer>().enabled = b;
        if(b){
            bornTime = PopulationManagerShip.elapsed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!alive) return;
        if(other.gameObject.tag == "dead") {
            Die();
            return;
        } else
        if(other.gameObject.tag == "win") {
            int id = int.Parse(other.gameObject.name);
            if(!goalsId.Contains(id)){
                goalsId.Add(id);
            }
            return;
        }
        
    }


    private void FixedUpdate () {
        if (!alive) return;
        timeAlive = PopulationManagerShip.elapsed - bornTime;
        if(timeAlive >= timeLimit){
            Die();
        }
        frame = frame + 1;
        if(frame >= frameLimit){
            float rot = (this.transform.rotation.z + dna.genes[i] - (turnAngleLimit/2)) % 360;
            this.transform.Rotate (0,  0, rot);
            i++;
            i = i % dnaLength;
        }
        this.transform.Translate (0,  speed, 0);
        distanceWalked += speed;
        frame = frame %  (int) frameLimit;
    }

    public float CalculateFitness(){
        switch (fitnessOpt)
        {
            case 1:
                return CalculateFitness1();
            case 2:
                return CalculateFitness1();
            case 3:
                return CalculateFitness1();
            default:
                return CalculateFitness1();
        }
    }

    public float CalculateFitness1(){
        return (goalsId.Count);
    }

    public float CalculateFitness2(){
        return ((distanceWalked - timeAlive));
    }

    public float CalculateFitness3(){
        return ((distanceWalked / timeAlive) + (10));
    }
}
