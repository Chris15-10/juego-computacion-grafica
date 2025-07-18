using Godot;
using System;

public partial class EnemigoShooter : Enemigo
{
    [Export] public float RangoDisparo = 200.0f;
    [Export] public float DistanciaMinima = 100.0f; // No se acerca más de esto
    [Export] public float VelocidadShooter = 40.0f;

    private Arma _arma;
    private Timer _timerDisparo;

    public override void _Ready()
    {
        base._Ready();

        _arma = GetNodeOrNull<Arma>("Arma");
        _timerDisparo = GetNodeOrNull<Timer>("Disparo");
        if (_timerDisparo != null)
            _timerDisparo.Timeout += Disparar;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (muerto || player == null) return;

        float distancia = GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distancia > RangoDisparo)
        {
            // Muy lejos: acércate lentamente
            Vector2 direccion = (player.GlobalPosition - GlobalPosition).Normalized();
            Velocity = direccion * VelocidadShooter;
            MoveAndSlide();
        }
        else if (distancia < DistanciaMinima)
        {
            // Demasiado cerca: aléjate
            Vector2 direccion = (GlobalPosition - player.GlobalPosition).Normalized();
            Velocity = direccion * VelocidadShooter;
            MoveAndSlide();
        }
        else
        {
            // En rango ideal: quieto
            Velocity = Vector2.Zero;
            MoveAndSlide();

            if (_arma != null)
            {
                _arma.LookAt(player.GlobalPosition);
            }
        }
    }

    private void Disparar()
    {
        if (muerto || player == null || _arma == null) return;

        float distancia = GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distancia <= RangoDisparo)
        {
            _arma.LookAt(player.GlobalPosition);
            _arma.Disparar(player.GlobalPosition);
        }
    }
}
