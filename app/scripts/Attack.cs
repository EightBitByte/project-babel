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

    public void Upgrade() {
        MinDamage += MinUpgradeDamage;
        MaxDamage += MaxUpgradeDamage;
        ++Level;
    }

    public void DEBUG_PRINT() {
        GD.Print("Name: ", Name, "\nMinDamage:", MinDamage, "\nCritChance: ", CritChance, "\nLevel: ", Level, "\nIcon: ", Icon, "\nMinUpgradeDamage: ", MinUpgradeDamage, "\nMaxUpgradeDamage: ", MaxUpgradeDamage);
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