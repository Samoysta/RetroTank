using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyManager : Node2D
{
	[Export] int maxEnemy;
	[Export] float enemySpawnCD;
	[Export] Character player;
	public int enemyAmount;
	float enemyTimer;
	[Export] public Node2D[] spawnPositions;
	RandomNumberGenerator rnd = new ();
	[Export] Node2D trash;
	//Lasers
	[Export] PackedScene Laser1;
	public Queue<Laser> laser1s = new();
	//Enemys
	[Export] PackedScene enemy1;
	public Queue<Enemy1> enemy1s = new();
	[Export] PackedScene enemy2;
	public Queue<Enemy2> enemy2s = new();
	int enemyPercent;
	public List<Node2D> AllEnemys = new();
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
		if (player.killAmount <= 10)
		{
			enemyPercent = 0;
		}
		else if (player.killAmount > 10 && player.killAmount <= 30)
		{
			enemyPercent = rnd.RandiRange(0,1);
		}
		else if (player.killAmount > 30 && player.killAmount <= 70)
		{
			enemyPercent = rnd.RandiRange(0,2);
		}
		else if (player.killAmount > 70 && player.killAmount <= 100)
		{
			
		}
	}

	public void SpawnEnemy()
	{
		Vector2 pos = spawnPositions[rnd.RandiRange(0,7)].GlobalPosition;

		///////Enemy-1///////
		if (enemyPercent == 0)
		{
			if (enemy1s.Count <= 0)
			{
				Enemy1 enem = (Enemy1)enemy1.Instantiate();
				enem.GlobalPosition = pos;
				GetTree().CurrentScene.AddChild(enem);
				enem.Init(this);
				enem.SetOn();
			}
			else
			{
				Enemy1 enem = enemy1s.Dequeue();
				enem.GlobalPosition = pos;
				enem.CallDeferred("SetOn");
			}	
		}
		else if(enemyPercent == 1)
		{
			//Laser
			int randomRotation = rnd.RandiRange(0 , 359);
			if (laser1s.Count <= 0)
			{
				Laser enem = (Laser)Laser1.Instantiate();
				enem.GlobalPosition = player.GlobalPosition;
				enem.GlobalRotationDegrees = randomRotation;
				enem.Init(this);
				enem.SetOn();
				GetTree().CurrentScene.AddChild(enem);
			}
			else
			{
				Laser enem = laser1s.Dequeue();
				enem.GlobalPosition = player.GlobalPosition;
				enem.GlobalRotationDegrees = randomRotation;
				enem.CallDeferred("SetOn");
			}	
			
		}
		else if(enemyPercent == 2)
		{
			if (enemy2s.Count <= 0)
			{
				Enemy2 enem = (Enemy2)enemy2.Instantiate();
				enem.GlobalPosition = pos;
				GetTree().CurrentScene.AddChild(enem);
				enem.Init(this);
				enem.SetOn();
			}
			else
			{
				Enemy2 enem = enemy2s.Dequeue();
				enem.GlobalPosition = pos;
				enem.CallDeferred("SetOn");
			}	
		}

		enemyAmount++;
	}

	public void SetPos(Node2D body)
	{
		body.GlobalPosition = trash.GlobalPosition;
	}
}
