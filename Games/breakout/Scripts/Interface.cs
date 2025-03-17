using Godot;
using System;

public partial class Interface : CanvasLayer
{
    private Label scoreLabel, livesLabel, messageLabel;
    private Vector2 messageTitlePos, messageTimerPos;
    private Timer messageTimer;
    private VBoxContainer buttonContainer;
    private Button buttonOne, buttonTwo, buttonThree, buttonFour; // General buttons because they are reused for both { start, scores, quit } and difficulty selection

    bool inDifficultySelect = false; // Used to decide how to display buttons on main menu
    bool inScoreSave = false;
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

        // Set up positions for the message label
        messageTimerPos = messageLabel.Position;
        messageTitlePos = new Vector2(messageLabel.Position.X, messageLabel.Position.Y - 250f);
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
        // Make sure main menu parameters are reset
        inDifficultySelect = false;
        SetButtonText();

        // Set the message to Breakout title
        messageLabel.Visible = true;
        messageLabel.Text = "Breakout!";
        messageLabel.Position = messageTitlePos;

        // Enable game buttons
        buttonContainer.Visible = true;

        // Hide score and lives counters
        scoreLabel.Visible = false;
        livesLabel.Visible = false;
    }

    private void SetButtonText()
    {
        // Hide button 3 and 4
        if (inScoreSave)
        {
            buttonOne.Text = "Save Score";
            buttonTwo.Text = "No Thanks";
            buttonThree.Visible = false;
            buttonFour.Visible = false;
            return;
        }
        // Else make sure button 3 and 4 are visible
        else
        {
            buttonThree.Visible = true;
            buttonFour.Visible = true;
        }
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

        // Make sure game is no longer set to over
        GameController.instance.isGameOver = false;

        // Hide buttons
        buttonContainer.Visible = false;

        // Enable message timer
        messageTimer.Start();
        isCountingDown = true;
        messageLabel.Position = messageTimerPos;
    }

    public void GameOverInterface()
    {
        // Set game over message and start timer
        messageLabel.Position = messageTitlePos;
        messageLabel.Text = "Game Over!";
        messageLabel.Visible = true;

        // Enable the buttons and enter score saving mode
        inScoreSave = true;
        SetButtonText();
        buttonContainer.Visible = true;

        //messageTimer.Start(); // Start the message timer without enabling the countdown bool
    }

    public void GameWonInterface()
    {
        // Set the victory message
        messageLabel.Position = messageTitlePos;
        messageLabel.Text = "You won!";
        messageLabel.Visible = true;

        // Enable the buttons and enter score saving mode
        inScoreSave = true;
        SetButtonText();
        buttonContainer.Visible = true;
    }

    private void OnButtonOnePressed()
    {
        // Enter score saving
        if (inScoreSave)
        {
            // TODO: Implement
        }
        // Enter difficulty select if not in difficulty select mode
        else if (!inDifficultySelect)
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
        // Return to main menu if in score save
        if (inScoreSave)
        {
            inScoreSave = false;
            EnableMainMenuInterface();
        }
        // Show local leaderboard if not in difficulty select mode
        else if (!inDifficultySelect)
        {
            // TODO: IMPLEMENT
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
        // Disable difficulty select and re-set the button text
        inDifficultySelect = false;
        SetButtonText();
    }

    private void OnMessageTimerTimeout()
    {
        // Run start game logic if not game over
        if (!GameController.instance.isGameOver)
        {
            // Toggle the timer and message label
            isCountingDown = false;
            messageLabel.Visible = false;

            // TODO: Start the game here
            EmitSignal(SignalName.StartGame, difficulty);
        }
        // Else run game over logic
        else
        {
            EnableMainMenuInterface();
        }
    }
}
