namespace Lab08;
public class PlayerController {
    (int y,int x) _currentPosition;
    int Health {get; set;}
    int Armor {get; set;}
    public List<Weapon>? Inventory {get; set;}
    bool _gameInProgress;
    GameBoard gameBoard;
    int length;
    int ArrowsLeft;
    public PlayerController(char? size){
        _currentPosition = (0,0);
        _gameInProgress = true;
        gameBoard = new GameBoard(size);
        ArrowsLeft = 5;
        Armor = 10;
        Health = 20;

        Inventory = new List<Weapon>();
        Inventory.Add(new Weapon("sword", 5));

        if(size == 's'){
            length = 4;
        }
        if(size == 'm'){
            length = 6;
        }
        if(size == 'l'){
            length = 8;
        }
    }
    public void PlayGame(){
        while(_gameInProgress){
            DisplayText();
            PlayerTurn();


            //Monster logic
            if(gameBoard._board[_currentPosition.y][_currentPosition.x].Monsters.Count > 0){
                List<Monster> monsters = new List<Monster>();
                monsters.AddRange(gameBoard._board[_currentPosition.y][_currentPosition.x].Monsters);
                foreach(Monster monster in monsters){
                    if(monster.Type == MonsterType.Amarok){
                        _gameInProgress = Fight(monster);
                    }
                    if(monster.Type == MonsterType.Maelstrom){
                        Random rand = new Random();
                        if(rand.NextDouble() > 0.5){
                            _gameInProgress = Fight(monster);
                        } else{ 
                            Console.WriteLine("A Maelstrom! You got thrown to a random room.");
                            Console.ReadKey();
                            _currentPosition = (rand.Next(0,length),rand.Next(0,length));
                            gameBoard.MoveMaelstrom();
                        }
                        
                    }
                }
                gameBoard.UpdateDescriptions();


                
            }

            //Pit logic
            if(gameBoard._board[_currentPosition.y][_currentPosition.x].Type == RoomType.Pit){
                Console.WriteLine("You slip into a pit and hurt your leg.");
                Health -= 5;
                Console.WriteLine($"You have \u001b[31m{Health}\u001b[0m health remaining");
                Console.ReadKey(true);

                if(Health <= 5 && Health > 0){
                    Console.WriteLine("You don't have the strngth to get out, and you slowly starve.");
                    _gameInProgress = false;
                } else if(Health <=0){
                    Console.WriteLine("Due to your low health, you die on impact.");
                    _gameInProgress = false;
                } else{
                    Console.WriteLine("Luckily, it isn't too deep and you make your way out. That could have been a lot worse.");
                }
                Console.ReadKey(true);
            }

        }
    }
    private bool Fight(Monster monster){
        Console.Clear();
        Console.WriteLine($"You have run into a(n) {monster.Type}");
        Console.WriteLine("You attack first.");

        Console.ReadKey(true);
        while(true){

            Weapon weaponOfChoice = new Weapon("fist",0);
            foreach(Weapon weapon in Inventory){
                if (weaponOfChoice.Damage < weapon.Damage){
                    weaponOfChoice = weapon;
                }
            }

            //Check armor and deal damage accordingly

            Random rand = new Random();
            if(monster.Armor > rand.Next(1,21)){
                Console.WriteLine($"You swing your {weaponOfChoice.Name}, but the {monster.Type}'s armor deflects most of the blow.");
                monster.Health -= weaponOfChoice.Damage/2;
            } else{
                monster.Health -= weaponOfChoice.Damage;
                Console.WriteLine($"You swing your {weaponOfChoice.Name} and deal {weaponOfChoice.Damage} damage!");
            }


            if(monster.Health <= 0){
                Console.WriteLine($"It falls over dead.");

                Inventory.AddRange(monster.Inventory);
                foreach(Weapon weapon in monster.Inventory){
                    Console.WriteLine($"You got a {weapon.Name} and some extra armor!");
                    Armor +=3;
                }

                gameBoard._board[_currentPosition.y][_currentPosition.x].Monsters.Remove(monster);

                Console.ReadKey();
                Console.WriteLine(Health);
                return Health > 0;
                
            } else{
                Console.WriteLine($"It now only has {monster.Health} health!");

                Weapon weaponOfChoice2 = new Weapon("fist",0);
                foreach(Weapon weapon in monster.Inventory){
                if (weaponOfChoice2.Damage < weapon.Damage){
                    weaponOfChoice2 = weapon;
                }
                Console.ReadKey(true);
            }

            if(Armor > rand.Next(1,21)){
                Console.WriteLine($"The {monster.Type} swings its {weaponOfChoice2.Name}, but it deflects off of your armor dealing minimal damage!");
                Health -= weaponOfChoice2.Damage/2;
            } else{
                Health -= weaponOfChoice2.Damage;
                Console.WriteLine($"The {monster.Type} swings its {weaponOfChoice2.Name} and deals {weaponOfChoice2.Damage} damage!");
            }

            
                if(Health <= 0){
                    Console.WriteLine("You fall over, health spent, and die.");
                    Console.ReadKey(true);
                    return false;
                }
            }

            Console.WriteLine($"You have \u001b[31m{Health}\u001b[0m health remaining");
            Console.ReadKey(true);

            
        }
    }

