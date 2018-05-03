using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{




    //parachuter
    /***I was testing things out, but this could be done in a much better way obviously , an inner class or inhiritance
    the overall changes are ,  when an enemy is created it can be a prachuter or a normal soldier, the prachuter falls to the ground in half speed and doesnt die from the first fall
     */
    public float[] enemyTypeProbability = { 0.5f, 0.35f, 0.15f };//they are sorted acording to their enemyTypeEnum
                                                                 //can be replaced with a map/dictionary , if needed
                                                                 // public int[] startEnemyTypeAfter = { 0, 20}; //after how many seconds, the enemy type starts entering the game
                                                                 //the same sorting as in enemyTypeEnum





    public bool hasParachuteBeenUsed = false;   //the parachute opens only if you throw the trooper high enough to kill him
    GameObject thisParachute;
    public enemyTypeEnum enemyType;
    public enemyTypeEnum initiallEnemeyType;
    public Sprite[] enemySprites;//TODO: i dont like this approuch  , we should change it
    public float maximumPossibleHight;//how high we can throw an enemy , TODO: change the name , this is a bad name
    public float maximumPossibleLeft;//how far we can throw an enemy , TODO: change the name , this is a bad name

    public float positionX;//this is used for the Allie npc
    public float positionY;//this is used for the Allie npc

    public bool isTargeted = false;
    public GameEngine gameEngine;
   // public GameObject gameEngineObject;


    // UPGRADES
    public bool isSlowedDown = false;


    //TODO: i dont like this approuch  , we should change it


    // NPC is any non-player character

    public float hitpoints;
    public float speedFactor;
    public Alignment alignment;
    public Activity activity;

    public bool isTouchingGround;


    public bool isTouchingCastle;
    public bool isIncapacitated;

    public bool shouldIncapacitate;
    public bool shouldDie;

    public float heightThreshold;
    public float highestY = 0;
    public float groundLevelY = 0;

    public bool isInGame = false;

    public float totalTimeElapsed = 0;
    public float timePassedInGame;//remove this;

    int currentWave;
    int amountOfArmor = 4;






    //=============================
    //            ENUMS
    //=============================





    public enum Alignment
    {
        Friendly,
        Hostile
    }

    public enum Activity
    {
        Walking,
        Attacking,
        Recovering,
        Incapacitated,
        Parachute,
        Armor,
        Dead
    }
    public enum enemyTypeEnum //TODO: should change the lettering?
    {
        Normal,
        Parachuter,




        Armor
    }









    //=============================
    //           METHODS
    //=============================




    // Use this for initialization
    void Start()
    {
        hitpoints = 100;
        gameEngine = GameObject.Find("GameEngine").GetComponent<GameEngine>();
        //when we start the game, each enemy gets a type


        SelectEnemyType();





    }

    public void setInGame(bool inGame)
    {
        groundLevelY = this.transform.position.y; // Set base Y level for ground
        highestY = 0;

        isInGame = inGame;

        GameEngine.GetInstance().npcs.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInGame) // Do nothing until NPC is activated by game engine
        {
            highestY = Mathf.Max(highestY, this.transform.position.y);


            //----to access the position more easily

            if(this.GetComponent<Rigidbody2D>() != null)
            { 
            positionX = this.GetComponent<Rigidbody2D>().position.x;
            positionY = this.GetComponent<Rigidbody2D>().position.y;
            }




            SelectActivity();
            PerformActivity();



            bool previousIsTouchingGround = isTouchingGround;
            isTouchingGround = this.transform.position.y <= groundLevelY;
            if (isTouchingGround)
            {
                this.transform.position = new Vector3(this.transform.position.x, groundLevelY, this.transform.position.z); // Frameloss compensation
            }
            if (!previousIsTouchingGround && isTouchingGround)
            {
                shouldDie = shouldEnemyDie();// since there are more than one enemy type , each type is checked if he should die
                                             //the fact the he crossed the threshold is not enough


                shouldIncapacitate = true;
            }
        }

        thrownEnemyLimmiter(); //so the enemies will not be thrown too far away
        totalTimeElapsed += Time.deltaTime;
    }




    /***
     * limits the throwing area into a reasonable area
     * prevents the enemies from beeing thrown way too high
     * prevents enemies from crossing the castle
     * prevent enemies from going too much too the left */
    public void thrownEnemyLimmiter()//TODO: change the name of the method? is it clear enough
    {
        try
        {
            Vector2 position = this.GetComponent<Rigidbody2D>().position;
            if (position.x < maximumPossibleLeft)
            {
                this.GetComponent<Rigidbody2D>().position = new Vector2(maximumPossibleLeft, this.GetComponent<Rigidbody2D>().position.y);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
            }
            if (position.x > GameEngine.GetInstance().castle.transform.position.x)
            {
                this.GetComponent<Rigidbody2D>().position = new Vector2(GameEngine.GetInstance().castle.transform.position.x, this.GetComponent<Rigidbody2D>().position.y);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
            }
            if (position.y > maximumPossibleHight)
            {
                this.GetComponent<Rigidbody2D>().position = new Vector2(this.GetComponent<Rigidbody2D>().position.x, maximumPossibleHight);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);
            }
        }
        catch { }
    }



    public void SelectEnemyType()
    {

        ////TODO: this needs to change  , its just a trick 
        ////if not enough time has passed to start the parachute type, then it will automatically select the normal enemy type

        //GameObject gameEngin = GameObject.Find("GameEngine");
        //timePassedInGame = gameEngin.GetComponent<GameEngine>().timeElapsed;
        //if (gameEngin.GetComponent<GameEngine>().timeElapsed <= startEnemyTypeAfter[1])


        //{
        //    enemyType = enemyTypeEnum.Normal;
        //    return;

        //}
        //TODO: this needs to 
        //int currentWave = gameEngine.getWaveNumber();
        //Debug.Log(currentWave);

        
        currentWave = gameEngine.getWaveNumber();
        Debug.Log("im here");
        float rnd = UnityEngine.Random.Range(0f, 1f);
        //float enemyProbabilityFinder = 0f;



        //int i;
        //for (i = 0; i < enemyTypeProbability.Length; i++)
        //{
        //    //enemyTypeProbability has a map of which enemy type should appear ,, in which probability , and selects the correct type randomly
        //    enemyProbabilityFinder += enemyTypeProbability[i];
        //    if (rnd < enemyProbabilityFinder) //THE LOGIC BEHIND THIS, if the normal enemy has 70% chance and the paracchuter 30% , then if our random falls inside the 70% nothing happens, thus it stays as a normal soldier
        //    {
        //        break;
        //    }




        //}

        //enemyType = (enemyTypeEnum)(Enum.GetValues(typeof(enemyTypeEnum))).GetValue(i)
        //  enemyType = enemyTypeEnum.Parachuter;
        //TODO: change graphics to a parachuter



        if (rnd > 0.5 && rnd < 0.85 && currentWave > 1)
        {
            this.GetComponent<SpriteRenderer>().sprite = enemySprites[1];
            enemyType = enemyTypeEnum.Parachuter;
        }

        else if (rnd > 0.85 && currentWave > 2)
        {
            this.GetComponent<SpriteRenderer>().sprite = enemySprites[3];
            this.GetComponent<DragAndDrop>().enabled = false;
            this.GetComponent<clickDitect>().enabled = true;
            enemyType = enemyTypeEnum.Armor;
        }

        //if (enemyType == enemyTypeEnum.Parachuter)
        //{
        //    this.GetComponent<SpriteRenderer>().sprite = enemySprites[1];
        //}

        //if (enemyType == enemyTypeEnum.Armor)
        //{
        //    this.GetComponent<SpriteRenderer>().sprite = enemySprites[3];
        //}
    }

    //TODO:  this seems redundant but, might be useful later on in the game
    /*** turns any enemy type to a normal enemy */
    private void turnToNormalEnemy()
    {
        this.GetComponent<SpriteRenderer>().sprite = enemySprites[0];
        this.GetComponent<Rigidbody2D>().gravityScale = 2f; //returns gravity to how it was, now he will fall like a normal trooper
        highestY = 0;
        enemyType = enemyTypeEnum.Normal;
        this.GetComponent<DragAndDrop>().enabled = true;
    }










    /* since the castle is in an angle, this function chekcs if the enemie has reached the castle */
    public bool hasReachedCaslte()
    {
        //TODO: a better method will be better

        return (positionX >= (-0.5503 * positionY + 6.497)); //this is kind of an estemation if the soldier reached the caslte, but its not the best

    }








    /*** returns if the soldier should die  AS WELL AS turns special troopers to normal troopers if needed */
    public bool shouldEnemyDie()//TODO: change the name of this method?
    {
        if ((highestY > heightThreshold + groundLevelY))
        {
            //after the parachuter uses the parachute and lands on earth , he becomes a normal trooper, so if he is thrown again he will die like a normal soldier would
            if (enemyType == enemyTypeEnum.Parachuter)
            {
                turnToNormalEnemy();
                return false;
            }

            //if for more types will be added later on

            else  //a normal trooper
            {
                return true;
            }
        }




        //if the threshold was not crossed, then don't kill
        return false;
    }









    public void SelectActivity()
    {
        // Die
        if (hitpoints <= 0 || shouldDie)
        {
            activity = Activity.Dead;
            return;
        }

        // Check if NPC should be incapacitated
        if (shouldIncapacitate)
        {
            shouldIncapacitate = false;
            isIncapacitated = true;

            activity = Activity.Incapacitated;

            LeanTween.delayedCall(2f, () =>
            {
                isIncapacitated = false;
            });
        }

        // If incapacitated, stop considering next actions
        if (isIncapacitated)
        {
            return;
        }

        //if he is a parachuter & he should open the parachute (parachute opens on the threshold)
        if (enemyType == enemyTypeEnum.Parachuter && !hasParachuteBeenUsed && !isTouchingGround && (highestY > heightThreshold + groundLevelY) && this.transform.position.y < heightThreshold && this.GetComponent<Rigidbody2D>().velocity.y < 0) //he slows down only at the end, so it wont take him too much time to fall
        {


            //Debug.Log("choosing parachute");
            activity = Activity.Parachute;
            return;
        }



        if (enemyType == enemyTypeEnum.Armor && !hasArmorLeft())
        {

        }




        // Choose behavior
        if (this.alignment == Alignment.Hostile)
        {
            HostileBehavior();
        }
        else
        {
            FriendlyBehavior();
        }
    }

    public bool hasArmorLeft()
    {
        if (this.amountOfArmor == 0)
        {
            return false;
        }

        return true;
    }

    public void HostileBehavior() // For enemies
    {
        if (isTouchingGround)
        {
            activity = Activity.Walking;



            if (hasReachedCaslte())

            {
                this.activity = Activity.Attacking;


            }
        }
        else
        {
            activity = Activity.Incapacitated;
        }
    }

    public void FriendlyBehavior() // For friendly NPCs
    {



    }

    public void PerformActivity()
    {
        switch (activity)
        {
            case Activity.Dead:
                Die();
                break;
            case Activity.Walking:
                Walk();
                break;
            case Activity.Incapacitated:
                Incapacitate();
                break;
            case Activity.Attacking:

                Attack();
                break;
            case Activity.Parachute:
                parachute();
                break;
        }
    }

    public void Attack()
    {
        try
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;



            
            LeanTween.delayedCall(0.5f, () =>


            {
                GameEngine.GetInstance().castle.TakeDamage(0.25f);

            });
        }
        catch { }
    }

    public void Incapacitate()
    {



    }

    public void Die()
    {
        
        GameObject.Destroy(this.GetComponent<Rigidbody2D>());

        if (GameEngine.GetInstance().npcs.Contains(this))
        {
            GameEngine.GetInstance().npcs.Remove(this);
        }

        LeanTween.scale(this.gameObject, Vector3.zero, 1f).setOnComplete(
            () =>
            {
                gameEngine.earnGoldByKilling(initiallEnemeyType);
                GameObject.Destroy(this.gameObject);
            });
    }

    public void Walk()
    {
        try
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.right * this.speedFactor;
        }
        catch { }
    }


    /*** the parachuter opens the parachute only once, the decision to do so is in SelectActivity()
     * the parachute starts , slowly descend , after reaching the ground
     * the trooper becomes a normal soldier by shouldEnemyDie() */
    public void parachute()
    {
        //a parachute can be used only once, second fall will kill the trooper
        if (!hasParachuteBeenUsed)
        {
            hasParachuteBeenUsed = true;
            this.GetComponent<SpriteRenderer>().sprite = enemySprites[2];
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            this.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
            //NOTE: if we just change the gravity scale, it will not help since the enemy has a great velocity 
        }
    }





    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision, true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HandleCollision(collision, false);
    }

    void HandleCollision(Collision2D collision, bool reverse)
    {
        bool previousIsTouchingGround = isTouchingGround;

        isTouchingGround = reverse ^ false;
        isTouchingCastle = reverse ^ false;

        foreach (ContactPoint2D contactPoint in collision.contacts)
        {
            string otherObjectTag = contactPoint.otherCollider.gameObject.tag;

            switch (otherObjectTag)
            {
                case "Ground":
                    isTouchingGround = reverse ^ true;
                    break;
                case "Castle":
                    isTouchingCastle = reverse ^ true;
                    break;
            }
        }

        if (!previousIsTouchingGround && isTouchingGround)
        {
            if (highestY > heightThreshold)
            {
                shouldDie = true;
            }

            shouldIncapacitate = true;
        }
    }
    */
}
