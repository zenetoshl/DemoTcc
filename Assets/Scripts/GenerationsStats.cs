using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationsStats : MonoBehaviour
{
    public class GenerationStats
    {
        public class IndividualStats
        {
            public float fitness;
            public float timeAlive;
            public string name;
            public int id;

            public IndividualStats(DNA individual, float fitness, float timeAlive){
                this.fitness = fitness;
                this.timeAlive = timeAlive;
                this.name = individual.name;
                this.id = individual.id;
            }
            
        }


        public List<IndividualStats> individuals;
        public int generation;
        public float fitnessMean { 
            get {
                float total = 0f;
                foreach( IndividualStats individual in individuals){
                    total += individual.fitness;
                }
                return total / individuals.Count;
        }}
        public int noPenalty;

        public GenerationStats (int generation, int noPenalty){
            this.generation = generation;
            this.noPenalty = noPenalty;
            individuals = new List<IndividualStats>();
        }

        public void AddIndividual(DNA individual, float fitness, float timeAlive){
            individuals.Add(new IndividualStats(individual, fitness, timeAlive));
        }
    }

    static GenerationsStats _instance;
    public static GenerationsStats instance {
        get {
            if (!_instance) {
                //first try to find one in the scene
                _instance = FindObjectOfType<GenerationsStats> ();

                if (!_instance) {
                    //if that fails, make a new one
                    GameObject go = new GameObject ("_GenerationsStats");
                    _instance = go.AddComponent<GenerationsStats> ();

                    if (!_instance) {
                        //if that still fails, we have a big problem;
                        Debug.LogError ("Fatal Error: could not create GenerationsStats");
                    }
                }
            }

            return _instance;
        }
    }
    
    public List<GenerationStats> generations = new List<GenerationStats>();
    public int maxIndex = -1;
    private int _index = 0;
    public int nextIndex { get { 
            if(_index >= maxIndex) {_index = maxIndex; return _index;}
            else {_index += 1; return _index;}
        }} 
    public int previousIndex { get { 
            if(_index <= 0) { _index += 0; return _index;}
            else {_index -= 1; return _index;}
        }}
        public int index { get { 
            return _index;
        }}
    public void SetMaxIndex(){
        _index = maxIndex;
    }
        
    public int CreateNewGeneration(int noPenalty){
        generations.Add(new GenerationStats(maxIndex+1, noPenalty));
        maxIndex += 1;
        return maxIndex;
    }

}
