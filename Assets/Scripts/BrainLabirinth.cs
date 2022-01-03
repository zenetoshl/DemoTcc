using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainLabirinth : Brain {
    
    private Vector3 direction = new Vector3(0, .1f, 0);

    public override void Init () {
        ToggleBrain(false);
        sprite.enabled = true;
        initPos = transform.position;
        dna = new DNA (dnaLength, 5);
        timeAlive = 0.0f;
        i = 1;
        distanceWalked = 0.0f;
    }

    public override void Init (DNA Elitedna, float fitness = 0) {
        ToggleBrain(false);
        sprite.enabled = true;
        initPos = transform.position;
        dnaLength = 100;
        dna = new DNA(Elitedna);
        timeAlive = 0.0f;
        i = 1;
        distanceWalked = 0.0f;
    }

    public override void ToggleBrain(bool b){
        alive = b;
        this.transform.GetComponent<SpriteRenderer>().enabled = b;
        if(b){
            bornTime = PopulationManager.elapsed;
        }
    }

    public override void Die(){
        alive = false;
        PopulationManager.populationDead++;
    }

    public override void SetColors(){
        Color color =  new Color(dna.r, dna.g, dna.b, 1);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "dead") {
            Die();
            winner = false;
        } else
        if(other.gameObject.tag == "win") {
            Die();
            PopulationManager.noPenalty++;
            sprite.enabled = false;
            winner = true;
        } else
        if(other.gameObject.tag != "decision") return;
        switch (dna.GetGene (i)) {
            case 1:
                direction = new Vector3(.1f, 0, 0);
                sprite.flipX = false;
                break;
            case 2:
                direction = new Vector3(0, .1f, 0);
                break;
            case 3:
                direction = new Vector3(-0.1f, 0, 0);
                sprite.flipX = true;
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
        timeAlive = PopulationManager.elapsed - bornTime;
        if(timeAlive >= timeLimit){
            Die();
        }
        this.transform.Translate (direction);
        distanceWalked = transform.position.z - initPos.z;
    }

    public override float CalculateFitness1(){
        return (distanceWalked * timeAlive);
    }

    public override float CalculateFitness2(){
        if(winner){
            return (distanceWalked + timeAlive);
        } else{
            return (-(distanceWalked + timeAlive));
        }
    }

    public override float CalculateFitness3(){
        return ((distanceWalked / timeAlive) + (winner ? 8 : 0));
    }
}