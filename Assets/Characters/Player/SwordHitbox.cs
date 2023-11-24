using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float swordDamage = 1f;
    public float knockbackForce = 5f;
    public Collider2D swordCollider;
    public Vector2 faceRight = new Vector2(0.089f,0.097f);
    public Vector2 faceLeft = new Vector2(-0.089f,0.097f);

    void Start() {
        if(swordCollider == null) {
            Debug.Log("Sword collider not set");
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if(damageableObject != null) {
            Vector3 parentPosition = transform.parent.position;

            Vector2 direction = (other.transform.position - parentPosition).normalized;
            Vector2 knockback = direction * knockbackForce;

            //other.SendMessage("OnHit", swordDamage, knockback);
            damageableObject.OnHit(swordDamage, knockback); 
        } else {
            Debug.Log("Collider does not implement IDamageable");
        }
        
    }

    void FacingDirection(bool isFacingRight){
        if(isFacingRight) {
            gameObject.transform.localPosition = faceRight;
        }
        else {
            gameObject.transform.localPosition = faceLeft;
        };
     }
}

