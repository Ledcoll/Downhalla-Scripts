using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomePedra : MonoBehaviour
{
    private EnemyScript enemy;

    public GameObject pedra;
    private bool isDmg = false;
    public float tempoInvulneravel;

    private void Start()
    {
        enemy = GameObject.FindWithTag("enemy").GetComponent<EnemyScript>();
        Invoke("tempoInvul", tempoInvulneravel);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDmg)
        {
            if (collision.gameObject.tag.Equals("enemy") || collision.gameObject.tag.Equals("Player"))
            {
                Destroy(gameObject, 0f);
                enemy.hit();
                enemy.TakeDamage(20);
            }
        }
    }

    void tempoInvul()
    {
        isDmg = true;
    }
}