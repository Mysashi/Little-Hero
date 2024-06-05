using Godot;
using System;

public partial class PlayerModel : Node2D
{
    public static float health = 100;
    public static float currentHealth = 100;
    public static float currentMana = 0;
    public static float MaxMana = 100;
    public static float Speed = 500.0f;
    public static float JumpVelocity = -1000.0f;
    public static float throwForce = 500f;
    public static Vector2 throwDirection = new Vector2(1, 0);
    public Vector2 velocity;
    public ProgressBar progress;
    public ProgressBar progressMana;
    public CharacterBody2D player2;
    public EnemyController motion;
    Signal knockback;
    private CollisionShape2D areaofSword;

    // Called when the node enters the scene tree for the first time.

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    private AudioStreamPlayer2D grassSound;
    public static AnimatedSprite2D sprite;
    public static AnimationPlayer animPlayer;
    public static EnemyView view = new EnemyView();
    public static RayCast2D Ray;
    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("PlayerController/Sprite2D");
        player2 = GetNode<CharacterBody2D>("PlayerController"); 
        progress = GetNode<ProgressBar>("PlayerView/HealthBar");
        animPlayer = GetNode<AnimationPlayer>("PlayerController/Sprite2D/DamageAnimations/AnimationPlayer");
        progressMana= GetNode<ProgressBar>("PlayerView/ManaBar");
        Ray = GetNode<RayCast2D>("PlayerController/Ray");



        health = 100f;
        progressMana.MaxValue = MaxMana;
        progressMana.Value = currentMana;

        progress.MaxValue = currentHealth;
        progress.Value = health;

    }

    public override void _Process(double delta)
    {
        progress.Value = health;
        progressMana.Value = currentMana;
        
        if (Ray.IsColliding())
        {
           
            TileMap tileData = (TileMap)Ray.GetCollider();
            var localToMapData = tileData.LocalToMap(player2.GlobalPosition);
            TileData tileNeed = tileData.GetCellTileData(0, localToMapData, false);
           /* if (tileData.GetLayerName(1) == "Grass" && player2.IsOnFloor())
            {
                
                PlayerView.RunSoundGrass();
            }*/




        }
    }


    public static void TakeDamage(float amount)
    {

        health -= amount;
            
        if (health <= 0)
        {
            PlayerView.AnimationDeath();
        }
            
    }

    public static void TakeMana(float amount)
    {

        currentMana += amount;
        
    }

    public async void _on_areaof_sword_body_entered(CharacterBody2D body)
    {
       Vector2 direction = GlobalPosition.DirectionTo(body.GlobalPosition);
        GD.Print(body.Name);
       if (body.Name == "Bat2" || body.Name == "Bat")
        {
            body.Set("death", true);
        }

       if (body.IsInGroup("Enemies"))
        {
            body.Set("hit", true);
            body.Call("TakeDamage");
           
        }

    }

    public void _on_areaof_sword_body_exited(CharacterBody2D body)
    {
        body.Set("vectorIsZero", true);
    }







}
