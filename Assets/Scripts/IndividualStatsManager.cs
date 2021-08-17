using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IndividualStatsManager : MonoBehaviour
{
    public Text name;
    public Text id;
    public Text fitness;
    public Text timeAlive;
    
    public void UpdateUi(int generationIndex, int individualIndex){
        GenerationsStats.GenerationStats.IndividualStats individual = GenerationsStats.instance.generations[generationIndex].individuals[individualIndex];
        name.text = individual.name;
        id.text = individual.id + "";
        fitness.text = individual.fitness + " fit";
        timeAlive.text = Math.Round(individual.timeAlive) + "s";
    }
}
