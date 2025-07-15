using Godot;
using System;

public partial class Arma : Node2D
{
    [Export] public ArmaConfig Config;
    [Export] public float shake;
    private AnimatedSprite2D _sprite;
    private Marker2D _canon;

    public override void _Ready()
    {
        shake = Config.shake;
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _canon = GetNode<Marker2D>("Cañón");

        if (Config != null && _sprite != null)
        {
            _sprite.SpriteFrames = Config.Animaciones;
        }
    }

    public void Disparar(Vector2 mousePos)
    {
        if (Config == null) return;

        PackedScene bala = Config.BalaScene;
        if (bala == null) return;

        Node2D balaInst = bala.Instantiate<Node2D>();

        balaInst.Position = _canon.GlobalPosition;

        Vector2 direction = (mousePos - _canon.GlobalPosition).Normalized();

        balaInst.Rotation = direction.Angle();

        balaInst.Call("Init", direction, Config.velocidad, Config.Dano, Config.bala);
        GetTree().CurrentScene.AddChild(balaInst);

        _sprite?.Play("disparo");

    }
    public void Aplicar()
    {
        if (Config != null && _sprite != null)
        {
            _sprite.SpriteFrames = Config.Animaciones;
        }
    }
}
