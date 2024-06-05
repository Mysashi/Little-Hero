using Godot;
using System;

public partial class characterController : CharacterBody2D
{
    private AnimatedSprite2D sprite2d;
    


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public  bool isAttacking = false;
    public bool isAttackinginBlink = false;
    private CollisionShape2D areaofSwordBlink;
    private CollisionShape2D areaofSword;
    private bool stun = false;
    private float CutterDirection;
    private RayCast2D Ray;
    private bool canAttack = true;
    private bool stopMovement = false;

    public override void _Ready()
    {
        areaofSword = GetNode<CollisionShape2D>("Sprite2D/AreaofSword/AreaAttack1");
        areaofSwordBlink = GetNode<CollisionShape2D>("Sprite2D/AreaofSword/AreaAttackBlink");
        Globals.playerBody = this;

    }


    public async override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor() && !stun)
        {
                velocity.Y = PlayerModel.JumpVelocity;
        }

        if ((velocity.X > 1 || velocity.X <-1))
        {
            if (!isAttacking && (!isAttackinginBlink))
                PlayerView.AnimationRunning(PlayerModel.sprite);
        }

        else
            if (!isAttacking && (!isAttackinginBlink))
            PlayerView.AnimationDefault(PlayerModel.sprite);


        if (!IsOnFloor())
        {
            {
                velocity.Y += gravity * (float)delta;
                PlayerView.AnimationJumping(PlayerModel.sprite);
            }
        }

        if (Input.IsActionJustPressed("attack") && (isAttackinginBlink == false) && canAttack)
        {
            if (IsOnFloor())
            {
                PlayerView.AnimationAttack(PlayerModel.animPlayer);
                isAttacking = true;         

            }
        }

        if (Input.IsActionPressed("attack_blink") && canAttack)
        {
            
            //PlayerModel.currentMana == 100
            if (IsOnFloor())
            {
                if (PlayerModel.sprite.FlipH == true)
                {
                    PlayerView.AnimationAttackBlink2(PlayerModel.animPlayer);
                }
                else
                    PlayerView.AnimationAttackBlink(PlayerModel.animPlayer);

                PlayerModel.currentMana = 0;
                isAttackinginBlink = true;
                stopMovement = true;
            }
        }


        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        if (stun)
        {
            velocity.X = 300 * CutterDirection;


        }
        else
        {
            if (stopMovement)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                Vector2 direction = Input.GetVector("left", "right", "jump", "ui_down");
                if (direction != Vector2.Zero)
                {
                    velocity.X = direction.X * PlayerModel.Speed;
                }
                else
                {
                    velocity.X = Mathf.MoveToward(Velocity.X, 0, PlayerModel.Speed);
                }
            }
            
        }
           

      

        Velocity = velocity;
        MoveAndSlide();

        if (velocity.X < 0)
        {
            PlayerModel.sprite.FlipH = true;
            areaofSword.Position = new Vector2(25, -2);
            areaofSwordBlink.Position = new Vector2(-28, 9);
        }

        if (velocity.X > 0)
        {
            PlayerModel.sprite.FlipH = false;
            areaofSword.Position = new Vector2(84, -2);
            areaofSwordBlink.Position = new Vector2(138, 9);
            

        }
    }

    public async void _on_animation_player_animation_finished(string anim)
    {

        if (anim == "attack")
        {
            GD.Print("done");
            isAttacking = false;
            canAttack = false;
            await ToSignal(GetTree().CreateTimer(1f), "timeout");
            canAttack = true;
        }

        if ((anim == "attack_blink") || (anim == "attack_blink2"))
        {
            isAttackinginBlink = false;
            stopMovement = false;
            
        }
    }



}
