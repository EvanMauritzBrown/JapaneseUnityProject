using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using System;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private bool jumping;
    private bool falling;
    private bool moving;
    private bool facingRight;
    private bool dead;
    public float jumpHeight;
    public float liftForce;
    public float damping;
    public float moveSpeed;
    public int coins = 0;
    Rigidbody2D rb;
    SpriteRenderer sr;
    UnityEngine.Vector2 prevDirection;
    Animator anim;
    float endTime = 3;
    private bool done;
    void Start()
    {
        jumping = false;
        falling = false;
        moving = false;
        facingRight = true;
        dead = false;
        done = false;
        prevDirection = new UnityEngine.Vector2(0, 0);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("Die", false);
        anim.SetBool("Ground", true);
        anim.SetBool("Running", false);
    }
    void Update()
    {
        if (dead)
        {
            anim.SetTrigger("Dead");
            Quit();
        }
        else if(done)
        {
            if(endTime <= 0)
            {
                SceneManager.LoadScene("End");
            }
            else
            {
                endTime -= Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                anim.SetTrigger("Attack");
            }
            if (!jumping && Input.GetKeyDown(KeyCode.Space))
            {
                prevDirection.y = jumpHeight;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, prevDirection, prevDirection.magnitude);
                if (hit.collider != null)
                { 
                    float distance = Mathf.Abs((hit.point.magnitude) - (transform.position.magnitude));
                    float heightError = jumpHeight - distance;
                    float force = liftForce * heightError - (rb.velocity.magnitude) * damping;
                    rb.AddForce(prevDirection * force, ForceMode2D.Impulse);
                }
                jumping = true;
                anim.SetTrigger("Jump");
                anim.ResetTrigger("Fall");
                anim.SetBool("Ground", false);
            }
            else
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                if (horizontalInput != 0)
                {
                    if (!moving)
                        anim.SetBool("Running", true);
                    moving = true;
                    if (horizontalInput < 0 && facingRight)
                    {
                        facingRight = false;
                        sr.flipX = true;
                    }
                    else if (horizontalInput > 0 && !facingRight)
                    {
                        facingRight = true;
                        sr.flipX = false;
                    }
                }
                else
                {
                    if (moving)
                        anim.SetBool("Running", false);
                    moving = false;
                }

                UnityEngine.Vector2 v = new UnityEngine.Vector2(horizontalInput, 0);
                if (jumping)
                    transform.Translate(v * (moveSpeed / 4) * Time.deltaTime);
                else
                    transform.Translate(v * moveSpeed * Time.deltaTime);
                prevDirection = v;

                if (rb.velocity.y < 0 && !falling)
                {
                    falling = true;
                    anim.ResetTrigger("Jump");
                    anim.SetTrigger("Fall");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            jumping = false;
            falling = false;
            anim.SetBool("Ground", true);
        }
        if ((col.gameObject.tag == "enemy"
            && !col.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("die"))
            || col.gameObject.tag == "water"
            || col.gameObject.tag == "projectile")
        {
            dead = true;
            anim.SetBool("Die", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "enemy"
                && anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            col.gameObject.GetComponent<Animator>().SetBool("Dead", true);
        }
        if (col.gameObject.tag == "pickup" && col.GetType() == typeof(CapsuleCollider2D))
        {
            coins++;
            UnityEngine.Object.Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "mask" && col.GetType() == typeof(CapsuleCollider2D))
        {
            UnityEngine.Object.Destroy(col.gameObject);
            done = true;
        }
    }

    void Quit()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            transform.position = new UnityEngine.Vector3(-1.8f, 0, 0);
            if (!facingRight)
                sr.flipX = false;
            Start();
        }
    }
}
