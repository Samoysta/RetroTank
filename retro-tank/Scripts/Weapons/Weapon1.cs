using Godot;
using System;

public partial class Weapon1 : Area2D
{
	[Export] PackedScene childWeapon;
	[Export] int WeaponAmount;
	[Export] float speed;
	CollisionShape2D hitbox;
	Node2D player;
	Sprite2D sprite;
	Node2D[] childWeapons;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hitbox = GetNode<CollisionShape2D>("CollisionShape2D");
		sprite = hitbox.GetNode<Sprite2D>("Sprite2D");
		childWeapons = new Node2D[WeaponAmount];
		for (int i = 0; i < WeaponAmount; i++)
		{
			Node2D child = (Node2D)childWeapon.Instantiate();
			child.Position = Vector2.Zero;
			child.RotationDegrees = 360 / WeaponAmount * i;
			AddChild(child);
			childWeapons[i] = child;
		}
		SetOff();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = player.GlobalPosition;
		GlobalRotationDegrees += speed * (float)delta;
	}

	public void SetOff()
	{
		for (int i = 0; i < WeaponAmount; i++)
		{
			Node2D child = childWeapons[i];
			child.Visible = false;
			child.GetNode<CollisionShape2D>("CollisionShape2D").CallDeferred("set_disabled",true);
		}
		SetProcess(false);
	}

	public void SetOn()
	{
		for (int i = 0; i < WeaponAmount; i++)
		{
			Node2D child = childWeapons[i];
			child.Visible = true;
			child.GetNode<CollisionShape2D>("CollisionShape2D").CallDeferred("set_disabled",false);
		}
		SetProcess(true);
	}

	void BodyEntered2D(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			player = body;
			sprite.Visible = false;
			hitbox.CallDeferred("set_disabled",true);
			SetOn();
		}
	}
}
