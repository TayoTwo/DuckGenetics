using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerationManager : MonoBehaviour
{
    public float mutationRate;
    public float mutationRange;
    public int startingAmount;
    public GameObject duckPrefab;
    public float generationTime;
    public float topCut;
    public float duckSpawnRange;
    public float timeScale = 1;
    [Header("Live Stats")]
    public int currentGeneration;
    public AnimationCurve totalFitnessCurve;
    public AnimationCurve averageFitnessCurve;
    public AnimationCurve maxFitnessCurve;
    public float maxFitness;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

        if(FindObjectsOfType<GenerationManager>().Length > 1){

            Destroy(gameObject);

        }

        DontDestroyOnLoad(gameObject);


        if(currentGeneration == 0){

            Initialize();

        }
    }

    void Initialize(){

        for(int i = 0; i < startingAmount;i++){

            float[] g = new float[4];

            //Run Speed
            g[0] = Random.Range(0,0.5f);
            //Turn Speed
            g[1] = Random.Range(0,0.5f);
            //Vision Angle
            g[2] = Random.Range(0,0.5f);

            Vector3 pos = new Vector3(Random.Range(-duckSpawnRange,duckSpawnRange),-0.5f,Random.Range(-duckSpawnRange,duckSpawnRange));
            Vector3 rot = Vector3.up * Random.Range(0f,360f);

            GameObject d = (GameObject)Instantiate(duckPrefab,pos,Quaternion.Euler(rot));
            Duck dObj = d.GetComponent<Duck>();
            dObj.OnSpawn(g);

        }

    }

    void NextGen(){

        currentGeneration++;

        Duck[] ducks = FindObjectsOfType<Duck>();
        List<Duck> topDucks = new List<Duck>();
        List<float[]> topCutGenomes = new List<float[]>();

        float totalFitness = 0;
        float avgFitness = 0;

        //Get the highests fitness ducks
        for(int i = 0;i < ducks.Length;i++){

            if(i/ducks.Length < topCut){

                topDucks.Add(ducks[i]);

            }

            totalFitness += ducks[i].fitness;

            if(ducks[i].fitness > maxFitness){

                maxFitness = ducks[i].fitness;

            }

        }

        //Add the top ducks to a list
        for(int i = 0;i < ducks.Length;i++){

            for(int j = 0;j < topDucks.Count;j++){

                if(ducks[i].fitness > topDucks[j].fitness && !topDucks.Exists(x => ducks[i] == x)){

                    topDucks[j] = ducks[i];

                }

            }

        }

        //Add the top ducks genomes to a list
        for(int i = 0;i < topDucks.Count;i++){

            topCutGenomes.Add(topDucks[i].genome);

        }

        //Adjust graphs
        avgFitness = totalFitness / ducks.Length;
        totalFitnessCurve.AddKey(currentGeneration,totalFitness);
        averageFitnessCurve.AddKey(currentGeneration,avgFitness);
        maxFitnessCurve.AddKey(currentGeneration,maxFitness);

        //Make children and randomly mutate genes
        for(int i = 0; i < startingAmount;i++){

            int genomeIndex = Random.Range(0,topCutGenomes.Count - 1);

            float[] g1 = topCutGenomes[genomeIndex];
            float[] g2 = topCutGenomes[genomeIndex + 1];          

            float[] childGenome = MergeGenome(g1,g2);

            //Debug.Log("PARENT 1 - " + g1[0] + "|"+ g1[1] + "|" + g1[2] + "|" + "PARENT 2 - " + g2[0] + "|"+ g2[1] + "|" + g2[2] + "CHILD - " + childGenome[0] + "|"+ childGenome[1] + "|" + childGenome[2]);

            Vector3 pos = new Vector3(Random.Range(-duckSpawnRange,duckSpawnRange),-0.5f,Random.Range(-duckSpawnRange,duckSpawnRange));
            Vector3 rot = Vector3.up * Random.Range(0f,360f);

            if(Random.value < mutationRate){

                int genToMutate = Random.Range(0,childGenome.Length);

                childGenome[genToMutate] += Random.Range(-mutationRange,mutationRange);

            }

            GameObject d = (GameObject)Instantiate(duckPrefab,pos,Quaternion.Euler(rot));
            Duck dObj = d.GetComponent<Duck>();
            dObj.OnSpawn(childGenome);

        }

    }

    float[] MergeGenome(float[] g1, float[] g2){

        int numberOfGenesFromParentOne = Random.Range(0,g1.Length-1);
        //Debug.Log(numberOfGenesFromParentOne);
        float[] genome = new float[g1.Length];

        for(int i = 0; i < numberOfGenesFromParentOne;i++){

            genome[i] = g1[i];

        }

        for(int i = numberOfGenesFromParentOne; i < g1.Length;i++){

            genome[i] = g2[i];

        }

        return genome;

    }

    // Update is called once per frame
    void Update(){

        Time.timeScale = timeScale;

        if(timer >= generationTime){

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            NextGen();
            timer = 0;
        }

        timer += Time.deltaTime;
        //Debug.Log(timer);
        
    }


}
