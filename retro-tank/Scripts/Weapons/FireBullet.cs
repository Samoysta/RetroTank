using Godot;
using System;

public partial class FireBullet : Area2D
{
	[Export] float speed;
	Weapon2 weapon2;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		Visible = false;
		weapon2.bullets.Enqueue(this);
		SetProcessMode(ProcessModeEnum.Disabled); 
	}
	public void SetOn()
	{
		Visible = true;
	}
}
