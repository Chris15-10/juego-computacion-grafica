using Godot;
using System;

public partial class Bala : Area2D
{
    private Vector2 _direccion;
    private int _daño;
    private int _velocidad;

    public void Init(Vector2 direction, int velocidad, int dano)
    {
        _direccion = direction;
        _velocidad = velocidad;
        _daño = dano;
    }
  
    public override void _PhysicsProcess(double delta)
    {
        Position += _direccion * _velocidad * (float)delta;

        if (!GetViewportRect().HasPoint(Position))
        {
            QueueFree();
        }
    }
}

