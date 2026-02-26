using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public partial class Character : CharacterBody2D
{
	[Export] float Speed;
	[Export] float speedAccel;
	[Export] Node2D bulletPos;
	[Export] PackedScene bullet1;
	[Export] float bulletCD;
	[Export] PackedScene hitEffect;
	[Export] PackedScene fireEffect;
	float bulletcd;
	public Queue<Bullet1> bullets = new ();
	public Queue<Effect> bulletHitEffects = new ();
	public Queue<Effect> fireEffects = new ();
	Vector2 velocity;
    public override void _Ready()
    {
		for (int i = 0; i < 6; i++)
		{
			Effect ef = (Effect)hitEffect.Instantiate();
			GetTree().CurrentScene.CallDeferred("add_child", ef);
			ef.SetOff();
			bulletHitEffects.Enqueue(ef);
		}
		for (int i = 0; i < 3; i++)
		{
			Effect ef = (Effect)fireEffect.Instantiate();
			GetTree().CurrentScene.CallDeferred("add_child", ef);
			ef.SetOff();
			fireEffects.Enqueue(ef);
		}
    }
    public override void _Process(double delta)
    {
		if (bulletcd > 0)
		{
			bulletcd -= (float)delta;
		}
        LookAt(GetGlobalMousePosition());
		//Ateş etme
		if (Input.IsActionPressed("LeftMouse"))
		{
			if (bulletcd <= 0)
			{
				for (int i = 0; i < 1; i++)
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
}
