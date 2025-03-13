using Godot;
using System;
using System.Threading.Tasks;

public partial class GameController : Node
{
    public static GameController instance { get; private set; } // Singleton instance

    private Interface gameInterface;
    private int playerOneScore = 0, playerTwoScore = 0;

    public AudioController audioController { get; private set; }

    public override void _Ready()
    {
        // Make sure there is only one instance of the game controller
        if (instance != null)
        {
            GD.PushError("More than one GameController instance detected!");
            QueueFree(); // Delete this instance
        }

        instance = this;

        // Get the menu interface
        gameInterface = GetNode<Interface>("Interface");

        // Get the AudioStreamPlayer
        audioController = GetNode<AudioController>("AudioStreamPlayer");
    }

    public void StartGame(bool isAgainstAI) 
    {
        Ball ball = GetNode<Ball>("Ball");
        ball.Visible = true;
        _ = ball.ResetBall();
    }

    public void UpdatePlayerOneScore(int points)
    {
        playerOneScore += points;
        gameInterface.UpdatePlayerOneScore(points);
    }

    public void UpdatePlayerTwoScore(int points)
    {
        playerTwoScore += points;
        gameInterface.UpdatePlayerTwoScore(points);
    }
}
