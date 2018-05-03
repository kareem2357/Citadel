using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allieSpawner : MonoBehaviour {
    public GameObject alliePrefab;
    //buying a wizard
    public int maximumNumberOfWizards=1;
    public int numberOfWizard = 0;
    public AllieNPC[] wizards;
    public Vector3[] wizardPosition;

    //buying an archer
    public int maximumNumberOfArcher;
    public int numberOfArchers = 0;
    public AllieNPC[] archers;
    public Vector3[] archerPosition;

    //TODO: maybe the maximum number of wizard and archer as well as how many there are , should be in the game engine



    // Use this for initialization
    void Start () {

        //TODO:  make this dynamic, this is a predetermened locations for the beta release
        archerPosition = new Vector3[2];
        wizardPosition = new Vector3[1];

        archerPosition[0] = new Vector3(6f, 3.89f);
        archerPosition[1] = new Vector3(8.02f, 0.51f);
        wizardPosition[0] = new Vector3(7.87f, 4.59f);


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnWizard()
    {


        //todo , this needs to change
        
        
        GameObject allie = GameObject.Instantiate(alliePrefab);
        AllieNPC allienpc = allie.GetComponent<AllieNPC>();
        allienpc.selectType(AllieNPC.allieTypeEnum.wizard);
        allie.transform.position = wizardPosition[numberOfWizard];
        allienpc.damage = 0;
        numberOfWizard++;

    }

    public void spawnArcher()
    {
        
        GameObject allie = GameObject.Instantiate(alliePrefab);
        AllieNPC allienpc = allie.GetComponent<AllieNPC>();
        allienpc.selectType(AllieNPC.allieTypeEnum.archer);
        allie.transform.position = archerPosition[numberOfArchers];
        
        numberOfArchers++;
    }



}
