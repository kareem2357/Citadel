using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{


    static GameEngine instance;
    static int highestTimeLasted = 0;

    public float gold = 200;

    public float earningAmount;
    public float earningDuration;
    public bool isANewHighScore = false;
    public allieSpawner allieSpawener;
    public Text goldText;
    public GameObject upgradeCanvas;


    public EnemySpawner enemySpawner;

    public Castle castle;
    public List<NPC> npcs;

    public Text timeElapsedText;
    public Text waveText;


   // public TextMesh timeElapsedText;


    public float timeElapsed = 0;
    public float totalTimeElapsed = 0;

    bool isInGame = false;
    bool endGame = false;
    private int earnedGoldAt = 0;//todo: name should be changed

    public int waveNumber = 1;
    float waveLength = 20;
    float waveLengthIncreaseFactor = 1.1f;

    public GameObject EndWaveScreen;
    public Text EndWaveScreenCompletedText;




    public GameObject EndGameScreen;





    //------------------------------
    //upgrades
    //------------------------------

    public float allieWaitingTime=3;
    //---wizard 
    public int wizardPrice = 50;
    public bool slowingSpell = false;
    public int slowingSpellPrice = 50;

    //---archer 
    public int archerPrice = 50;
    public int archerRangeLevel = 0;
    public float[] archerRange = { 4, 5, 7 }; //[0] = no upgrades
    public float[] archerRangePrice = { 0, 80, 240 }; //[0] = no upgrades,   [1] price of first upgrade
    public bool piercingArrows = false;
    public int piercingArrowsPrice = 25;


    //prices remove after alpha
    public Text ArcherUpgradeText;


    //------------game methods

    public static GameEngine GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        LeanTween.init(10000);


        instance = this;

        npcs = new List<NPC>();
    }
    // Use this for initialization
    void Start()
    {
        StartGame();
    }
    // Update is called once per frame
    void Update()
    {
        if (isInGame)
        {
            ArcherUpgradeText.text = archerRangePrice[archerRangeLevel + 1].ToString();

            timeElapsed = timeElapsed + Time.deltaTime;
            totalTimeElapsed = totalTimeElapsed + Time.deltaTime;
            timeElapsedText.text = "Survived: "+ ((int)totalTimeElapsed / 60).ToString() + "m "+((int)totalTimeElapsed % 60).ToString() + "s";
            goldText.GetComponent<Text>().text = "gold: " + gold;//TODO: this is called too many times, change this
        }
    }
    public int getWaveNumber()
    {
        return this.waveNumber;
    }
    public void StartGame()
    {
        isInGame = true;
        isANewHighScore = false;
        enemySpawner.StartSpawning();
        LeanTween.delayedCall(waveLength, () =>
        {
            EndWave();
        });

        waveText.text = "Wave " + waveNumber.ToString();
        EndWaveScreenCompletedText.text = "You've Completed Wave " + waveNumber.ToString();




    }

    public void EndWave()

    {
        ClearGame();

        waveLength *= waveLengthIncreaseFactor;


        ShowEndWaveScreen();
    }

    public void ShowEndWaveScreen()
    {
        if (endGame)
            return;





        //EndWaveScreen.SetActive(true);
        LeanTween.delayedCall(1f, () =>
        {
            HideEndWaveScreen();
        });

    }
    public void ShowEndGameScreen()
    {
        EndGameScreen.SetActive(true);
    }

    public void HideEndWaveScreen()
    {
        LeanTween.delayedCall(3f, () =>
        {
            EndWaveScreen.SetActive(false);
            NextWave();
        });

    }

    public void NextWave()
    {
        waveNumber++;
        waveText.text = "Wave " + waveNumber.ToString();
        EndWaveScreenCompletedText.text = "You've Completed Wave " + waveNumber.ToString();
        LeanTween.delayedCall(4f, () =>
        {
            StartGame();

        });

    }



    public void ClearGame()
    {
        timeElapsed = 0;

        enemySpawner.StopSpawning();

        isInGame = false;

        foreach (NPC npc in npcs.ToArray())

        {
          //  npc.Die();

        }
    }










    public void EndGame()
    {
        ClearGame();
        endGame = true;

        isInGame = false;



        enemySpawner.StopSpawning();





        foreach (NPC npc in npcs.ToArray())
        {
            npc.Die();
        }





        if (timeElapsed > highestTimeLasted)
        {




            isANewHighScore = true;
            highestTimeLasted = (int)timeElapsed;





            timeElapsedText.text = "HIGH SCORE You have survived " + ((int)timeElapsed).ToString() + " seconds!";





        }

        else if (!isANewHighScore) //the method gets activated more than once, isANewHighScore prevents tehe text from being overridden

        {

           // Debug.Log("highest time lasted" + highestTimeLasted + "   elapsed time" + timeElapsed);
            //timeElapsedText.text = "Game Over! You have survived " + ((int)timeElapsed).ToString() + " seconds!";
        }

















        timeElapsed = 0;


        LeanTween.delayedCall(3f, () =>
        {

            ShowEndGameScreen();
        });

    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }





    //TODO UPGRADES tune how much you should earn, maybe even earn more for more difficult armored enemy
    public void earnGoldByKilling(NPC.enemyTypeEnum initialType)
    {
        if (initialType == NPC.enemyTypeEnum.Normal)
        {
            gold += 1;
        }
        else if (initialType == NPC.enemyTypeEnum.Parachuter)
        {
            gold += 2;
        }
        else if (initialType == NPC.enemyTypeEnum.Armor)
        {
            gold += 4;
        }




    }


    //---------------------------------------
    //               upgrades
    //---------------------------------------
    public void hideUpgradeCanvas()
    {

        upgradeCanvas.SetActive(false);
    }

    public void showUpgradeCanvas()
    {
        if (upgradeCanvas.activeSelf)
        {
            upgradeCanvas.SetActive(false);
        }
        else
        {
            upgradeCanvas.SetActive(true);
        }
    }
    public void upgradeArcherRange()
    {
        //remember [0] is without updates
        if (archerRange.Length > (archerRangeLevel + 1))
        {
            if (gold >= archerRangePrice[archerRangeLevel + 1])
            {
                gold -= archerRangePrice[archerRangeLevel+1];
                archerRangeLevel++;

            }
        }

        if(archerRangeLevel >= archerRange.Length)
        {
            GameObject archerRange = GameObject.Find("archerRange");
            archerRange.SetActive(false);
        }
    }
    public void upgradeSlowingSpell()
    {
        if (!slowingSpell)
        {
            if (gold >= slowingSpellPrice)
            {
                slowingSpell = true;
                gold -= slowingSpellPrice;
            }
        }
        if (slowingSpell)
        {
            GameObject slowspell = GameObject.Find("slowingSpell");
            slowspell.SetActive(false);
        }
    }


    public void upgradePiercingArrows()
    {


        if (!piercingArrows)
        {
            if (gold >= piercingArrowsPrice)
            {
                piercingArrows = true;
                gold -= piercingArrowsPrice;
            }
        }
        if (piercingArrows)
        {
            GameObject armorePiercing = GameObject.Find("armorePiercing");
            armorePiercing.SetActive(false);
        }
    }


    public float getArcherRange()
    {
        return archerRange[archerRangeLevel];
    }




    //------buying units
    public void buyWizard()
    {

        if (allieSpawener.numberOfWizard < allieSpawener.maximumNumberOfWizards)
        {
            if (gold >= wizardPrice)
            {
                allieSpawener.spawnWizard();
                gold -= wizardPrice;
            }
        }
        //the number of wizard has been changed by the spawner
        if (allieSpawener.numberOfWizard >= allieSpawener.maximumNumberOfWizards)
        {
            GameObject wizardButton = GameObject.Find("buyWizard");
            wizardButton.SetActive(false);

        }

    }

    public void buyArcher()
    {



        if (allieSpawener.numberOfArchers < allieSpawener.maximumNumberOfArcher)
        {
            if (gold >= archerPrice)
            {
                allieSpawener.spawnArcher();
                gold -= archerPrice;
            }
        }
        //the number of wizard has been changed by the spawner
        if (allieSpawener.numberOfArchers >= allieSpawener.maximumNumberOfArcher)
        {
            GameObject archerButton = GameObject.Find("buyArcher");
            archerButton.SetActive(false);

        }



    }







}


