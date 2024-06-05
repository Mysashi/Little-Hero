using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PlayerView : CanvasLayer
{
    private Timer blinker;
    private Timer durationTimer;
    private CharacterBody2D player;
    private double invincible_duration = 1.5;
    private PlayerModel PlayerModel;
    private Node2D player2;
    private CharacterBody2D playerController;
    private AnimatedSprite2D sprite;
    private AnimationPlayer animPlayer;
    private static AnimationPlayer deathScreen;
    public Game game = new Game();
    public static AudioStreamPlayer2D grassSound;
    public override void _Ready()
    {
        grassSound = GetNode<AudioStreamPlayer2D>("runSoundGrass");
        blinker = GetNode<Timer>("Blinker/BlinkingTimer");
        durationTimer = GetNode<Timer>("Blinker/DurationTimer");
        playerController = GetNode<CharacterBody2D>("../PlayerController");
        sprite = GetNode<AnimatedSprite2D>("../PlayerController/Sprite2D");
        deathScreen = GetNode<AnimationPlayer>("Adapter/EndScreen/Death");
        grassSound = GetNode<AudioStreamPlayer2D>("runSoundGrass");
        AnimationIncluder();

    }




    public static void AnimationRunning(AnimatedSprite2D sprite)
    {
        sprite.Animation = "running";
    }

    public static void AnimationJumping(AnimatedSprite2D sprite)
    {
        sprite.Animation = "jumping";
    }
    public static void AnimationDefault(AnimatedSprite2D sprite)
    {
        sprite.Animation = "default";
    }

    public async static void AnimationAttack(AnimationPlayer sprite)
    {
        sprite.Play("attack");
       
    }

    public static void AnimationAttackBlink(AnimationPlayer sprite)
    {
        sprite.Play("attack_blink");
        

    }

    public static void AnimationAttackBlink2(AnimationPlayer sprite)
    {
        sprite.Play("attack_blink2");
    }


    public static void RunSoundGrass()
    {
        if (!grassSound.Playing)
            grassSound.Play();
    }
    public static void AnimationDeath()
    {
        deathScreen.Play("open");
    }
    public static void AnimationIncluder()
    {
        deathScreen.Play("start");
    }

    public static void AnimationChanger()
    {
        deathScreen.Play("change");
    }



    public void start_blinking(CharacterBody2D playerNode, double duration = 3)
    {
        player = playerNode;
        durationTimer.WaitTime = duration;
        blinker.Start();
        durationTimer.Start();
    }

    public void _on_blinking_timer_timeout()
    {
       player.Visible = !player.Visible;
       player.SetCollisionLayerValue(4, false);
       player.SetCollisionMaskValue(2, false);
    }

    public void _on_duration_timer_timeout()
    {
        blinker.Stop();
        player.Visible = true;
        player.SetCollisionLayerValue(4, true);
        player.SetCollisionMaskValue(2, true);
    }


    public void _on_hitbox_shape_area_entered(Area2D area)
    {
        GD.Print("printed");
        if (area.Owner.IsInGroup("Enemies") || area.Owner.IsInGroup("Obstacles"))
        {
            GD.Print("OUCH");
            start_blinking(playerController, invincible_duration);
            
          
        }
        if (area.Owner.IsInGroup("Collectible"))
        {
            GD.Print("Collected");
        }

    }

    public async void _on_death_animation_finished(string anim)
    {
       if (anim == "open")
        {
            Game.reload = true;
            //game.ReloadScene();
            //game.ReloadScene(GetTree().CurrentScene.Name);
        }



    }

    public void _on_death_current_animation_changed(string current)
    {

    }





}
