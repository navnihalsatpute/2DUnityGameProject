using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlArrow : MonoBehaviour
{
    public Health playerHealth;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(1, gameObject.tag);
            gameObject.SetActive(false);
        }
    }
}