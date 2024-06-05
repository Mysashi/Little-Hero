using Godot;
using System;

public partial class woodCutterController : CharacterBody2D
{
	public const float speed = 150.0f;
	public const float JumpVelocity = -400.0f;
	public bool detected = false;
	private CharacterBody2D player;
    private AnimatedSprite2D animatedSprite;
    private Enemy enemy = new Enemy();
    private EnemyView view = new EnemyView();
    private CollisionShape2D collision;
    private CollisionShape2D closefAttack;
    private CollisionShape2D widefAttack;
    private AnimationPlayer animatorPlayer;
    private CollisionShape2D distantArea;
    private CollisionShape2D midAttack;
    private bool clDistance = false;
    private bool midDistance = false;
    private RayCast2D Ray;
    private RayCast2D Ray2;
    public float health;
    private bool death = false;
    private bool hit = false;


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("Cutter");
        collision = GetNode<CollisionShape2D>("Collision");
        closefAttack = GetNode<CollisionShape2D>("AttacksArea/CloseAttack");
        midAttack = GetNode<CollisionShape2D>("MediumLastAttack/MediumDistance");
        widefAttack = GetNode<CollisionShape2D>("MediumDistance/AttackWide");
        animatorPlayer = GetNode<AnimationPlayer>("Cutter/AttackAnimations/AnimationPlayer");
        distantArea = GetNode<CollisionShape2D>("CloseDistance/Distance");
        Ray = GetNode<RayCast2D>("Ray");
        Ray2 = GetNode<RayCast2D>("Ray2");
        health = 120f;
    }
    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
        
        
        if (death)
        {
            velocity.Y = 0f;
            detected = false;
            view.WoodCutterDeath(animatorPlayer);
        }
                
        else
        {
            if (hit)
            {
                velocity = Vector2.Zero;
                view.WoodCutterHit(animatorPlayer);
            }
            else
            {
                


                if (!IsOnFloor())
                {
                    velocity.Y += gravity * (float)delta;


                }

                if (!detected)
                {
                    velocity = Vector2.Zero;
                }
                else
                {
                    if (!clDistance)
                    {

                       

                        velocity.X = GlobalPosition.DirectionTo(player.GlobalPosition).X * speed;
                        if (Ray.IsColliding() || Ray2.IsColliding())
                        {
                            GD.Print("Coliding");
                            velocity.Y = -800f;
                        }
                        if (midDistance && !death)
                        {
                            velocity = Vector2.Zero;
                            if (animatedSprite.FlipH == true)
                                view.WoodCutterAttackFlipped1(animatorPlayer);
                            else
                                view.WoodCutterWideAttackFlipped(animatorPlayer);
                        }
                        if (velocity != Vector2.Zero)
                        {
                            view.WoodCutterRun(animatedSprite);
                        }
                    }
                    else
                    {
                        if (clDistance && !death)
                        {
                            velocity = Vector2.Zero;
                            view.WoodCutterAttack1(animatorPlayer);


                        }


                    }
                }


            }
        }

        



		Velocity = velocity;
		MoveAndSlide();


        if (velocity.X < 0)
        {
           animatedSprite.FlipH = true;
           collision.Position = new Vector2(15, 0);
           widefAttack.Position = new Vector2(-41, 2);
           closefAttack.Position = new Vector2(-1, 0);
           distantArea.Position = new Vector2(-1, 0);
           midAttack.Position = new Vector2(-41, 2);

        }

        if (velocity.X > 0)
        {
            animatedSprite.FlipH = false;
            collision.Position = new Vector2(0, 0);
            widefAttack.Position = new Vector2(51, 2);
            closefAttack.Position = new Vector2(14, 0);
            midAttack.Position = new Vector2(51, 2);
            distantArea.Position = new Vector2(14, 0);
        }


    }
    public void OnDetectionWoodCutter(CharacterBody2D body)
    {
		detected = true;
		player = body;
    }

    public void TakeDamage()
    {
        GD.Print(health);
        health -= 30f;
        if (health <= 0)
        { 
            death = true;
        }
    }

    public  async void _on_attacks_area_body_entered(CharacterBody2D body)
    {
        body.Set("stun", true);
        PlayerModel.TakeDamage(20);
        body.Set("CutterDirection", Position.DirectionTo(player.GlobalPosition).X);
        widefAttack.Disabled = false;
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        body.Set("stun", false);


    }

    public void OnMediumLastAttack(CharacterBody2D body)
    {
        PlayerModel.TakeDamage(20);
    }


    public async void OnCloseBody(CharacterBody2D body)
    {
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        clDistance = true;
    }

    public void OnAnimationFinished(string anim)
    {
        if (anim == "closeAttack")
        {
            clDistance = false;
            view.WoodCutterIdle(animatedSprite);
        }

        if (anim == "wideAttack" || anim == "wideAttackFlipped")
        {
            midDistance = false;
            midAttack.Disabled = true;

            view.WoodCutterIdle(animatedSprite);
        }
        if (anim == "death")
        {
            QueueFree();
        }

        if (anim == "hit")
        {
            hit = false;
            view.WoodCutterRun(animatedSprite);
        }
    }
    public async void OnMediumDistance(CharacterBody2D body)
    {
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        midDistance = true;
        midAttack.Disabled = true;
        GD.Print("I've come");
        body.Set("stun", false);
    }

}








