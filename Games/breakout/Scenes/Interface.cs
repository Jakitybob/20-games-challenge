using Godot;
using System;

public partial class Interface : CanvasLayer
{
    private Label scoreLabel, livesLabel, messageLabel;
    private Timer messageTimer;
    private VBoxContainer buttonContainer;
    private Button buttonOne, buttonTwo, buttonThree, buttonFour; // General buttons because they are reused for both { start, scores, quit } and difficulty selection

    bool inDifficultySelect = false; // Used to decide how to display buttons on main menu
    bool isCountingDown = false;
    int difficulty = 1; // The difficulty to pass out, set by the buttons

    [Signal] public delegate void StartGameEventHandler(int difficulty);

    public override void _Ready()
    {
        // Get all relevant nodes of the main menu
        scoreLabel = GetNode<Label>("ScoreLabel");
        livesLabel = GetNode<Label>("LivesLabel");
        messageLabel = GetNode<Label>("MessageLabel");
        messageTimer = GetNode<Timer>("MessageTimer");
        buttonContainer = GetNode<VBoxContainer>("VBoxContainer");
        buttonOne = buttonContainer.GetNode<Button>("Button1");
        buttonTwo = buttonContainer.GetNode<Button>("Button2");
        buttonThree = buttonContainer.GetNode<Button>("Button3");
        buttonFour = buttonContainer.GetNode<Button>("Button4");
    }

    public override void _Process(double delta)
    {
        // if counting down, update the message label
        if (isCountingDown)
        {
            if (messageTimer.TimeLeft < 1)
            {
                messageLabel.Text = "Start!";
            }
            else
            {
                messageLabel.Text = ((int)messageTimer.TimeLeft).ToString() + "...";
            }
        }
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = "Score: " + score.ToString();
    }

    public void UpdateLives(int lives)
    {
        livesLabel.Text = "Lives: " + lives.ToString();
    }

    public void EnableMainMenuInterface()
    {
        // Set the message to Breakout title
        messageLabel.Visible = true;
        messageLabel.Text = "Breakout!";
        messageLabel.Position = new Vector2(messageLabel.Position.X, messageLabel.Position.Y - 250f); // Increase the offset by 250px up for the title card

        // Enable game buttons
        buttonContainer.Visible = true;

        // Hide score and lives counters
        scoreLabel.Visible = false;
        livesLabel.Visible = false;
    }

    private void SetButtonText()
    {
        // Set up standard button text
        if (!inDifficultySelect)
        {
            buttonOne.Text = "Start Game";
            buttonTwo.Text = "Leaderboard";
            buttonThree.Text = "Quit Game";
            buttonFour.Visible = false;
        }
        // Else set up difficulty button text
        else 
        {
            buttonOne.Text = "Level One";
            buttonTwo.Text = "Level Two";
            buttonThree.Text = "Level Three";
            buttonFour.Visible = true;
        }
    }

    public void EnableGameplayInterface()
    {
        // Enable score and life counters
        scoreLabel.Visible = true;
        livesLabel.Visible = true;

        // Hide buttons
        buttonContainer.Visible = false;

        // Enable message timer
        messageTimer.Start();
        isCountingDown = true;
        messageLabel.Position = new Vector2(messageLabel.Position.X, messageLabel.Position.Y + 250f);
    }

    private void OnButtonOnePressed()
    {
        // Enter difficulty select if not in difficulty select mode
        if (!inDifficultySelect)
        {
            inDifficultySelect = true;
            SetButtonText();
        }
        // Otherwise select difficulty one and start the game
        else 
        {
            difficulty = 1;
            EnableGameplayInterface();
        }
    }

    private void OnButtonTwoPressed()
    {
        // Show local leaderboard if not in difficulty select mode
        if (!inDifficultySelect)
        {

        }
        // Otherwise select difficulty two and start the game
        else 
        {
            difficulty = 2;
            EnableGameplayInterface();
        }
    }

    private void OnButtonThreePressed()
    {
        // Quit game if not in difficulty select mode
        if (!inDifficultySelect)
        {
            GetTree().Quit();
        }
        // Otherwise select difficulty three and start the game
        else 
        {
            difficulty = 3;
            EnableGameplayInterface();
        }
    }

    private void OnButtonFourPressed()
    {
        // Disble difficulty select and re-set the button text
        inDifficultySelect = false;
        SetButtonText();
    }

    private void OnMessageTimerTimeout()
    {
        // Toggle the timer and message label
        isCountingDown = false;
        messageLabel.Visible = false;

        // TODO: Start the game here
        EmitSignal(SignalName.StartGame, difficulty);
    }
}
