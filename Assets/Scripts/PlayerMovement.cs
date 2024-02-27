using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 3f;
    public float jumpSpeed = 5f;
    Vector2 moveVector;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    bool isAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        // Stop if player is dead
        if (isAlive == false) { return; }


        // Update player velocity
        myRigidbody.velocity = new Vector2(moveVector.x * runSpeed, myRigidbody.velocity.y);


        // If player is running and velocity.x is not 0, update animator
        // to play running animation, otherwise play idle animation
        if (myRigidbody.velocity.x != 0)
        {
            myAnimator.SetBool("isRunning", true);


            // Change player facing direction if running
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }


    // Move input action has been triggered. "Move" action becomes "OnMove" method
    void OnMove(InputValue value)
    {
        // Stop if player is dead
        if (isAlive == false) { return; }


        moveVector = value.Get<Vector2>();
    }


    // Jump input action has been triggered. "Jump" action becomes "OnJump" method
    void OnJump()
    {
        // Stop if player is dead
        if (isAlive == false) { return; }


        // Jump only if player is touching the platform to avoid flying
        int platformLayer = LayerMask.GetMask("Platform");
        bool playerIsTouchingPlatform = myCapsuleCollider.IsTouchingLayers(platformLayer);


        if (playerIsTouchingPlatform)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
        }
    }


    // Called when another collider makes contact with player's collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Make player die when he's alive and touching hazards
        int hazardsLayer = LayerMask.GetMask("Hazards");
        bool playerIsTouchingHazards = myCapsuleCollider.IsTouchingLayers(hazardsLayer);
        if (playerIsTouchingHazards && isAlive)
        {
            Die();
        }
    }


    void Die()
    {
        // Trigger death animation
        myAnimator.SetTrigger("isDead");


        // Set isAlive to false
        isAlive = false;
    }


    // Reloads current level. Called at the end of death animation. 
    void RestartLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}












