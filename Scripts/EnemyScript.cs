using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Burst.CompilerServices;
using TMPro;
using DG.Tweening;

public class EnemyScript : MonoBehaviour
{
    public static EnemyScript instance;
    public chrisScript chris;
    private Animator PlayerAnimator;
    Player player;
    public int health;
    public int maxHealth = 100;

    public string playerName;
    public Color color1;
    public Color color2;
    public GameObject prefab;
    public GameObject rock;

    public string textToDisplay;

    private void Awake()
    {
       instance = this;

    }
    public bool isDead
    {
        get
        {
            return health <= 0;
        }
    }

    public int atackDMG;
    public int hAtackDMG;


    private void Start()
    {
        health = maxHealth;
        PlayerAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 100)
        {
            hit();
            GameObject dmgTemp = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            dmgTemp.transform.localPosition = new Vector3(-0.9f, 2.9f, 0f);
            dmgTemp.GetComponent<TextMeshPro>().color = color2;
            dmgTemp.GetComponent<RectTransform>().DOLocalMove(new Vector3(-0.9f, 3.2f, 0f), 1.5f);
            dmgTemp.GetComponent<TextMeshPro>().DOFade(0f, 0.9f);
            dmgTemp.GetComponent<TextMeshPro>().DOColor(color1, 0.7f);

            dmgTemp.GetComponent<TextMeshPro>().text = "" +  amount;

            Destroy(dmgTemp, 1f);
        }

        if (isDead && health <=0)
        {
            die();
            Invoke("timerAnimation", 1.5f);
        }

    }
    public void hit()
    {
        PlayerAnimator.SetTrigger ("Hit");
    }
    public void die()
    {
        PlayerAnimator.SetTrigger("Die");
    }
    private void timerAnimation() 
    {
        gameObject.SetActive(false);
    }

}
