using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class WeaponMenu : Node2D
{
	[Export] PackedScene weaponMenu;
	[Export] int weaponAmount;
	Node2D[] weaponNodes;
	[Export] float darlik;
	[Export] Label weaponInfo;
	//Weapons
	[Export] PackedScene[] weapons;
	[Export] Texture2D[] icons;
	[Export] string[] weaponNames;
	[Export] string[] weaponDescriptions;
	List<PackedScene> weapons2D;
	List<Texture2D> icons2D;
	List<string> Names;
	List<string> Descriptions;
	Character character;
	Tween tween;
	bool selected;
	RandomNumberGenerator rnd = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		character = (Character)GetParent().GetParent();
		weaponNodes = new Node2D[(weaponAmount * 2) - 1];
		rnd.Randomize();
		for (int i = -weaponAmount + 1; i < weaponAmount; i++)
		{
			Node2D wp = (Node2D)weaponMenu.Instantiate();
			wp.GlobalPosition = new Vector2(0,-1080 / darlik * i);
			AddChild(wp);
			weaponNodes[i + weaponAmount - 1] = wp;
		}
		Names = new List<string>(weaponNames);
		icons2D = new List<Texture2D>(icons);
		weapons2D = new List<PackedScene>(weapons);
		Descriptions = new List<string>(weaponDescriptions);
		Visible = false;
		Scale = Vector2.Zero;
		ProcessMode = ProcessModeEnum.Disabled;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RandomWeapon()
	{
		List<int> weaponPercents = new ();
		for (int i = 0; i < weaponNodes.Length; i++)
		{
			if (i >= weapons2D.Count)
        	{
            	weaponNodes[i].Visible = false;
				weaponNodes[i].ProcessMode = ProcessModeEnum.Disabled;
            	continue;
        	}
			int weaponPercent = rnd.RandiRange(0,weapons2D.Count - 1);
			if (weaponPercents.Contains(weaponPercent))
			{
				while (weaponPercents.Contains(weaponPercent))
				{
					weaponPercent = rnd.RandiRange(0,weapons2D.Count - 1);
				}
			}
			weaponPercents.Add(weaponPercent);
			weaponNodes[i].GetNode<Sprite2D>("Control/Weapon Icon").Texture = icons2D[weaponPercent];
			weaponNodes[i].GetNode<Label>("Control/Weapon Button").Text = Names[weaponPercent];
		}

	}

	public void WeaponSpawn(string weaponName)
	{
		if (!selected)
		{
			int index = Names.IndexOf(weaponName);

			Node2D weapon = (Node2D)weapons2D[index].Instantiate();
			weapon.GlobalPosition = character.GlobalPosition;
			character.AddChild(weapon);

			Names.RemoveAt(index);
			icons2D.RemoveAt(index);
			weapons2D.RemoveAt(index);
			Descriptions.RemoveAt(index);
			SetOff();	
			selected = true;
			weaponInfo.Text = "Select a weapon";
		}
	}
	public void SetOn()
	{
		if (weapons2D.Count > 0)
		{
			selected = false;
			tween?.Kill();
			tween = CreateTween();
			tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
			tween.TweenProperty(this, "scale", new Vector2(1,1), 0.5f);
			RandomWeapon();	
		}
		else
		{
			Visible = false;
			GetTree().Paused = false;
			ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	public void SetOff()
	{
		tween?.Kill();
		tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "scale", new Vector2(0,0), 0.3f).Finished += () => {ProcessMode = ProcessModeEnum.Disabled; Visible = false;};
		GetTree().Paused = false;
	}

	public void Toggled(string weaponName)
	{
		int index = Names.IndexOf(weaponName);
		weaponInfo.Text = Descriptions[index];
	}
}
