using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            string damageSourceTag = gameObject.tag;
            collision.GetComponent<Health>().TakeDamage(damage,damageSourceTag);
        }
    }
}
