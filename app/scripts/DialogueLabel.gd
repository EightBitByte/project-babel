extends Label

const chars_per_update = 1
const margin = 20

func update():
	var dialogue_box = get_parent()
	self.rect_size.x = dialogue_box.rect_size.x - 2*margin
	self.rect_size.y = dialogue_box.rect_size.y - 2*margin
	self.rect_position.x = margin
	self.rect_position.y = margin



# Called when the node enters the scene tree for the first time.
func _ready():
	self.visible_characters = 0
	self.autowrap = true
	self.valign = VALIGN_TOP


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update()
	self.visible_characters += chars_per_update
#	pass
