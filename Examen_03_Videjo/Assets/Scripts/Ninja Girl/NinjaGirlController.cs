using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaGirlController : MonoBehaviour
{
    //public properties
    public float velocityX = 9;
    public float jumpForce = 48;

    //transform

    private Transform cameraTransform;
    private Vector3 cameraPosition;

    //private components
    private SpriteRenderer sr;
    private Animator animator;
    private Rigidbody2D rb;


    //bool states
    private bool isJumping = false;
    private bool isDead = false;
    private bool gameEnded = false;

    //timer
    public float tiempoJuego = 0;

    //animations
    private const int ANIMATION_RUN = 0;
    private const int ANIMATION_JUMP = 1;
    private const int ANIMATION_DEAD = 2;

    //layers
    private const int LAYER_GROUND = 8;
    private const int LAYER_ZOMBIEHEAD = 10;

    //tags
    private const string TAG_ZOMBIE = "Zombie";



    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        cameraTransform = Camera.main.transform;
        cameraPosition = new Vector3(0,10,-10);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            cameraPosition.x = transform.position.x;
            cameraTransform.position = cameraPosition;

            rb.velocity = new Vector2(velocityX, rb.velocity.y);
            changeAnimation(ANIMATION_RUN);
            
            if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                changeAnimation(ANIMATION_JUMP);
                isJumping = true;
            }

        }

        if(gameEnded && tiempoJuego < 2f)
        {
            tiempoJuego += Time.deltaTime;
        }
        else if (gameEnded && tiempoJuego >= 2f)
        {
            Time.timeScale = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.layer == LAYER_GROUND)
        {
            isJumping = false;
        }
        if(collision.gameObject.tag == TAG_ZOMBIE)
        {
            Debug.Log("Ninja Girl Muerta");
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            deadNinjaGirl();
            gameEnded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.gameObject.layer == LAYER_ZOMBIEHEAD)
        {
            MainController.countZombies += 1;
            Debug.Log("Contador Zombies: " + MainController.countZombies);
        }
    }


    private void deadNinjaGirl()
    {
        if(!isDead)
        {
            velocityX = 0;
            changeAnimation(ANIMATION_DEAD);
        }
        isDead = true;
        
        
    }

    private void changeAnimation(int animation)
    {
        animator.SetInteger("Estado", animation);
    }
}
