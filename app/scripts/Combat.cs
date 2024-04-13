// Define this flag for combat loading from JSON debug output.
//# define COMBAT_LOAD_DEBUG
# define COMBAT_LOG_DEBUG

using System;
using System.Collections.Generic;
using Godot;

public class Combat : Node
{
    const string PLAYER_ATTACK_FILE = "playerAttacks";
    const string PLAYER_FILE = "player";
    const int DEFAULT_MAX_HP = 20;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CombatPlayer cp = LoadCombatPlayer();

        #if COMBAT_LOAD_DEBUG
        cp.DEBUG_PRINT();
        #endif
    }

    private CombatPlayer LoadCombatPlayer() {
        // Read the attack collection and player stats
        Godot.Collections.Dictionary allAttacks = Json.ReadJSON("res://data/" + PLAYER_ATTACK_FILE + ".json");
        Godot.Collections.Dictionary playerDict = Json.ReadJSON("res://data/" + PLAYER_FILE + ".json");

        Godot.Collections.Array knownAttacks = playerDict["knownAttacks"] as Godot.Collections.Array;
        int knownLength = knownAttacks.Count;

        Dictionary<int, Attack> attackDict = new();
        for (int index = 0; index < knownLength; ++index) {
            int attackID = int.Parse((string)knownAttacks[index]);

            Attack toAdd = new(allAttacks[attackID.ToString()] as Godot.Collections.Dictionary);
            attackDict.Add(attackID, toAdd);
        }

        return new(
            DEFAULT_MAX_HP,
            DEFAULT_MAX_HP,
            attackDict
        );
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private Node playerNode;
    private List<Node> enemyNode;
}
