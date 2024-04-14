// Define this flag for combat loading from JSON debug output.
// # define COMBAT_LOAD_DEBUG
# define COMBAT_LOG_DEBUG

using GDictionary = Godot.Collections.Dictionary;
using GArray = Godot.Collections.Array;

using System.Collections.Generic;
using Godot;

public class Combat : Node
{
	const string PLAYER_ATTACK_FILE = "playerAttacks";
	const string PLAYER_FILE = "player";
	const string ENEMY_ATTACK_FILE = "enemyAttacks";
	const string ENEMY_FILE = "enemies";
	const int DEFAULT_MAX_HP = 20;
	const int DEFAULT_SPEED = 5;


	/// <summary>
	/// Combat setup.
	/// </summary>
	public override void _Ready()
	{
		playerData = LoadCombatPlayer();
		enemyDataList = LoadEnemies(1, 0, 2);
		turnOrder = new();
		currentTurn = 0;

		#if COMBAT_LOAD_DEBUG
			GD.Print("========== Player ==========\n");
			playerData.DEBUG_PRINT();
			
			GD.Print("========== Enemies ==========\n");
			foreach(Enemy e in enemyDataList) {
				e.DEBUG_PRINT();
			}
		#endif

		#if COMBAT_LOG_DEBUG
			roundNum = 1;
		#endif

		// Instantiation of buttons lol
		GetNode<Button>((NodePath)"Target1").Connect("button_up", this, "SetPlayerTarget", new(){"1"});
		GetNode<Button>((NodePath)"Target2").Connect("button_up", this, "SetPlayerTarget", new(){"2"});
		GetNode<Button>((NodePath)"Target3").Connect("button_up", this, "SetPlayerTarget", new(){"3"});
		GetNode<Button>((NodePath)"Target4").Connect("button_up", this, "SetPlayerTarget", new(){"4"});
		GetNode<Button>((NodePath)"Chop").Connect("button_up", this, "InitiateAttack", new(){"0"});
		GetNode<Button>((NodePath)"Pommel").Connect("button_up", this, "InitiateAttack", new(){"1"});

		SelectedAttackButton = -1;

		turnOrder = PopulateOrder();

		#if COMBAT_LOG_DEBUG
		GD.Print("========== ROUND 1 ==========");
		#endif
	}


	/// <summary>
	/// Loads the CombatPlayer object from the json found in PLAYER_ATTACK_FILE and PLAYER_FILE.
	/// </summary>
	/// <returns>A Combat Player object.</returns>
	private CombatPlayer LoadCombatPlayer() {
		// Read the attack collection and player stats
		GDictionary allAttacks = Json.ReadJSON("res://data/" + PLAYER_ATTACK_FILE + ".json");
		GDictionary playerDict = Json.ReadJSON("res://data/" + PLAYER_FILE + ".json");

		Dictionary<int, Attack> attackDict = LoadKnownAttacks(allAttacks, playerDict);

		// NOTE: This may need changing in the future, since this instantiates 
		// the player at full health for some arbitrary cap.
		return new(
			DEFAULT_MAX_HP,
			DEFAULT_MAX_HP,
			DEFAULT_SPEED,
			attackDict
		);
	}


	/// <summary>
	/// Loads a single enemy's data from the json into an Enemy object. 
	/// </summary>
	/// <param name="enemyID">The ID of the enemy to instantiate.</param>
	/// <param name="allEnemyDict">The dictionary from which to read the enemy data from all of the enemies available.</param>
	/// <param name="enemyAttackDict">The dictionary from which to read the enemy attack data.</param>
	/// <returns>An Enemy object representing the enemy at enemyID in enemyDict.</returns>
	private Enemy LoadEnemyData(int enemyID, GDictionary allEnemyDict, GDictionary enemyAttackDict) {
		GDictionary enemyDict = allEnemyDict[enemyID.ToString()] as GDictionary;

		Dictionary<int, Attack> attackDict = LoadKnownAttacks(enemyAttackDict, enemyDict);
		int entityHealth = int.Parse((string)enemyDict["health"]);

		// NOTE: This may need changing in the future, since this instantiates 
		// the enemy at full health for some arbitrary cap.
		return new (
			(string)enemyDict["name"],
			entityHealth,
			entityHealth,
			int.Parse((string)enemyDict["speed"]),
			attackDict
		);
	}


	/// <summary>
	/// Loads enemy data from the json at ENEMY_FILE and ENEMY_ATTACK_FILE.
	/// </summary>
	/// <param name="enemyID">The first enemy at id ID to instantiate.</param>
	/// <param name="otherIDs">A variable amount of enemies to instantiate.</param>
	/// <returns></returns>
	private List<Enemy> LoadEnemies(int enemyID, params int[] otherIDs) {
		// Read the attack collection and player stats
		GDictionary enemyAttacks = Json.ReadJSON("res://data/" + ENEMY_ATTACK_FILE + ".json");
		GDictionary enemyData = Json.ReadJSON("res://data/" + ENEMY_FILE + ".json");

		List<Enemy> enemyList = new() { LoadEnemyData(enemyID, enemyData, enemyAttacks) };

		// Instantiate all other enemies with the IDs provided
		for (int index = 0; index < otherIDs.Length; ++index) {
			enemyList.Add(LoadEnemyData(otherIDs[index], enemyData, enemyAttacks));
		}

		return enemyList;
	}

