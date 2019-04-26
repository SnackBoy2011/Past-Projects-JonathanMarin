using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dunga : Character {

    public GameObject dunga;
    public GameObject emoteText;
    public GameObject currentWaypoint;
    public Transform arrowSpawnLocation;
    public GameObject rockPrefab;

    private bool enemyInRange = false;
    public EnemyMarauder enemyM;
    public EnemyArcher enemyA;
    private GameObject[] currentStageEnemies;
    private GameObject[] rocks;

    public bool barrierActive = false;
    public bool hasRock = false;


    private void Awake()
    {
        dialogueC = GameObject.Find("DialogueBox");
        currentWaypoint = GameObject.Find("DungaWP1");
    }

    // Use this for initialization
    void Start () {
        ToggleEmoteText();
        healthbarCanvas.SetActive(false);
        moveSpeed = 2.5f;
        currentScene = SceneManager.GetActiveScene();


        if (currentScene.name == "GameScene")
        {
            moveSpeed = 5.0f;
        }

        minDist = 2;
        crb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        health = startingHealth;
        

        //startPos = dunga.transform.position;
        //endPos = dunga.transform.position + Vector3.right * distance;

        gameManager = GameObject.Find("GameManager");

    }
	
	// Update is called once per frame
	void Update () {
       // Debug.Log("EnemyM = " + enemyM);
       // Debug.Log("EnemyA = " + enemyA);
        if (combatActive == true)
        {
            foreach (GameObject enemy in gameManager.GetComponent<PauseGame>().currentStageEnemies)
            {
                if (enemy.gameObject.GetComponent<EnemyMarauder>() && enemy.gameObject.GetComponent<EnemyMarauder>().health > 0)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= minDist)
                    {
                        enemyM = enemy.gameObject.GetComponent<EnemyMarauder>();
                    }
                }


                else if (enemy.gameObject.GetComponent<EnemyArcher>() && enemy.gameObject.GetComponent<EnemyArcher>().health > 0)
                {
                    enemyM = null;
                    enemyA = enemy.gameObject.GetComponent<EnemyArcher>();
                }

                else
                {
                    enemyM = null;
                    enemyA = null;
                }

            }


            //Barrie-er Activation(get it (its cause kev is from barrie (if you didnt get it) ) )XD
            //Needs to work with icon
            if (Input.GetKeyDown(KeyCode.B) && barrierActive == false) {
                BarrierGO();
               
            }
            else if (barrierActive == true)
                Invoke("BarrierSTOP", 2.0f);

        }

        if (Vector3.Distance(transform.position, currentWaypoint.transform.position) >= minDist)
        {
            MoveTowards();
            if (currentScene.name == "OpeningScene")
            {
                spriteAnimator.Play("Walking");
            }
            else
                spriteAnimator.Play("Running");
        }

        else if (combatActive == false)
        {
            spriteAnimator.Play("Idle");
        }

        if (combatActive == true)
        {
            HealthReduction();
            //Lerping into scene
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }
            float perc = currentLerpTime / lerpTime;

            // turning off dunga's movement for now, for testing
            /// dunga.transform.position = Vector3.Lerp(startPos, endPos, perc);

            //HealthReduction();
            if (enemyM != null)
            {
                if (Vector3.Distance(transform.position, enemyM.transform.position) >= minDist)
                {
                    enemyInRange = false;
                }
                else
                {
                    enemyInRange = true;
                }

                if (enemyInRange == true && enemyM.health > 0)
                    Attack();
            }

            else if (enemyA != null && hasRock)
            {
                ThrowRocks();
            }

            else if (enemyA != null && !hasRock)
            {
                PickupRock();
            }
            
        }
    }

    private void Attack()
    {
        if (Time.time > nextAttack)
        {
            spriteAnimator.Play("Attacking");
            critCheck = Random.Range(1, 11);
            damage = Random.Range(15, 21);
            if (critCheck == 1)
            {
                damage *= 1.5f;
                damage = Mathf.Round(damage);
            }
            nextAttack = Time.time + attackRate;
            enemyM.Damage(damage, critCheck);
            //Debug.Log("Damage passed to enemy: " + damage);
        }
    }

    private void ThrowRocks()
    {
        if (Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            GameObject rock = Instantiate(rockPrefab, arrowSpawnLocation.position, Quaternion.identity);
            rock.transform.Rotate(Random.Range(0,180), Random.Range(0, 180), Random.Range(0, 180));

            hasRock = false;
        }
    }

    private void PickupRock()
    {
        if (GameObject.FindGameObjectsWithTag("Rock") != null)
        {
            foreach (GameObject rock in GameObject.FindGameObjectsWithTag("Rock"))
            {
                float rockDistance = Vector3.Distance(rock.transform.position, transform.position);
                if (rockDistance < minDist && rock.GetComponent<Rock_rock>().isHeld == false)
                {
                    Destroy(rock.gameObject);
                    hasRock = true;
                    break;
                }
            }
        }
    }

    void MoveTowards()
    {
        transform.position += transform.right * 1 * moveSpeed * Time.deltaTime;
    }

    //Function that handles health reduction of dunga each frame - called in update (visually)
    void HealthReduction()
    {
        //health --;

        //healthFill.fillAmount = health / 100;

        //uiHealthNumber.text = health + " / 100";
    }

    void TargetEnemy()
    {

    }

    void AttackEnemy()
    {

    }

    protected override void Kill()
    {

    }

    private void StartNarrative()
    {
        dialogueC.SetActive(true);
        StartCoroutine(dialogueManager.GetComponent<DialogueTest>().Type());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ExclamationCollider")
        {
            ToggleEmoteText();
            emoteText.GetComponent<TextMeshProUGUI>().text = "!";
            Invoke("ToggleEmoteText", 1.5f);
            GameObject.Find("DialogueManager").GetComponent<Dialogue>().Invoke("StartGameNarrative", 3.0f);
        }
    }

    private void ToggleEmoteText()
    {
        if (emoteText.activeSelf)
        {
            emoteText.SetActive(false);
        }

        else
        {
            emoteText.SetActive(true);
        }
    }


    void BarrierGO()
    {
        barrierActive = true;
        Instantiate(barrierPrefab, transform.position, Quaternion.identity);
    }
    void BarrierSTOP()
    {
        //this is a bad way 2 do this i think
        barrierActive = false;
        Destroy(GameObject.FindGameObjectWithTag("Barrier"));
    }
}
