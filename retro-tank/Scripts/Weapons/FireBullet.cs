using Godot;
using System;

public partial class FireBullet : Area2D
{
	[Export] float speed;
	Weapon2 weapon2;
	bool setOffed;
	CpuParticles2D effect;
	Sprite2D sprite;
	CollisionShape2D hitBox;
	CpuParticles2D boom;
	AnimatedSprite2D smokeSprite;

	public override void _Ready()
	{
		effect = GetNode<CpuParticles2D>("CPUParticles2D");
		boom = GetNode<CpuParticles2D>("Boom");
		sprite = GetNode<Sprite2D>("CollisionShape2D/Sprite2D");
		hitBox = GetNode<CollisionShape2D>("CollisionShape2D");
		smokeSprite = GetNode<AnimatedSprite2D>("Smoke");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MoveLocalX(speed * (float)delta);
	}

	public void Init(Weapon2 body)
	{
		weapon2 = body;
	}
	void BodyEntered2D(Node2D body)
	{
		if (!body.IsInGroup("Player"))
		{
			if (body.HasMethod("TakeDamage"))
			{
				body.Call("TakeDamage",1);
			}
			CallDeferred("SetOff");
		}
	}

	void SetOff()
	{
		if (setOffed)
		{
			return;
		}
		setOffed = true;
		sprite.Visible = false;
		weapon2.bullets.Enqueue(this);
		effect.Emitting = false;
		hitBox.Disabled = true;
		SetProcess(false);
		boom.Emitting = true;
		smokeSprite.Play("Start");
	}
	public void SetOn()
	{
		setOffed = false;
		sprite.Visible = true;
		effect.Emitting = true;
		hitBox.Disabled = false;
		SetProcess(true);
	}
}
