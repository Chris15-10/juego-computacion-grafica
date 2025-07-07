using Godot;
using System;

public partial class Cofre : Area2D
{
    [Export] public ObjetoLootable[] ObjetosPosibles;

    private Label _mensaje;
    private AnimatedSprite2D _sprite;

    private bool _jugadorDentro = false;
    private bool _abierto = false;

    public override void _Ready()
    {
        _mensaje = GetNode<Label>("Mensaje");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        _mensaje.Visible = false;

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        if (_jugadorDentro && !_abierto && Input.IsActionJustPressed("x"))
        {
            Abrir();
        }
    }

    private void Abrir()
    {
        _abierto = true;
        _sprite.Play("abierto");
        _mensaje.Visible = false;
        InstanciarObjetos();
    }

    private void InstanciarObjetos()
    {
        int direccion = 1; // Comienza hacia la derecha

        for (int i = 0; i < ObjetosPosibles.Length; i++)
        {
            var obj = ObjetosPosibles[i];

            if (obj.Escena == null) continue;

            var instancia = obj.Escena.Instantiate();
            if (instancia is Item item)
            {
                // Posiciona el objeto alternando derecha e izquierda
                Vector2 offset = new Vector2(32 * direccion, 50);
                item.Position = GlobalPosition + offset;

                // Asigna los datos del arma
                item.Arma = obj.ArmaConfig;

                // Añade al árbol de nodos
                GetTree().CurrentScene.AddChild(item);

                direccion *= -1; // Alterna dirección
            }
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Personaje1)
        {
            _jugadorDentro = true;
            _mensaje.Visible = true;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Personaje1)
        {
            _jugadorDentro = false;
            _mensaje.Visible = false;
        }
    }
}
