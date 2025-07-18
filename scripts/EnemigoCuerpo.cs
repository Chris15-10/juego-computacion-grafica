using Godot;
using System;

public partial class EnemigoCuerpo : Enemigo
{
    [Export] public int dano = 20;
    [Export] public float rango_ataque = 40.0f;
    private Timer _timerAtaque;

    public override void _Ready()
    {
        base._Ready();
        _timerAtaque = GetNodeOrNull<Timer>("Ataque");
        if (_timerAtaque != null)
            _timerAtaque.Timeout += AtacarJugador;
    }

    private void AtacarJugador()
    {
        if (muerto) return;
        if (player == null) return;

        float distancia = GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distancia <= rango_ataque)
        {
            if (player.IsInGroup("jugador"))
            {
                var vida = player.GetNodeOrNull<Vida>("Vida");
                vida.RecibirDano(dano);
            }
        }
    }
}
