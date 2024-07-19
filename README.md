# FlappyBirdUnity
Flappy Bird Game made in Unity with basic tests with the help of Unity Test Framework

## Overview of The Task:

1. Downloaded and installed Unity Hub from the official Unity website.

2. Used Unity Hub to install Unity Editor version 2022.3.10f1.

3. Followed a tutorial to create a simple Flappy Bird game in Unity.

4. Watched a tutorial to understand the basics of Unity's testing framework.

5. Installed the Unity Test Framework via the Package Manager.

6. Wrote unit tests to verify the functionality of individual methods and components.

7. Wrote integration tests to verify that different components of the game work together correctly.

## Making Flappy Bird Steps:

1. Setting the architecture of the game involves organizing everything into the correct folders, such as creating folders for Art, Sounds, Prefabs, Scenes, and Scripts.

2. I learned about GameObjects as containers and created my first SpriteRenderer, adding a bird image to it.

3. To make the bird object interact with physics, I added a Rigidbody2D component to enable gravity effects and wrote a script that allows the user to press the spacebar to make the bird fly upwards.

4. I created a pipe prefab with a script that moves the pipes to the left, creating the illusion of infinite movement for the bird. To manage memory efficiently, the script deletes pipes that move outside the camera's view. Additionally, I implemented a Pipe Spawner component to ensure that new pipes are generated at regular intervals.

5. Implementing the game logic and UI, I ensured that the player's score is tracked and displayed. The score increases each time the bird successfully passes between pipes.

6. Ensuring that when the bird hits the pipes or goes beyond the camera boundary, the bird's status changes to dead, the game freezes, and a game over UI with a play again button is displayed.

7. I added a 'Press Space to Start' note for the player and ensured that the game remains frozen at the beginning until the user presses the spacebar for the first time.

## Challenges Encountered:

1. Setting up the test environment and understanding the Unity Test Framework.

2. How to set up a project for testing. Making sure there are Edit Mode tests and Play Mode tests.

3. If generally the tests are arranged in three steps - Arrange, Act, Assert, then correctly assigning the different components to test is crucial.

4. Not being able to test things smoothly, I had to make certain assumptions.

5. Some tests in Play Mode don't work. The issue arises when setting up components for the tests, not during the testing itself.

## How Challenges Were Resolved:

1. Successfully set up the test environment by following tutorials and reading documentation, which helped in understanding the Unity Test Framework and how to integrate it into the project.

2. Learning the difference between Edit Mode tests and Play Mode tests. The difference is that Play Mode in Unity simulates the game running in real-time to test dynamic behaviors and interactions, while Edit Mode allows you to design and configure the game’s assets and components statically without running the game. That’s why integration tests would be most likely in Play Mode while unit tests would be most likely in Edit Mode.

3. Gained a solid understanding of the Arrange, Act, Assert pattern in testing, which emphasized the importance of correctly assigning and setting up different components to ensure accurate and effective tests.

4. To address the difficulty of testing smoothly, I made the game functions more general and modular. This approach allowed me to isolate individual components and test them under assumed conditions.

5. To resolve issues with Play Mode tests arising during component setup, I implemented mock tests to simplify and isolate the testing environment.

## Tests Overview:

### Unit Tests (Edit Mode):

1. **TestAddScore**: Verifies that the player's score increases correctly and updates the score text when the `addScore` method is called.

2. **TestInputUnfreezesGame**: Ensures that the game unfreezes, the start text is hidden, and the game state updates correctly when the space key is pressed.

3. **TestGameOver**: Checks that the game over screen is activated, the start text is hidden, and the game is frozen when the `gameOver` method is called.

4. **TestCheckBoundsWhenOutOfBounds**: Confirms that the bird's status changes to dead and the game over screen is shown when the bird moves out of the camera bounds.

5. **TestFlap**: Verifies that the bird's velocity is updated correctly to simulate a flap when the `flap` method is called.

6. **TestCheckBoundsWhenInBounds**: Ensures that the bird remains alive and the game over screen is not shown when the bird stays within the camera bounds.

7. **TestAddScoreSound**: Verifies that the score-increase sound is played correctly when the addScore method is called.

### Integration Tests (Play Mode):

1. **TestPipeMovesLeft**: Verifies that a pipe moves left over time when its move speed is set.

2. **TestPipesAreSpawnedAndLimited**: Ensures that pipes are spawned at regular intervals and that the total number of pipes is limited to prevent excessive memory usage.

3. **TestAtStartGameIsFrozen**: Confirms that the game is initially frozen at the start by checking that the time scale is set to zero.

### The following tests have been written with mocking:

1. **TestBirdMovement**: Verifies that the bird moves correctly according to player input and game physics.

2. **TestBirdCollidingWithPipes**: Ensures that the game correctly handles collisions between the bird and pipes, triggering game over conditions.

3. **TestBirdGoesBetweenPipesTriggerAddingScore**: Confirms that the score increases when the bird successfully passes between pipes.

4. **TestButtonLoadsNewScene**: Verifies that the game loads new scene (restarts in the context of the game) correctly when the restart button is pressed after a game over.

### Issues Identified:

1. **Edit Mode Tests**: Initially encountered `NullReferenceException` errors due to improper setup of components.

2. **Play Mode Tests**: Some tests failed because components were not set up correctly, causing runtime errors.

### Issues Resolved:

1. **Edit Mode Tests**: Refined the setup methods to ensure all necessary components (like `Rigidbody2D`, `Camera`, and `LogicScript`) were correctly instantiated and assigned, which resolved the `NullReferenceException` errors.

2. **Play Mode Tests**: Improved the test setup by ensuring all components were properly initialized. For unresolved tests, implemented mock tests to isolate and verify specific functionalities.

---
