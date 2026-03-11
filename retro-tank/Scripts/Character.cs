using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public partial class Character : CharacterBody2D
{
	[Export] int health;
	[Export] float Speed;
	[Export] float speedAccel;
	[Export] Node2D bulletPos;
	[Export] AnimationPlayer damageAnim;
	[Export] PackedScene bullet1;
	[Export] float bulletCD;
	[Export] PackedScene hitEffect;
	[Export] PackedScene fireEffect;
	[Export] PackedScene damageEffect;
	[Export] int bulletPerFrame;
	[Export] Godot.Label hpLabel;
	[Export] Godot.Label killLabel;
	[Export] WeaponMenu weaponMenu;
	float damageCD;
	float bulletcd;
	public int killAmount;
	public Queue<Bullet1> bullets = new ();
	public Queue<Effect> bulletHitEffects = new ();
	public Queue<Effect> fireEffects = new ();
	public Queue<Effect> damageEffects = new(); 
	public Queue<Coin> coins = new();
	Tween tween1;
	Tween tween2;
	Vector2 velocity;
	CollisionShape2D hitBox;
	[Export] int[] killAmountGiftNumbers;
	[Export] public PackedScene Coin;
	public int coinAmount;
	[Export] Label coinLabel;
	[Export] public EnemyManager manager;
    public override void _Ready()
    {
		hpLabel.Text = $"{health}";
		hitBox = GetNode<CollisionShape2D>("CollisionShape2D/HitBox/CollisionShape2D");
		for (int i = 0; i < 6; i++)
		{
			Effect ef = (Effect)hitEffect.Instantiate();
			GetTree().CurrentScene.CallDeferred("add_child", ef);
			ef.SetOff();
			bulletHitEffects.Enqueue(ef);
		}
		for (int i = 0; i < 5; i++)
		{
			Effect ef = (Effect)fireEffect.Instantiate();
			GetTree().CurrentScene.CallDeferred("add_child", ef);
			ef.SetOff();
			fireEffects.Enqueue(ef);
		}
		for (int i = 0; i < 50; i++)
		{
			Effect ef = (Effect)damageEffect.Instantiate();
			GetTree().CurrentScene.CallDeferred("add_child", ef);
			ef.SetOff();
			damageEffects.Enqueue(ef);
		}
    }
    public override void _Process(double delta)
    {
		coinLabel.Text = $"Samoys : {coinAmount}";
		//Timers
		if (damageCD > 0)
		{
			damageCD -= (float)delta;
		}
		else
		{
			hitBox.CallDeferred("set_disabled", false);
		}
		if (bulletcd > 0)
		{
			bulletcd -= (float)delta;
		}
		//Can
		if (health <= 0)
		{
			Die();
		}
        LookAt(GetGlobalMousePosition());
		//Ateş etme
		if (Input.IsActionPressed("LeftMouse"))
		{
			if (bulletcd <= 0)
			{
				for (int i = -bulletPerFrame + 1; i < bulletPerFrame; i++)
				{
					SpawnBullet(i);	
				}
				Effect ef = fireEffects.Dequeue();
				ef.GlobalPosition = bulletPos.GlobalPosition;
				ef.GlobalRotationDegrees = GlobalRotationDegrees;
				ef.SetOn();
				fireEffects.Enqueue(ef);
				bulletcd = bulletCD;
			}
		}
    }
	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;
		Vector2 direction = Input.GetVector("A", "D", "W", "S");
		velocity = direction * Speed;
		Velocity = Velocity.Lerp(velocity, speedAccel * (float)delta);
		MoveAndSlide();
	}

	void SpawnBullet(int index)
	{
		if (bullets.Count == 0)
		{
			Bullet1 bullet = (Bullet1)bullet1.Instantiate();
			bullet.GlobalRotationDegrees = GlobalRotationDegrees + index * 5;
			bullet.GlobalPosition = bulletPos.GlobalPosition;
			GetTree().CurrentScene.AddChild(bullet);
			bullet.Call("Init", this);
		}
		else
		{
			Bullet1 bullet = bullets.Dequeue();
			bullet.GlobalPosition = bulletPos.GlobalPosition;
			bullet.GlobalRotationDegrees = GlobalRotationDegrees + index * 5;
			bullet.SetOn();
		}
		
	}

	public void TakeDamage(int damage)
	{
		if (damageCD <= 0)
		{
			health -= damage;
			damageAnim.Play("TakeDamage");
			damageAnim.Seek(0);
			damageCD = 1;	
			hitBox.CallDeferred("set_disabled", true);
			hpLabel.Text = $"{health}";
			hpLabel.Scale /= 4;
			tween1?.Kill();
			tween1 = CreateTween();
			tween1.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Elastic);
			tween1.TweenProperty(hpLabel, "scale", hpLabel.Scale * 4, 0.9);
			tween2?.Kill();
			tween2 = CreateTween();
			tween2.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Linear);
			tween2.TweenProperty(hpLabel, "modulate", Colors.DarkOrange, 0.1);
			tween2.TweenProperty(hpLabel, "modulate", Colors.White, 0.45);
		}
	}

	public void setKillAmount()
	{
		killAmount++;
		if (killAmountGiftNumbers.Contains(killAmount))
		{
			GetTree().Paused = true;
			weaponMenu.ProcessMode = ProcessModeEnum.Always;
			weaponMenu.Visible = true;
			weaponMenu.SetOn();
			if (manager.enemySpawnCD > 0.1f)
			{
				manager.enemySpawnCD -= 0.1f;
			}
			if (manager.enemySpawnCD <= 0.1f)
			{
				manager.enemySpawnCD = 0.1f;
			}
		}
		killLabel.Text = $"{killAmount}";
	}
	public void BodyEntered(Node2D body)
	{
		if (body.IsInGroup("SmallEnemy"))
		{
			TakeDamage(1);
		}
		else if (body.IsInGroup("MediumEnemy"))
		{
			TakeDamage(2);
		}
		else if (body.IsInGroup("BigEnemy"))
		{
			TakeDamage(3);
		}
	}

	public void Die()
	{
		Visible = false;
		ProcessMode = ProcessModeEnum.Disabled;
	}
}
