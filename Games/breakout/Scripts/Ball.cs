using Godot;
using System;
using System.Threading.Tasks;

public partial class Ball : RigidBody2D
{
    [Export] float startingSpeed = 450f;
    private float currentSpeed;
    [Export] Vector2 startingPos = new Vector2(960, 880);

    private Vector2 direction;

    public override void _Ready()
    {
        // Disable gravity
        GravityScale = 0;

        // Activate monitoring
        ContactMonitor = true;

        // Set up basic parameters
        //Visible = false;
        currentSpeed = startingSpeed;

        // TODO: Remove
        direction = GenerateRandomDirection();
    }

    public override void _Process(double delta)
    {
        // Reset the ball if it goes out of bounds
        if (Position.Y > GetViewportRect().Size.Y)
        {
            _ = ResetBall(); // Syntax "discards" the async task so it doesn't give me that annoying warning :^)
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move the ball and check for collisions
        KinematicCollision2D collision = MoveAndCollide(direction.Normalized() * currentSpeed * (float)delta);

        // If a collision occurred
        if (collision != null)
        {
            // TODO: Play bounce SFX

            // Bounce the ball
            Vector2 reflect = collision.GetRemainder().Reflect(collision.GetNormal()); // Get the immediate reflection of the ball from the surface
            direction = direction.Bounce(collision.GetNormal()); // Get the new direction based on the bounce
            MoveAndCollide(reflect); // Move the ball once in the direction of the reflection

            // Check if a brick was hit and delete it if so
            if (collision.GetCollider().GetType() == typeof(Brick))
            {
                collision.GetCollider().Free();
            }
        }
    }

    private Vector2 GenerateRandomDirection()
    {
        // Create the direction vector to return later
        Vector2 newDirection;

        // Generate the angle from -45 to 45 degrees
        float angle = (float)GD.RandRange(-1f, 1f);

        newDirection = Vector2.Up;
        newDirection.X = angle;

        return newDirection.Normalized();
    }

    public async Task ResetBall()
    {
        // Remove a life
        // NOTE: This should likely be a signal that is called with the game controller listening BUT I am lazy and this is a small project :^)
        GameController.instance.RemoveLife();

        // Recenter the ball
        Position = startingPos;
        direction = Vector2.Zero; // Zero out the direction

        // Generate a new random velocity after a time and send the ball off
        await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
        direction = GenerateRandomDirection();
    }

    public void FreezeBall()
    {
        direction = Vector2.Zero;
        Visible = false;
    }

    public void UpdateSpeed(float modifier)
    {
        currentSpeed = startingSpeed + startingSpeed * modifier;
    }
}
