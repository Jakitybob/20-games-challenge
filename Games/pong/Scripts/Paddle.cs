using Godot;
using System;

public enum PlayerPosition
{
    PlayerOne,
    PlayerTwo,
    ArtificalPlayer
}

public partial class Paddle : StaticBody2D
{
    [Export] float speed = 300f; // Arbitrary value idk man
    [Export] PlayerPosition playerPosition = PlayerPosition.PlayerOne; // Default paddles to player one

    public override void _Process(double delta)
    {
        HandleInput((float)delta);

        // Clamp paddle to bounds of screen with paddle and wall size in mind
        float height = GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.Y;
        float wallHeight = GetTree().CurrentScene.GetNode<StaticBody2D>("Wall").GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.Y;
        Vector2 clampedPosition = new Vector2(Position.X, Mathf.Clamp(Position.Y, 0 + height / 2 + wallHeight, GetViewportRect().Size.Y - height/2 - wallHeight));
        Position = clampedPosition;
    }

    private void HandleInput(float delta)
    {
        if (playerPosition == PlayerPosition.PlayerOne) // if player one
        {
            if (Input.IsActionPressed("up_player_one"))
            {
                Position += new Vector2(0, (float)(speed * delta * -1));
            }
            if (Input.IsActionPressed("down_player_one"))
            {
                Position += new Vector2(0, (float)(speed * delta));
            }
        }
        else if (playerPosition == PlayerPosition.PlayerTwo) // if player two
        {
            if (Input.IsActionPressed("up_player_two"))
            {
                Position += new Vector2(0, (float)(speed * delta * -1));
            }
            if (Input.IsActionPressed("down_player_two"))
            {
                Position += new Vector2(0, (float)(speed * delta));
            }
        }
    }
}
