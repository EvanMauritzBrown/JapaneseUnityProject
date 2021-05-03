using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{

    public float timeoutDestructor;
    public bool facingRight;
    SpriteRenderer sr;
    // Start is called before the first frame update

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (facingRight)
            sr.flipX = true;
        Object.Destroy(gameObject, timeoutDestructor);
    }

    void Update()
    {
        if(facingRight)
            transform.Translate(UnityEngine.Vector2.right * 5 * Time.deltaTime);
        else
            transform.Translate(UnityEngine.Vector2.left * 5 * Time.deltaTime);
    }
}
