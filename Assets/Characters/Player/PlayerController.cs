using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    bool IsMoving {
        set{
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }

    public float moveSpeed = 3f;
    public float maxSpeed = 8f;

    // Each frame of physics, what percentage of the speed should be shaved off the velocity out of 1 (100%)
    public float idleFriction = 0.9f;

    public GameObject swordHitbox;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    Collider2D swordCollider;
    Vector2 moveInput = Vector2.zero;


    bool isMoving = false;
    bool canMove = true;


    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordCollider = swordHitbox.GetComponent<Collider2D>();
    }

    void FixedUpdate() {
        
        if(canMove == true && moveInput != Vector2.zero) {
            // Move animation and add velocity

            // Accelerate the player while run direction is pressed
            // BUT don't allow player to run faster than the max speed in any direction
            //rb.velocity = Vector2.ClampMagnitude(rb.velocity + (moveInput * moveSpeed * Time.deltaTime), maxSpeed);
            rb.AddForce(moveInput* moveSpeed * Time.deltaTime);

            if(rb.velocity.magnitude > maxSpeed) {
                float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
                rb.velocity = rb.velocity.normalized * limitedSpeed;

            }

            // Control whether looking left or right
            if(moveInput.x > 0) {
                spriteRenderer.flipX = false;
                gameObject.BroadcastMessage("FacingDirection", true);
            } else if (moveInput.x < 0) {
                spriteRenderer.flipX = true;
                gameObject.BroadcastMessage("FacingDirection", false);
            }

            IsMoving = true;
            
        } else {
            // No movement so interpolate velocity towards 0
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);

            IsMoving = false;
        }

    }


    // Get input values for player movement
    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void OnFire(){
        animator.SetTrigger("swordAttack");
    }

    void LockMovement(){
        canMove = false;    
    }
    void UnLockMovement(){
        canMove = true;    
    }
}
