using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public SomePedra some;
    public EnemyScript enemyScript;
    public chrisScript chris;

    private Animator PlayerAnimator;

    private Rigidbody2D PlayerRB;

    public Transform GroundCheck;


    public Vector2 moveDirection;
    public Vector3 slideDir;

    public AnimationClip heavyAttackclip;
    public AnimationClip dodgeclip;

    public GameObject rockPrefeb;

    public float speed;
    public int maxJumps = 2;
    private int jumps;
    public float jumpForce;
    public GameObject ChrisRock;
    public GameObject currentRock;

    public bool LookBack;
    float horizontalInput;
    float verticalInput;

    public int condition;
    public bool Ground;

    public bool attacking;
    public int atackDMG;
    public int hAtackDMG;

    public bool immortal;
    public float speedDash;
    public bool isDashing = false;
    public float dashPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingColldown = 1f;
    public bool hasRock;
    public bool canCatch;
    public bool joojRock;

    public GameObject pedra;
    public float inicialTime;
    public float outTime = 0.01f;


    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        inicialTime = Time.time;

    }
    private void FixedUpdate()
    {
        Ground = Physics2D.OverlapCircle(GroundCheck.position, 0.02f);

    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Move();

        // Anima��o de pulo + execusao da acao
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        // Animacao de ataque leve + execusao da acao
        if (Input.GetButtonDown("Fire1"))
        {
            if (attacking == false)
            {
                Atack();
                atackDMG = 5;
            }
            else
            {
                attacking = true;
            }

        }

        // Animacao de ataque pesado + execusao da acao
        if (Input.GetButtonDown("Fire2"))
        {
            if (attacking == false)
            {
                heavyAtack();
                hAtackDMG = 10;
            }
            else
            {
                attacking = true;
            }
        }

        // Animacao da rolada + execusao da acao
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dodge();
            immortal = true;
            Invoke("backToMortal", dodgeclip.length);
            StartCoroutine(dashToPosition());
        }

        // Arremecar
        if (Input.GetKeyDown(KeyCode.E) && !hasRock && canCatch)
        {
            hasRock = true;
            ChrisRock.SetActive(true);
            attacking = true;
            currentRock.GetComponent<chrisScript>().CatchMe();
        }
        else if (Input.GetKeyDown(KeyCode.E) && hasRock)
        {
            hasRock = false;
            attacking = false;
            joojRock = true;
            fresno();
        }

    }
    // Codigo de execusao da rolada! (uii)
    public IEnumerator dashToPosition()
    {

        float t = 0f;
        Vector3 currentPosition = transform.position;
        while (t < 1)
        {
            t += Time.deltaTime / dodgeclip.length;
            if (LookBack)
            {
                transform.position = Vector3.Lerp(currentPosition, new Vector3(currentPosition.x - speedDash, currentPosition.y, currentPosition.z), t);
            }
            else
            {
                transform.position = Vector3.Lerp(currentPosition, new Vector3(currentPosition.x + speedDash, currentPosition.y, currentPosition.z), t);
            }

            yield return null;
        }
    }

    // Codigo de execusao da movimentacao!
    public void Move()
    {
        moveDirection = new Vector2(horizontalInput, verticalInput);

        PlayerRB.velocity = new Vector2(horizontalInput * speed, PlayerRB.velocity.y);

        PlayerAnimator.SetFloat("Vertical", verticalInput);
        PlayerAnimator.SetFloat("Horizontal", horizontalInput);

        if (horizontalInput < 0 && isDashing != true)
        {
            transform.localScale = new Vector2(-0.16f, 0.16f);
            LookBack = true;
        }
        else if (horizontalInput > 0 && isDashing != true)
        {
            transform.localScale = new Vector2(0.16f, 0.16f);
            LookBack = false;
        }
        else
        {
            if (LookBack)
            {
                transform.localScale = new Vector2(-0.16f, 0.16f);
            }
            else
            {
                transform.localScale = new Vector2(0.16f, 0.16f);
            }
        }


    }

    // Condicoes basicas do personagem.
    void OnCollisionEnter2D(Collision2D collider)
    {

        if (collider.gameObject.tag == "ground" && Ground == true)
        {
            jumps = maxJumps;
            speed = 2f;
            condition = 2;
        }
    }

    // C�digo de execu��o do pulo.
    public void Jump()
    {
        PlayerAnimator.SetTrigger("Jump");
        if (jumps > 0)
        {
            PlayerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumps--;
            Ground = false;
        }

        if (jumps == 0)
        {
            return;
        }
    }

    // C�digo de execu��o do ataque leve.
    public void Atack()
    {
        float criticDMG = Random.value;
        PlayerAnimator.SetTrigger("Attack");
        float distance = Vector2.Distance(transform.position, enemyScript.gameObject.transform.position);

        if (distance <= 0.47f && attacking == false)
        {
            if (immortal == false && criticDMG < .80f && !hasRock)
            {
                enemyScript.TakeDamage(5);
            }
            else if (immortal == false && !hasRock)
            {
                enemyScript.TakeDamage(8);
            }
        }
    }

    // C�digo de execu��o do ataque pesado.
    public void heavyAtack()
    {
        PlayerAnimator.SetTrigger("HeavyAttack");
        float criticDMG = Random.value;


        PlayerAnimator.speed = 0.8f;
        float distance = Vector2.Distance(transform.position, enemyScript.gameObject.transform.position);

        if (distance <= 0.57f && attacking == false)
        {
            if (immortal == false && criticDMG < .90f && !hasRock)
            {
                enemyScript.TakeDamage(10);
            }
            else if (immortal == false && !hasRock)
            {
                enemyScript.TakeDamage(12);
            }
        }
        Invoke("backToNormal", heavyAttackclip.length);
    }

    // Condi��es e status.
    public void backToMortal()
    {
        immortal = false;
        isDashing = false;
    }
    public void backToNormal()
    {
        PlayerAnimator.speed = 1f;
    }

    // Codigo de condi��es da rolada!
    public void dodge()
    {
        PlayerAnimator.SetTrigger("Dodge");
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + 20f, transform.position.y, transform.position.z), Time.deltaTime);
        isDashing = true;
    }

    public void fresno()
    {
        ChrisRock.SetActive(false); //pedra fake
        GameObject rock = (GameObject)Instantiate(rockPrefeb, transform.position, rockPrefeb.transform.rotation);
        rock.transform.position = new Vector3(transform.localPosition.x , transform.position.y + 0.3f, 0f);
        rock.transform.localScale = new Vector3(0.15f, 0.15f, 0);
        Rigidbody2D projectileRb = rock.GetComponent<Rigidbody2D>();


        if (LookBack == false)
        {
            projectileRb.AddForce(0.06f * Vector3.right, ForceMode2D.Force);
        }
        else
        {
            projectileRb.AddForce(0.06f * Vector3.left, ForceMode2D.Force);
        }

      
    }
}
