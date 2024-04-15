extends KinematicBody2D

export var enemyData: Resource

var m_speed = 400
var halfWidth: float
var halfHeight: float
	
var currentMotion: String
var followingPlayer = false
var player: Node2D
var seenTimer: float = 0
	
var directionToVector
var isDirOpen
	
var random
	
# Called when the node enters the scene tree for the first time.
func _ready():
	# Get Collision size
	var rectShape: RectangleShape2D = get_child(0).shape
	halfWidth = rectShape.extents.x / 2
	halfHeight = rectShape.extents.y / 2
	
	# Add collision layer and masks
	self.collision_layer = 1 << 2
	self.collision_mask = 1 << 1
	
	# Get player object
	player = get_node("/root/movement/Player")
	
	# Scale speed and tileWidth vars to this object's scale
	m_speed *= self.scale.x
	
	# Makes direction strings to vectors
	directionToVector = {
		"left": Vector2(-1, 0),
		"up": Vector2(0, -1),
		"right": Vector2(1, 0),
		"down": Vector2(0, 1)
	}
	
	# Stores if each direction has a wall in front of it or not
	isDirOpen = {
		"left": true,
		"up": true,
		"right": true,
		"down": true
	}
	
	currentMotion = "right"
	
	random = RandomNumberGenerator.new()
	random.randomize();

	
# Checks each direction of collisions using raycasts and
# updates the collision dictionary
func _check_collisions():
	var spaceState = get_world_2d().direct_space_state
	var topLeft = self.global_position + Vector2(-halfWidth, -halfHeight)
	var topRight = self.global_position + Vector2(halfWidth, -halfHeight)
	var botLeft = self.global_position + Vector2(-halfWidth, halfHeight)
	var botRight =  self.global_position + Vector2(halfWidth, halfHeight)
	
	var rightTRColision = spaceState.intersect_ray(topRight, topRight + Vector2(10, 0), [self], 2)
	var rightBRColision = spaceState.intersect_ray(botRight, botRight + Vector2(10, 0), [self], 2)
	var downBLColision = spaceState.intersect_ray(botLeft, botLeft + Vector2(0, 10), [self], 2)
	var downBRColision = spaceState.intersect_ray(botRight, botRight + Vector2(0, 10), [self], 2)
	var leftBLColision = spaceState.intersect_ray(botLeft, botLeft + Vector2(-10, 0), [self], 2)
	var leftTLColision = spaceState.intersect_ray(topLeft, topLeft + Vector2(-10, 0), [self], 2)
	var upTRColision = spaceState.intersect_ray(topRight, topRight + Vector2(0, -10), [self], 2)
	var upTLColision = spaceState.intersect_ray(topLeft, topLeft + Vector2(0, -10), [self], 2)
	
	isDirOpen["right"] = !rightTRColision || !rightBRColision
	isDirOpen["down"] = !downBLColision || !downBRColision
	isDirOpen["left"] = !leftBLColision || !leftTLColision
	isDirOpen["up"] = !upTRColision || !upTLColision


func _can_see_player():
	var spaceState = get_world_2d().direct_space_state;
	var rayToPlayer = spaceState.intersect_ray(self.global_position, player.global_position, [self]);
	if rayToPlayer:
		var otherCollider: Node2D = rayToPlayer["collider"];
		return otherCollider.name == "Player"

	return false;
	
# Chooses a random direction to move in from available directions
func _ChooseAvailableDirection():	
	var randomNum = random.randf() * 4
	return isDirOpen.keys()[randomNum]

	
func _IdleMovement(delta):
	if !isDirOpen[currentMotion]:
		currentMotion = _ChooseAvailableDirection()
	
	var randomNum = random.randf()
	
	if randomNum < delta * 0.5: 
		currentMotion = _ChooseAvailableDirection()
	
	self.move_and_slide(directionToVector[currentMotion] * m_speed)

	
func _FollowPlayerMovement():
	var direction = player.global_position - self.global_position
	
	if (!isDirOpen["right"] && direction.x > 0) || (!isDirOpen["left"] && direction.x < 0):
		direction = Vector2(0, direction.y)
	if (!isDirOpen["up"] && direction.y < 0) || (!isDirOpen["down"] && direction.y > 0):
		direction = Vector2(direction.x, 0)
		
	self.move_and_slide(direction.normalized() * m_speed)
	
# Called once per frame
func _physics_process(delta):
	if get_node("/root/movement/CombatManager").InCombat:
		 return
	
	if _can_see_player():
		seenTimer = 2
		followingPlayer = true
	
	_check_collisions()
	
	if followingPlayer:
		if seenTimer > 0:
			seenTimer -= delta
		else:
			followingPlayer = false
		_FollowPlayerMovement()
	else:
		if followingPlayer:
			seenTimer = 2
		
		_IdleMovement(delta)
	
	
func _on_PlayerCheck_body_entered(body):
	if body.name == "Player":
		get_node("/root/movement/CombatManager").BeginCombat()
		print("Battle Enter!")
		self.queue_free()
