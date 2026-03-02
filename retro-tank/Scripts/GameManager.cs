using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export] EnemyManager managerEnemy;
	RandomNumberGenerator rnd = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rnd.Randomize();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	void BodyEntered(Node2D body)
	{
		body.GlobalPosition = Vector2.Zero;
	}
}
