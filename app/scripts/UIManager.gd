extends Node2D

func toggle_inventory():
	get_node("/root/Node2D/Camera2D/BigMenuBorder").toggle()
	get_node("/root/Node2D/Camera2D/InventoryBackground").toggle()

func show_inventory():
	get_node("/root/Node2D/Camera2D/BigMenuBorder").show()
	get_node("/root/Node2D/Camera2D/InventoryBackground").show()

func hide_inventory():
	get_node("/root/Node2D/Camera2D/BigMenuBorder").hide()
	get_node("/root/Node2D/Camera2D/InventoryBackground").hide()


func _input(event):
	if event is InputEventKey and event.pressed:
		if event.scancode == KEY_E:
			toggle_inventory()
			
# Called when the node enters the scene tree for the first time.
func _ready():
	hide_inventory()
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
