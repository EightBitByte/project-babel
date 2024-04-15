extends HBoxContainer


func update_stretch():
	self.size_flags_horizontal = SIZE_EXPAND_FILL
	self.size_flags_stretch_ratio = 1.0
	pass

func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update_stretch()
