namespace Lab08;
public class Maelstrom : Monster
{
    public Maelstrom(){
        Health = 10;
        Inventory = new List<Weapon>{};
        Random rand = new Random();
        Inventory.Add(new Weapon("Wind Hammer", rand.Next(3,6)));
        Armor = rand.Next(5,10);

        Type = MonsterType.Maelstrom;
    }
}