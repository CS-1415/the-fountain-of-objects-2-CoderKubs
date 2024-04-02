namespace Lab08;
public class GameBoard{
    public List<List<Room>> _board;
    public bool FountainActive {get; set;}
    public int length {get;}
    public GameBoard(char? size){
        List<(int,int)> PitNumbers = new List<(int, int)>();
        length = 4;
        Random rand = new Random();

        if(size == 's'){
            length = 4;
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
        }
        if(size == 'm'){
            length = 6;
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
        }
        if(size == 'l'){
            length = 8;
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
            PitNumbers.Add((rand.Next(0,length-1),rand.Next(0,length-1)));
        }

        FountainActive = false;
        _board = new List<List<Room>>();
        (int,int) FountainRoomNumber;
        

        //Make sure fountain room number does not = 0 and pits don't collide
        List<(int,int)> CorrectPitNumbers = new List<(int, int)>();

        //Check each item in PitNumbers for copies. If any exist, reroll and try again.
        for(int i = 0; i < PitNumbers.Count; i++){
            (int,int) number = PitNumbers[i];
            while(true){
                if(!CorrectPitNumbers.Contains(number)){
                    CorrectPitNumbers.Add(number);
                    break;
                } else{
                    number = (rand.Next(0,length-1),rand.Next(0,length-1));
                }
            }
        }

        //Make Fountain Room Number Unique
        while(true){
            FountainRoomNumber = (rand.Next(0,length-1),rand.Next(0,length-1));

            if(!CorrectPitNumbers.Contains(FountainRoomNumber) && FountainRoomNumber != (0,0)){
                break;
            }
        }

        for(int i = 0; i < length; i ++){
            _board.Add(new List<Room>());
            for(int j = 0; j < length; j++){

                if((i,j) == FountainRoomNumber){
                    _board[i].Add(new Room("You can hear the faint dripping of water. You have found the fountain!",RoomType.Fountain, new List<Monster>()));
                } else if((i,j) ==(0,0)){
                    _board[i].Add(new Room("You can see the exit of the cave.",RoomType.Exit, new List<Monster>()));
                } else if(CorrectPitNumbers[0] == (i,j)) {
                    _board[i].Add(new Room("A maelstrom! You are thrown to a random room!",RoomType.Blank, new List<Monster>(){new Maelstrom()}));

                    //Medium level amaroks
                } else if((CorrectPitNumbers[1] == (i,j)) && length == 6) {
                    _board[i].Add(new Room("An Amarok! You die.",RoomType.Blank, new List<Monster>(){new Amarok()}));

                    //Hard level amaroks
                } else if(length == 8 && ((CorrectPitNumbers[1] == (i,j)) || CorrectPitNumbers[2] == (i,j) || CorrectPitNumbers[3] == (i,j))) {
                    _board[i].Add(new Room("An Amarok! You die.",RoomType.Blank, new List<Monster>(){new Amarok()}));
                    //Easy level amaroks
                } else if(((CorrectPitNumbers[1] == (i,j)) || CorrectPitNumbers[2] == (i,j)) && length == 4) {
                    _board[i].Add(new Room("An Amarok! You die.",RoomType.Blank, new List<Monster>(){new Amarok()}));
                } else if(CorrectPitNumbers.Contains((i,j))) {
                    _board[i].Add(new Room("A Pit!",RoomType.Pit, new List<Monster>()));
                }else{
                    _board[i].Add(new Room("You do not hear anything",RoomType.Blank, new List<Monster>()));
                }
            }
        }

        UpdateDescriptions();
    }

