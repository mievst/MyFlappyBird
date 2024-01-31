using Godot;

using System;
using System.Collections.Generic;
using System.Threading;

using static Godot.WebSocketPeer;

public partial class main : Node2D
{
	public List<Node2D> floors = new List<Node2D>();
	public List<Node2D> roofs = new List<Node2D>();
	public List<Node2D> pillars = new List<Node2D>();
	private List<string> floorAnimName = new List<string>{
		"1",
		"2",
		"3",
		"4",
	};
	private Random random = new();
	private int points;
	private int speed;
	private PackedScene floor;
	private PackedScene pillar;

	private List<Vector2> pillarStartPositions = new List<Vector2>
	{
		new Vector2(2450, 2400),
		new Vector2(2450, 900)
	};
	private const int pillarSize = 800;
	private int canAddPillar = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		floor = (PackedScene)ResourceLoader.Load("res://nodes/floor.tscn");
		pillar = (PackedScene)ResourceLoader.Load("res://nodes/pillar.tscn");
		SetFloorItem(0);
		for (int i = 0; i < 50; i++)
		{
			SetFloorItem(i);
		}
		while (floors[^1].Position.X <= 3000)
		{
			SetFloorItem(floors.Count);
		}
		SetRoofItem(0);
		while (roofs[^1].Position.X <= 3000)
		{
			SetRoofItem(roofs.Count);
		}
		points = 0;
		SetScore(points);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		try
		{
			player player = (player)FindChild("Player");
			if (player.IsDead)
			{
				if (Input.IsActionJustPressed("ui_accept"))
				{
					Thread.Sleep(500);
					GetTree().ReloadCurrentScene();
				}
				return;
			}
			if (player.Velocity.IsZeroApprox())
			{
				return;
			}
			else
			{
				speed = points + 5;
			}

			foreach (var item in floors)
			{
				item.Position = new Vector2
				{
					X = item.Position.X - speed,
					Y = item.Position.Y
				};

			}
			foreach (var item in roofs)
			{
				item.Position = new Vector2
				{
					X = item.Position.X - speed,
					Y = item.Position.Y
				};

			}
			foreach (var item in pillars)
			{
				if (item.Position.X > player.Position.X && item.Position.X - speed < player.Position.X)
				{
					points++;
					SetScore(points);
				}
				item.Position = new Vector2
				{
					X = item.Position.X - speed,
					Y = item.Position.Y
				};

			}
			if (floors[0].Position.X < -10)
			{
				floors.RemoveAt(0);
			}
			while (floors[^1].Position.X < 3000)
			{
				SetFloorItem(floors.Count);
			}
			if (roofs[0].Position.X < -10)
			{
				roofs.RemoveAt(0);
			}
			while (roofs[^1].Position.X < 3000)
			{
				SetRoofItem(roofs.Count);
			}
			if (canAddPillar == 0)
			{
				canAddPillar = random.Next(150, 400) - points;
				if (canAddPillar < 100){
					canAddPillar = 100;
				}
				SetPillarItem();
			}
			if (pillars.Count>0 && pillars[0].Position.X < -10)
			{
				roofs.RemoveAt(0);
			}
			canAddPillar--;
		}
		catch (Exception ex)
		{
			speed += 0;
			throw;
		}

	}

	private void SetFloorItem(int position)
	{
		Node2D floorInstance = (Node2D)floor.Instantiate();
		floorInstance.Name = $"Floor_{new Guid()}";
		var startPosition = 0;
		if (position != 0){
			startPosition = (int)floors[0].Position.X;
		}
		floorInstance.Position = new Vector2(startPosition + (position * 80), 2400);
		floorInstance.Scale = new Vector2(5, 5);
		var sprite = (AnimatedSprite2D)floorInstance.FindChild("AnimatedSprite2D");
		sprite.Animation = floorAnimName[random.Next(4)];
		AddChild(floorInstance);
		floors.Add(floorInstance);
	}

	private void SetRoofItem(int position)
	{
		Node2D floorInstance = (Node2D)floor.Instantiate();
		floorInstance.Name = $"Floor_{new Guid()}";
		var startPosition = 0;
		if (position != 0){
			startPosition = (int)floors[0].Position.X;
		}
		floorInstance.Position = new Vector2(startPosition + (position * 80), 100);
		floorInstance.Scale = new Vector2(-5, -5);
		var sprite = (AnimatedSprite2D)floorInstance.FindChild("AnimatedSprite2D");
		sprite.Animation = floorAnimName[random.Next(4)];
		AddChild(floorInstance);
		roofs.Add(floorInstance);
	}

	private void SetPillarItem()
	{
		int variant;
		if (points > 10)
		{
			variant = random.Next(4);
		}
		else
		{
			variant = random.Next(2);
		}
		Node2D pillarInstance = (Node2D)pillar.Instantiate();
		Node2D pillarInstance2;
		switch (variant)
		{
			case 0:
				pillarInstance.Name = $"Pillar_{new Guid()}";
				pillarInstance.Position = pillarStartPositions[0];
				pillarInstance.Scale = new Vector2(10, 10);
				AddChild(pillarInstance);
				pillars.Add(pillarInstance);
				break;
			case 1:
				pillarInstance.Name = $"Pillar_{new Guid()}";
				pillarInstance.Position = pillarStartPositions[1];
				pillarInstance.Scale = new Vector2(10, 10);
				AddChild(pillarInstance);
				pillars.Add(pillarInstance);
				break;
			case 2:
				pillarInstance.Name = $"Pillar_{new Guid()}";
				pillarInstance.Position = pillarStartPositions[0];
				pillarInstance.Scale = new Vector2(10, 10);
				AddChild(pillarInstance);
				pillars.Add(pillarInstance);

				pillarInstance2 = (Node2D)pillar.Instantiate();
				pillarInstance2.Name = $"Pillar_{new Guid()}";
				var position_up = new Vector2
				{
					X = pillarStartPositions[0].X,
					Y = pillarStartPositions[0].Y - pillarSize
				};
				pillarInstance2.Position = position_up;
				pillarInstance2.Scale = new Vector2(10, 10);
				AddChild(pillarInstance2);
				pillars.Add(pillarInstance2);
				break;
			case 3:
				pillarInstance.Name = $"Pillar_{new Guid()}";
				pillarInstance.Position = pillarStartPositions[1];
				pillarInstance.Scale = new Vector2(10, 10);
				AddChild(pillarInstance);
				pillars.Add(pillarInstance);

				pillarInstance2 = (Node2D)pillar.Instantiate();
				pillarInstance2.Name = $"Pillar_{new Guid()}";
				var position_down = new Vector2
				{
					X = pillarStartPositions[1].X,
					Y = pillarStartPositions[1].Y + pillarSize
				};
				pillarInstance2.Position = position_down;
				pillarInstance2.Scale = new Vector2(10, 10);
				AddChild(pillarInstance2);
				pillars.Add(pillarInstance2);
				break;
			default:
				break;
		}
	}

	private void SetScore(int points)
	{
		Node2D scoreNode = (Node2D)FindChild("Score");
		scoreNode.TopLevel = true;
		var score = (RichTextLabel)scoreNode.FindChild("RichTextLabel");
		score.Text = $"Your score: {points}";
	}
}
