using Godot;
using System;
using System.Linq;

public partial class Effect : Node2D
{
	[Export] CpuParticles2D[] cpu;
	[Export] GpuParticles2D[] gpu;
	[Export] AnimatedSprite2D[] animSprite;
	[Export] AnimationPlayer[] animPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetOn()
	{
		Visible = true;
		if (cpu.Count() > 0)
		{
			foreach(CpuParticles2D cp in cpu)
			{
				cp.Emitting = true;
			}	
		}
		if (gpu.Count() > 0)
		{
			foreach(GpuParticles2D gp in gpu)
			{
				gp.Emitting = true;
			}	
		}
		if (animSprite.Count() > 0)
		{
			foreach(AnimatedSprite2D anim in animSprite)
			{
				anim.Frame = 0;
				anim.Play("Start");
			}	
		}
		if (animPlayer.Count() > 0)
		{
			foreach(AnimationPlayer anim in animPlayer)
			{
				anim.Seek(0);
				anim.Play("Start");
			}	
		}
	}
	public void SetOff()
	{
		Visible = false;
	}
}
