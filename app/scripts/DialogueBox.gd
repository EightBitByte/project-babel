extends PanelContainer

func update():
	var viewport = get_viewport()
	self.rect_size.x = viewport.size.x*0.98
	self.rect_size.y = viewport.size.y/3
	self.rect_position.x = - self.rect_size.x/2
	self.rect_position.y = viewport.size.y/6 - viewport.size.x*0.01
	

func show_dialogue(dialogue_text):
	$DialogueLabel.visible_characters = 0
	$DialogueLabel.text = dialogue_text
	self.show()
	$DialogueLabel.show()
	print("I am printing from show dialaogue \n")
	
	
func hide_dialogue():
	self.hide()
	$DialogueLabel.hide()
	$DialogueLabel.text = ""
	print("I am printing from hide dialaogue \n")
# Called when the node enters the scene tree for the first time.
func _ready():
	
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update()
