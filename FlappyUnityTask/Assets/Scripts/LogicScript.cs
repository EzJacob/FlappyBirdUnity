using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public Text scoreText;
    public GameObject gameOverScreen;
    public Text startText;
    public bool startTextDisplay = true;

    // Start is called before the first frame update
    void Start()
    {
        // Freeze the game at the start
        Time.timeScale = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        checkToResumeGame(Input.GetKeyDown(KeyCode.Space));
    }

        // Function to add score to player
    [ContextMenu("Increase Score")] // Allows this function to be run from the Unity editor's context menu
    public void addScore(int scoreToAdd)
    {
        playerScore += scoreToAdd; // Update the player's score
        scoreText.text = playerScore.ToString(); // Update the score display text
    }

    public void restartGame()
    {
        // Reloads the current active scene, effectively restarting the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        // Displaying the game over screen
        gameOverScreen.SetActive(true);

        // Hide the start text
        startText.gameObject.SetActive(false);

        // Freeze the game
        Time.timeScale = 0;
    }

    public void checkToResumeGame(bool userInputPressed)
    {
        // Check for space key press to resume the game
        if (userInputPressed == true && startTextDisplay == true)
        {
            Time.timeScale = 1; // Resume the game
            // Hide the start text
            startText.gameObject.SetActive(false);
            startTextDisplay = false;
        }
    }
}
