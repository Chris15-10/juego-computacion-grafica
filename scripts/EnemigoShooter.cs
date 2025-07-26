using Godot;
using System;

public partial class EnemigoShooter : Enemigo
{
    [Export] public enemigoRecurso config;
    [Export] public float RangoDisparo = 200.0f; // distancia desde la que disparan (determina el tamaño del Raycast)
    [Export] public float DistanciaMinima = 100.0f; // distancia a partir de la que los enemigos se alejan del jugador
    [Export] public float VelocidadShooter = 40.0f;
    protected AnimatedSprite2D _animatedSprite;
    private Arma _arma;
    private Timer _timerDisparo;
    private RayCast2D _rayJugador;

    public override void _Ready()
    {
        base._Ready();
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _arma = GetNodeOrNull<Arma>("Arma");
        _timerDisparo = GetNodeOrNull<Timer>("Disparo");
        _rayJugador = GetNodeOrNull<RayCast2D>("RayJugador");

        if (config != null)
        {
            _animatedSprite.SpriteFrames = config._sprite;
            _animatedSprite.Scale = new Vector2(config.tamano, config.tamano);
            RangoDisparo = config.RangoDisparo;
            DistanciaMinima = config.DistanciaMinima;
            VelocidadShooter = config.velocidad;
        }
        
        if (_rayJugador != null)
            _rayJugador.Enabled = true;

        if (_timerDisparo != null)
            _timerDisparo.Timeout += Disparar;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (muerto || player == null) return;

        Node2D centro = player.GetNodeOrNull<Node2D>("Centro");
        if (centro == null) return;

        Vector2 direccionCentro = (centro.GlobalPosition - GlobalPosition).Normalized();

        if (_rayJugador != null)
        {
            _rayJugador.TargetPosition = direccionCentro * RangoDisparo;
            _rayJugador.ForceRaycastUpdate();
        }

        float distancia = GlobalPosition.DistanceTo(centro.GlobalPosition);

        bool jugadorVisible = false;
        if (_rayJugador != null && _rayJugador.IsColliding())
        {
            Node? collider = _rayJugador.GetCollider() as Node;
            if (collider != null && (collider.IsInGroup("jugador") || (collider.GetParent()?.IsInGroup("jugador") == true)))
                jugadorVisible = true;
        }

        if (distancia < DistanciaMinima)
        {
            Velocity = -direccionCentro * VelocidadShooter;
        }
        else if (jugadorVisible)
        {
            Velocity = Vector2.Zero;
            _arma?.LookAt(centro.GlobalPosition);
        }
        else
        {
            Velocity = direccionCentro * VelocidadShooter;
        }

        MoveAndSlide();

        if (direccionCentro != Vector2.Zero)
            ReproducirAnimacionPorDireccion(direccionCentro);
    }

    private void Disparar()
    {
        if (muerto || _arma == null || _rayJugador == null) return;

        Vector2 direccion = (player.GlobalPosition - GlobalPosition).Normalized();
        _rayJugador.TargetPosition = direccion * RangoDisparo;
        _rayJugador.ForceRaycastUpdate();

        if (_rayJugador.IsColliding())
        {
            Node? collider = _rayJugador.GetCollider() as Node;
            if (collider != null && (collider.IsInGroup("jugador") || (collider.GetParent()?.IsInGroup("jugador") == true)))
            {
                _arma.LookAt(player.GlobalPosition);
                _arma.Disparar(player.GlobalPosition);
                return;
            }
        }
    }
}
