using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float moveX = 1f;
    public float moveY = 0f;
    public float velo = 2f;
    public Rigidbody2D rb;
    public float range = 10f;
    bool moveForward = true;
    Vector2 firstPosition;
    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position,firstPosition) >= range)
        {
            moveX *= -1;
            moveY *= -1;
        }
        rb.velocity = new Vector2(moveX, moveY) * velo;
    }
}
