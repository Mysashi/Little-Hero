using Godot;
using System;

public partial class Game : Node2D
{
	private AudioStreamPlayer2D audio;
	public static bool reload;
	public Camera2D camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//audio = GetNode<AudioStreamPlayer2D>("backGround");
		var p = GetTree().CurrentScene.Name;
		
        if (p == "Level01")
		{
            //audio.Play(GlobalVars.musicProgress);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (reload)
        {
			//GlobalVars.musicProgress = audio.GetPlaybackPosition();
            GetTree().ReloadCurrentScene();
			reload = false;
		}
	}

    public void OnTeleport(CharacterBody2D body)
	{
		var p = body.GetTree().CurrentScene.Name;
		GD.Print(body);
		if (p == "Level01")
		{
            body.GetTree().ChangeSceneToFile("res://level_03.tscn");
        }

		if (p == "Level02")
		{
			
			body.GetTree().ChangeSceneToFile("res://level04.tscn");
		}
		if (p == "Level03")

			body.GetTree().ChangeSceneToFile("res://level04.tscn");
	}




}
