using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBrain : MonoBehaviour
{
    public static int dnaLength = 6;
    public float timeAlive = 0.0f;
    public float distanceWalked = 0.0f;
    public static int fitnessOpt = 3;
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
        initPos = transform.position;
        dna = new DNA (dnaLength, 2);
        timeAlive = 0.0f;
        i = 0;
        distanceWalked = 0.0f;
        alive = true;
        Debug.Log(initPos);
    }

    public void Init (DNA Elitedna) {
        //DNA Sequence
        //0 - frente
        //1 - pulo
        initPos = transform.position;
        dnaLength = 100;
        dna = new DNA(Elitedna);
        timeAlive = 0.0f;
        i = 0;
        distanceWalked = 0.0f;
        alive = true;
        Debug.Log(initPos);
    }
    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "dead") {
            alive = false;
            PopulationManagerDino.populationCount--;
            winner = false;
            Debug.Log("dead");
            return;
        } else
        if(other.gameObject.tag == "win") {
            alive = false;
            PopulationManagerDino.populationCount--;
            winner = true;
            Debug.Log("win");

            return;
        } if(other.gameObject.tag != "decision") return;
        Debug.Log(other.gameObject.tag);
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
        timeAlive = PopulationManagerDino.elapsed;
        this.transform.Translate (0.05f,  0, 0);
        if(jump){
            Jump();
        }
        distanceWalked = transform.position.x - initPos.x;
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
