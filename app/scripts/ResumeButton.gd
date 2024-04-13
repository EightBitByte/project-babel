extends Button

onready var hover_font = DynamicFont.new()

func update_size():
	if is_hovered():
		hover_font.size = 100
		
	else:
		hover_font.size = 80

	self.set("custom_fonts/font", hover_font)

# Called when the node enters the scene tree for the first time.
func _ready():
	hover_font.font_data = load("res://data/pixeldroidMenuRegular.ttf")
	
	self.text = "resume"
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update_size()
	pass
