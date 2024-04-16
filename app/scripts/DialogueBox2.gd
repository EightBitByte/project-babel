extends MarginContainer


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	var margin_value = 10
	add_constant_override("margin_top", margin_value)
	add_constant_override("margin_left", margin_value)
	add_constant_override("margin_bottom", margin_value)
	add_constant_override("margin_right", margin_value)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
