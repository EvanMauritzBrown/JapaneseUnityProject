using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private bool facingRight;
    float moveSpeed;
    Rigidbody2D rb;
    SpriteRenderer sr;
    private float center;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = false;
        moveSpeed = 3;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("Dead", false);
        center = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("Dead") == true && anim.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            Object.Destroy(this.gameObject);
        }
        if (transform.position.x >= center + 2 && facingRight)
        {
            facingRight = false;
            sr.flipX = false;
        }
        if (transform.position.x <= center - 2 && !facingRight)
        {
            facingRight = true;
            sr.flipX = true;
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

    private void OnCollisionEnter2D(Collision2D col)
    {
    }
}
