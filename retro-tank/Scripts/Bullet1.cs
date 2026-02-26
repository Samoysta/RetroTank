using Godot;
using System;

public partial class Bullet1 : Node2D
{
	[Export] ShapeCast2D shapeCast;
	[Export] float speed;
	int damage;
	Character character;
	Vector2 beforePos;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		beforePos = GlobalPosition;
	}
    public override void _PhysicsProcess(double delta)
    {
		MoveLocalX(speed * (float)delta);
		Vector2 distance = new Vector2(speed * (float)delta,0);
		shapeCast.GlobalPosition = beforePos;
		shapeCast.TargetPosition = distance;
		beforePos = GlobalPosition;
        if (shapeCast.IsColliding())
		{
			Node2D body = (Node2D)shapeCast.GetCollider(0);
			if (!body.IsInGroup("Player"))
			{
				Vector2 hitPos = shapeCast.GetCollisionPoint(0);
				if (body.HasMethod("TakeDamage"))
				{
					body.Call("TakeDamage", damage);
				}
				SetOff();
				HitEffect(hitPos);
			}
		}
    }
	public void HitEffect(Vector2 hitPos)
	{
		Effect ef = character.bulletHitEffects.Dequeue();
		ef.GlobalPosition = hitPos;
		ef.GlobalRotationDegrees = GlobalRotationDegrees;
		ef.SetOn();
		character.bulletHitEffects.Enqueue(ef);
	}
	public void Init(Character body)
	{
		character = body;
	}

	public void SetOff()
	{
		Visible = false;
		SetPhysicsProcess(false);
		shapeCast.Enabled = false;
		character.bullets.Enqueue(this);
	}

	public void SetOn()
	{
		Visible = true;
		beforePos = GlobalPosition;
		SetPhysicsProcess(true);
		shapeCast.Enabled = true;
	}

}
