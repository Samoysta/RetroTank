using Godot;
using System;

public partial class DamageArea : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void BodyEntered2D(Node2D body)
	{
		if (!body.IsInGroup("Player"))
		{
			if (body.HasMethod("TakeDamage"))
			{
				body.Call("TakeDamage", 2);
			}
		}
	}
}
