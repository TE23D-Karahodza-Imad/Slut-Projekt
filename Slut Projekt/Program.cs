


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello and welcome to my Arena.. I the King, present you with 3 classes which you can use..");
        Console.WriteLine("Press enter to continue:");
        Console.ReadLine();


        Console.Write("Now tell me.. what is your name? - Type your name: ");
        string playerName = Console.ReadLine();

        // Här ska du välja en klass, det finns 3 val, jag har inte gjort att man kan göra fel, eller att de kommer upp när man skriver 
        //fel nummer eller ett ord, ska göra det senare.
        Console.WriteLine("\nYour name sounds interesting.. never heard of it.");
        Console.WriteLine("Press enter to continue:");
        Console.ReadLine();

        Console.WriteLine("\nChoose your class, gladiator, you have three options:");
        Console.WriteLine("1: Knight (High Defense)");
        Console.WriteLine("2: Mage (High Attack)");
        Console.WriteLine("3: Archer (Balanced)");

        int classChoice;
        bool validChoice = false;

        do
        {
            Console.Write("Enter the number of your choice (1, 2, or 3): ");
            string input = Console.ReadLine();

            // TryParse ska returna false även om det är inte en nummer.
            if (int.TryParse(input, out classChoice) && (classChoice >= 1 && classChoice <= 3))
            {
                validChoice = true;
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter a number between 1 and 3.");
            }

    } while (!validChoice);

        // Variabler för spelaren
        int playerHP = 100;
        int playerAttack = 0;
        int playerDefense = 0;
        string playerClass = "";

        // Det här är stats, för viss klass du väljer.
        if (classChoice == 1)
        {
            playerClass = "Knight";
            playerAttack = 10;
            playerDefense = 15;
        }
        else if (classChoice == 2)
        {
            playerClass = "Mage";
            playerAttack = 20;
            playerDefense = 5;
        }
        else if (classChoice == 3)
        {
            playerClass = "Archer";
            playerAttack = 15;
            playerDefense = 10;
        }
        else
        {
            Console.WriteLine("Invalid choice! Defaulting to Archer.");
            playerClass = "Archer";
            playerAttack = 15;
            playerDefense = 10;
        }

        Console.WriteLine($"\n{playerName} the {playerClass} enters the battlefield!");

        // 
        Enemy enemy = CreateRandomEnemy();

        Console.WriteLine($"\n A wild {enemy.Name} appears! ");

        // Fight Loop
        while (playerHP > 0 && enemy.HP > 0)
        {
            // Spelarens turn
            Console.WriteLine($"\n{playerName}'s Turn:");
            int damageDealt = playerAttack - enemy.Defense;
            damageDealt = Math.Max(0, damageDealt); // Ingen negativ damage kan ske

            enemy.HP -= damageDealt;
            Console.WriteLine($"You deal {damageDealt} damage to {enemy.Name}! (Enemy HP: {enemy.HP})");

            if (enemy.HP <= 0)
            {
                Console.WriteLine($"\n You defeated the {enemy.Name}! Victory!");
                break;
            }

            // Enemies turn att köra
            Console.WriteLine($"\n{enemy.Name}'s Turn:");
            int damageTaken = enemy.Attack - playerDefense;
            damageTaken = Math.Max(0, damageTaken);
            playerHP -= damageTaken;
            Console.WriteLine($"{enemy.Name} deals {damageTaken} damage to you! (Your HP: {playerHP})");

            if (playerHP <= 0)
            {
                Console.WriteLine($"\n You have been defeated by the {enemy.Name}...");
                break;
            }
        }

        Console.WriteLine("\nGame Over. Thanks for playing!");
    }

    // Metod att skapa en random enemy-
    static Enemy CreateRandomEnemy()
    {
        List<Enemy> enemies = new List<Enemy>()
            {
                new Enemy("Goblin", 50, 10, 5),
                new Enemy("Orc", 70, 15, 10),
                new Enemy("Dark Knight", 100, 20, 15),
                new Enemy("Dragon", 150, 25, 20)
            };

        Random rnd = new Random();
        int index = rnd.Next(enemies.Count);
        return enemies[index];
    }
}


    // Enemy klass
    class Enemy
    {
        public string Name;
        public int HP;
        public int Attack;
        public int Defense;

        public Enemy(string name, int hp, int attack, int defense)
        {
            Name = name;
            HP = hp;
            Attack = attack;
            Defense = defense;
        }
    }
