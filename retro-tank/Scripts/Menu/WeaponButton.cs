using Godot;
using System;
using System.Collections;

public partial class WeaponButton : Node2D
{
	Button weaponButton;
	WeaponMenu weaponMenu;
	Tween tween;
	Vector2 firstScale;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		weaponButton = GetNode<Button>("Weapon Button");
		weaponMenu = (WeaponMenu)GetParent();
		firstScale = weaponButton.Scale;
		weaponButton.MouseEntered += OnMouseEntered;
		weaponButton.MouseExited += OnMouseExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void ButtonDown()
	{
		string weaponName = weaponButton.Text;
		weaponMenu.Call("WeaponSpawn",weaponButton.Text);
	}

	void OnMouseEntered()
	{
		tween?.Kill();
		tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(weaponButton, "scale", firstScale * 1.2f, 0.3f);
	}
	void OnMouseExited()
	{
		tween?.Kill();
		tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(weaponButton, "scale", firstScale, 0.3f);
	}
}
