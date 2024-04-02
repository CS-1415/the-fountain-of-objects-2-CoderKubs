using System.Linq;

namespace Lab08.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void TestRoomContains()
    {
        GameBoard gameBoard = new GameBoard('l')
        {
            _board = new List<List<Room>>() { new List<Room>() { new Room("test", RoomType.Blank, new List<Monster>() { new Maelstrom() }) } }
        };

        Assert.That(true, Is.EqualTo(gameBoard.RoomContains(MonsterType.Maelstrom,0,0)));
    }
}