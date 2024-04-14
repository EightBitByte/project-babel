extends Node2D

func _input(event):
	if event is InputEventKey and event.pressed:
		if event.scancode == KEY_E:
			get_node("/root/Node2D/Camera2D/BigMenuBorder").toggle()

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
