using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMaleController : MonoBehaviour
{
    //public properties
    public float velocityX = 2;
    public float jumpForce = 48;

    //private components
    private SpriteRenderer sr;
    private Animator animator;
    private Rigidbody2D rb;

    //static properties

    //public bool playerDead = NinjaGirlController.isDead;

    //layers

    private const int LAYER_GROUND = 8;

    //tags
    private const string TAG_NINJAGIRL = "Ninja Girl";

    //bool states

    private bool isJumping = false;

    //Constrants

    private const int ANIMATION_WALK = 0;
    private const int ANIMATION_JUMP = 1;
    private const int ANIMATION_IDLE = 2;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.flipX = true;
        changeAnimation(ANIMATION_WALK);
        if(MainController.countZombies == 10)
        {
            if(!isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                changeAnimation(ANIMATION_JUMP);
                isJumping = true;
            }
        }
        else if(MainController.countZombies == 20)
        {
            if(!isJumping && sr.flipX)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                rb.velocity = new Vector2(-velocityX, rb.velocity.y);
                changeAnimation(ANIMATION_JUMP);
                isJumping = true;
                sr.flipX = false;
            }
            if(!isJumping && !sr.flipX)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                rb.velocity = new Vector2(velocityX, rb.velocity.y);
                changeAnimation(ANIMATION_JUMP);
                isJumping = true;
                sr.flipX = true;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.layer == LAYER_GROUND)
        {
            isJumping = false;
        }    
    }



    private void changeAnimation(int animation)
    {
        animator.SetInteger("Estado", animation);
    }
}
