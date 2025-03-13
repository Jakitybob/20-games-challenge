using Godot;
using System;

public partial class AudioController : AudioStreamPlayer
{
    [Export] AudioStreamMP3 ballHitSound;
    [Export] AudioStreamMP3 pointGainSound;

    public void PlayBallHitSound()
    {
        Stream = ballHitSound;
        Play();
    }

    public void PlayPointGainSound()
    {
        Stream = pointGainSound;
        Play();
    }
}
