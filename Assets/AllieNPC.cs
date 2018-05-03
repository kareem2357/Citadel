using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllieNPC : MonoBehaviour {


    //NPC of an allie , 
    public float health;
    public int damage;
    public allieTypeEnum allieType;
    public float locationX;
    public float locationLane;
    public bool isInCastle;
    public NPC selectedEnemy; //TODO Remove this , keep the one in the Attack()
    public GameEngine gameEngine;
    public Sprite[] allieSprites;//TODO: i dont like this approuch  , we should change it

    //timing---
    private float time = 0.0f;
    public float interpolationPeriod = 3.0f;

    //=============================
    //            ENUMS
    //=============================

    public enum spellTypesEnum
    {
        slowDown,
        turnToHuman
    }

    public enum Activity
    {
        waiting,
        attacking,
        defending
    }

    public enum allieTypeEnum //TODO: should change the lettering?
    {
        defaultType,
        archer,
        wizard,

    }


    //=============================
    //           METHODS
    //=============================




    // Use this for initialization
    void Start() {
        health = 100;//UPGRADE   todo:this should be in select type

        setInGame(true);
        gameEngine = GameObject.FindGameObjectsWithTag("GameEngine")[0].GetComponent<GameEngine>();
        
        
        

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time >= gameEngine.allieWaitingTime)
        {
            attack();
            time = 0;
        }
    }

        /// <summary>
        /// TODO: shoof sho ma3mool bhadakw  shoof sbhakel 3am eash ent bte7taj to'7 kede teo7noot
        /// </summary>
        /// <param name="inGame"></param>
        public void setInGame(bool inGame)
    {

    }


    public void selectType(allieTypeEnum type)
    {
        if(type==allieTypeEnum.archer)
        {
            this.allieType = allieTypeEnum.archer;
            this.GetComponent<SpriteRenderer>().sprite = allieSprites[1];
            isInCastle = true;

        }
        if (type == allieTypeEnum.wizard)
        {
            this.allieType= allieTypeEnum.wizard;
            this.GetComponent<SpriteRenderer>().sprite = allieSprites[0];
            isInCastle = true;
        }
    }

    //-------------------------------------------------------------------
    //                      ATTACKING
    //-------------------------------------------------------------------

    /// <summary>
    /// TODO: does all the attacking methods
    /// </summary>
    public void attack()
    {
       // Debug.Log("attack");

        //TODO: make sure enemies had
        //NPC selectedEnemy;
       
        selectedEnemy = selectEnemy();
            selectedEnemy = targetting(selectedEnemy);
        
        if (selectedEnemy == null) //nothing was targetted
            { return; } //if no enemy is targetted 
            selectedEnemy.isTargeted = true;
            damaging(selectedEnemy);


        if (allieType == allieTypeEnum.wizard)
        {
            spell(selectedEnemy);
        }  




       


    }

    //----- select enemy

    /// <summary>
    /// TODO: hada msh lazem ikon  void hada lazem irajee3 enemy
    /// </summary>
    public NPC selectEnemy()
    {
        //gets all the enemies, puts their script in possible enmies
        GameObject[] enemiesObject = GameObject.FindGameObjectsWithTag("Enemy");
        NPC[] possibleEnemies= new NPC[enemiesObject.Length];
        int i = 0;
        foreach (GameObject possibleEnemy in enemiesObject)
        {
            possibleEnemies[i] = possibleEnemy.GetComponent<NPC>();
            i++;
        }
        //TODO importnat: check if to get all the NPC scripts at once, or every time you call the 

        if(allieType == allieTypeEnum.wizard)
        {
            return getFastestEnemy(possibleEnemies);
        }



        //this is in case of archer or any other damaging type allie
            return getClosestEnemy(possibleEnemies);
        


    }
    private NPC getFastestEnemy(NPC[] enemies)
    {
        float fastest = float.MinValue;
        NPC fastestEnemy = enemies[0];
        if (isInCastle)
        {
            foreach (NPC enemy in enemies)
            {
                if (enemy.isTouchingGround && !enemy.isSlowedDown) //UPGRADES: one of the upgrades will be to shoot parachuting enemies??
                {
                
                    
                        if (fastest < enemy.speedFactor  && enemy.positionX < 1) //so it will not select enemies that are too close to the castle
                        {
                            fastest = enemy.speedFactor;
                               fastestEnemy = enemy;
                        }
                    
                }
            }
        }
        //TODO: the else statment, will be for allies that are in the field and not in/on the castle

        return fastestEnemy;
    }

    //TODO: major important, this two archers might select the same enemy!!!!!!
    private NPC getClosestEnemy(NPC[] enemies)
    {
        float Max = float.MinValue;
        NPC closestEnemy = enemies[0] ;
        if(isInCastle)
        {
            foreach(NPC enemy in enemies)
            {
                if(enemy.isTouchingGround) //UPGRADES: one of the upgrades will be to shoot parachuting enemies??
                {
                    if(enemy.hasReachedCaslte())//priority to enemies 
                    {
                        
                        return enemy;
                    }
                    else
                    {
                        if(Max < enemy.positionX)
                        {
                            Max = enemy.positionX;
                            closestEnemy = enemy;
                        }
                    }
                }
            }
        }
        //TODO: the else statment, will be for allies that are in the field and not in/on the castle
        
        return closestEnemy;
    }

    //-----targeting
    //TODO:  this is important you probably need to change this!!!!
    public NPC targetting(NPC enemy)
    {
     //   Debug.Log("targetting");
        float probability = Random.Range(0.0f, 1.0f);
        //   Debug.Log("random probability: " + probability);
        //checks if you should damage it, returns the enemy you should damnage
        float gotHitProbability = getHittingProbability(enemy); //TODO: change this?
      //  Debug.Log("random probability: " + probability + "   enemy probability " + gotHitProbability);

        if ( probability <= gotHitProbability)
        {
            Debug.Log("enemy got targetted    random probability: "+probability+"   enemy probability "+ gotHitProbability);
            return enemy;
        }
        //if no one was targeted return null
        return null;
    }
    public float getHittingProbability(NPC enemy)
    {
    //    Debug.Log("getting targetting probability");
     if(allieType == allieTypeEnum.wizard)//later on it will include mine
        {
            return 1.0f;
        }
     else if(allieType == allieTypeEnum.archer)
        {

            float shootingDistance = gameEngine.getArcherRange(); //UPDATE  this should really change


            //bel gae engine , bekon fe gesha ll upgrades, metel enum el be2olak shu wade3 el upgrade bel castle
            //bo;7od hada el eshe w be7seb el probability
            //UPDATE   TODO: change this        distance should matter
            //if its out of range return 0,   if its in range select the probability

            float distance = Mathf.Abs(locationX - enemy.positionX);
           
            if (distance <= shootingDistance)
            {
               // Debug.Log("shot probability "+(float)((shootingDistance-distance) / shootingDistance));
                return (float)((shootingDistance - distance) / shootingDistance); //TODO: check if this actually returns a float or 0
            }
        }
        
        return 0f;
    }
    //-----damaging
    public void damaging(NPC enemy)
    {
      //  Debug.Log("damaging    type:"+allieType);
        //TODO: change this, this shouldnt be like this
        if (enemy == null)
            return;
        if (allieType == allieTypeEnum.archer)
        {
            //  Debug.Log(damage + " amount of damage was done");
            if (enemy.enemyType == NPC.enemyTypeEnum.Normal || enemy.enemyType == NPC.enemyTypeEnum.Parachuter)
            {
                enemy.hitpoints -= 100;
            }

            if (enemy.enemyType == NPC.enemyTypeEnum.Armor && gameEngine.piercingArrows)
            {
                //TODO UPDATE : DAVID'S code ,    lower the armor , by 2 clicks on an armored allient
            }
        }

        


    }


    //-----spell
    public void spell(NPC enemy)
    {

      /*
        //----------change wolfman to human
        if(enemy.enemyType==NPC.enemyTypeEnum.wolfman)
        {
            enemy.turnToNormalEnemy();
        } */

        //UPDATE   if you upgrades the slowing down , then it can slow down enemies
        //---------- slow down spell
        if (gameEngine.slowingSpell)
        {
            if (enemy.enemyType == NPC.enemyTypeEnum.Normal && !enemy.isSlowedDown)
            {

                enemy.speedFactor /= 2.0f;
                enemy.isSlowedDown = true;
            }
        }
        
    }


}