	/// <summary>
	/// Loads the entities known attacks into a dictionary.
	/// </summary>
	/// <param name="allAttacks">A dictionary containing all possible knowable attacks by an entity.</param>
	/// <param name="entityDict">A dictionary containing the entities' data ["knownAttacks"]</param>
	/// <returns>A dictionary indexed by attack ID representing all of the attacks known by the entity.</returns>
	private Dictionary<int, Attack> LoadKnownAttacks(GDictionary allAttacks, GDictionary entityDict) {
		GArray knownAttacks = entityDict["knownAttacks"] as GArray;
		int knownLength = knownAttacks.Count;

		Dictionary<int, Attack> attackDict = new();

		// For each attack the entity knows, initialize the attack as an 
		// Attack object and add it to the entity's attackDict.
		for (int index = 0; index < knownLength; ++index) {
			int attackID = int.Parse((string)knownAttacks[index]);
			attackDict.Add(attackID, LoadAttack(allAttacks[attackID.ToString()] as GDictionary));
		}

		return attackDict;
	}


	/// <summary>
	/// Initializes an Attack object from a JSON dict.
	/// </summary>
	/// <param name="attackDict">A JSON dictionary with all necessary kv pairs.</param>
	private Attack LoadAttack(GDictionary attackDict) {
		// Initialize all without existence checking except for Icon and Favorability;
		// Icon is player-specific, and Favorability is enemy-specific.
		return new(
			(string)attackDict["name"],
			int.Parse((string)attackDict["minBaseDMG"]),
			int.Parse((string)attackDict["maxBaseDMG"]),
			double.Parse((string)attackDict["critChance"]),
			attackDict.Contains("icon") ?
				(string)attackDict["icon"] : "NULL",
			attackDict.Contains("favorability") ?
						double.Parse((string)attackDict["favorability"]) : 0.0,
			int.Parse((string)attackDict["minUpgradeDMG"]),
			int.Parse((string)attackDict["maxUpgradeDMG"]),
			(StatusEffect)int.Parse((string)attackDict["effect"])
		);
	}

	/// <summary>
	/// Populates the List with the turn order of the entities (player and foe alike) based on speed.
	/// </summary>
	/// <returns></returns>
	private List<Entity> PopulateOrder() {
		List<Entity> newOrder = new();

		foreach (Enemy e in enemyDataList) 
			newOrder.Add(e);
		newOrder.Add(playerData);
		newOrder.Sort(Entity.Compare);

		#if COMBAT_LOAD_DEBUG
		foreach (Entity e in newOrder)
			if (e is CombatPlayer p)
				GD.Print("Player");
			else if (e is Enemy en)
				GD.Print(en.Name);
		#endif

		return newOrder;
	}


	private void SetPlayerTarget(int target) {
		SelectedEnemy = target;
	}

	private void InitiateAttack(int attackButton) {
		Attack outgoing = playerData.GetAttack(attackButton);
		int damage = outgoing.GetDamage();

		# if COMBAT_LOG_DEBUG
			GD.Print("Player ", outgoing.Name, "s ", enemyDataList[SelectedEnemy].Name, " for ", damage);
		# endif
		enemyDataList[SelectedEnemy].TakeDamage(damage);
		++currentTurn;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta) {
		//TODO: Remove enemy from turn order when dead
		// If we've reached the end of the turn order, repopulate it based on speed
		if (currentTurn == turnOrder.Count) {
			# if COMBAT_LOG_DEBUG
			GD.Print("\n========== ROUND ", ++roundNum, " ==========");
			GD.Print("Player: ", playerData.Health);
			foreach (Enemy e in enemyDataList)
				GD.Print(e.Name, ": ", e.Health);
			# endif

			turnOrder = PopulateOrder();
			currentTurn = 0;
		}

		// Get first in turn order queue
		Entity attacker = turnOrder[currentTurn];

		// TODO: Check status effects of current attacker, apply 

		// If an attacker is an enemy, get enemy's attack and show/update
		if (attacker is Enemy enemy) {
			Attack incoming = enemy.GetAttack();
			int damage = incoming.GetDamage();

			// damage = ShowEnemyAttack(damage, incoming);

			# if COMBAT_LOG_DEBUG
				GD.Print(enemy.Name, " ", incoming.Name, "s Player for ", damage);
			# endif

			bool isDead = playerData.TakeDamage(damage);
			++currentTurn;
		}

		// If the attacker is the player, await player choice

	}

	private List<Entity> turnOrder;
	private int currentTurn;
	private CombatPlayer playerData;
	private List<Enemy> enemyDataList;
	private int SelectedEnemy;
	private int SelectedAttackButton;

	#if COMBAT_LOG_DEBUG
	private int roundNum;
	#endif
}
