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
    private BoxCollider collider;
    private float moveRight = 0;
    private float moveSpeed = 3;
    private Animator animator;
    public bool onGround = true;
    public float jumpForce = 450f;

    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip hitSound;
    public AudioClip dropSound;
    private AudioSource playerAudio;

    public PhysicMaterial reflect;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameOver)
        {
            return;
        }

        if (onGround)
        {
            updateCharingStatus();
            moveRight = Input.GetAxisRaw("Horizontal");

            if (!isSpaceDown)
            {
                transform.Translate(moveRight * Vector3.right * Time.deltaTime * moveSpeed);
            }
        }
        if (playerRb.velocity.y < -0.5f)
        {
            animator.SetBool("isTop", true);
            onGround = false;
        }
        else if (playerRb.velocity.y < 0f)
        {
            animator.SetBool("isTop", false);
            animator.SetTrigger("land");
            onGround = true;
        }
    }

    void updateCharingStatus()
    {
        if (isSpaceDown && (charingTime > maxCharingTime || Input.GetKeyUp(KeyCode.Space)))
        {
            animator.SetBool("isCharing", false);
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
            animator.SetBool("isCharing", true);
        }
    }

    void jump()
    {
        playerAudio.PlayOneShot(jumpSound);
        animator.SetTrigger("jump");
        onGround = false;
        playerRb.AddForce(jumpForce * (Vector3.up * (float)(Math.Log(charingTime + 1, 2) + 0.4) + Vector3.right * moveRight), ForceMode.Impulse) ;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (!collision.gameObject.CompareTag("Wall"))
        {
            return;
        }

        Vector3 hitPoint = collision.collider.ClosestPointOnBounds(transform.position);
        float objBound = collider.size.x / 2;

        if (hitPoint.y < transform.position.y && (transform.position.x - hitPoint.x <= objBound && transform.position.x - hitPoint.x >= -objBound))
        {
            playerAudio.PlayOneShot(landSound);
            playerRb.velocity = Vector3.zero;
            onGround = true;
            animator.SetBool("isTop", false);
            animator.SetTrigger("land");
        }
        else
        {
            playerAudio.PlayOneShot(hitSound);
        }
    }
}
