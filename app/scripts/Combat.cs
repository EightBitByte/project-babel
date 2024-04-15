// Define this flag for combat loading from JSON debug output.
// # define COMBAT_LOAD_DEBUG
//# define COMBAT_LOG_DEBUG

using GDictionary = Godot.Collections.Dictionary;
using GArray = Godot.Collections.Array;

using System.Collections.Generic;
using Godot;

// I am the storm that is approaching...
public class Combat : Node
{
	const string PLAYER_ATTACK_FILE = "playerAttacks";
	const string PLAYER_FILE = "player";
	const string ENEMY_ATTACK_FILE = "enemyAttacks";
	const string ENEMY_FILE = "enemies";
	const int DEFAULT_MAX_HP = 20;
	const int DEFAULT_SPEED = 5;
	const int MAX_NUM_ENEMIES = 3;
	const int BASE_ATTACK_X = -42;
	const int BASE_ATTACK_Y = 69;


	/// <summary>
	/// Combat setup.
	/// </summary>
	public override void _Ready()
	{
		// Instantiate buttons.
		Button target1 = GetNode<Button>((NodePath)"Target1");
		target1.Connect("button_up", this, "InitiateAttack", new(){"0"});
		target1.Connect("mouse_entered", this, "ShowEnemyText", new(){"0"});
		target1.Connect("mouse_exited", this, "HideEnemyText");

		Button target2 = GetNode<Button>((NodePath)"Target2");
		target2.Connect("button_up", this, "InitiateAttack", new(){"1"});
		target2.Connect("mouse_entered", this, "ShowEnemyText", new(){"1"});
		target2.Connect("mouse_exited", this, "HideEnemyText");

		Button target3 = GetNode<Button>((NodePath)"Target3");
		target3.Connect("button_up", this, "InitiateAttack", new(){"2"});
		target3.Connect("mouse_entered", this, "ShowEnemyText", new(){"2"});
		target3.Connect("mouse_exited", this, "HideEnemyText");

		AttackButton1 = GetNode<Button>((NodePath)"Attack1");
		AttackButton1.Connect("button_up", this, "SelectAttack", new(){"0"});
		AttackButton1.Connect("mouse_entered", this, "ShowAttackText", new(){"0"});
		AttackButton1.Connect("mouse_exited", this, "HideAttackText");

		AttackButton2 = GetNode<Button>((NodePath)"Attack2");
		AttackButton2.Connect("button_up", this, "SelectAttack", new(){"1"});
		AttackButton2.Connect("mouse_entered", this, "ShowAttackText", new(){"1"});
		AttackButton2.Connect("mouse_exited", this, "HideAttackText");

		AttackButton3 = GetNode<Button>((NodePath)"Attack3");
		AttackButton3.Connect("button_up", this, "SelectAttack", new(){"2"});
		AttackButton3.Connect("mouse_entered", this, "ShowAttackText", new(){"2"});
		AttackButton3.Connect("mouse_exited", this, "HideAttackText");

		Control playerArea = GetNode<Control>((NodePath)"PlayerArea");
		playerArea.Connect("mouse_entered", this, "ShowPlayerText");
		playerArea.Connect("mouse_exited", this, "HidePlayerText");


		Highlight = GetNode<Sprite>((NodePath)"Highlight");
		Highlight.Visible = false;

		// Instantiate member variables for the scene nodes.
		playerScene = GetNode<PlayerScene>("PlayerScene");
		enemySceneArray = new EnemyScene[MAX_NUM_ENEMIES];
		for (int i = 1; i <= MAX_NUM_ENEMIES; ++i)
			enemySceneArray[i-1] = GetNode<EnemyScene>($"Enemy{i}Scene");


		playerData = LoadCombatPlayer();
		enemyDataList = LoadEnemies(3);//, 0, 2);
		
		IsDead = new bool[4];
		for (int i = 0; i < 4; ++i) {
			IsDead[i] = false;
		}
		
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


		// Instantiate member variables for health bars.
		healthBars = new TextureProgress[MAX_NUM_ENEMIES+1];
		healthBars[0] = GetNode<TextureProgress>("PlayerHealthBar");
		healthBars[0].Value = 64;
		
		GetNode<TextureProgress>("./EnemyHealthBar1").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar2").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar3").Visible = false;
		
		for (int i = 1; i <= MAX_NUM_ENEMIES; ++i) {
			healthBars[i] = GetNode<TextureProgress>($"EnemyHealthBar{i}");
			healthBars[i].Value = 64;
			if (i < enemyCount) {
				healthBars[i].Visible = true;
			}
		}

		// Instantiate member variables for the labels.
		AttackNameLabel = GetNode<RichTextLabel>("AttackName");
		AttackDescriptionLabel = GetNode<RichTextLabel>("AttackDesc");

