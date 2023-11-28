using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;

    public float speed = 5.0f;

    private GameObject focalPoint;

    private float powerupStrength = 15.0f;

    public GameObject powerupIndicator;

    // Powerup
    public bool hasPowerUp = false;
    // Start is called before the first frame update
    void Start()
    {
        focalPoint = GameObject.Find("Focal Point");
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput *  speed);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Powerup"))
            { 
                hasPowerUp = true;
                Destroy(other.gameObject);
                // Once the above is true, timer starts
                StartCoroutine(PowerupCountdownRoutine());
                powerupIndicator.SetActive(true);
            }
    }

    // Countdown Timer
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
                Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

                enemyRigidBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);  

               Debug.Log("Collided with: " + collision.gameObject.name + " with Powerup set to " + hasPowerUp);
        }
    }
}
