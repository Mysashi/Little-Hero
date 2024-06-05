using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class EnemyView : Node
{

    public void BatAnimationDeath(AnimatedSprite2D sprite)
    {
        sprite.Play("death");
    }

    public void MinotaurRun(AnimatedSprite2D sprite)
    {
        sprite.Play("run");
    }

    public void MinotaurHit(AnimationPlayer sprite)
    {
        sprite.Play("hit");
    }

    public void MinotaurStance(AnimatedSprite2D sprite)
    {
        sprite.Play("seen");
    }
    public void MinotaurJump(AnimatedSprite2D sprite)
    {
        sprite.Play("jump");
    }

    public void MinotaurCircleAttack(AnimationPlayer animator)
    {
        animator.Play("circle_attack");
    }
    public void MinotaurDeath(AnimationPlayer animator)
    {
        animator.Play("death");
    }

    public void WoodCutterRun(AnimatedSprite2D sprite)
    {
        sprite.Play("walk");

    }

    public void WoodCutterIdle(AnimatedSprite2D sprite)
    {
        sprite.Play("Idle");

    }
    public void WoodCutterAttack1(AnimationPlayer sprite)
    {
        sprite.Play("closeAttack");

    }
    public void WoodCutterAttackFlipped1(AnimationPlayer sprite)
    {
        sprite.Play("wideAttackFlipped");
    }
    public void WoodCutterWideAttackFlipped(AnimationPlayer sprite)
    {
        sprite.Play("wideAttack");
    }
    public void WoodCutterDeath(AnimationPlayer sprite)
    {
        sprite.Play("death");
    }

    public void WoodCutterHit(AnimationPlayer sprite)
    {
        sprite.Play("hit");
    }


}
