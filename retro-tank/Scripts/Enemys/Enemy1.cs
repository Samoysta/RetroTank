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
	RandomNumberGenerator rnd = new();
	float updateT = 0.5f;
	float updateT2 = 1f;
	bool SetOffed;
	Vector2 velocity;
	Vector2 distance;
	int rotation;
	Vector2 targetPos;
    public override void _Ready()
    {
		rnd.Randomize();
        updateT = updateTimer;
		if (target == null)
		{
			target = GetParent().GetNode<CharacterBody2D>("Character");
		}
    }
    public override void _Process(double delta)
    {
		if (updateT2 > 0)
		{
			updateT2 -= (float)delta;
		}
		else
		{
			if (target.GlobalPosition.DistanceTo(this.GlobalPosition) < 200)
			{
				distance = Vector2.Zero;
			}
			else
			{
				distance = new Vector2(rnd.RandiRange(0,200),0);
				rotation = rnd.RandiRange(0,359);
				updateT2 = 1;	
			}
		}
        if (health <= 0)
		{
			SetOff();
		}
		targetPos = target.GlobalPosition + distance.Rotated(Mathf.DegToRad(rotation));
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
			Vector2 dir = (targetPos - GlobalPosition).Normalized();
			velocity = dir * speed;
			updateT = updateTimer;	
		}
		LookAt(GlobalPosition + Velocity);
		Velocity = Velocity.Lerp(velocity, 0.1f);
		MoveAndSlide();
	}

	public void TakeDamage(int damage)
	{
		Character character = (Character)target;
		Effect ef = character.damageEffects.Dequeue();
		ef.GlobalPosition = GlobalPosition;
		ef.GlobalRotationDegrees = GlobalRotationDegrees;
		ef.SetOn();
		character.damageEffects.Enqueue(ef);
		health -= damage;
		anim.Play("Hit");
		anim.Seek(0);
	}
}
