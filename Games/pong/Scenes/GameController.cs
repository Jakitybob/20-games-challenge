using Godot;
using System;
using System.Threading.Tasks;

public partial class GameController : Node
{
    public static GameController instance { get; private set; } // Singleton instance

    private Label playerOneScoreLabel, playerTwoScoreLabel;
    private int playerOneScore = 0, playerTwoScore = 0;

    public override void _Ready()
    {
        // Make sure there is only one instance of the game controller
        if (instance != null)
        {
            GD.PushError("More than one GameController instance detected!");
            QueueFree(); // Delete this instance
        }

        instance = this;

        // Get the score labels for each side
        playerOneScoreLabel = GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("PlayerOneScore");
        playerTwoScoreLabel = GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("PlayerTwoScore");
    }

    public void UpdatePlayerOneScore(int points)
    {
        playerOneScore += points;
        playerOneScoreLabel.Text = playerOneScore.ToString();
    }

    public void UpdatePlayerTwoScore(int points)
    {
        playerTwoScore += points;
        playerTwoScoreLabel.Text = playerTwoScore.ToString();
    }
}
