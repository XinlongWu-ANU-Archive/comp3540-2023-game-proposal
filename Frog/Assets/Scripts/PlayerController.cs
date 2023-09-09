using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private const float maxCharingTime = 1.0f;
    private float charingTime = 0;
    private bool isSpaceDown = false;
    private Rigidbody playerRb;
    private float moveRight = 0;
    private float moveSpeed = 3;
    public bool onGround = true;
    public float jumpForce = 450f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onGround)
        {
            updateCharingStatus();
            moveRight = Input.GetAxisRaw("Horizontal");

            if (!isSpaceDown)
            {
                transform.Translate(moveRight * Vector3.right * Time.deltaTime * moveSpeed);
            }
        }
    }

    void updateCharingStatus()
    {
        if (isSpaceDown && (charingTime > maxCharingTime || Input.GetKeyUp(KeyCode.Space)))
        {
            jump();
            isSpaceDown = false;
            charingTime = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpaceDown = true;
        }

        if (isSpaceDown)
        {
            charingTime += Time.deltaTime;
        }
    }

    void jump()
    {
        onGround = false;
        playerRb.AddForce(jumpForce * (Vector3.up * (float)(Math.Log(charingTime + 1, 2) + 0.4) + Vector3.right * moveRight), ForceMode.Impulse) ;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (!collision.gameObject.CompareTag("Wall"))
        {
            return;
        }

        Vector3 distence = collision.gameObject.transform.position - transform.position;
        Vector3 collisionBound = collision.gameObject.transform.localScale / 2;

        if (distence.y < -collisionBound.y)
        {
            playerRb.velocity = Vector3.zero;
            onGround = true;
        }
    }
}
