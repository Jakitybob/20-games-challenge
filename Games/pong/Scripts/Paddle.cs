using Godot;
using System;

public partial class Paddle : StaticBody2D
{
    [Export] float speed = 300f; // Arbitrary value idk man

    public override void _Process(double delta)
    {
        // Handle input to move paddle
        if (Input.IsActionPressed("up"))
        {
            Position += new Vector2(0, (float)(speed * delta * -1));
        }
        if (Input.IsActionPressed("down"))
        {
            Position += new Vector2(0, (float)(speed * delta));
        }

        // Clamp paddle to bounds of screen
        // TODO: actually clamp with the paddle size in mind :)
        Vector2 clampedPosition = new Vector2(Position.X, Mathf.Clamp(Position.Y, 0, GetViewportRect().Size.Y));
        Position = clampedPosition;
    }
}
