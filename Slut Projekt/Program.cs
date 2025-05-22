//-----------------------------------------------------//

Console.WriteLine("Hello and welcome to my Arena.. I the King, present you with 3 classes which you can use..");
Console.WriteLine("Press enter to continue:");
Console.ReadLine();

//-----------------------------------------------------//





//-----------------------------------------------------//
//Här koden gör så att spelaren måste skriva ett namn. Om spelaren bara trycker Enter utan att skriva något –
//då säger spelet: "You must enter a name, brave warrior!" Och frågar igen.
//Det fortsätter så, om och om igen, tills spelaren faktiskt har skrivit ett namn.

string playerName = "";
do
{
    Console.Write("Now tell me... what is your name? - Type your name: ");
    playerName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(playerName))
    {
        Console.WriteLine("You must enter a name, brave warrior!");
    }

} while (string.IsNullOrWhiteSpace(playerName));

//-----------------------------------------------------//


//-----------------------------------------------------//
// Här ska du välja en klass, det finns 3 val.

Console.WriteLine("\nYour name sounds interesting.. never heard of it.");
Console.WriteLine("Press enter to continue:");
Console.ReadLine();

Console.WriteLine("\nChoose your class, gladiator, you have three options:");
Console.WriteLine("1: Knight (High Defense)");
Console.WriteLine("2: Mage (High Attack)");
Console.WriteLine("3: Archer (Balanced)");

//-----------------------------------------------------//


//-----------------------------------------------------//
//Den här koden är kopplad till mina 3 val.Om spelaren skriver något annat som bokstäver, tomt, eller fel nummer får de ett..
//..felmeddelande och får försöka igen.
//TryParse försöker göra om text till en siffra, och vi kollar att den är mellan 1 och 3.
//Loopen fortsätter tills spelaren skrivit ett giltigt nummer.

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

//-----------------------------------------------------//

//-----------------------------------------------------//
//Det här delen av koden tar spelarens val och sätter rätt klassnamn och stats för attack och försvar, så att spelet vet..
//..hur stark och tålig spelaren är beroende på vilken klass som valdes. Om valet inte är giltigt får spelaren en standardklass.

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

//-----------------------------------------------------//

//-----------------------------------------------------//
//Här kommer det lite svårt..
//Den här koden skriver först ut en rad där den visar spelarens namn och klass, till exempel "Micke the Knight enters the battlefield!".


Console.WriteLine($"\n{playerName} the {playerClass} enters the battlefield!");


Enemy enemy = CreateRandomEnemy();

Console.WriteLine($"\n A wild {enemy.Name} appears! ");
//Sedan skapas en slumpmässig fiende med metoden CreateRandomEnemy(). Efter det skrivs det ut att en vild fiende har dykt upp, till exempel "A wild Goblin appears!".

// Sedan börjar en Fight loop som fortsätter så länge både spelaren och fienden har liv kvar. Först är det spelarens tur, där spelet skriver ut att det är spelarens tur.
while (playerHP > 0 && enemy.HP > 0)
{
    // Spelarens turn
    Console.WriteLine($"\n{playerName}'s Turn:");
    int damageDealt = playerAttack - enemy.Defense;
    damageDealt = Math.Max(0, damageDealt);
    //Skadan som spelaren gör beräknas genom att ta spelarens attack minus fiendens försvar..
    //..och om resultatet blir negativt sätts skadan till 0 eftersom man inte kan göra negativ skada.


    enemy.HP -= damageDealt;
    Console.WriteLine($"You deal {damageDealt} damage to {enemy.Name}! (Enemy HP: {enemy.HP})");

    //Den här skadan dras sedan av från fiendens liv. Min spel skriver ut hur mycket skada spelaren gjorde och hur mycket liv fienden har kvar.

    if (enemy.HP <= 0)
    {
        Console.WriteLine($"\n You defeated the {enemy.Name}! Victory!");
        break;
    }
    //Om fiendens liv blir 0 eller mindre betyder det att fienden är besegrad, och spelet skriver ut att spelaren har vunnit mot en enemy, och loopen avslutas med break.
    //-----------------------------------------------------//



    // Enemies turn att köra
    Console.WriteLine($"\n{enemy.Name}'s Turn:");
    int damageTaken = enemy.Attack - playerDefense;
    damageTaken = Math.Max(0, damageTaken);
    playerHP -= damageTaken;
    Console.WriteLine($"{enemy.Name} deals {damageTaken} damage to you! (Your HP: {playerHP})");

    //Fienden attackerar och skadan räknas ut som fiendens attack minus spelarens försvar, men skadan kan inte bli mindre än noll. 
    //Den dras av från spelarens liv och spelet visar hur mycket skada du fått och hur mycket liv som är kvar.

    if (playerHP <= 0)
    {
        Console.WriteLine($"\n You have been defeated by the {enemy.Name}...");
        break;
    }
}
    //Om ditt liv når noll eller mindre, förlorar du och spelet säger att du förlorade, sedan avslutas fighten.


//-----------------------------------------------------//

Console.WriteLine("\nGame Over. Thanks for playing!");


//-----------------------------------------------------//
// Den här metoden skapar en lista med olika fiender, där varje fiende har ett namn och olika värden för liv, attack och försvar.
//Sedan skapar den ett slumpmässigt tal som används för att välja en av enemy i listan. Metoden returnerar sedan den valda fienden..
//..alltså en slumpmässig enemy från listan, som används i spelet.
Enemy CreateRandomEnemy()
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

//-----------------------------------------------------//


//-----------------------------------------------------//
//I det här sista delen gäller det att denskapar en klass som heter Enemy..
//..alltså en mall för fiender i spelet. Klassen har fyra egenskaper:
//Name (namnet på enemy), 
//HP (hur mycket liv enemy har), 
//Attack (hur mycket skada enemy kan göra) och..
//Defense (hur mycket enemy kan försvara sig).

//metoden med samma namn som klassen, används för att skapa nya enemy objects med specifika värden för..
//..namn, liv, attack och försvar när en ny fiende skapas. 
//Det gör att varje fiende kan ha olika styrkor och egenskaper.
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

//-----------------------------------------------------//

//Slut..