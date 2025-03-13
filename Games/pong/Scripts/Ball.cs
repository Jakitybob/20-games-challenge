using Godot;
using System;
using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

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

        // Turn off the ball
        Visible = false;
    }

    public override void _Process(double delta)
    {
        // Check if the ball is out of range on either side and update the score accordingly
        if (Position.X < -5) // Player two scored
        {
            GameController.instance.UpdatePlayerTwoScore(1);
            _ = ResetBall();
        }
        else if (Position.X > 1925) // Player one scored
        {
            GameController.instance.UpdatePlayerOneScore(1);
            _ = ResetBall();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move the ball and check for collisions
        KinematicCollision2D collision = MoveAndCollide(direction.Normalized() * speed * (float)delta);

        // If a collision occurred
        if (collision != null)
        {
            // Play bounce SFX
            GameController.instance.audioController.PlayBallHitSound();

            // Bounce the ball
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

    public async Task ResetBall()
    {
        // Play point SFX
        GameController.instance.audioController.PlayPointGainSound();

        // Recenter the ball
        Position = new Vector2(GetViewportRect().Size.X/2, GetViewportRect().Size.Y/2);
        direction = Vector2.Zero; // Zero out the direction

        // Generate a new random velocity after a time and send the ball off
        await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
        direction = GenerateRandomDirection();
    }

    public void PauseBall()
    {
        direction = Vector2.Zero;
        Visible = false;
    }
}
