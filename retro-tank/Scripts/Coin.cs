using Godot;
using System;

public partial class Coin : Node2D
{
	Character player;
	float distance;
	bool setOffed;
	[Export] int spawnDistance;
	Tween tween;
	RandomNumberGenerator rnd = new();
	bool goPlayer;
	float speed;
	Sprite2D sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		rnd.Randomize();
		SetOn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
    public override void _PhysicsProcess(double delta)
    {
        distance = GlobalPosition.DistanceTo(player.GlobalPosition);
		if (distance < 40)
		{
			SetOff();
			player.coinAmount++;
		}
		if (goPlayer)
		{
			speed += 700 * (float)delta;
			GlobalPosition = GlobalPosition.MoveToward(player.GlobalPosition, speed * (float)delta);
			
		}
    }
	public void Init(Character character)
	{
		player = character;
	}

	public void SetOff()
	{
		if (!setOffed)
		{
			sprite.Visible = false;
			SetPhysicsProcess(false);
			player.coins.Enqueue(this);
			goPlayer = false;
			setOffed = true;
			speed = 0;
		}
	}

	public void SetOn()
	{
		Vector2 direction = new Vector2(spawnDistance,0).Rotated(rnd.RandiRange(0,359));
		tween?.Kill();
		tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "global_position", GlobalPosition + direction, 0.5f).Finished += () =>
		{
			goPlayer = true;
		};
		setOffed = false;
		sprite.Visible = true;
		SetPhysicsProcess(true);
	}

}
