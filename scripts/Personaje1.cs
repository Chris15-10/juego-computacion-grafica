using Godot;
using System;

public partial class Personaje1 : CharacterBody2D
{
    [Export] public PersonajeConfig Config;

    private AnimatedSprite2D _animatedSprite;
    [Export] private Arma _arma;
    private Vector2 pos;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (Config != null)
        {
            _animatedSprite.SpriteFrames = Config.Animaciones;
        }

        _arma = GetNode<Arma>("Arma");

        pos = _arma.Position;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (Config == null) return;

        Vector2 direction = Input.GetVector("izquierda", "derecha", "arriba", "abajo");

        Velocity = direction * Config.MoveSpeed;
        MoveAndSlide();

        if (direction.Length() > 0)
            _animatedSprite.Play("caminando");
        else
            _animatedSprite.Play("idle");

        if (_arma != null)
        {
            _arma.LookAt(GetGlobalMousePosition());

            if (_arma.GlobalPosition.X < GetGlobalMousePosition().X)
            {
                _animatedSprite.FlipH = false;
                _arma.Position = pos;
                Voltear(_arma);
            }
            else
            {
                _animatedSprite.FlipH = true;
                _arma.Position = new Vector2(-pos.X - 8, pos.Y);
                Voltear(_arma);
            }

        }
        if (Input.IsActionJustPressed("disparo"))
        {
            if (_arma != null)
            {
                _arma.Disparar(GetGlobalMousePosition());
            }
        }
    }
    private void Voltear(Arma arma)
    {
        float rot = Mathf.Wrap(arma.RotationDegrees, 0f, 360f);
        if (rot > 90 && rot < 270)
        {
            arma.Scale = new Vector2(arma.Scale.X, -Math.Abs(arma.Scale.Y));
        }
        else
        {
            arma.Scale = new Vector2(arma.Scale.X, Math.Abs(arma.Scale.Y));
        }
    }
    public ArmaConfig RecogerArma(ArmaConfig Arma)
    {
        if (_arma != null)
        {
            ArmaConfig anterior = _arma.Config;
            _arma.Config = Arma;
            _arma.Aplicar();
            return anterior;
        }
        return null;
    }
}

    
