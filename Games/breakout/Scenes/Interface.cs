using Godot;
using System;

public partial class Interface : CanvasLayer
{
    private Label scoreLabel;

    public override void _Ready()
    {
        scoreLabel = GetNode<Label>("ScoreLabel");
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = "Score: " + score.ToString();
    }
}
