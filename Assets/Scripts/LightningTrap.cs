using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrap : MonoBehaviour
{
    [SerializeField] private BoxCollider2D trapCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float damage = 0.5f;

    // Function to be called at certain frames of the lightning animation
    public void CheckForPlayerCollision()
    {
        // Check for player collision within the trap's collider
        Collider2D[] colliders = Physics2D.OverlapBoxAll(trapCollider.bounds.center, trapCollider.bounds.size, 0, playerLayer);

        // Loop through all colliders and damage the player if found
        foreach (Collider2D collider in colliders)
        {
            Health playerHealth = collider.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, "Traps");
            }
        }
    }

    // You can also use OnDrawGizmos to visualize the trap's collider in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(trapCollider.bounds.center, trapCollider.bounds.size);
    }
}
