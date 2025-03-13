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
    [Export] public PlayerPosition playerPosition = PlayerPosition.PlayerOne; // Default paddles to player one

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
        if (Input.IsActionPressed("escape")) // Should be done in an input manager but I'm doing it here instead :^)
        {
            GameController.instance.EndGame();
        }

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

            return;
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

            return;
        }
        else if (playerPosition == PlayerPosition.ArtificalPlayer) // if against "ai"
        {
            // handle AI movement here

            // Can't follow ball too quickly or will be impossible
            // How to add variance to the AI to make it play but not impossibly well?
            // RNG a 1 in 3 chance to miss bouncing the ball?

            // Get the ball to help calculate position of the paddle
            Ball ball = GetParent().GetNode<Ball>("Ball");

            // Only move if the ball is in the final fifth-ish of the court
            if (ball.Position.X > GetViewportRect().Size.X * 0.85)
            {
                // Calculate whether to go up or down
                if (ball.Position.Y > Position.Y)
                {
                    Position += new Vector2(0, (float)(speed / 1.5 * delta));
                }
                else if (ball.Position.Y < Position.Y)
                {
                    Position += new Vector2(0, (float)(speed / 1.5 * delta * -1));
                }
            }
        }
    }
}
