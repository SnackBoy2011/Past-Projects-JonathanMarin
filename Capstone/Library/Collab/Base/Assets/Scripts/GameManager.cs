using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int eCount;
    ///public bool stageClear = false;
    public GameObject[] enemies;

    // Use this for initialization
    void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        eCount = enemies.Length;
    }
	
	// Update is called once per frame
	void Update () {
       

        if (eCount <= 0)
        {
            nextStage();
        }
       
    }

    void nextStage()
    {
        // switch to next stage

        // spawn more enemies

        // count enemies again
        //countEnemies();
    }

    void countEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        eCount = enemies.Length;
          
    }
}
