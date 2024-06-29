using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class chrisScript : MonoBehaviour
{
    public EnemyScript enemyScript;
    public Player player;
    private Rigidbody2D rb;
    public GameObject rock;
    public float rockForce;
    public bool isProjectle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isProjectle)
        {
            if (collision.gameObject.tag == "Player")
            {
                player.GetComponent<Player>().attacking = false;
                player.GetComponent<Player>().canCatch = true;
                player.GetComponent<Player>().currentRock = gameObject;
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collider)
    {
        if (!isProjectle)
        {
            if (collider.gameObject.tag == "Player")
            {
                player.GetComponent<Player>().attacking = true;
                player.GetComponent<Player>().canCatch = false;
            }
           
        } 
    }

    public void CatchMe()
    {
            Destroy(gameObject);
    }

}
