using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntressArrowController : MonoBehaviour
{
    public GameObject arrowObject;  // Reference to the Arrow GameObject
    public float arrowSpeed = 5f;   // Adjust the arrow speed as needed

    public GameObject player;

    private Vector2 initialPosition;
    // public Animator arrowanim;

    private void Start()
    {
        // Disable the arrowObject at the beginning
        arrowObject.SetActive(false);
        initialPosition = arrowObject.transform.position;
    }

    // Called by the Huntress's attack animation event
    public void StartArrowAttack()
    {
        // Enable the arrowObject when the attack animation starts
        arrowObject.SetActive(true);

        // Calculate the direction towards the player
        Vector2 direction = (PlayerPosition() - (Vector2)arrowObject.transform.position).normalized;

        // Set the arrow's velocity based on the calculated direction and speed
        arrowObject.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * arrowSpeed, 0f);

        StartCoroutine(DisableArrowAfterDelay());

        arrowObject.transform.position = initialPosition;
    }

    private IEnumerator DisableArrowAfterDelay()
    {
        yield return new WaitForSeconds(0.8f);
        arrowObject.transform.position = initialPosition;  
        if (arrowObject.activeSelf)
        {
            arrowObject.SetActive(false);
        }
    }

    private Vector2 PlayerPosition()
    {
        if (player != null)
        {
            return player.transform.position;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
