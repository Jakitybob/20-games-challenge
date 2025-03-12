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

        // Clamp paddle to bounds of screen with paddle and wall size in mind
        float height = GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.Y;
        float wallHeight = GetTree().CurrentScene.GetNode<StaticBody2D>("Wall").GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.Y;
        Vector2 clampedPosition = new Vector2(Position.X, Mathf.Clamp(Position.Y, 0 + height / 2 + wallHeight, GetViewportRect().Size.Y - height/2));
        Position = clampedPosition;
    }
}
