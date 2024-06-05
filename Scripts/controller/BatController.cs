using Godot;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static Godot.TextServer;

public partial class BatController : CharacterBody2D
{
    [Export] public float speed = 0.2f;
    public Vector2 dir;
    public bool is_chase;
    private Timer timer;
    private CharacterBody2D player;
    public bool knockback = false;
    public bool death = false;
    public bool finished = false;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    [Export] public PathFollow2D path;
    public EnemyView view = new EnemyView();
    public CollisionShape2D damageArea;
    public AnimatedSprite2D sprite;
    public CollisionShape2D hitboxArea;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        is_chase = false;
        sprite = GetNode<AnimatedSprite2D>("BatSprite");
        damageArea = GetNode<CollisionShape2D>("DamageArea/Damage");
        hitboxArea = GetNode<CollisionShape2D>("Hitbox/Hitbox");
        
    }


    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        if (!death)
        {
            if (!is_chase)
            {
                path.ProgressRatio += speed * (float)delta;
            }

        }
        else
        {
            if (!finished)
            {

                view.BatAnimationDeath(sprite);
                velocity.X = 0f;
                velocity.Y = gravity * (float)delta;
                this.SetCollisionLayerValue(2, false);
                this.SetCollisionMaskValue(4, false);
                damageArea.Disabled = true;
                hitboxArea.Disabled = true;
                finished = true;
            }
        }


        Velocity = velocity;
        MoveAndSlide();

    }

    public void _on_timer_timeout()
    {
        double[] numbers = { 1.0, 1.5, 2.0 };
        Vector2[] vectors = { Vector2.Right, Vector2.Up, Vector2.Left, Vector2.Up, Vector2.Down };
        timer.WaitTime = choose(numbers);
        if (!is_chase)
        {
            dir = choose(vectors);
            GD.Print(dir);
        }
    }
    public T choose<T>(T[] massiv)
    {
        var rng = new Random();
        rng.Shuffle(massiv);
        return massiv[0];
    }


    public void handle_animation()
    {

    }
    public void TakeDamage()
    {

    }

    public void _on_damage_area_body_entered(CharacterBody2D body)
    {
        GD.Print("entered");
        PlayerModel.TakeDamage(20);
    }
}


static class Shuffling
{
    public static void Shuffle<T>(this Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
