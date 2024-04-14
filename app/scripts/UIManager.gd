extends Node2D

func toggle_inventory_menu():
	get_node("/root/Node2D/Camera2D/BigMenuBorder").toggle()
	get_node("/root/Node2D/Camera2D/InventoryBackground").toggle()

func _input(event):
	if event is InputEventKey and event.pressed:
		if event.scancode == KEY_E:
			toggle_inventory_menu()
			
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
