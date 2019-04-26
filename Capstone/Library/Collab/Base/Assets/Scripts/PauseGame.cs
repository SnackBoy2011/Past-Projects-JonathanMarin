using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PauseGame : MonoBehaviour {

    public bool combatActive = false;
    public Transform canvas;
    public Transform pauseMenu;
    public Transform optionsMenu;
    public Transform arrowSpawn;
    public Animator ani;
    public GameObject[] Stage1Enemies = new GameObject[1];
    public GameObject[] Stage2Enemies = new GameObject[1];
    public GameObject[] Stage3Enemies = new GameObject[2];
    public GameObject[] Stage4Enemies = new GameObject[3];
    public GameObject[] Stage5Enemies = new GameObject[2];
    public GameObject[] currentStageEnemies;
    public GameObject arrowPrefab;

    private Scene currentScene;
    private Dialogue dialogueScript;
    private Dunga dungaScript;
    private EnemyMarauder marauderScript;
    private EnemyArcher archerScript;
    private GameObject dialogueParent;
    private GameObject gameOver;
    private GameObject Dunga;
    private GameObject Marauder;
    private TextMeshProUGUI stageText;
    private TextMeshProUGUI healthText;
    private float stageHealth;
    private int currentStage;


    private void Start()
    {
        stageText = GameObject.Find("StageText").GetComponent<TextMeshProUGUI>();
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
        arrow.GetComponent<Projectile>().target = GameObject.Find("DungaWP1").transform;
        currentStage = 1;
        currentScene = SceneManager.GetActiveScene();
        dialogueParent = GameObject.Find("DialogueBox");
        dialogueScript = GameObject.Find("DialogueManager").GetComponent<Dialogue>();
        dungaScript = GameObject.Find("Character").GetComponent<Dunga>();
        gameOver = GameObject.Find("GameOver Screen");
        gameOver.SetActive(false);
        dialogueParent.SetActive(false);

        //SetScripts();

    }

    // Update is called once per frame
    void Update ()
    {
        foreach (GameObject enemy in currentStageEnemies)
        {
        Debug.Log(enemy.gameObject.name);
        }

        CheckStageHealth();
        stageText.text = "Stage " + currentStage + " of 5";
        healthText.text = dungaScript.health + " / 100";

        if (Input.GetKeyDown(KeyCode.K))
            foreach (GameObject enemy in currentStageEnemies)
            {
                if (enemy.gameObject.name == "EnemyMarauder")
                {
                    enemy.GetComponent<EnemyMarauder>().health = 0;
                }

                if (enemy.gameObject.name == "EnemyArcher")
                {
                    enemy.GetComponent<EnemyArcher>().health = 0;
                }

            }


        if (stageHealth <= 0 && combatActive == true)
        {
            Debug.Log("Stage Complete");
            StopCombat();
            dungaScript.ResetHealth();

            if (currentStage == 1)
            {
                dialogueScript.Invoke("StartGameNarrative", 2.0f);
            }

            else if (currentStage == 2)
            {
                Invoke("NextStage", 2.0f);
                dungaScript.currentWaypoint = GameObject.Find("DungaWP3");
                dialogueScript.Invoke("StartGameNarrative", 8.0f);
            }

            else if (currentStage == 3)
            {
                Invoke("NextStage", 2.0f);
                dungaScript.currentWaypoint = GameObject.Find("DungaWP4");
                dialogueScript.Invoke("StartGameNarrative", 8.0f);
            }

            else if (currentStage == 4)
            {
                Invoke("NextStage", 2.0f);
                dungaScript.currentWaypoint = GameObject.Find("DungaWP5");
                dialogueScript.Invoke("StartGameNarrative", 8.0f);
            }


        }

        //When Escape is pressed it starts the Pause method
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        //STRICTLY TESTING
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene(5);
            gameOver.SetActive(true);
            ani.SetBool("IsOpen", true);
            
            //Time.timeScale = 0;
        }

        //if (Input.GetKeyUp(KeyCode.G))
        //{
        //    gameOver.SetActive(false);
        //    ani.SetBool("IsOpen", false);
        //}


        //if (GetComponent<Dunga>().health == 0)
        //{
        //}
    }

    public void ResetScene() {    
        SceneManager.LoadScene(6);
    }

    //Makes sure everything in pause menu is all in order
    public void Pause()
    {
        if (canvas.gameObject.activeInHierarchy == false)
        {
            if(pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
                optionsMenu.gameObject.SetActive(false);
            }
            
            canvas.gameObject.SetActive(true);
            //Pauses the game when escape is pressed 
            Time.timeScale = 0;
            //Pauses the music when escape is pressed 
            AudioListener.volume = 0;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            //Continues the game when escape is pressed
            Time.timeScale = 1;
            //Continues the music when escape is pressed
            AudioListener.volume = 1;
        }

    }
    
    //Turns off the Canvas in pause menu
    public void StartButton()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }

    //Goes back to Main Menu
    public void QuitButton()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Quittededed");
    }

    //Opens Options menu/canvas
    public void Options(bool open)
    {
        Debug.Log(open);

        if (open)
        {
            optionsMenu.gameObject.SetActive(true);
            pauseMenu.gameObject.SetActive(false);
        }

        if (!open)
        {
            optionsMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
        }
    }

    private void CheckStageHealth()
    {
        stageHealth = 0;

        SetCurrentStageEnemies();

        foreach (GameObject enemy in currentStageEnemies)
        {
            if (enemy.GetComponent<EnemyMarauder>() != null)
            {
                stageHealth += enemy.GetComponent<EnemyMarauder>().health;
            }

            if (enemy.GetComponent<EnemyArcher>() != null)
            {
                stageHealth += enemy.GetComponent<EnemyArcher>().health;
            }
        }
    }

    //private void SetScripts()
    //{
    //    foreach (GameObject enemy in currentStageEnemies)
    //    {
    //        if (enemy.gameObject.name == "EnemyMarauder")
    //            {
    //            marauderScript = enemy.GetComponent<EnemyMarauder>();
    //        }

    //        if (enemy.gameObject.name == "EnemyArcher")
    //        {
    //            archerScript = enemy.GetComponent<EnemyArcher>();
    //        }

    //        //if (GameObject.Find("Character") != null)
    //        //{
    //        //    dungaScript = GameObject.Find("Character").GetComponent<Dunga>();
    //        //}
    //    }

    //    dungaScript = GameObject.Find("Character").GetComponent<Dunga>();
    //}

    private void SetCurrentStageEnemies()
    {
        if (currentStage == 1)
        {
            currentStageEnemies = Stage1Enemies;
        }

        if (currentStage == 2)
        {
            currentStageEnemies = Stage2Enemies;
        }

        if (currentStage == 3)
        {
            currentStageEnemies = Stage3Enemies;
        }

        if (currentStage == 4)
        {
            currentStageEnemies = Stage4Enemies;
        }

        if (currentStage == 5)
        {
            currentStageEnemies = Stage5Enemies;
        }
    }

    public void NextStage()
    {
        Debug.Log("Stage+");
        currentStage++;
    }

    public void ChangeStageEnemySpeed(float speed)
    {
        foreach (GameObject enemy in currentStageEnemies)
        {
            if (enemy.gameObject.name == "EnemyMarauder")
            {
                enemy.GetComponent<EnemyMarauder>().moveSpeed = speed;
            }

            if (enemy.gameObject.name == "EnemyArcher")
            {
                enemy.GetComponent<EnemyArcher>().moveSpeed = speed;
            }
        }
    }

    //public void ActivateCombat()
    //{
    //    combatActive = true;
    //    dungaScript.healthbarCanvas.SetActive(true);
    //    marauderScript.healthbarCanvas.SetActive(true);
    //    archerScript.healthbarCanvas.SetActive(true);
    //    dungaScript.combatActive = true;
    //    marauderScript.combatActive = true;
    //    archerScript.combatActive = true;
    //}

    public void ActivateCombat()
    {
        combatActive = true;
        dungaScript.healthbarCanvas.SetActive(true);
        dungaScript.combatActive = true;

        foreach (GameObject enemy in currentStageEnemies)
        {
            if (enemy.gameObject.name == "EnemyMarauder")
            {
                enemy.GetComponent<EnemyMarauder>().combatActive = true;
                enemy.GetComponent<EnemyMarauder>().healthbarCanvas.SetActive(true);
            }

            if (enemy.gameObject.name == "EnemyArcher")
            {
                enemy.GetComponent<EnemyArcher>().combatActive = true;
                enemy.GetComponent<EnemyArcher>().healthbarCanvas.SetActive(true);
            }
        }

    }

    //public void StopCombat()
    //{
    //    combatActive = false;
    //    dungaScript.healthbarCanvas.SetActive(false);
    //    marauderScript.healthbarCanvas.SetActive(false);
    //    archerScript.healthbarCanvas.SetActive(false);
    //    dungaScript.combatActive = false;
    //    marauderScript.combatActive = false;
    //    archerScript.combatActive = false;
    //}

    public void StopCombat()
    {
        combatActive = false;
        dungaScript.healthbarCanvas.SetActive(false);
        dungaScript.combatActive = false;

        foreach (GameObject enemy in currentStageEnemies)
        {
            if (enemy.gameObject.name == "EnemyMarauder")
            {
                enemy.GetComponent<EnemyMarauder>().combatActive = false;
                enemy.GetComponent<EnemyMarauder>().healthbarCanvas.SetActive(false);
            }

            if (enemy.gameObject.name == "EnemyArcher")
            {
                enemy.GetComponent<EnemyArcher>().combatActive = false;
                enemy.GetComponent<EnemyArcher>().healthbarCanvas.SetActive(false);
            }
        }
    }
}
