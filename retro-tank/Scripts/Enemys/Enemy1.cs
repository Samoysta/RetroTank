using Godot;
using System;
using System.Collections;

public partial class Enemy1 : CharacterBody2D
{
	[Export] int health;
	[Export] int damage = 1;
	[Export] float speed;
	[Export] Node2D target;
	[Export] float updateTimer;
	[Export] CollisionShape2D hitBox;
	[Export] AnimationPlayer anim;
	EnemyManager manager;
	float updateT = 0.5f;
	bool SetOffed;
	Vector2 velocity;
    public override void _Ready()
    {
        updateT = updateTimer;
		if (target == null)
		{
			target = GetParent().GetNode<CharacterBody2D>("Character");
		}
    }
    public override void _Process(double delta)
    {
        if (health <= 0)
		{
			SetOff();
		}
    }

	public void SetOff()
	{
		if (!SetOffed)
		{
			if (target is Character character)
			{
				character.setKillAmount();
			}
			Visible = false;
			SetProcess(false);
			hitBox.CallDeferred("set_disabled", true);
			SetPhysicsProcess(false);
			manager.enemy1s.Enqueue(this);
			SetOffed = true;
			manager.enemyAmount--;
			manager.Call("SetPos", this);
		}
	}
	public void SetOn()
	{
		health = 5;
		Visible = true;
		SetProcess(true);
		SetPhysicsProcess(true);
		SetOffed = false;
		hitBox.CallDeferred("set_disabled", false);
	}

	public void Init(EnemyManager who)
	{
		manager = who;
	}
	public override void _PhysicsProcess(double delta)
	{
		if (updateT > 0)
		{
			updateT -= (float)delta;
		}
		if (updateT <= 0)
		{
			if (target.IsInsideTree())
			{
				Vector2 dir = (target.GlobalPosition - GlobalPosition).Normalized();
				velocity = dir * speed;
				updateT = updateTimer;	
			}
		}
		LookAt(GlobalPosition + Velocity);
		Velocity = Velocity.Lerp(velocity, 0.1f);
		MoveAndSlide();
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		anim.Play("Hit");
		anim.Seek(0);
	}
}
