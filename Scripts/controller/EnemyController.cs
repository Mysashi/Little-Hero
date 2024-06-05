using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public partial class EnemyController : CharacterBody2D

{
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimatedSprite2D sprite2D;
    private Enemy enemy = new Enemy();
    private PlayerModel player = new PlayerModel();
    private bool knockback = false;
    private float speed = -150f;
    private Timer timerKnockback;
    private bool vectorIsZero = false;
    private int direction = 1;
    

    public override void _Ready()
    {

        sprite2D = GetNode<AnimatedSprite2D>("SlimeSprite");
        timerKnockback = GetNode<Timer>("Timer");
    }

    public override async void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.


        // Handle Jump.




        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;

        }

        if (IsOnWall())
        {
            enemy.speed = -enemy.speed;
            direction *= -1;
        }

        velocity.X = enemy.speed;


        
     



        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.


        Velocity = velocity;
        MoveAndSlide();
        if (velocity.X < 0)
            sprite2D.FlipH = true;
        if (velocity.X > 0)
            sprite2D.FlipH = false;
    }



    public void _on_hitbox_body_entered(CharacterBody2D body)
    {
        CallDeferred("free");

    }

    public void _on_timer_timeout()
    {

    }



    public void _on_damage_area_body_entered(CharacterBody2D body)
    {
        if (body.Name == "PlayerController")
        {
            PlayerModel.TakeDamage(120);
            body.Velocity = new Vector2(0, -500);

            if (PlayerModel.health <= 1)
            {
                PlayerView.AnimationDeath();
                PlayerModel.health = 100;

            }
        }
    }




}






