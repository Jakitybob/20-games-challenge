using Godot;
using System;
using System.Linq.Expressions;
using System.Threading;

public partial class Ball : RigidBody2D
{
    [Export] float speed = 200f;

    private Vector2 direction;

    public override void _Ready()
    {
        // Disable gravity
        GravityScale = 0;

        // Activate monitoring
        ContactMonitor = true;

        // Set ball off with base direction
        direction = GenerateRandomDirection();
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move the ball and check for collisions
        KinematicCollision2D collision = MoveAndCollide(direction.Normalized() * speed * (float)delta);

        // If a collision occurred
        if (collision != null)
        {
            Vector2 reflect = collision.GetRemainder().Reflect(collision.GetNormal()); // Get the immediate reflection of the ball from the surface
            direction = direction.Bounce(collision.GetNormal()) * (float)delta; // Get the new direction based on the bounce
            MoveAndCollide(reflect); // Move the ball once in the direction of the reflection
        }
    }

    private Vector2 GenerateRandomDirection()
    {
        // Create the direction vector to return later
        Vector2 newDirection;

        // Generate the angle from -45 to 45 degrees
        float angle = (float)GD.RandRange(-1f, 1f);

        // Set the direction to the left if random number is even
        if (GD.Randi() % 2 == 0)
        {
            newDirection = Vector2.Left;
        }
        // Else set the direction to right if the number is odd
        else
        {
            newDirection = Vector2.Right;
        }

        newDirection.Y = angle;
        return newDirection.Normalized();
    }
}
