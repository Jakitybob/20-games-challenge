using Godot;
using System;

public partial class Ball : RigidBody2D
{
    [Export] float speed = 200f;

    public override void _Ready()
    {
        // Activate contact monitoring
        ContactMonitor = true;
        MaxContactsReported = 5;

        LinearVelocity = new Vector2(-250f, 0); // Make this a random direction towards either side
    }

    public void OnBodyEntered(Node body)
    {
        GD.Print("Boop!");
    }
}
