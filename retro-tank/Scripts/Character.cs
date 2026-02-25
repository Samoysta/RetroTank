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
	float bulletcd;
	public Queue<Bullet1> bullets = new ();
	Vector2 velocity;
    public override void _Process(double delta)
    {
		if (bulletcd > 0)
		{
			bulletcd -= (float)delta;
		}
        LookAt(GetGlobalMousePosition());
		if (Input.IsActionPressed("LeftMouse"))
		{
			if (bulletcd <= 0)
			{
				SpawnBullet();
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

	void SpawnBullet()
	{
		if (bullets.Count == 0)
		{
			Bullet1 bullet = (Bullet1)bullet1.Instantiate();
			bullet.GlobalRotationDegrees = GlobalRotationDegrees;
			bullet.GlobalPosition = bulletPos.GlobalPosition;
			GetTree().CurrentScene.AddChild(bullet);
			bullet.Call("Init", this);	
		}
		else
		{
			Bullet1 bullet = bullets.Dequeue();
			bullet.GlobalPosition = bulletPos.GlobalPosition;
			bullet.GlobalRotationDegrees = GlobalRotationDegrees;
			bullet.SetOn();
		}
		
	}
}
