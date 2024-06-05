using Godot;
using System;
using System.Threading.Tasks;

public partial class Obstacles : Node2D
{
    public int health = 100;
    public float throwForce = 500f;
    public Vector2 throwDirection = new Vector2(1, 0);
    public PlayerModel player = new PlayerModel();
    public ObstaclesView obstaclesView = new ObstaclesView();
    public float knockbackForce = 500f;
    private Timer timer;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

    private void _on_area_2d_body_entered(CharacterBody2D body)
    {
        if (body.Name == "PlayerController")
        {
            GD.Print(body.Velocity.Y);
            body.Velocity = new Vector2(-200, -1600);
            PlayerModel.TakeDamage(20);

        }
        
    }


}

   