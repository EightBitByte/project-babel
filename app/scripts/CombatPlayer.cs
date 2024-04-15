// CombatPlayer.cs
//
// Implements the CombatPlayer class to represent the player in combat scenes.

using System.Collections.Generic;

class CombatPlayer : Entity {
    public CombatPlayer (int health, int maxHP, int speed, Dictionary<int, Attack> aDict) 
           : base (health, maxHP, speed, aDict) {
        // Empty Constructor
    }

    public Attack GetAttack(int attackID) {
        return attackDict[attackID];
    }
}