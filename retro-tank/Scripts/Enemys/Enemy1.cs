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
	[Export] AnimationPlayer anim;
	float updateT = 0.5f;
	Vector2 velocity;
    public override void _Ready()
    {
        updateT = updateTimer;
    }
    public override void _Process(double delta)
    {
        if (health <= 0)
		{
			QueueFree();
		}
    }

	public override void _PhysicsProcess(double delta)
	{
		//Çarpışma
		int collisionCount = GetSlideCollisionCount();
        for (int i = 0; i < collisionCount; i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if (collision != null)
			{
				Node2D body = (Node2D)collision.GetCollider();
				if (body != null && body.IsInGroup("Player"))
				{
					body.Call("TakeDamage", damage);
				}
			}
		}
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
