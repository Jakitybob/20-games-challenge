using Godot;
using System;

public enum BrickColors
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple
}

public partial class GameController : Node
{
    public static GameController instance;

    private Interface userInterface;

    [Export] PackedScene brickObject;
    [Export] int brickSeparation = 25;
    private int totalBricks = 0;
    private int bricksRemaining = 0;

    private int lives = 3;
    public bool isGameOver = false;

    public override void _EnterTree() // So that it registers prior to other objects existing
    {
        // Make sure this is the only GameController instance
        if (instance != null)
        {
            GD.PushError("More than one GameController instance detected!");
            QueueFree();
        }

        instance = this;

        // Make sure a brick object has been added
        if (brickObject == null)
        {
            GD.PushError("No Brick Object was set!");
        }
    }

    public override void _Ready()
    {
        // Get the UI canvas layer
        userInterface = GetNode<Interface>("Interface");

        // Enable the main menu
        userInterface.EnableMainMenuInterface();
    }

    // Called when the interface's countdown finishes and the game should start
    private void OnInterfaceStartGame(int difficulty)
    {
        // Make sure the game is not set to over
        isGameOver = false;

        // Reset score and lives
        userInterface.UpdateScore(0);
        lives = 3;
        userInterface.UpdateLives(lives);

        // Get the paddle so its minimum size can be updated based on difficulty and recenter it
        Paddle paddle = GetNode<Paddle>("Paddle");
        paddle.Position = paddle.startingPosition;

        // Generate bricks based on difficulty
        switch (difficulty)
        {   
            case 1:
                GenerateBricks(new Vector2(960, 150), 3, 21);
                paddle.minimumSize = 100f;
                break;
            case 2:
                GenerateBricks(new Vector2(960, 150), 4, 21);
                paddle.minimumSize = 75f;
                break;
            case 3:
                GenerateBricks(new Vector2(960, 150), 6, 23);
                paddle.minimumSize = 35f;
                break;
            default:
                GD.PrintErr("Difficulty was incorrectly set. Fix this please.");
                break;
        }

        // Set up remaining bricks
        bricksRemaining = totalBricks;

        // Launch the ball
        GetNode<Ball>("Ball").StartBall();
    }

    public void RemoveBrick()
    {
        // Update the score
        bricksRemaining--;
        userInterface.UpdateScore(totalBricks - bricksRemaining);

        // Update the paddle size and ball speed based on the percentage of balls destroyed
        float modifier = 1 - ((float)bricksRemaining / (float)totalBricks);
        GetNode<Paddle>("Paddle").UpdatePaddleSize(modifier);
        GetNode<Ball>("Ball").UpdateSpeed(modifier);
    }

    public void RemoveLife()
    {
        // Update remaining lives and UI
        lives -= 1;
        userInterface.UpdateLives(lives);

        // TODO: Check for game over condition when out of lives
        if (lives <= 0)
        {
            isGameOver = true;
            GameOver();
        }

    }

    private void GenerateBricks(Vector2 rowCenterPos, int rows, int cols)
    {
        // Calculate the true starting X based off the center of the row
        Vector2 startingPos = new Vector2(rowCenterPos.X - (cols / 2 * brickObject.Instantiate().GetNode<ColorRect>("ColorRect").Size.X) - (cols / 2 * brickSeparation), rowCenterPos.Y);

        // Generate each row
        for (int row = 0; row < rows; row++)
        {
            // Generate each column of each row
            for (int col = 0; col < cols; col++)
            {
                // Create the brick instance
                Brick brick = (Brick)brickObject.Instantiate();
                brick.SetColor((BrickColors)row);

                // Calculate the x and y of this brick
                float x = startingPos.X + brickSeparation * col + (col * brick.GetNode<ColorRect>("ColorRect").Size.X);
                float y = startingPos.Y + brickSeparation * row + (row * brick.GetNode<ColorRect>("ColorRect").Size.Y);
                brick.Position = new Vector2(x, y);

                // Add the brick to the scene
                AddChild(brick);
                totalBricks++;
            }
        }
    }

    private void EndGame()
    {
        // TODO: Save out and serialize the scores here

        // Iterate through all children and delete any bricks found
        // This could be way more efficient but given there aren't that many bricks
        // its not worth the effort to overengineer.
        Godot.Collections.Array<Node> bricks = GetChildren();
        foreach (Node child in bricks)
        {
            if (child.GetType() == typeof(Brick))
            {
                child.QueueFree();
            }
        }

        // Remove the ball
        GetNode<Ball>("Ball").FreezeBall();
    }

    // To be used when the player runs out of lives
    private void GameOver()
    {
        EndGame();
        userInterface.GameOverInterface();
    }
}
