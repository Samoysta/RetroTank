using Godot;
using System;

public partial class Laser : Area2D
{
	[Export] AnimationPlayer anim;
	EnemyManager manager;
	float timer = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (timer > 0)
		{
			timer -= (float)delta;
		}
		if (timer <= 0)
		{
			SetOff();
		}
	}

	public void SetOn()
	{
		anim.Play("Start");
		Visible = true;
		timer = 1.5f;
		SetProcess(true);

	}
	public void SetOff()
	{
		SetProcess(false);
		Visible = false;
		manager.laser1s.Enqueue(this);
		manager.enemyAmount --;
	}

	public void Init(EnemyManager man)
	{
		manager = man;
	}

	void BodyEntered2D(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			body.Call("TakeDamage", 1);
		}
	}
}
