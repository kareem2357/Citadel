using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class menuEnemySpawner : MonoBehaviour {


    public bool isSpawning;

    public GameObject enemyPrefab;

    public float xRange;

    public float lastDifficultyUpadteTime = 0;
    public float totalElapsedTime = 0;
    public float difficultyDelta;
    public float initialDelay;
    public float speedFactor = 1;



    // Use this for initialization
    void Start () {


       
        StartSpawning();

    }
	
	// Update is called once per frame
	void Update () {
        totalElapsedTime += Time.deltaTime;

        if(totalElapsedTime > initialDelay)
        { 

        if (totalElapsedTime > lastDifficultyUpadteTime + difficultyDelta)
        {
            lastDifficultyUpadteTime = totalElapsedTime;
              

        }

        }
    }



    

    public void StartSpawning()
    {

        //TODO this should be changed 
    

        isSpawning = true;
        Spawn();
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
    

    void Spawn()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        { 

        bool shouldSpawn = true;

      
        if (isSpawning && shouldSpawn)
        {
            GameObject enemy = GameObject.Instantiate(enemyPrefab);
            enemy.transform.position = new Vector2(-5f, 15f);

            // Set Y anywhere in range, this the ground level for this NPC
         //   enemy.transform.position += (Vector3.up * 25f);
            // Set Y anywhere in range, this the ground level for this NPC
            enemy.transform.position += (Vector3.right * UnityEngine.Random.Range(0 - xRange, xRange));

            
            
        }

        LeanTween.delayedCall(UnityEngine.Random.Range(0.5f, 2), () =>
        {
            Spawn();
        });
    }
    }


}
