using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditModeTests
{
    private GameObject birdGameObject;
    private BirdScript birdScript;
    private Rigidbody2D myRigidbody;
    private GameObject logicGameObject;
    private LogicScript logicScript;
    private Text scoreText;
    private GameObject startTextGameObject;
    private Text startText;
    private GameObject gameOverScreenGameObject;
    private Camera mainCamera;
    private AudioSource addScoreSound;

    [SetUp]
    public void Setup()
    {
        // Create and set up the camera
        mainCamera = new GameObject("MainCamera").AddComponent<Camera>();
        mainCamera.transform.position = new Vector3(0, 0, -10); // Position the camera correctly

        // Create a new GameObject for the bird
        birdGameObject = new GameObject("Bird");
        myRigidbody = birdGameObject.AddComponent<Rigidbody2D>();
        birdScript = birdGameObject.AddComponent<BirdScript>();
        birdScript.myRigidbody = myRigidbody;
        birdScript.flapStrength = 20f; // Set a flap strength value

        // Create a new GameObject for the logic script
        logicGameObject = new GameObject("Logic");
        logicScript = logicGameObject.AddComponent<LogicScript>();

        // Create and configure the score text
        GameObject scoreTextGameObject = new GameObject("Score Text");
        scoreText = scoreTextGameObject.AddComponent<Text>();
        logicScript.scoreText = scoreText;

        // Create and configure the start text
        startTextGameObject = new GameObject("Start Text");
        startText = startTextGameObject.AddComponent<Text>();
        logicScript.startText = startText;

        // Create and configure the game over screen
        gameOverScreenGameObject = new GameObject("Game Over Screen");
        logicScript.gameOverScreen = gameOverScreenGameObject;
        logicScript.gameOverScreen.SetActive(false);

        // Set the initial score
        logicScript.playerScore = 0;
        logicScript.scoreText.text = logicScript.playerScore.ToString();

        // Add an AudioSource component
        addScoreSound = logicGameObject.AddComponent<AudioSource>();
        logicScript.addScoreSound = addScoreSound;

        // Assign the logic script to the bird script
        birdScript.logic = logicScript;

        // Assign the main camera to the bird script
        birdScript.mainCamera = mainCamera;


    }

    [TearDown]
    public void Teardown()
    {
        // Clean up all objects created in the Setup method
        Object.DestroyImmediate(birdGameObject);
        Object.DestroyImmediate(logicGameObject);
        Object.DestroyImmediate(scoreText.gameObject);
        Object.DestroyImmediate(startTextGameObject);
        Object.DestroyImmediate(gameOverScreenGameObject);
        Object.DestroyImmediate(mainCamera.gameObject);
    }

    [Test]
    public void TestAddScore()
    {
        // Define initial Score and score to add
        int initialScore = logicScript.playerScore = 1;
        int scoreToAdd = 1;

        // Call the addScore method
        logicScript.addScore(scoreToAdd);

        // Check if the player score has increased correctly
        Assert.AreEqual(initialScore + scoreToAdd, logicScript.playerScore);
        Assert.AreEqual((initialScore + scoreToAdd).ToString(), logicScript.scoreText.text);
    }


    [UnityTest]
    public IEnumerator TestAddScoreSound()
    {
        // Arrange
        logicScript.addScoreSound.clip = AudioClip.Create("TestClip", 44100, 1, 44100, false);

        logicScript.addScoreSound.Stop();

        // Act
        logicScript.addScore(10);

        // Wait for a frame to let the audio start playing
        yield return null;

        // Assert
        Assert.IsTrue(logicScript.addScoreSound.isPlaying);
    }



    [Test]
    public void TestInputUnfreezesGame()
    {
        // Arrange
        bool inputSimulateDown = true;

        //Act
        logicScript.checkToResumeGame(inputSimulateDown);

        // Assert
        Assert.AreEqual(1, Time.timeScale);
        Assert.IsFalse(logicScript.startText.gameObject.activeSelf, "startText GameObject is active, but it should be inactive.");
        Assert.IsFalse(logicScript.startTextDisplay);
    }

    [Test]
    public void TestGameOver()
    {
        // Act
        // Call the gameOver method to simulate the game over state
        logicScript.gameOver();

        // Assert
        // Check that the game over screen is now active
        Assert.IsTrue(logicScript.gameOverScreen.activeSelf, "Game over screen should be active.");

        // Check that the start text is not active
        Assert.IsFalse(logicScript.startText.gameObject.activeSelf, "startText GameObject is active, but it should be inactive.");

        // Check that the game is frozen
        Assert.AreEqual(0, Time.timeScale, "Time scale should be 0, indicating the game is frozen.");
    }

    [Test]
    public void TestCheckBoundsWhenOutOfBounds()
    {
        // Arrange: Positions that should trigger game over
        Vector3[] outOfBoundsPositions = new Vector3[]
        {
            new Vector3(-2000, 0, 0), // Left
            new Vector3(2000, 0, 0), // Right
            new Vector3(0, -2000, 0), // Bottom
            new Vector3(0, 2000, 0) // Top
        };

        foreach (var position in outOfBoundsPositions)
        {
            // Reset state before each check
            birdScript.birdIsAlive = true;
            logicScript.gameOverScreen.SetActive(false);

            // Act
            birdScript.checkBounds(position);

            // Assert
            Assert.IsFalse(birdScript.birdIsAlive, $"Bird should not be alive when out of bounds at position {position}.");
            Assert.IsTrue(logicScript.gameOverScreen.activeSelf, "Game over screen should be active.");
        }
    }

    [Test]
    public void TestFlap()
    {
        // Arrange
        myRigidbody.velocity = Vector2.zero; // Reset velocity

        // Act
        birdScript.flap();

        // Assert
        Assert.AreEqual(Vector2.up * birdScript.flapStrength, myRigidbody.velocity);
    }

    [Test]
    public void TestCheckBoundsWhenInBounds()
    {
        // Arrange: Position that should not trigger game over
        Vector3 inBoundsPosition = new Vector3(0, 0, 0);

        // Act
        birdScript.checkBounds(inBoundsPosition);

        // Assert
        Assert.IsTrue(birdScript.birdIsAlive, "Bird should be alive when in bounds.");
        Assert.IsFalse(logicScript.gameOverScreen.activeSelf, "Game over screen should not be active.");
    }

}