    private void DisplayText(){
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine($"Current Position: {_currentPosition.x},{_currentPosition.y}");
            Console.WriteLine($"Arrows left: {ArrowsLeft}");
            Console.WriteLine(gameBoard._board[_currentPosition.y][_currentPosition.x].Description);
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("What do you do?");
    }
    private void PlayerTurn(){

        char validAction = GetValidAction();
        if(validAction == 'w'){
            _currentPosition.y--;
        }
        if(validAction == 's'){
            _currentPosition.y++;
        }
        if(validAction == 'd'){
            _currentPosition.x++;
        }
        if(validAction == 'a'){
            _currentPosition.x--;
        }

        //Shoot up
        if(validAction == 'i'){
            if(ArrowsLeft > 0){
                gameBoard.ShootAt(_currentPosition.x, _currentPosition.y - 1);
                ArrowsLeft--;
            } else {
                Console.WriteLine("You are out of arrows.");
                Console.ReadKey(true);
            }
        }
        //Shoot down
        if(validAction == 'k'){
            if(ArrowsLeft > 0){
                gameBoard.ShootAt(_currentPosition.x, _currentPosition.y + 1);
                ArrowsLeft--;
            } else {
                Console.WriteLine("You are out of arrows.");
                Console.ReadKey(true);
            }
        }

        //Shoot right
        if(validAction == 'l'){
            if(ArrowsLeft > 0){
                gameBoard.ShootAt(_currentPosition.x + 1, _currentPosition.y);
                ArrowsLeft--;
            } else {
                Console.WriteLine("You are out of arrows.");
                Console.ReadKey(true);
            }
        }
        //Shoot left
        if(validAction == 'j'){
            if(ArrowsLeft > 0){
                gameBoard.ShootAt(_currentPosition.x - 1, _currentPosition.y);
                ArrowsLeft--;
            } else {
                Console.WriteLine("You are out of arrows.");
                Console.ReadKey(true);
            }
        }
        if(validAction == ' '){
            if(gameBoard._board[_currentPosition.y][_currentPosition.x].Type == RoomType.Exit){
                if(gameBoard.FountainActive == true){
                    _gameInProgress = false;
                    Console.Clear();
                    Console.WriteLine("You win!");
                }
            } else{
                gameBoard.FountainActive = true;
                gameBoard._board[_currentPosition.y][_currentPosition.x] = new Room("The Fountain is flowing!",RoomType.Fountain, new List<Monster>());
            }
        }
    
    }

    private char GetValidAction(){
        bool GoodChar = false;
        char? input = ' ';
        while(!GoodChar){
            input = Console.ReadKey(true).KeyChar;
            if(input != null){

                //Remove all non possible actions
                List<char?> ValidActions = new List<char?>() {'w', 'a', 's', 'd',' ','i', 'j', 'k', 'l',};
                if(_currentPosition.y == 0){
                    ValidActions.Remove('w');
                    ValidActions.Remove('i');
                }
                if(_currentPosition.y == length-1){
                    ValidActions.Remove('s');
                    ValidActions.Remove('k');
                }
                if(_currentPosition.x == 0){
                    ValidActions.Remove('a');
                    ValidActions.Remove('j');
                }
                if(_currentPosition.x == length-1){
                    ValidActions.Remove('d');
                    ValidActions.Remove('l');
                }
                if(gameBoard._board[_currentPosition.y][_currentPosition.x].Type != RoomType.Fountain && gameBoard._board[_currentPosition.y][_currentPosition.x].Type != RoomType.Exit){
                    ValidActions.Remove(' ');
                }
                if(ValidActions.Contains(input)){
                    GoodChar = true;
                }
            }
        }
        return (char)input!;
    }
}