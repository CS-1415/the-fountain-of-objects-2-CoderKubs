using Lab08;

Console.Clear();
Console.WriteLine(@"Welcome to the cave of objects. Your job is to find the Fountain of objects and escape without dieing.
Use WASD to move your character and IJKL to shoot arrows. Press spacebar to activate the fountain and to escape.");
Console.WriteLine("Do you want a (s)mall, (m)edium, or (l)arge world?");
bool GoodChar = false;
char? input = ' ';

while(!GoodChar){
    input = Console.ReadKey(true).KeyChar;
    if(input != null){
        List<char?> ValidActions = new List<char?>() {'s','m','l'};

        if(ValidActions.Contains(input)){
            GoodChar = true;
        }
    }
}


PlayerController controller = new PlayerController(input);



controller.PlayGame();