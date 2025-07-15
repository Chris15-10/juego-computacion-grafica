using Godot;
using System;

public partial class Enemigo : CharacterBody2D
{
    [Export] public float Speed = 100.0f; // velocidad del enemigo
    [Export] public float ActivacionDistancia = 200.0f; // distancia a partir de la cual persigue al personaje
    private Node2D player1;
    private AnimatedSprite2D _sprite;

    // ataque al jugador
    [Export] public int dano = 20;
    [Export] public float rango_ataque = 40.0f;

    private Timer _timerAtaque;

    public override void _Ready()
    {
        var players = GetTree().GetNodesInGroup("jugador");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (players.Count > 0)
        {
            player1 = players[0] as Node2D;
        }
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _timerAtaque = GetNode<Timer>("Ataque");
        _timerAtaque.Timeout += AtacarJugador;
    }

    private bool muerto = false;

    public override void _PhysicsProcess(double delta)
    {
        if (muerto || player1 == null) return;

        float distancia = GlobalPosition.DistanceTo(player1.GlobalPosition);

        if (distancia <= ActivacionDistancia)
        {
            Vector2 dir = GlobalPosition.DirectionTo(player1.GlobalPosition);

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

    private void AtacarJugador()
    {
        if (muerto) return;
        
        if (player1 == null) return;

        float distancia = GlobalPosition.DistanceTo(player1.GlobalPosition);

        if (distancia <= rango_ataque)
        {
            // componente de vida dentro del jugador
            var vida = player1.GetNodeOrNull<Vida>("Vida");
            if (vida != null)
            {
                vida.RecibirDano(dano);
            }
        }
    }
    
    public void SetMuerto(bool estado)
    {
        muerto = estado;
    }
    
    private void ReproducirAnimacionPorDireccion(Vector2 direccion)
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
}
