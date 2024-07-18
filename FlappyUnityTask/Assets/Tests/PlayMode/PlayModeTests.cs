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
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestBirdCollidingWithPipes()
    {
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestBirdGoesBetweenPipesTriggerAddingScore()
    {
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestRestartingGameWithButton()
    {
        yield return null;
    }
}
