using Godot;
using System;

public partial class Bala : Area2D
{
    private Vector2 _direccion;
    private int _daño;
    private int _velocidad;

    private Sprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        BodyEntered += OnBodyEntered;
    }

    public void Init(Vector2 direction, int velocidad, int dano, Texture2D textura)
    {
        _direccion = direction;
        _velocidad = velocidad;
        _daño = dano;

        // Asegúrate que el sprite ya esté referenciado
        if (_sprite == null)
        {
            _sprite = GetNode<Sprite2D>("Sprite2D");
        }

        if (_sprite != null && textura != null)
        {
            _sprite.Texture = textura;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direccion * _velocidad * (float)delta;

        if (!GetViewportRect().HasPoint(Position))
        {
            QueueFree();
        }
    }
    private void OnBodyEntered(Node body)
    {
        var vida = body.GetNodeOrNull<Vida>("VidaEnemigo");

        if (vida != null)
        {
            GD.Print("Bala golpeó a: " + body.Name);
            vida.RecibirDano(_daño);
        }

        QueueFree(); 
    }
}


