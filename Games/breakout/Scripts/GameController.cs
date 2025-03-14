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

    public override void _EnterTree() // So that it registers prior to other objects existing
    {
        // Make sure this is the only GameController instance
        if (instance != null)
        {
            GD.PushError("More than one GameController instance detected!");
            QueueFree();
        }

        // Make sure a brick object has been added
        if (brickObject == null)
        {
            GD.PushError("No Brick Object was set!");
        }

        instance = this;
    }

    public override void _Ready()
    {
        // Get the UI canvas layer
        userInterface = GetNode<Interface>("Interface");

        // Generate the bricks
        // TODO: Tie this into main menu with difficulty selection
        GenerateBricks(new Vector2(960, 150), 5, 23);
        bricksRemaining = totalBricks;
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
}
