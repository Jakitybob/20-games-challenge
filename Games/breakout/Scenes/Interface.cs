using Godot;
using System;

public partial class Interface : CanvasLayer
{
    private Label scoreLabel, livesLabel, messageLabel;
    private Timer messageTimer;

    public override void _Ready()
    {
        // Get all relevant nodes of the main menu
        scoreLabel = GetNode<Label>("ScoreLabel");
        livesLabel = GetNode<Label>("LivesLabel");
        messageLabel = GetNode<Label>("MessageLabel");
        messageTimer = GetNode<Timer>("MessageTimer");
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = "Score: " + score.ToString();
    }

    public void UpdateLives(int lives)
    {
        livesLabel.Text = "Lives: " + lives.ToString();
    }
}
