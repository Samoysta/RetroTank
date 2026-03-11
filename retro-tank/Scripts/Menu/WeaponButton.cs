using Godot;
using System;
using System.Collections;

public partial class WeaponButton : Node2D
{
	Control weaponButton;
	WeaponMenu weaponMenu;
	Tween tween;
	Vector2 firstScale;
	Label text;
	bool toggled;
	AnimationPlayer anim;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		weaponButton = GetNode<Control>("Control");
		weaponMenu = (WeaponMenu)GetParent();
		text = weaponButton.GetNode<Label>("Weapon Button");
		firstScale = weaponButton.Scale;
		weaponButton.MouseEntered += OnMouseEntered;
		weaponButton.MouseExited += OnMouseExited;
		anim = weaponButton.GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("LeftMouse"))
		{
			if (toggled)
			{
				ButtonDown();
			}
		}
	}

	void ButtonDown()
	{
		string weaponName = text.Text;
		weaponMenu.Call("WeaponSpawn",weaponName);
	}

	void OnMouseEntered()
	{
		anim.Play("Start");
		toggled = true;
		tween?.Kill();
		tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(weaponButton, "scale", firstScale * 1.2f, 0.3f);
		weaponMenu.Call("Toggled",text.Text);
	}
	void OnMouseExited()
	{
		anim.PlayBackwards("Start");
		toggled = false;
		tween?.Kill();
		tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(weaponButton, "scale", firstScale, 0.3f);
	}
}
