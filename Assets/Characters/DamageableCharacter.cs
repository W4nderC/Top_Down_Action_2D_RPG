using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageableCharacter : MonoBehaviour, IDamageable
{
    public GameObject healthText;
    public bool disableSimulation = false;
    public bool isInvincibleEnabled = false;
    public float invincibilityTime = 0.25f;
    Animator animator;
    Rigidbody2D rb;
    bool isAlive = true;
    private float invincibleTimeElapsed = 0f;
    Collider2D physicsCollider;

    public float Health {
        set {
            if(value < _health){
                animator.SetTrigger("hit");
                RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            
                Canvas canvas = GameObject.FindAnyObjectByType<Canvas>();
                textTransform.SetParent(canvas.transform);
            }

            _health = value;

            
            if(_health <= 0) {
                animator.SetBool("isAlive",false);
                Targetable = false;
            }
        } 
        get {
            return _health;
        }
    }

    public bool Targetable { get {
        return _targetable;
    }
    set {
        _targetable = value;
        if(disableSimulation) {
            rb.simulated = false;
        }
        
        physicsCollider.enabled = value;
    } }

    public bool Invincible { get { return _invincible;
    }
    set {
        _invincible = value;

        if(_invincible == true) {
            invincibleTimeElapsed = 0f;
        }
    } }

    public float _health = 3f;
    public bool _targetable = true;
    public bool _invincible = false;

    public void Start(){
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if(!isInvincibleEnabled || !Invincible) {
            Health -= damage;

            rb.AddForce(knockback, ForceMode2D.Impulse);

            if(isInvincibleEnabled) {
                //Active invincibility and timer
                Invincible = true;
            }
        }
        
    }

    public void OnHit(float damage)
    {
        if(!isInvincibleEnabled || !Invincible) {
            Health -= damage;
            if(isInvincibleEnabled) {
                //Active invincibility and timer
                Invincible = true;
            }
        }
        
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }
    
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void FixedUpdate()
    {
        if(Invincible) {
            invincibleTimeElapsed += Time.deltaTime;

            if(invincibleTimeElapsed > invincibilityTime) {
                Invincible = false;
            }
        }
    }
}
