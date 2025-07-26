using Godot;
using System;

public partial class SpawnerEnemigos : Area2D
{
    [Export] public PackedScene[] enemigos; // spawnean tanto enemigos shooter como cuerpo a cuerpo
    [Export] public int maximo = 3;
    [Export] public float tiempo = 1.5f; // tiempo de spawn

    private Timer _timer;
    private RandomNumberGenerator _rng = new();
    private bool inside = false; // si el jugador esta dentro del àrea
    private int cont_enemigos = 0;

    public override void _Ready()
    {
        _timer = new Timer();
        AddChild(_timer);
        _timer.WaitTime = tiempo;
        _timer.Timeout += SpawnEnemigo;

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("jugador")) 
        {
            inside = true;
            _timer.Start();
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body.IsInGroup("jugador"))
        {
            inside = false;
            _timer.Stop(); // se detiene el spawner
        }
    }

    private void SpawnEnemigo()
    {
        if (!inside || GetTree().Paused) return;
        if (cont_enemigos >= maximo)
        {
            _timer.Stop();
            return;
        }

        CollisionShape2D shape = GetNode<CollisionShape2D>("CollisionShape2D");
        Rect2 rect = shape.Shape is RectangleShape2D rectangle
            ? new Rect2(GlobalPosition - rectangle.Size / 2, rectangle.Size)
            : new Rect2(GlobalPosition, Vector2.One);
        
        Vector2 random_pos = new Vector2(_rng.RandfRange(rect.Position.X, rect.End.X), _rng.RandfRange(rect.Position.Y, rect.End.Y));

        var escena_enemigo = enemigos[_rng.RandiRange(0, enemigos.Length - 1)];
        var enemigo = escena_enemigo.Instantiate<Node2D>();
        enemigo.GlobalPosition = random_pos;

        GetParent().AddChild(enemigo);
        
        cont_enemigos++;
    }
}


