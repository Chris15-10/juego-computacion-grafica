using Godot;
using System;

public partial class Enemigo : CharacterBody2D
{
    [Export] public float Speed = 100f;
    [Export] public float ActivacionDistancia = 200f;

    protected Node2D player;
    protected AnimatedSprite2D _sprite;

    protected bool muerto = false;

    public override void _Ready()
    {
        var jugadores = GetTree().GetNodesInGroup("jugador");
        if (jugadores.Count > 0)
            player = jugadores[0] as Node2D;

        _sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (muerto || player == null) return;

        float distancia = GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distancia <= ActivacionDistancia)
        {
            Vector2 dir = GlobalPosition.DirectionTo(player.GlobalPosition);

            Velocity = dir * Speed;
            MoveAndSlide();

            if (dir.X != 0)
                _sprite.FlipH = dir.X < 0;

            ReproducirAnimacionPorDireccion(dir);
        }
        else
        {
            Velocity = Vector2.Zero;
            _sprite.Play("walk1");
        }
    }

    protected virtual void ReproducirAnimacionPorDireccion(Vector2 direccion)
    {
        if (direccion == Vector2.Zero)
            return;

        float angulo = direccion.Angle();
        angulo = Mathf.RadToDeg(angulo);

        if (angulo < 0)
            angulo += 360;

        int indice = Mathf.FloorToInt(angulo / 40.0f) + 1;

        if (indice > 9)
            indice = 9;

        string anim = "walk" + indice;

        bool voltear = (indice >= 6);

        _sprite.FlipH = voltear;
        _sprite.Play(anim);
    }

    public virtual void SetMuerto(bool estado)
    {
        muerto = estado;
    }
}
