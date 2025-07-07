using Godot;
using System;

public partial class Vida : Node
{
    [Export] public int salud_max = 100;
    public int salud_act;

    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        salud_act = salud_max;

        var padre = GetParent();
        _sprite = padre.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

        if (_sprite != null)
        {
            _sprite.AnimationFinished += OnAnimacionTerminada;
        }
    }

    public void RecibirDano(int cant)
    {
        salud_act -= cant;
        if (salud_act <= 0)
            Morir();
    }

    public void Morir()
    {
        GD.Print("☠️ Personaje ha muerto.");

        var padre = GetParent();

        // si es enemigo reproduce animación
        if (_sprite != null && padre.IsInGroup("enemigo"))
        {
            if (padre is Enemigo enemigo)
                enemigo.SetMuerto(true);

            _sprite.Play("muerte");
        }
        else
        {

            padre.QueueFree();
        }
    }

    private void OnAnimacionTerminada()
    {
        if (_sprite.Animation != "muerte") return;

        Node padre = GetParent();

        bool esJugador = false;

        foreach (Node nodo in padre.GetChildren())
        {
            if (nodo.IsInGroup("jugador"))
            {
                esJugador = true;
                break;
            }
        }

        if (esJugador)
        {
            padre.QueueFree();
        }
        else if (padre.IsInGroup("enemigo"))
        {
            // Desactiva colisiones del enemigo muerto
            var colision = padre.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            if (colision != null)
            {
                colision.Disabled = true;
            }
        }
    }
}
