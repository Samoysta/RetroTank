using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Weapon2 : Area2D
{
	[Export] PackedScene fireBullet;
	[Export] float  fireCD;
	float firecd;
	Character player;
	EnemyManager manager;
	public Queue<FireBullet> bullets = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		firecd = fireCD;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player == null)
		{
			return;
		}
		GlobalPosition = player.GlobalPosition;
		if (firecd > 0)
		{
			firecd -= (float)delta;
		}
		else
		{
			firecd = fireCD;
			SpawnBullet();
		}
	}

	void SpawnBullet()
	{
		Node2D nearestEnemy = null;
		float nearestDistance = 1920;
		for (int i = 0; i < manager.AllEnemys.Count; i++)
		{
			float distance = manager.AllEnemys[i].GlobalPosition.DistanceTo(player.GlobalPosition);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestEnemy = manager.AllEnemys[i];
			}
		}
		if (bullets.Count > 0)
		{
			FireBullet bullet = bullets.Dequeue();
			bullet.GlobalPosition = GlobalPosition;
			if (nearestEnemy != null)
			{
				bullet.LookAt(nearestEnemy.GlobalPosition);
			}
			bullet.SetOn();
			bullet.ProcessMode = ProcessModeEnum.Pausable;
			return;
		}
		else
		{
			FireBullet bullet = fireBullet.Instantiate<FireBullet>();
			bullet.GlobalPosition = GlobalPosition;
			if (nearestEnemy != null)
			{
				bullet.LookAt(nearestEnemy.GlobalPosition);
			}
			bullet.Init(this);
			GetTree().CurrentScene.AddChild(bullet);
		}

	}

	void BodyEntered2D(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			Visible = false;
			GetNode<CollisionShape2D>("CollisionShape2D").CallDeferred("set_disabled",true);
			player = (Character)body;
			manager = player.manager;
		}
	}
}
