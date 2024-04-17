using System;
using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	public const float Speed = 5.0f;

	public int fallAcceleration { get; set; } = 14;
	private Vector3 targetVelocity = Vector3.Zero;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();


	private Vector3 facingDirection = Vector3.Zero;
	private float rotationSpeed = 0.1f;

	public void updateFacingDirection()
	{
		if (Input.IsActionPressed("move_right"))
		{
			facingDirection.X = Math.Clamp(facingDirection.X - rotationSpeed, -1.0f, 1.0f);
		}
		if (Input.IsActionPressed("move_left"))
		{
			facingDirection.X = Math.Clamp(facingDirection.X + rotationSpeed, -1.0f, 1.0f);
		}
		if (Input.IsActionPressed("move_back"))
		{
			facingDirection.Z = Math.Clamp(facingDirection.Z - rotationSpeed, -1.0f, 1.0f);
		}
		if (Input.IsActionPressed("move_forward"))
		{
			facingDirection.Z = Math.Clamp(facingDirection.Z + rotationSpeed, -1.0f, 1.0f);
		}

		if (facingDirection != Vector3.Zero)
		{
			facingDirection = facingDirection.Normalized();
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(facingDirection);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		updateFacingDirection();
		// We create a local variable to store the input direction.
		var direction = Vector3.Zero;

		// We check for each move input and update the direction accordingly.
		if (Input.IsActionPressed("move_right"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			// Notice how we are working with the vector's X and Z axes.
			// In 3D, the XZ plane is the ground plane.
			direction.Z -= 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z += 1.0f;
		}

		// Ground velocity
		targetVelocity.X = direction.X * Speed;
		targetVelocity.Z = direction.Z * Speed;

		if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
		{
			targetVelocity.Y -= fallAcceleration * (float)delta;
		}

		// Moving the character
		Velocity = targetVelocity;
		MoveAndSlide();
	}
}
