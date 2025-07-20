using Godot;
using System;

public partial class Bala : Area2D
{
    private Vector2 _direccion;
    private int _daño;
    private int _velocidad;
    private Sprite2D _sprite;
    private string _grupo;
    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        BodyEntered += OnBodyEntered;
    }

    public void Init(Vector2 direction, int velocidad, int dano, Texture2D textura, string grupo)
    {
        _direccion = direction;
        _velocidad = velocidad;
        _daño = dano;
        _grupo = grupo;

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
    if (body is TileMapLayer)
    {
        QueueFree(); 
        return;
    }

    if (body.IsInGroup(_grupo))
    {
        var vida = body.GetNodeOrNull<Vida>("Vida");
        if (vida != null)
        {
            vida.RecibirDano(_daño);
        }
        QueueFree();
    }
}



}

