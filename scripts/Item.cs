using Godot;
using System;

public partial class Item : Area2D
{
	[Export] public ArmaConfig Arma;

	private Sprite2D _icono;
	private Label _mensaje;
	private bool _jugadorCerca = false;
	private Personaje1 _personaje;

	public override void _Ready()
	{
		_icono = GetNode<Sprite2D>("Sprite2D");
		_mensaje = GetNode<Label>("Mensaje");
		_mensaje.Visible = false;

		if (Arma != null && Arma.Icono != null)
		{
			_icono.Texture = Arma.Icono;
		}

		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		if (_jugadorCerca && Input.IsActionJustPressed("x"))
		{
			ArmaConfig Anterior = _personaje.RecogerArma(Arma);
			cargar(Anterior);
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Personaje1 pj)
		{
			_jugadorCerca = true;
			_personaje = pj;
			_mensaje.Visible = true;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body == _personaje)
		{
			_jugadorCerca = false;
			_mensaje.Visible = false;
		}
	}
	public void cargar(ArmaConfig nueva)
	{
		if (nueva != null)
		{
			Arma = nueva;
		}
		if (Arma != null && _icono != null)
		{
			_icono.Texture = Arma.Icono;
		}
	}
}
