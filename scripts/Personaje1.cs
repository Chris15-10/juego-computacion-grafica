using Godot;
using System;

public partial class Personaje1 : CharacterBody2D
{
	[Export]
	public float MoveSpeed = 300.0f;

	[Export]
	public AnimatedSprite2D _animatedSprite2D;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Obtener la dirección de entrada (movimiento en X e Y)
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction.X < 0)
		{
			_animatedSprite2D.FlipH = true;
		}
		else
		{
			_animatedSprite2D.FlipH = false;
		}

		if (direction.X == 0 && direction.Y == 0)
		{
			_animatedSprite2D.Play("default");
		}
		else
		{
			_animatedSprite2D.Play("caminando");
		}

		if (direction != Vector2.Zero)
		{
			direction = direction.Normalized();
			velocity = direction * MoveSpeed;
		}
		else
		{
			velocity = Vector2.Zero;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
