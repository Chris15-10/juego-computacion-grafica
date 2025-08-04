extends Control

@onready var reproductor = $Reproductor

func _ready():
	reproductor.play()

func _process(delta):
	if Input.is_action_just_pressed("ui_accept") or Input.is_action_just_pressed("ui_select"):
		get_tree().change_scene_to_file("res://escenas/InterfazInicio.tscn")

func _on_Reproductor_finished():
	get_tree().change_scene_to_file("res://escenas/InterfazInicio.tscn")


func _on_reproductor_finished() -> void:
	pass # Replace with function body.
