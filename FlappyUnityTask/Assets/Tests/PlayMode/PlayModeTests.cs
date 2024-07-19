using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayModeTests
{


    [UnityTest]
    public IEnumerator TestPipeMovesLeft()
    {
        var pipe = new GameObject("Pipe").AddComponent<PipeMoveScript>();
        pipe.moveSpeed = 10f;  // Set the speed for testing
        var initialPosition = pipe.transform.position;

        // Wait for a few frames to let the pipe move
        yield return new WaitForSeconds(1f);

        // Assert that the pipe has moved left
        Assert.Less(pipe.transform.position.x, initialPosition.x);

        Debug.Log($"pipe.transform.position.x = {pipe.transform.position.x} , initialPosition.x = {initialPosition.x}");
    }

    [UnityTest]
    public IEnumerator TestPipesAreSpawnedAndLimited()
    {
        // Create a pipe prefab with PipeMoveScript attached
        var pipePrefab = new GameObject("Pipe");
        pipePrefab.AddComponent<PipeMoveScript>();

        // Create a spawner and set its properties
        var spawner = new GameObject("Pipe Spawner").AddComponent<PipeSpawnerScript>();
        spawner.pipe = pipePrefab;
        spawner.spawnRate = 1f;

        // Wait for a bit longer than the spawn rate to ensure a pipe is spawned
        yield return new WaitForSeconds(2f);

        // Assert that at least 1 pipe has been spawned and it is limited by 10
        Assert.IsTrue(GameObject.FindObjectsOfType<PipeMoveScript>().Length > 0);
        Assert.IsTrue(GameObject.FindObjectsOfType<PipeMoveScript>().Length < 10);

        Debug.Log($"number of pipes: {GameObject.FindObjectsOfType<PipeMoveScript>().Length}");
    }

    [UnityTest]
    public IEnumerator TestAtStartGameIsFrozen()
    {
        // Arrange

        var logicGameObject = new GameObject("Logic");
        var logicScript = logicGameObject.AddComponent<LogicScript>();

        // Act
        // Wait for a frame to ensure Start is processed
        yield return null;

        // Assert
        Assert.AreEqual(0, Time.timeScale, "Game is not frozen at the start.");

        // Unfreeze the game to clean up
        Time.timeScale = 1;
        Debug.Log("resueming after check game is frozen");
    }

    [UnityTest]
    public IEnumerator TestBirdMovement()
    {
        // Arrange
        GameObject bird = new GameObject();
        var rigidBody = bird.AddComponent<Rigidbody2D>();
        rigidBody.gravityScale = 4.5f; // Set gravity scale to 4.5
        var initialPosition = rigidBody.transform.position;
        float tempPosition;
        var flapStrength = 20f;
        var flapVelocity = Vector2.up * flapStrength;

        // Act - Initial state to check gravity
        yield return new WaitForSeconds(0.5f); // Wait for gravity to take effect

        // Assert that the object has fallen due to gravity
        Assert.Less(rigidBody.transform.position.y, initialPosition.y);

        // Store the current position after gravity has taken effect
        tempPosition = rigidBody.transform.position.y;

        // Act - Flap to move up
        rigidBody.velocity = flapVelocity;
        yield return new WaitForSeconds(0.2f); // Wait for the flap to take effect

        // Assert that the object has moved up after the flap
        Assert.Greater(rigidBody.transform.position.y, tempPosition);
    }

    public class MockBirdScript : MonoBehaviour
    {
        public bool birdIsAlive = true;

        public void gameOver()
        {
            birdIsAlive = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            gameOver();
        }
    }

    [UnityTest]
    public IEnumerator TestBirdCollidingWithPipes()
    {
        // Arrange
        var bird = new GameObject("Bird");
        var birdRigidbody = bird.AddComponent<Rigidbody2D>();
        bird.AddComponent<BoxCollider2D>();
        var mockBirdScript = bird.AddComponent<MockBirdScript>();

        var pipe = new GameObject("Pipe");
        pipe.AddComponent<BoxCollider2D>();

        // Position the pipe in front of the bird
        bird.transform.position = new Vector2(0, 0);
        pipe.transform.position = new Vector2(1, 0);

        // Act - Move the bird towards the pipe
        birdRigidbody.velocity = new Vector2(2, 0);

        // Wait for the collision to occur
        yield return new WaitForSeconds(0.1f);

        // Assert - Check if the bird is no longer alive after the collision
        Assert.IsFalse(mockBirdScript.birdIsAlive, "birdIsAlive should be false after collision.");

        Debug.Log("Bird collided with pipe and birdIsAlive was set to false.");
    }

    public class MockPipeMiddleScript : MonoBehaviour
    {
        public bool addScore = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Trigger entered");
            addScore = true;
            Debug.Log("addScore set to true");
        }
    }

    [UnityTest]
    public IEnumerator TestBirdGoesBetweenPipesTriggerAddingScore()
    {
        // Arrange
        var birdMock = new GameObject("MockBird");
        var birdRigidbodyMock = birdMock.AddComponent<Rigidbody2D>();
        var birdCollider = birdMock.AddComponent<BoxCollider2D>();
        birdRigidbodyMock.gravityScale = 0; // Ensure gravity is disabled for the bird
        birdRigidbodyMock.bodyType = RigidbodyType2D.Kinematic; // Set Rigidbody to Kinematic to prevent movement

        var middlePipe = new GameObject("MiddlePipe");
        var middlePipeCollider = middlePipe.AddComponent<BoxCollider2D>();
        middlePipeCollider.isTrigger = true;
        var mockPipeMiddleScript = middlePipe.AddComponent<MockPipeMiddleScript>();

        // Position the bird and the middle pipe
        birdMock.transform.position = new Vector3(0, 0); // Bird stays stationary
        middlePipe.transform.position = new Vector3(2, 0); // Start pipe to the right of the bird

        // Log initial positions
        Debug.Log($"Initial bird position: {birdMock.transform.position}");
        Debug.Log($"Middle pipe position: {middlePipe.transform.position}");

        float moveSpeed = 2f; // Units per second

        // Act - Move the pipe towards the bird
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            middlePipe.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Log final position
        Debug.Log($"Final bird position: {birdMock.transform.position}");
        Debug.Log($"Final pipe position: {middlePipe.transform.position}");

        // Assert - Check if the addScore flag was set
        Assert.IsTrue(mockPipeMiddleScript.addScore, "addScore should be true after pipe passes over the bird.");

        Debug.Log("Pipe passed over bird and addScore was set to true.");
    }


    [UnityTest]
    public IEnumerator TestButtonLoadsNewScene()
    {
        // Arrange
        var restartButtonGameObject = new GameObject("RestartButton");
        var restartButton = restartButtonGameObject.AddComponent<Button>();

        // Define the scene loading logic
        void LoadNewScene()
        {
            SceneManager.LoadScene("TestScene");
        }

        // Assign the scene loading logic to the button's onClick event
        restartButton.onClick.AddListener(LoadNewScene);

        // Act - Simulate the button click
        restartButton.onClick.Invoke();

        // Wait for the new scene to load
        yield return new WaitForSeconds(0.2f);

        // Assert - Check if the new scene is loaded
        Assert.AreEqual("TestScene", SceneManager.GetActiveScene().name, "The new scene was not loaded correctly.");

        Debug.Log("New scene loaded successfully.");
    }


}
