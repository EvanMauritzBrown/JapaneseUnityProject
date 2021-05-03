using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    private bool facingRight;
    float moveSpeed;
    Rigidbody2D rb;
    SpriteRenderer sr;
    private float center;
    float lagTime;
    private bool firstFlip;
    bool fired;
    bool shoot;
    Animator anim;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = false;
        moveSpeed = 3;
        lagTime = 1;
        firstFlip = false;
        fired = false;
        shoot = false;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("Jumping", false);
        anim.SetBool("Dead", false);
        center = transform.position.x + 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetBool("Dead") == true && anim.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            Object.Destroy(this.gameObject);
        }
        if (lagTime <= 0)
        {
            anim.SetBool("Jumping", true);
            if (transform.position.x >= center + 2 && facingRight)
            {
                if (firstFlip)
                {
                    firstFlip = false;
                    anim.SetBool("Jumping", false);
                    anim.ResetTrigger("Shoot");
                    //fired = false;
                    lagTime = 1;
                }
                else
                {
                    firstFlip = true;
                    facingRight = false;
                    sr.flipX = false;
                }
            }
            else if (transform.position.x <= center - 2 && !facingRight)
            {
                if (firstFlip)
                {
                    firstFlip = false;
                    anim.SetBool("Jumping", false);
                    anim.ResetTrigger("Shoot");
                    //fired = false;
                    lagTime = 1;
                }
                else
                {
                    firstFlip = true;
                    facingRight = true;
                    sr.flipX = true;
                }
            }
            if (facingRight)
            {
                transform.Translate(UnityEngine.Vector2.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(UnityEngine.Vector2.left * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            UnityEngine.Vector2 origin = new UnityEngine.Vector2(transform.position.x, transform.position.y);

            if (!shoot && anim.GetCurrentAnimatorStateInfo(0).IsName("shooterIdle"))
                lagTime -= Time.deltaTime;
            if (shoot && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
            {
                GameObject clone;
                clone = Instantiate(projectile, origin, transform.rotation);
                clone.GetComponent<projectileScript>().facingRight = facingRight;
                clone.GetComponent<projectileScript>().timeoutDestructor = 1.5f;
                shoot = false;
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("shooterIdle"))//!fired)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, UnityEngine.Vector2.right, 5);
                origin = new UnityEngine.Vector2(transform.position.x, transform.position.y);
                if (facingRight)
                {
                    origin.x += 0.5f;
                    hit = Physics2D.Raycast(origin, UnityEngine.Vector2.right, 5);
                }
                else
                {
                    origin.x -= 0.5f;
                    hit = Physics2D.Raycast(origin, UnityEngine.Vector2.left, 5);
                }
                if (hit.collider.gameObject.tag == "player")
                {
                    //fired = true;
                    anim.SetTrigger("Shoot");
                    shoot = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
    }
}
