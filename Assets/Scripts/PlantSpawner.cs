using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{

    public GameObject plantPrefab;
    public bool runStarted;
    public int maxSpawnAmount;
    public float spawnRange;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < maxSpawnAmount; i++){

            SpawnPlant();

        }
        
    }

    void SpawnPlant(){

        Vector3 pos = new Vector3(Random.Range(-spawnRange,spawnRange),-0.5f,Random.Range(-spawnRange,spawnRange));
        Vector3 rot = Vector3.up * Random.Range(0f,360f);

        Instantiate(plantPrefab,pos, Quaternion.Euler(rot));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
