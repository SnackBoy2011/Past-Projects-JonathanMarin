using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class EnemyMarauder : Character {

    public GameManager gm;
    Dunga target;

    private float moveSpeed = 2.5f;

    private bool DungaInRange = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        crb = GetComponent<Rigidbody>();
        minDist = 2;
    }
    // Use this for initialization
    void Start () {
        // set target as Dunga
        target = GameObject.FindObjectOfType<Dunga>();
        health = startingHealth;
        // set start position as current position, end position as Dunga
        startPos = character.transform.position;
        endPos = target.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, target.transform.position) >= minDist)
        {
            DungaInRange = false;
            MoveTowards();
            // animate sprite
        }
        else
        {
            DungaInRange = true;
            // end animation
        }

        if (DungaInRange == true)
            Attack();
    }

    void MoveTowards()
    {
        transform.position += transform.right * -1 * moveSpeed * Time.deltaTime;
    }

    void Attack()
    {
        if (Time.time > nextAttack)
        {
            critCheck = Random.Range(1, 11);
            damage = Random.Range(15, 21);
            if (critCheck == 1)
            {
                damage *= 1.5f;
                damage = Mathf.Round(damage);
            }
            nextAttack = Time.time + attackRate;
            target.Damage(damage, critCheck);
            //Debug.Log("Damage passed to Dunga: " + damage);
        }
    }

    protected override void Kill()
    {
        gm.eCount--;
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Dialogue")
        {
            Debug.Log("Dialogue Start");
            
            
        }
    }
}
