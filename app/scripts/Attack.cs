// Attack.cs
//
// Represents the data for an attack.
using System;
using Godot;

enum StatusEffect {
    None = -1,
    Stun,
    Weaken,
    Slow,
    Blind
}

class Attack {
    /// <summary>
    /// Default initializer.
    /// </summary>
    public Attack(string name, int minDamage, int maxDamage, double critChance, 
                  string icon, double favorability,int minUpgradeDamage, 
                  int maxUpgradeDamage, StatusEffect effect) {
        Name = name;
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        CritChance = critChance;
        Icon = icon;
        Favorability = favorability;
        MinUpgradeDamage = minUpgradeDamage;
        MaxUpgradeDamage = maxUpgradeDamage;
        Effect = effect;
    }
    

    /// <summary>
    /// Upgrades the minimum and maximum damage by the modifiers, and 
    /// increases level.
    /// </summary>
    public void Upgrade() {
        MinDamage += MinUpgradeDamage;
        MaxDamage += MaxUpgradeDamage;
        ++Level;
    }


    /// <summary>
    /// Prints the member variables of the Attack object. 
    /// For debug purposes only.
    /// </summary>
    public void DEBUG_PRINT() {
        GD.Print("\tName: ", Name, 
                 "\n\tMinDamage:", MinDamage, 
                 "\n\tCritChance: ", CritChance, 
                 "\n\tLevel: ", Level, 
                 "\n\tIcon: ", Icon, 
                 "\n\tFavorability: ", Favorability,
                 "\n\tMinUpgradeDamage: ", MinUpgradeDamage, 
                 "\n\tMaxUpgradeDamage: ", MaxUpgradeDamage);
    }

    /// <summary>
    /// Gets the damage done for this attack.
    /// </summary>
    /// <returns>The amount of damage that this will do.</returns>
    public int GetDamage() {
        Random rand = new();

        bool isCrit = rand.NextDouble() <= CritChance;
        int damage = rand.Next(MinDamage, MaxDamage);

        // If crit, double damage
        return isCrit ? damage * 2 : damage;
    }


    public string Name { get; }
    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }
    public double CritChance { get; }
    public int Level { get; private set; }
    public string Icon { get; }
    // For use only with enemies - determines which attack they choose
    public double Favorability { get; } 
    private readonly int MinUpgradeDamage;
    private readonly int MaxUpgradeDamage;
    private readonly StatusEffect Effect;
} 