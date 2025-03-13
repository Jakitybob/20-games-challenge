using Godot;
using System;

public partial class Interface : CanvasLayer
{
    private Label playerOneScoreLabel, playerTwoScoreLabel, messageLabel, controlLabelOne, controlLabelTwo;
    private TextureRect divider;
    private VBoxContainer buttonBox;

    public override void _Ready()
    {
        // Get the game interface, score labels, and hide them all
        playerOneScoreLabel = GetNode<Label>("PlayerOneScore");
        playerTwoScoreLabel = GetNode<Label>("PlayerTwoScore");
        controlLabelOne = GetNode<Label>("ControlLabelOne");
        controlLabelTwo = GetNode<Label>("ControlLabelTwo");
        divider = GetNode<TextureRect>("TextureRect");
        messageLabel = GetNode<Label>("MessageLabel");
        buttonBox = GetNode<VBoxContainer>("ButtonBox");

        StartMenuInterface();
    }

    public void StartGameInterface()
    {
        playerOneScoreLabel.Visible = true;
        playerTwoScoreLabel.Visible = true;
        divider.Visible = true;

        messageLabel.Visible = false;
        controlLabelOne.Visible = false;
        controlLabelTwo.Visible = false;
        buttonBox.Visible = false;
    }

    public void StartMenuInterface()
    {
        playerOneScoreLabel.Visible = false;
        playerTwoScoreLabel.Visible = false;
        divider.Visible = false;

        messageLabel.Visible = true;
        controlLabelOne.Visible = true;
        controlLabelTwo.Visible = true;
        buttonBox.Visible = true;
    }

    public void UpdatePlayerOneScore(int points)
    {
        playerOneScoreLabel.Text = points.ToString();
    }

    public void UpdatePlayerTwoScore(int points)
    {
        playerTwoScoreLabel.Text = points.ToString();
    }

    private void OnTwoPlayerButtonPressed()
    {
        StartGameInterface();
        GameController.instance.StartGame(false);
        // Call start game from GameController
    }

    private void OnOnePlayerButtonPressed()
    {
        StartGameInterface();
        GameController.instance.StartGame(true);
    }

    private void OnExitGameButtonPressed()
    {
        GetTree().Quit();
    }
}
