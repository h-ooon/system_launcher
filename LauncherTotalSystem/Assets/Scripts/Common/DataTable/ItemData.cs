public class ItemData
{
    public int ItemId;
    public string ItemName;
    public int AttackPower;
    public int Defense;
}

public enum ItemType
{
    Weapon = 1,
    Shield,
    ChestArmor,
    Gloves,
    Boots,
    Accessory,
}

public enum ItemGrade
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary
}