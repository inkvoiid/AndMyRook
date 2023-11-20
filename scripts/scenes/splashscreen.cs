using Godot;
using System;

public partial class splashscreen : ColorRect
{
    [Export] private AnimationPlayer _animationPlayer;
	public override void _Ready()
	{
		GD.Print("And My Rook! beta 0.1.0");
		_animationPlayer.Play("zoom_logo");
		GetNode<Timer>("Timer").Timeout +=
		() => GetNode<sceneloader>("/root/sceneloader").ChangeToScene("titlescreen.tscn");
    }
}
