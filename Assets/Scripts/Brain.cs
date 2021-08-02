using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
    public static int dnaLength = 6;
    public float timeAlive = 0.0f;
    public float bornTime = 0.0f;
    public float distanceWalked = 0.0f;
    public static int fitnessOpt = 3;
    public float timeLimit = 15f;
    public DNA dna;
    public GameObject eyes;
    bool alive = true;
    public bool winner = false;
    private Vector3 direction = new Vector3(0, .1f, 0);
    int i = 0;
    Vector3 initPos;

    public void Init () {
        //DNA Sequence
        //0 - frente
        //1 - direita
        //2 - tras
        //3 - esquerda
        ToggleBrain(false);
        initPos = transform.position;
        dna = new DNA (dnaLength, 5);
        timeAlive = 0.0f;
        i = 1;
        distanceWalked = 0.0f;
    }

    public void Init (DNA Elitedna) {
        //DNA Sequence
        //0 - frente
        //1 - direita
        //2 - tras
        //3 - esquerda
        ToggleBrain(false);
        initPos = transform.position;
        dnaLength = 100;
        dna = new DNA(Elitedna);
        timeAlive = 0.0f;
        i = 1;
        distanceWalked = 0.0f;
    }

    public void ToggleBrain(bool b){
        alive = b;
        this.transform.GetComponent<SpriteRenderer>().enabled = b;
        if(b){
            bornTime = PopulationManager.elapsed;
        }
    }

    private void Die(){
        alive = false;
        PopulationManager.populationDead++;
    }

    public void SetColors(){
        Color color =  new Color(dna.r, dna.g, dna.b, 1);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "dead") {
            Die();
            winner = false;
        } else
        if(other.gameObject.tag == "win") {
            Die();
            winner = true;
        } else
        if(other.gameObject.tag != "decision") return;
        switch (dna.GetGene (i)) {
            case 1:
                direction = new Vector3(.1f, 0, 0);
                break;
            case 2:
                direction = new Vector3(0, .1f, 0);
                break;
            case 3:
                direction = new Vector3(-0.1f, 0, 0);
                break;
            case 4:
                direction = new Vector3(0, -0.1f, 0);
                break;
        }
        if(i >= dnaLength - 2){
            i = 0;
        }
        i++;
    }

    private void FixedUpdate () {
        if (!alive) return;
        timeAlive = PopulationManager.elapsed;
        if(timeAlive >= timeLimit){
            Die();
        }
        this.transform.Translate (direction);
        distanceWalked = transform.position.z - initPos.z;
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
        return (distanceWalked * timeAlive);
    }

    public float CalculateFitness2(){
        if(winner){
            return (distanceWalked + timeAlive);
        } else{
            return (-(distanceWalked + timeAlive));
        }
    }

    public float CalculateFitness3(){
        return ((distanceWalked / timeAlive) + (winner ? 8 : 0));
    }
}