using Godot;
using System;

public partial class Brick : StaticBody2D
{
    public override void _ExitTree()
    {
        GameController.instance.RemoveBrick();
    }
}
