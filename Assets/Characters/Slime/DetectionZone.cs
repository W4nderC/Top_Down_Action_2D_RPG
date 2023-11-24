using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string tagTarget = "Player";
    public List<Collider2D> detectedObjs = new List<Collider2D>();
    public Collider2D col;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == tagTarget) {
            detectedObjs.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == tagTarget) {
            detectedObjs.Remove(other);
        }
    }
}
