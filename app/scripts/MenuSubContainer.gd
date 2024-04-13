extends HBoxContainer

const left_stretch = 4
const right_stretch = 1
const total_stretch = left_stretch + right_stretch

func update_stretch():
	var unit_x = self.rect_size.x / total_stretch
	$LeftSide.rect_size.x = left_stretch*unit_x
	$RightSide.rect_size.x = right_stretch*unit_x 
	
	$LeftSide.rect_size.y = self.rect_size.y
	$RightSide.rect_size.y = self.rect_size.y

func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update_stretch()
