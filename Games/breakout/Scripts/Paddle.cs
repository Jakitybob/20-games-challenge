using Godot;
using System;

public partial class Paddle : StaticBody2D
{
    [Export] float speed = 350f; // Arbitrary value idk man

    public override void _Process(double delta)
    {
        HandleInput((float)delta);

        // Clamp paddle to bounds of screen with paddle and wall size in mind
        float width = GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.X;
        Vector2 clampedPosition = new Vector2(Mathf.Clamp(Position.X, 0 + width / 2, GetViewportRect().Size.X - width/2), Position.Y);
        Position = clampedPosition;
    }

    private void HandleInput(float delta)
    {
        if (Input.IsActionPressed("escape")) // Should be done in an input manager but I'm doing it here instead :^)
        {
            //GameController.instance.EndGame();
        }

        if (Input.IsActionPressed("left"))
        {
            Position -= new Vector2(speed * delta, 0);
        }
        if (Input.IsActionPressed("right"))
        {
            Position += new Vector2(speed * delta, 0);
        }
    }
}
