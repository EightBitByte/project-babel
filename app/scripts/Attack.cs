// Attack.cs
//
// Represents the data for an attack.
using Godot;

enum StatusEffect {
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
                  string icon, int minUpgradeDamage, int maxUpgradeDamage, StatusEffect effect) {
        Name = name;
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        CritChance = critChance;
        Icon = icon;
        MinUpgradeDamage = minUpgradeDamage;
        MaxUpgradeDamage = maxUpgradeDamage;
        Effect = effect;
    }
    

    /// <summary>
    /// Initializes an Attack object from a JSON dict.
    /// </summary>
    /// <param name="attackDict">A JSON dictionary with all necessary kv pairs.</param>
    public Attack(Godot.Collections.Dictionary attackDict) {
        Name = (string)attackDict["name"];
        MinDamage = int.Parse((string)attackDict["minBaseDMG"]);
        MaxDamage = int.Parse((string)attackDict["maxBaseDMG"]);
        CritChance = double.Parse((string)attackDict["critChance"]);
        Icon = (string)attackDict["icon"];
        MinUpgradeDamage = int.Parse((string)attackDict["minUpgradeDMG"]);
        MaxUpgradeDamage = int.Parse((string)attackDict["maxUpgradeDMG"]);
        Effect = (StatusEffect)int.Parse((string)attackDict["effect"]);
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
        GD.Print("Name: ", Name, "\nMinDamage:", MinDamage, "\nCritChance: ", 
                 CritChance, "\nLevel: ", Level, "\nIcon: ", Icon, 
                 "\nMinUpgradeDamage: ", MinUpgradeDamage, "\nMaxUpgradeDamage: ", 
                 MaxUpgradeDamage);
    }


    public string Name { get; }
    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }
    public double CritChance { get; }
    public int Level { get; private set; }
    public string Icon { get; }
    private readonly int MinUpgradeDamage;
    private readonly int MaxUpgradeDamage;
    private readonly StatusEffect Effect;
} 