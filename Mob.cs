using Godot;
using System;

public partial class Mob : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	private Vector3 facingDirection = Vector3.Zero;
	private float rotationSpeed = 1f / 100f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	Player player;

	public void SetLocation(Vector3 location)
	{
		Position = location;
	}

	public override void _Ready()
	{
		Random random = new Random();
		facingDirection.X = (float)random.NextDouble();
		facingDirection.Y = 0;
		facingDirection.Z = (float)random.NextDouble();

		Timer timer = GetNode<Timer>("DeathTimer");
		timer.Timeout += killIfTimeout;

		player = GetNode<Player>("../Player");

		timer.Start();
	}

	private void killIfTimeout()
	{
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player != null)
		{
			Vector3 directionToPlayer = Position.DirectionTo(player.Position);

			var angle = facingDirection.SignedAngleTo(directionToPlayer, new Vector3(0, 1, 0));
			if (!directionToPlayer.Equals(facingDirection))
			{
				facingDirection = facingDirection.Rotated(new Vector3(0, 1, 0), (float) angle * rotationSpeed);
				GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(facingDirection);
			}
			// We set the mob's target to the player's position.
		}

		Velocity = new Vector3(facingDirection.X*Speed, 0, facingDirection.Z*Speed);
		// KinematicCollision3D kinematicCollision3D = MoveAndCollide(Velocity);
		// if (kinematicCollision3D.GetCollider() is Mob)
		// {
		// 	facingDirection = -facingDirection;
		// }
		MoveAndSlide();
	}
}
