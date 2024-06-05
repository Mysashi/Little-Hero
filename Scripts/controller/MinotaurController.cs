using Godot;
using System;
using System.Reflection.Metadata;
using static Godot.TextServer;

public partial class MinotaurController : CharacterBody2D
{
    public float Speed = 300f;
    public float SpeedAttack = 50f;
    public const float JumpVelocity = -400.0f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    private AnimatedSprite2D sprite2D;
    private CollisionShape2D seenArea;
    private bool detected = false;
    private CharacterBody2D player;
    private bool hit = false;
    private bool isJumping;
    private bool isFirstLanding;
    private AnimationPlayer animator;
    private bool seen;
    public float health = 100f;
    private bool dead = false;
    private Enemy enemy = new Enemy();
    private EnemyView view = new EnemyView();

    public override void _Ready()
    {
        sprite2D = GetNode<AnimatedSprite2D>("Minotaur");
        Globals.minotaurSprite = sprite2D;
        seenArea = GetNode<CollisionShape2D>("Minotaur/SeenArea/Seen");
        animator = GetNode<AnimationPlayer>("Minotaur/AttackSystem/AnimationPlayer");
        Globals.animatorMinotaur = animator;



    }
    public async override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        if (dead)
        {
            velocity = Vector2.Zero;
            view.MinotaurDeath(animator);
        }

        else
        {
            if (hit)
            {
                velocity = Vector2.Zero;
                view.MinotaurHit(animator);
                
            }
            else
            {
                if (!IsOnFloor())
                {
                    velocity.Y += gravity * (float)delta;
                    view.MinotaurJump(sprite2D);
                    isJumping = true;


                }

                else
                    if (isJumping)
                    {
                        this.SetCollisionLayerValue(2, false);
                        if (isFirstLanding)
                        {
                            seenArea.Disabled = true;
                            velocity.X = 0f;
                            view.MinotaurCircleAttack(animator);
                            isFirstLanding = false;
                        }
                        isJumping = false;

                }


                // Add the gravity.
                if ((detected) && (IsOnFloor()) && (isFirstLanding))
                {
                    //velocity.X = (Position.DirectionTo(player.GlobalPosition) * SpeedAttack).X;
                    //GD.Print(velocity);
                    //GD.Print(player.GlobalPosition);

                    //var jump_distance = (player_position - self_position).Normalized();
                    //var distance_to_player = player_position.DistanceTo(self_position);

                    this.SetCollisionLayerValue(2, true);
                    var hor_speed = 800f;
                    GD.Print(velocity.X);
                    velocity.Y = -2000f;
                    velocity.X = hor_speed * GlobalPosition.DirectionTo(player.GlobalPosition).X;

                }

                if (!detected && IsOnFloor())
                {
                    this.SetCollisionLayerValue(2, true);
                    if (IsOnWall())
                    {
                        Speed = -Speed;
                    }

                    if (seen)
                    {
                        velocity = Vector2.Zero;
                        view.MinotaurStance(sprite2D);
                    }
                    else
                        velocity.X = Speed;



                }

            }





            //velocity.X = enemy.speed;
            //velocity.X = Enemy.speed;







            // Get the input direction and handle the movement/deceleration.
            // As good practice, you should replace UI actions with custom gameplay actions.


            Velocity = velocity;
            MoveAndSlide();
            if (velocity.X < 0)
            {
                sprite2D.FlipH = true;
                seenArea.Position = new Vector2(-66, 6);
            }

            if (velocity.X > 0)
            {
                sprite2D.FlipH = false;
                seenArea.Position = new Vector2(66, 6);

            }
        }

     



    }

    public void TakeDamage()
    {
        GD.Print(health);
        health -= 20f;
        if (health <= 0)
        {
            view.MinotaurDeath(animator);
            dead = true;
        }
    }
    public async void _on_seen_area_body_entered(CharacterBody2D body)
    {
        seen = true;
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        seen = false;
        seenArea.Visible = false;
        detected = true;
        isFirstLanding = true;
        player = body;
        seenArea.Visible = true;
    }

    public async void _on_animation_player_animation_finished(string body)
    {
        if (body == "circle_attack")
        {
            seenArea.Disabled = false;
            detected = false;
            view.MinotaurRun(sprite2D);
        }
        if (body == "death")
        {
            GD.Print("death");
            QueueFree();
        }
        if (body == "hit")
        {
            GD.Print("done");
            hit = false;
            view.MinotaurRun(sprite2D);
        }
    }

    public void _on_damage_zone_area_entered(Area2D area)
    {
        PlayerModel.TakeDamage(30);
    }

}