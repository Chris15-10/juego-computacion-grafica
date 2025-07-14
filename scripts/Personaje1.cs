using Godot;
using System;

public partial class Personaje1 : CharacterBody2D
{
    [Export] public PersonajeConfig Config;

    private AnimatedSprite2D _animatedSprite;
    [Export] private Arma _arma;
    private Vector2 pos;
    private AnimationPlayer _anim;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _anim = GetNode<AnimationPlayer>("AnimationPlayer");
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
            const float escala = 8f;
            float direccion = GetGlobalMousePosition().X - _arma.GlobalPosition.X;
            if (Mathf.Abs(direccion) > escala)
            {
                if (direccion > 0)
                {
                    _animatedSprite.FlipH = false;
                    _arma.Position = pos;
                    _arma.Scale = new Vector2(_arma.Scale.X, Math.Abs(_arma.Scale.Y));
                }
                else
                {
                    _animatedSprite.FlipH = true;
                    _arma.Position = new Vector2(-pos.X - 8, pos.Y);
                    _arma.Scale = new Vector2(_arma.Scale.X, -Math.Abs(_arma.Scale.Y));
                }
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
    public ArmaConfig RecogerArma(ArmaConfig Arma)
    {
        if (_arma != null)
        {
            ArmaConfig anterior = _arma.Config;
            _arma.Config = Arma;
            if (_animatedSprite.FlipH)
            {
                _anim.Play("cambio arma_2");
                
            }
            else
            {
                _anim.Play("cambio arma");
            }
                
            return anterior;
        }
        return null;
    }
}

    
