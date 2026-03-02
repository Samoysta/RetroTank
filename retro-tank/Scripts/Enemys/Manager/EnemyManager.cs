using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyManager : Node2D
{
	[Export] int maxEnemy;
	[Export] float enemySpawnCD;
	public int enemyAmount;
	float enemyTimer;
	[Export] public Node2D[] spawnPositions;
	RandomNumberGenerator rnd = new ();
	[Export] Node2D trash;
	//Enemys
	[Export] PackedScene enemy1;
	public Queue<Enemy1> enemy1s = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rnd.Randomize();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (enemyTimer > 0)
		{
			enemyTimer -= (float)delta;
		}
		if (enemyTimer <= 0)
		{
			if (enemyAmount < maxEnemy)
			{
				SpawnEnemy();
				enemyTimer = enemySpawnCD;	
			}
		}
	}

	public void SpawnEnemy()
	{
		Vector2 pos = spawnPositions[rnd.RandiRange(0,3)].GlobalPosition;
		///////Enemy-1///////
		if (enemy1s.Count <= 0)
		{
			Enemy1 enem = (Enemy1)enemy1.Instantiate();
			enem.GlobalPosition = pos;
			enem.Init(this);
			enem.SetOff();
			enem.SetOn();
			GetTree().CurrentScene.AddChild(enem);
		}
		else
		{
			Enemy1 enem = enemy1s.Dequeue();
			enem.GlobalPosition = pos;
			enem.CallDeferred("SetOn");
		}

		enemyAmount++;
	}

	public void SetPos(Node2D body)
	{
		body.GlobalPosition = trash.GlobalPosition;
	}
}
