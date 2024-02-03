using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody2D
{
	public const float Gravity = 50f;
	public const float Jump = -1000f;
	public bool Started = false;
	public bool IsDead = false;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		if (IsDead) return;
		Vector2 velocity = new Vector2(Velocity.X, Velocity.Y);

		if (!Velocity.IsZeroApprox() || Started)
		{
			velocity.Y += Gravity;
			if (velocity.Y > 1000) velocity.Y = 1000;
			if (velocity.Y > 0)
			{
				var sprite = (AnimatedSprite2D)FindChild("AnimatedSprite2D");
				sprite.Animation = "Idle";
			}
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && !IsDead)
		{
			Started = true;
			/* if (Position.Y < 500)
			{
				velocity.Y += Jump / 500 - Position.Y;
			}
			else
			{
				velocity.Y += Jump;

			} */
			velocity.Y = Jump;
			var sprite = (AnimatedSprite2D)FindChild("AnimatedSprite2D");
			sprite.Animation = "Up";
		}


		Velocity = velocity;
		bool colided = MoveAndSlide();
		if (Position.Y < 5)
		{
			Position = new Vector2
			{
				X = Position.X,
				Y = 5
			};
		}
		if (colided)
		{
			Velocity = new Vector2(0, 0);
			Node2D main = (Node2D)FindParent("main");
			Node2D scoreNode = (Node2D)main.FindChild("Score");
			var score = (RichTextLabel)scoreNode.FindChild("RichTextLabel");
			score.Text += $"\nYOU LOSE!";
			IsDead = true;
		}
	}
}
