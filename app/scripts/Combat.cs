using System;
using System.Collections.Generic;
using Godot;


public class Combat : Node
{
    const int DEFAULT_MAX_HP = 20;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private CombatPlayer LoadCombatPlayer() {
        // Read the attack collection and player stats
        Godot.Collections.Dictionary allAttacks = Json.ReadJSON("res://data/playerAttacks");
        Godot.Collections.Dictionary playerDict = Json.ReadJSON("res://player.json");

        Godot.Collections.Array knownAttacks = playerDict["knownAttacks"] as Godot.Collections.Array;
        int knownLength = knownAttacks.Count;

        Dictionary<int, Attack> attackDict = new();
        for (int index = 0; index < knownLength; ++index) {
            int attackID = (int)knownAttacks[index];

            attackDict.Add(attackID, JSONToAttack(allAttacks[attackID] as Godot.Collections.Dictionary));
        }

        return new(
            DEFAULT_MAX_HP,
            DEFAULT_MAX_HP,
            attackDict
        );
    }

    private Attack JSONToAttack(Godot.Collections.Dictionary attackDict) {
        return new(
            (string)attackDict["name"],
            int.Parse((string)attackDict["minBaseDMG"]),
            int.Parse((string)attackDict["maxBaseDMG"]),
            double.Parse((string)attackDict["critChance"]),
            (string)attackDict["icon"],
            int.Parse((string)attackDict["minUpgradeDMG"]),
            int.Parse((string)attackDict["maxUpgadeDMG"])
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
