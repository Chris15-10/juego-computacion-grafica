extends Control

@onready var sonido_click = $ReproductorSonido

func _on_BotonJugar_pressed():
	sonido_click.play()
	await get_tree().create_timer(0.3).timeout
	get_tree().change_scene_to_file("res://escenas/mapa nivel 1.tscn")

func _on_BotonSalir_pressed():
	sonido_click.play()
	await get_tree().create_timer(0.3).timeout
	get_tree().quit()
