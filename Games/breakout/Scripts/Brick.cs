using Godot;
using System;

public partial class Brick : StaticBody2D
{
    public void SetColor(BrickColors color)
    {
        ColorRect sprite = GetNode<ColorRect>("ColorRect");

        if (color == BrickColors.Red)
        {
            sprite.Color = Colors.Red;
        }
        else if (color == BrickColors.Orange)
        {
            sprite.Color = Colors.Orange;
        }
        else if (color == BrickColors.Yellow)
        {
            sprite.Color = Colors.Yellow;
        }
        else if (color == BrickColors.Green)
        {
            sprite.Color = Colors.Green;
        }
        else if (color == BrickColors.Blue)
        {
            sprite.Color = Colors.Blue;
        }
        else if (color == BrickColors.Purple)
        {
            sprite.Color = Colors.Purple;
        }
    }

    public override void _ExitTree()
    {
        GameController.instance.RemoveBrick();
    }
}
