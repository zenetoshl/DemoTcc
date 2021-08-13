using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBrain : MonoBehaviour
{
    public static int dnaLength = 6;
    public float timeAlive = 0.0f;
    public float bornTime = 0.0f;
    public float distanceWalked = 0.0f;
    private float oldDistanceWalked = 0.0f;
    private int frameCount = 0;
    public static int fitnessOpt = 3;
    public float timeLimit = 15f;
    public DNA dna;
    bool alive = true;
    public bool winner = false;
    int i = 0;
    bool canJump = false;
    bool jump = false;
    Vector3 initPos;
    public Animator animator;

    public void Init () {
        //DNA Sequence
        //0 - frente
        //1 - pulo
        ToggleBrain(false);
        initPos = transform.position;
        dna = new DNA (dnaLength, 2);
        timeAlive = 0.0f;
        i = 0;
        distanceWalked = 0.0f;
    }

    public void Init (DNA Elitedna) {
        //DNA Sequence
        //0 - frente
        //1 - pulo
        ToggleBrain(false);
        initPos = transform.position;
        dnaLength = 100;
        dna = new DNA(Elitedna);
        timeAlive = 0.0f;
        i = 0;
        distanceWalked = 0.0f;
    }

    public void ToggleBrain(bool b){
        alive = b;
        this.transform.GetComponent<SpriteRenderer>().enabled = b;
        if(b){
            bornTime = PopulationManagerDino.elapsed;
        }
    }
    private void Die(){
        alive = false;
        if(winner){
            PopulationManagerDino.noPenalty++;
        }
        PopulationManagerDino.populationDead++;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "dead") {
            winner = false;
            Die();
            return;
        } else
        if(other.gameObject.tag == "win") {
            winner = true;
            Die();
            return;
        } if(other.gameObject.tag != "decision") return;
        float turn = 0;
        switch (dna.GetGene (i)) {
            case 0:
                FowardRun();
                break;
            case 1:
                Jump();
                break;
        }
        i++;
        i = i % dnaLength;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // allow jumping again
        canJump = true;
        animator.SetBool("New Bool", false);
    }
 
    private void OnCollisionExit2D(Collision2D col)
    {
        canJump = false;
    }

    private void FowardRun(){
    }

    private void Jump(){
        jump = true;
        if(canJump){
            animator.SetBool("New Bool", true);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5), ForceMode2D.Impulse);
            jump = false;
        }
    }

    private void FixedUpdate () {
        if (!alive) return;
        timeAlive = PopulationManagerDino.elapsed - bornTime;
        if(timeAlive >= timeLimit){
            Die();
        }
        this.transform.Translate (0.05f,  0, 0);
        if(jump){
            Jump();
        }
        distanceWalked = transform.position.x - initPos.x;

        //matar individuos que ficaram parados
        frameCount = (frameCount + 1) % 10;
        if(frameCount == 0){
            if(distanceWalked - oldDistanceWalked <= 0.05f){
                winner = false;
                Die();
            } else {
                oldDistanceWalked = distanceWalked;
            }
        }
    }

    public float CalculateFitness(){
        switch (fitnessOpt)
        {
            case 1:
                return CalculateFitness1();
            case 2:
                return CalculateFitness2();
            case 3:
                return CalculateFitness3();
            default:
                return CalculateFitness3();
        }
    }

    public float CalculateFitness1(){
        return (distanceWalked);
    }

    public float CalculateFitness2(){
        if(winner){
            return (distanceWalked);
        } else{
            return ((distanceWalked - timeAlive));
        }
    }

    public float CalculateFitness3(){
        return ((distanceWalked / timeAlive) + (winner ? 10 : 0));
    }
}
