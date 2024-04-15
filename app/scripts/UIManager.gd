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

func toggle_pause():
	get_node("/root/movement/Camera2D/PauseContainer").toggle()
	get_node("/root/movement/Camera2D/PauseBackground").toggle()

func show_pause():
	get_node("/root/movement/Camera2D/PauseContainer").show()
	get_node("/root/movement/Camera2D/PauseBackground").show()
	
func hide_pause():
	get_node("/root/movement/Camera2D/PauseContainer").hide()
	get_node("/root/movement/Camera2D/PauseBackground").hide()

func show_kill():
	get_node("/root/movement/Camera2D/KillContainer").show()
	get_node("/root/movement/Camera2D/KillBackground").show()

func hide_kill():
	get_node("/root/movement/Camera2D/KillContainer").hide()
	get_node("/root/movement/Camera2D/KillBackground").hide()


# handle key inputs
func _input(event):
	if event is InputEventKey and event.pressed:
		if event.scancode == KEY_E:
			toggle_inventory_menu()
		if event.scancode == KEY_ESCAPE:
			toggle_pause()
			
		if event.scancode == KEY_K:
			show_kill()
			
# Called when the node enters the scene tree for the first time.
func _ready():
	hide_inventory_menu()
	hide_kill()
	hide_pause()
	get_node("/root/movement/Camera2D/DialogueBox").hide_dialogue();
	#get_node("/root/movement/Camera2D/DialogueBox").show_dialogue("i said a thing")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
