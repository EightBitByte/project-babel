extends VBoxContainer

func center_self():
	self.rect_position.x = ( - self.rect_size.x)/2
	self.rect_position.y = ( - self.rect_size.y)/2


func toggle():
	if is_visible():
		hide()
	else:
		show()


# Called when the node enters the scene tree for the first time.
func _ready():
	self.alignment = ALIGN_CENTER
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	center_self()
