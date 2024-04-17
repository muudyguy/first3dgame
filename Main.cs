using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var mobTimer = GetNode<Timer>("MobTimer");
		mobTimer.Timeout += OnMobTimerTimeout;
		mobTimer.Start();
	}

	private void OnMobTimerTimeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();

		// Choose a random location on the SpawnPath.
		// We store the reference to the SpawnLocation node.
		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		// And give it a random offset.
		mobSpawnLocation.ProgressRatio = GD.Randf();

		Vector3 playerPosition = GetNode<Player>("Player").Position;
		mob.SetLocation(mobSpawnLocation.Position);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
