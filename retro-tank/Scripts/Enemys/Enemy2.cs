using Godot;
using System;
using System.Collections;

public partial class Enemy2 : CharacterBody2D
{
	[Export] int health;
	[Export] int damage = 1;
	[Export] float speed;
	[Export] Character target;
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
			target = GetParent().GetNode<Character>("Character");
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
			SpawnCoin();
			SpawnCoin();
			Visible = false;
			SetProcess(false);
			hitBox.CallDeferred("set_disabled", true);
			SetPhysicsProcess(false);
			manager.enemy2s.Enqueue(this);
			SetOffed = true;
			manager.enemyAmount--;
			manager.Call("SetPos", this);
			manager.AllEnemys.Remove(this);
		}
	}
	void SpawnCoin()
	{
		if (target.coins.Count == 0)
		{
			Coin coin = (Coin)target.Coin.Instantiate();
			coin.GlobalPosition = GlobalPosition;
			coin.Call("Init", target);
			GetTree().CurrentScene.AddChild(coin);
		}
		else
		{
			Coin coin = target.coins.Dequeue();
			coin.GlobalPosition = GlobalPosition;
			coin.SetOn();
		}
		
	}
	public void SetOn()
	{
		health = 7;
		Visible = true;
		SetProcess(true);
		SetPhysicsProcess(true);
		SetOffed = false;
		hitBox.CallDeferred("set_disabled", false);
		manager.AllEnemys.Add(this);
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
		Velocity = Velocity.Lerp(velocity, 5 * (float)delta);
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
