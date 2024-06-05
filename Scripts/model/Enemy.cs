using Godot;
using System;

public partial class Enemy : Node
{
	public float speed = -250f;
	public float MinotaurHealth = 100;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MinotaurHealth = 120f;
	}




}
