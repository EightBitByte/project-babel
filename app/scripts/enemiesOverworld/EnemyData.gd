extends Resource

export var id: int
export var speed: int
export var size: int
export var spritePath: String
# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _init(p_id = 0, p_speed = 0, p_size=1, p_spritePath = ""):
	id = p_id
	speed = p_speed
	spritePath = p_spritePath
	size = p_size


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
