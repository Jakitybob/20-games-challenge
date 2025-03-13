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
        // Recenter the paddles
        Paddle paddleOne = GetNode<Paddle>("PaddleOne");
        Paddle paddleTwo = GetNode<Paddle>("PaddleTwo");
        paddleOne.Position = new Vector2(paddleOne.Position.X, GetViewport().GetWindow().Size.Y/2);
        paddleTwo.Position = new Vector2(paddleTwo.Position.X, GetViewport().GetWindow().Size.Y/2);

        // Enable the ball and give it a direction
        Ball ball = GetNode<Ball>("Ball");
        ball.Visible = true;
        _ = ball.ResetBall(); // This syntax gets rid of the silly warning about not awaiting by "discarding" the task

        // Enable "AI" input for the left paddle if desired
        if (isAgainstAI)
        {
            GetNode<Paddle>("PaddleTwo").playerPosition = PlayerPosition.ArtificalPlayer;
        }
    }

    public void EndGame()
    {
        GetNode<Ball>("Ball").PauseBall();
        gameInterface.StartMenuInterface();
    }

    public void UpdatePlayerOneScore(int points)
    {
        playerOneScore += points;
        gameInterface.UpdatePlayerOneScore(playerOneScore);
    }

    public void UpdatePlayerTwoScore(int points)
    {
        playerTwoScore += points;
        gameInterface.UpdatePlayerTwoScore(playerTwoScore);
    }
}
