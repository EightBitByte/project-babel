extends PanelContainer

# ratio of menu size to viewport size
const size_ratio_x = 0.6
const size_ratio_y = 0.6

func update():
	var viewport = get_viewport()
	
	self.rect_size.x = viewport.size.x*size_ratio_x
	self.rect_size.y = viewport.size.y*size_ratio_y
	self.rect_position.x = -self.rect_size.x/2
	self.rect_position.y = -self.rect_size.y/2
	

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update()
	pass
