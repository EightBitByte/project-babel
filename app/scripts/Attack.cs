// Attack.cs
//
// Represents the data for an attack.
class Attack {
    public Attack(string name, int minDamage, int maxDamage, double critChance, 
                  string icon, int minUpgradeDamage, int maxUpgradeDamage) {
        Name = name;
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        CritChance = critChance;
        Icon = icon;
        MinUpgradeDamage = minUpgradeDamage;
        MaxUpgradeDamage = maxUpgradeDamage;
    }

    void Upgrade() {
        MinDamage += MinUpgradeDamage;
        MaxDamage += MaxUpgradeDamage;
        ++Level;
    }

    public string Name { get; }
    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }
    public double CritChance { get; }
    public int Level { get; private set; }
    public string Icon { get; }
    private readonly int MinUpgradeDamage;
    private readonly int MaxUpgradeDamage;
} 