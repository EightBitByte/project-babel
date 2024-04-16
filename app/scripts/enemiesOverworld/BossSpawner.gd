extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

export var prefabPath: String
export var resourcePaths: Array

var enemyPrefab
var bossData: Array
var random

var instance

# Called when the node enters the scene tree for the first time.
func _ready():
	bossData = []
	random = RandomNumberGenerator.new()
	random.randomize()
	enemyPrefab = load(prefabPath)
	for path in resourcePaths:
		bossData.append(load(path))
	spawn_enemy()
	get_node("../ExitStairs").hide()
	get_node("../ExitStairs").get_child(0).disabled = true

func spawn_enemy():
	var randomBoss = bossData[randi() % resourcePaths.size()]
	instance = enemyPrefab.instance()
	instance.enemyData = randomBoss
	get_node("/root/movement/Enemies").add_child(instance)
	instance.position = self.global_position

func _process(delta):
	if !instance:
		get_node("../ExitStairs").show()
		get_node("../ExitStairs").get_child(0).disabled = false
