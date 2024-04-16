extends "res://scripts/MenuButton.gd"

func _on_pressed():
	get_node("/root/movement/UIManager").hide_kill()


# Called when the node enters the scene tree for the first time.
func _ready():
	self.text = "revive at the cost of your sanity"
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	passs
