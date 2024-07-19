using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody; // Special slot on the script to reference Rigidbody 2D
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Finds the GameObject with the tag "Logic" and retrieves the LogicScript component attached to it
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && birdIsAlive == true)
        {
            flap();
        }

        checkBounds(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birdIsAlive = false;
    }

    public void checkBounds(Vector3 pos)
    {
        // Convert the bird's position to viewport coordinates
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(pos);
        

        // Check if the bird is outside the camera's viewport
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            // The bird is off the screen, trigger game over
            logic.gameOver();
            birdIsAlive = false;
        }
    }

    public void flap()
    {
        myRigidbody.velocity = Vector2.up * flapStrength;
    }
}