		EnemyNameLabel = GetNode<Label>("EnemyName");
		EnemyDescriptionLabel = GetNode<Label>("EnemyDesc");

		AttackNameLabel.Text = "";
		AttackDescriptionLabel.Text = "";
		EnemyNameLabel.Text = "";
		EnemyDescriptionLabel.Text = "";
		EnemyNameLabel.Align = Label.AlignEnum.Right;
		EnemyDescriptionLabel.Align = Label.AlignEnum.Right;

		SelectedAttackButton = -1;

		turnOrder = PopulateOrder();

		#if COMBAT_LOG_DEBUG
		GD.Print("========== ROUND 1 ==========");
		#endif
	}
	
	public void Reset()
	{
		// Instantiate member variables for the scene nodes.
		playerScene = GetNode<PlayerScene>("PlayerScene");
		enemySceneArray = new EnemyScene[MAX_NUM_ENEMIES];
		for (int i = 1; i <= MAX_NUM_ENEMIES; ++i)
			enemySceneArray[i-1] = GetNode<EnemyScene>($"Enemy{i}Scene");


		playerData = LoadCombatPlayer();
		enemyDataList = LoadEnemies(3);//, 0, 2);
		
		IsDead = new bool[4];
		for (int i = 0; i < 4; ++i) {
			IsDead[i] = false;
		}
		
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


		// Instantiate member variables for health bars.
		GetNode<TextureProgress>("./EnemyHealthBar1").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar2").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar3").Visible = false;
		healthBars[0].Value = 64;
		for (int i = 1; i <= MAX_NUM_ENEMIES; ++i) {
			healthBars[i].Value = 64;
			if (i < enemyCount) {
				healthBars[i].Visible = true;
			}
		}


		AttackNameLabel.Text = "";
		AttackDescriptionLabel.Text = "";
		EnemyNameLabel.Text = "";
		EnemyDescriptionLabel.Text = "";
		EnemyNameLabel.Align = Label.AlignEnum.Right;
		EnemyDescriptionLabel.Align = Label.AlignEnum.Right;

		SelectedAttackButton = -1;

		turnOrder = PopulateOrder();


		combatOver = false;
		playerWin = false;
		sequenceOver = false;
		#if COMBAT_LOG_DEBUG
		GD.Print("========== ROUND 1 ==========");
		#endif
	}
	
	public void Hide() {
		GetNode<TextureProgress>("./PlayerHealthBar").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar1").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar2").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar3").Visible = false;
		
		GetNode<Button>("./Target1").Visible = false;
		GetNode<Button>("./Target2").Visible = false;
		GetNode<Button>("./Target3").Visible = false;

		GetNode<Button>("./Attack1").Visible = false;
		GetNode<Button>("./Attack2").Visible = false;
		GetNode<Button>("./Attack3").Visible = false;
		
		GetNode<RichTextLabel>("./AttackName").Visible = false;
		GetNode<RichTextLabel>("./AttackDesc").Visible = false;
		
		GetNode<Label>("./EnemyName").Visible = false;
		GetNode<Label>("./EnemyDesc").Visible = false;
		
		GetNode<Sprite>("./Highlight").Visible = false;
		
		GetNode<PlayerScene>("./PlayerScene").Visible = false;
		GetNode<EnemyScene>("./Enemy1Scene").Visible = false;
		GetNode<EnemyScene>("./Enemy2Scene").Visible = false;
		GetNode<EnemyScene>("./Enemy3Scene").Visible = false;
	}
	public void Show() {
		GetNode<TextureProgress>("./PlayerHealthBar").Visible = true;

		GetNode<TextureProgress>("./EnemyHealthBar1").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar2").Visible = false;
		GetNode<TextureProgress>("./EnemyHealthBar3").Visible = false;
		for (int i = 1; i <= MAX_NUM_ENEMIES; ++i) {
			if (i < enemyCount + 1) {
				GetNode<TextureProgress>($"EnemyHealthBar{i}").Visible = true;
			}
		}
		
		GetNode<Button>("./Target1").Visible = true;
		GetNode<Button>("./Target2").Visible = true;
		GetNode<Button>("./Target3").Visible = true;

		GetNode<Button>("./Attack1").Visible = true;
		GetNode<Button>("./Attack2").Visible = true;
		GetNode<Button>("./Attack3").Visible = true;
		
		GetNode<RichTextLabel>("./AttackName").Visible = true;
		GetNode<RichTextLabel>("./AttackDesc").Visible = true;
		
		GetNode<Label>("./EnemyName").Visible = true;
		GetNode<Label>("./EnemyDesc").Visible = true;
		
		//GetNode<Sprite>("./Highlight").Visible = true;
		
		GetNode<PlayerScene>("./PlayerScene").Visible = true;
		GetNode<EnemyScene>("./Enemy1Scene").Visible = true;
		GetNode<EnemyScene>("./Enemy2Scene").Visible = true;
		GetNode<EnemyScene>("./Enemy3Scene").Visible = true;
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

		int index = 0;
		foreach (var pair in attackDict) {
			switch(index) {
				case 0:
					AttackButton1.Icon = ResourceLoader.Load(pair.Value.Icon) as Texture;
					break;
				case 1:
					AttackButton2.Icon = ResourceLoader.Load(pair.Value.Icon) as Texture;
					break;
				case 2:
					AttackButton3.Icon = ResourceLoader.Load(pair.Value.Icon) as Texture;
					break;
			}
			++index;
		}

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
	private Enemy LoadEnemyData(int enemyID, GDictionary allEnemyDict, GDictionary enemyAttackDict, int index) {
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
			attackDict,
			index
		);
	}


	/// <summary>
	/// Loads enemy data from the json at ENEMY_FILE and ENEMY_ATTACK_FILE.
	/// </summary>
	/// <param name="enemyID">The first enemy at id ID to instantiate.</param>
	/// <param name="otherIDs">A variable amount of enemies to instantiate.</param>
	/// <returns></returns>
	private List<Enemy> LoadEnemies(int enemyID, params int[] otherIDs) {
		enemyCount = 1 + otherIDs.Length;
		enemiesLeft = enemyCount;
		// Read the attack collection and player stats
		GDictionary enemyAttacks = Json.ReadJSON("res://data/" + ENEMY_ATTACK_FILE + ".json");
		GDictionary enemyData = Json.ReadJSON("res://data/" + ENEMY_FILE + ".json");

		List<Enemy> enemyList = new() { LoadEnemyData(enemyID, enemyData, enemyAttacks, 0) };
		enemySceneArray[0].SetAnim(enemyID);

		// Instantiate all other enemies with the IDs provided
		for (int index = 0; index < otherIDs.Length; ++index) {
			enemyList.Add(LoadEnemyData(otherIDs[index], enemyData, enemyAttacks, index + 1));
			
			enemySceneArray[index].SetAnim(otherIDs[index]);
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


	private void SelectAttack(int attackID) {
		SelectedAttackButton = SelectedAttackButton == attackID ? -1 : attackID;

		if (SelectedAttackButton != -1) {
			SetAttackText(SelectedAttackButton);
			ShowHighlight(SelectedAttackButton);
		} else {
			HideHighlight();
		}
	}


	private void SetPlayerTarget(int target) {
		SelectedEnemy = target;

		# if COMBAT_LOG_DEBUG
		GD.Print("Player selected enemy ", enemyDataList[SelectedEnemy].Name);
		# endif
	}

	private void InitiateAttack(int enemyIndex) {
		// Check if it is currently the player's turn. Otherwise, ignore the signal.
		if (!isPlayerTurn) return;
		isPlayerTurn = false;

		// If the player is attacking a dead enemy: don't.
		if (IsDead[enemyIndex]) return;

		// Also check to make sure that the player has selected a skill to attack with. Otherwise, ignore the signal.
		if (SelectedAttackButton == -1) return;

		Attack outgoing = playerData.GetAttack(SelectedAttackButton);
		int damage = outgoing.GetDamage();

		# if COMBAT_LOG_DEBUG
			GD.Print("Player ", outgoing.Name, "s ", enemyDataList[enemyIndex].Name, " for ", damage);
		# endif
		
		// Play the player attack animation.
		playerScene.AttackAnimation();
		
		bool dead = enemyDataList[enemyIndex].TakeDamage(damage);
		ShowEnemyText(enemyIndex);

		if (dead) {
			IsDead[enemyIndex] = true;
			// NOTE: We may want to visually move the enemy behind the killed one up to make it visually more aesthetic.
			// Tombstone :D
			enemySceneArray[enemyIndex].Kill();
			//enemySceneArray[enemyIndex].Visible = false;
			--enemiesLeft;
			healthBars[enemyIndex+1].Visible = false;
			
			if (enemiesLeft == 0) {
				// Win!
				combatOver = true;
				playerWin = true;
				timer = 0.0f;

				//GetNode<CombatManager>("/root/movement/CombatManager").WinCombat();
			}
		}
		
		
		
		healthBars[enemyIndex+1].Value = enemyDataList[enemyIndex].GetFractionalHealth();
		++currentTurn;
	}

	private void ShowHighlight(int attackIndex) {
		Highlight.Visible = true;
		Highlight.Position = new(BASE_ATTACK_X+(48*attackIndex), BASE_ATTACK_Y);
	}

	private void HideHighlight() {
		Highlight.Visible = false;
	}


	private void ShowPlayerText() {
		if (SelectedAttackButton == -1) {
			AttackNameLabel.Text = "You";
			AttackDescriptionLabel.Text = $"{playerData.Health}/{playerData.MaxHealth} HP";
		}
	}


	private void HidePlayerText() {
		if (SelectedAttackButton == -1) {
			AttackNameLabel.Text = "";
			AttackDescriptionLabel.Text = "";
		}
	}


	private void ShowAttackText(int attackButton) {
		if (SelectedAttackButton == -1) 
			SetAttackText(attackButton);
	}

	
	private void SetAttackText(int attackButton) {
			Attack hoveredAttack = playerData.GetAttack(attackButton);

			AttackNameLabel.BbcodeText = $"[b]{hoveredAttack.Name}[/b]";
			AttackDescriptionLabel.Text = $"Deals {hoveredAttack.MinDamage} - {hoveredAttack.MaxDamage} Damage\nCrit Chance: {hoveredAttack.CritChance * 100}.0%";
	}



	private void HideAttackText() {
		if (SelectedAttackButton == -1) {
			AttackNameLabel.Text = "";
			AttackDescriptionLabel.Text = "";
		}
	}


	private void ShowEnemyText(int enemyIndex) {
		if (enemyIndex >= enemyCount) { return; }

		Enemy currentEnemy = enemyDataList[enemyIndex];

		EnemyNameLabel.Text = currentEnemy.Name;
		EnemyDescriptionLabel.Text = $"{currentEnemy.Health}/{currentEnemy.MaxHealth} HP";
	}

	
	private void HideEnemyText() {
		EnemyNameLabel.Text = "";
		EnemyDescriptionLabel.Text = "";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta) {
		if (sequenceOver) { return; }
		//If we've reached the end of the turn order, repopulate it based on speed
		if (currentTurn == turnOrder.Count) {
			# if COMBAT_LOG_DEBUG
			// Debug printing
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

		// If the enemy is dead, skip their turn without waiting to process.
		if (attacker is Enemy ene && IsDead[ene.Position])
			++currentTurn;

		// Process every ~1.5 seconds
		if (timer < 1.5F) {
			timer += delta;
			return;
		}
		timer = 0.0F;
		
		if (combatOver) {
			sequenceOver = true;
			if (playerWin) {
				GetNode<CombatManager>("/root/movement/CombatManager").WinCombat();
			}
			else {
				GetNode<CombatManager>("/root/movement/CombatManager").LoseCombat();
			}
		}

		// TODO: Check status effects of current attacker, apply 

		// If an attacker is an enemy, get enemy's attack and show/update
		if (attacker is Enemy enemy) {
			// Block player action.
			isPlayerTurn = false;
			
			Attack incoming = enemy.GetAttack();
			int damage = incoming.GetDamage();

			// Play the enemy attack animation.
			enemySceneArray[enemy.Position].AttackAnimation();

			// damage = ShowEnemyAttack(damage, incoming);

			# if COMBAT_LOG_DEBUG
				GD.Print(enemy.Name, " ", incoming.Name, "s Player for ", damage);
			# endif

			bool isDead = playerData.TakeDamage(damage);
			if (isDead) {
				combatOver = true;
				playerWin = false;
				timer = 0.0f;
				//GetNode<CombatManager>("/root/movement/CombatManager").LoseCombat();
			}
			
			healthBars[0].Value = playerData.GetFractionalHealth();

			++currentTurn;
			return;
		}

		// If the attacker is the player, await player choice; set the flag to player turn.
		isPlayerTurn = true;
	}

	// Timer variable for processing in intervals.
	float timer = 0.0F;
	// Flag variable for whether it is the player's turn.
	bool isPlayerTurn = false;
	bool combatOver = false;
	bool playerWin = false;
	bool sequenceOver = false;

	private int enemiesLeft;
	private int enemyCount;

	private List<Entity> turnOrder;
	public bool[] IsDead;
	private int currentTurn;
	private CombatPlayer playerData;
	private List<Enemy> enemyDataList;
	private int SelectedEnemy;
	private int SelectedAttackButton;

	private Button AttackButton1, AttackButton2, AttackButton3;

	// Scene node resources
	private PlayerScene playerScene;
	private EnemyScene[] enemySceneArray;
	private RichTextLabel AttackNameLabel, AttackDescriptionLabel;
	private Label EnemyNameLabel, EnemyDescriptionLabel;
	private Sprite Highlight;

	// Scene health bar resources
	private TextureProgress[] healthBars;

	#if COMBAT_LOG_DEBUG
	private int roundNum;
	#endif
}
