using Godot;
using System;

public partial class Paddle : StaticBody2D
{
    [Export] float speed = 350f; // Arbitrary value idk man
    [Export] float startingWidth = 256f;
    private ColorRect sprite;
    private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        // Get the sprite and collision shape
        sprite = GetNode<ColorRect>("ColorRect");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        // Set up the sprite and collision shape size
        UpdatePaddleSize(0);
    }

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

    public void UpdatePaddleSize(float modifier)
    {
        // Calculate the new width
        float width = startingWidth - startingWidth * modifier;
        Mathf.Clamp(width, 35f, startingWidth); // Make sure the minimum size is 35px

        // Update the paddle parameters
        sprite.Size = new Vector2(width, sprite.Size.Y);
        sprite.Position = new Vector2(-1 * sprite.Size.X / 2, -1 * sprite.Size.Y / 2); // Make sure the color rect stays centered
        RectangleShape2D rect = new RectangleShape2D();
        rect.Size = new Vector2(startingWidth - startingWidth * modifier, collisionShape.Shape.GetRect().Size.Y);
        collisionShape.Shape = rect;
    }
}
