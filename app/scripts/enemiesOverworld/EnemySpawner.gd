extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

export var prefabPath: String
export var resourcePath: String
export var chance: float


var enemyPrefab
var enemyData
var random

# Called when the node enters the scene tree for the first time.
func _ready():
	random = RandomNumberGenerator.new()
	random.randomize()
	enemyPrefab = load(prefabPath)
	enemyData = load(resourcePath)
	spawn_enemy()

func spawn_enemy():
	if random.randf() < chance:
		var instance = enemyPrefab.instance()
		instance.enemyData = enemyData
		get_node("/root/movement/Enemies").add_child(instance)
		instance.position = self.global_position
