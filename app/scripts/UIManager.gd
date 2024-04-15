extends Node2D

func toggle_inventory_menu():
	get_node("/root/movement/Camera2D/BigMenuBorder").toggle()
	get_node("/root/movement/Camera2D/InventoryBackground").toggle()

func show_inventory_menu():
	get_node("/root/movement/Camera2D/BigMenuBorder").show()
	get_node("/root/movement/Camera2D/InventoryBackground").show()

func hide_inventory_menu():
	get_node("/root/movement/Camera2D/BigMenuBorder").hide()
	get_node("/root/movement/Camera2D/InventoryBackground").hide()

# handle key inputs
func _input(event):
	if event is InputEventKey and event.pressed:
		if event.scancode == KEY_E:
			toggle_inventory_menu()
		if event.scancode == KEY_ESCAPE:
			get_tree().change_scene("res://scenes/pause.tscn")
			
# Called when the node enters the scene tree for the first time.
func _ready():
	hide_inventory_menu()
	get_node("/root/movement/Camera2D/DialogueBox").hide_dialogue();
	#get_node("/root/movement/Camera2D/DialogueBox").show_dialogue("i said a thing")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
