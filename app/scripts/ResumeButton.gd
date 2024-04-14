extends "res://scripts/MenuButton.gd"

func _on_pressed():
	get_tree().change_scene("res://scenes/movement.tscn")


# Called when the node enters the scene tree for the first time.
func _ready():
	self.text = "resume"
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
