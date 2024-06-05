using Godot;
using System;

public partial class Collectible : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _on_collectible_area_area_entered(Area2D area)
	{
		GD.Print("I'm in zone");
		PlayerModel.TakeMana(20);
		CallDeferred("free");
	}
}
