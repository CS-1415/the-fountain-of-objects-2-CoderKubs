namespace Lab08;
class Amarok : Monster
{
    public Amarok(){
        Health = 20;
        Inventory = new List<Weapon>{};
        Random rand = new Random();
        Inventory.Add(new Weapon("club", rand.Next(5,10)));
        Armor = rand.Next(0,5);

        Type = MonsterType.Amarok;
    }
}