    public void MoveMaelstrom(){
        (int x,int y) newPlace = (-1,-1);
        Random rand = new Random();
        while(true){
            newPlace = (rand.Next(0,_board.Count-1),(rand.Next(0,_board.Count-1)));
            if(_board[newPlace.y][newPlace.x].Type == RoomType.Blank){
                for (int i = 0; i < _board.Count; i++){
                    for(int j = 0; j <_board.Count; j++){
                        bool ContainsMaelstrom;
                        ContainsMaelstrom = RoomContains(MonsterType.Maelstrom, i, j);
                        if(ContainsMaelstrom){

                            List<Monster> monsters = _board[i][j].Monsters;
                            for(int k = 0; k < _board[i][j].Monsters.Count(); k++){
                                if(_board[i][j].Monsters[k].Type == MonsterType.Maelstrom){
                                    monsters.Remove(_board[i][j].Monsters[k]);
                                }
                            }
                            _board[i][j] = new Room("You do not hear anything",RoomType.Blank, monsters);
                        }
                    }
                }
                List<Monster>? newMonsters = _board[newPlace.y][newPlace.x].Monsters;
                newMonsters.Add(new Maelstrom());
                _board[newPlace.y][newPlace.x] = new Room("A maelstrom! You are thrown to a random room!",RoomType.Blank, newMonsters);
                break;
            }
        }
        UpdateDescriptions();
    }
    public void UpdateDescriptions(){
        for (int i = 0; i < length; i++){
            for(int j = 0; j < length; j++){
                if(_board[i][j].Type == RoomType.Blank){
                    _board[i][j].Description = "You do not hear anything";
                } else if(_board[i][j].Type == RoomType.Exit){
                    _board[i][j].Description = "You can see the exit of the cave";
                } else if(_board[i][j].Type == RoomType.Pit){
                    _board[i][j].Description = "A Pit!";
                } else if(_board[i][j].Type == RoomType.Fountain){
                    if(FountainActive == false){
                        _board[i][j].Description = "You can hear the faint dripping of water. You have found the fountain!";
                    } else{
                        _board[i][j].Description = "The Fountain is active!";
                    }
                    
                }

                bool pit = false;
                bool maelstrom = false;
                bool amarok = false;

                if (i + 1 < length && _board[i + 1][j].Type == RoomType.Pit) {
                    pit = true;
                } 
                if (i - 1 >= 0 && _board[i - 1][j].Type == RoomType.Pit) {
                    pit = true;
                } 
                if (j + 1 < length && _board[i][j + 1].Type == RoomType.Pit) {
                    pit = true;
                } 
                if (j - 1 >= 0 && _board[i][j - 1].Type == RoomType.Pit) {
                    pit = true;
                }
                if (i + 1 < length && RoomContains(MonsterType.Maelstrom, i + 1, j)) {
                    maelstrom = true;
                } 
                if (i - 1 >= 0 && RoomContains(MonsterType.Maelstrom, i - 1, j)) {
                    maelstrom = true;
                } 
                if (j + 1 < length && RoomContains(MonsterType.Maelstrom, i, j + 1)) {
                    maelstrom = true;
                } 
                if (j - 1 >= 0 && RoomContains(MonsterType.Maelstrom, i, j - 1)) {
                    maelstrom = true;
                }
                if (i + 1 < length && RoomContains(MonsterType.Amarok, i + 1, j)) {
                    amarok = true;
                } 
                if (i - 1 >= 0 && RoomContains(MonsterType.Amarok, i - 1, j)) {
                    amarok = true;
                } 
                if (j + 1 < length && RoomContains(MonsterType.Amarok, i, j + 1)) {
                    amarok = true;
                } 
                if (j - 1 >= 0 && RoomContains(MonsterType.Amarok, i, j - 1)) {
                    amarok = true;
                }

                if(pit){
                    _board[i][j].Description += " You can sence a pit nearby.";
                }
                if(maelstrom){
                    _board[i][j].Description += " You can hear a maelstrom nearby.";
                }
                if(amarok){
                    _board[i][j].Description += " You can smell the stench of an amarok in a nearby room.";
                }
            }
        }
    }
    public void ShootAt(int x, int y){
        //Check if it is a maelstrom
        if(RoomContains(MonsterType.Maelstrom, y, x)){
            Console.Clear();
            Console.WriteLine("You silly! Maelstroms cannot be hit by arrows.");
            Console.ReadKey();


            //Damage amaroks
        } else if(RoomContains(MonsterType.Amarok, y, x)){
            Console.Clear();
            Console.WriteLine("You hear the screams of a wounded Amarok.");
            
            //Set the list
            List<Monster> monsters = _board[y][x].Monsters;

            //Loop through and check for the Amarok.
            for(int k = 0; k < _board[y][x].Monsters.Count(); k++){
                if(_board[y][x].Monsters[k].Type == MonsterType.Amarok){

                    //Damage it
                    _board[y][x].Monsters[k].Health -= 5;

                    //Remove it if it has no more health
                    if(_board[y][x].Monsters[k].Health <= 0){
                        monsters.Remove(_board[y][x].Monsters[k]);
                        _board[y][x] = new Room("You do not hear anything",RoomType.Blank, monsters);
                        Console.WriteLine("Luckily, it has died.");
                    }
                    
                }
            }
            Console.ReadKey();
            UpdateDescriptions();
        }else{
            Console.Clear();
            Console.WriteLine("Your arrow flyes into an empty room.");
            Console.ReadKey();
        }
    }

    public bool RoomContains(MonsterType monsterToFind, int y, int x){
        foreach(Monster monster in _board[y][x].Monsters){
            if(monster.Type == monsterToFind){
                return true;
            }
        }
        return false;
    }
}