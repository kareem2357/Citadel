using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public bool isSpawning;

    public GameObject enemyPrefab;

    public float yRange;
    public float enemyCap;

    public float lastDifficultyUpadteTime = 0;
    public float totalElapsedTime = 0;
    public float difficultyDelta;

    public float speedFactor = 1;

    void Start()
    {

    }

    public void StartSpawning()
    {
        isSpawning = true;
        Spawn();
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    void Spawn()
    {
        bool shouldSpawn = true;

        // Too many enemies?
        if (GameEngine.GetInstance().npcs.Where((npc) => { return npc.alignment == NPC.Alignment.Hostile; }).Count() > (int)enemyCap)
            shouldSpawn = false;

        if (isSpawning && shouldSpawn)
        {
            GameObject enemy = GameObject.Instantiate(enemyPrefab);
            enemy.transform.position = this.transform.position;

            // Set Y anywhere in range, this the ground level for this NPC
            enemy.transform.position += Vector3.up * UnityEngine.Random.Range(0 - yRange, yRange);

            // Activate NPC to lock ground level and start behavior
            enemy.GetComponent<NPC>().setInGame(true);

            enemy.GetComponent<NPC>().speedFactor = 1.0f + UnityEngine.Random.Range(0, speedFactor / 10);
        }

        LeanTween.delayedCall(UnityEngine.Random.Range(1, 2), () =>
        {
            Spawn();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            totalElapsedTime += Time.deltaTime;

            if (totalElapsedTime > lastDifficultyUpadteTime + difficultyDelta)
            {
                lastDifficultyUpadteTime = totalElapsedTime;
                if(totalElapsedTime <40f)
                {
                    enemyCap = enemyCap + 0.25f;
                }
                else
                {
                    enemyCap = enemyCap + 0.1f;
                }
                
                speedFactor = speedFactor + 0.03f;
            }
        }
    }
}