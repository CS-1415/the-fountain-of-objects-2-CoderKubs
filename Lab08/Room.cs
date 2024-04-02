namespace Lab08;

public class Room {
    public string Description {get; set;}
    public RoomType Type {get;}
    public List<Monster>? Monsters {get; set;}
    public Room(string description, RoomType type, List<Monster> monsters){
        Description = description;
        Type = type;
        Monsters = monsters;
    }
}