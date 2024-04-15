extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

export var prefab: String
export var chance: float


var enemyPrefab
var random

# Called when the node enters the scene tree for the first time.
func _ready():
	random = RandomNumberGenerator.new()
	random.randomize()
	enemyPrefab = load(prefab)

func spawn_enemy():
	if random.randf() < chance:
		
	
	var instance = enemyPrefab.instance()
	add_child(instance)
