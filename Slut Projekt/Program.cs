﻿using System;
using System.Collections.Generic;

class Program
{
    //-----------------------------------------------------//
    //Det här är min random damage metod, som måste va placeras innan Main() (chatgpt sa det). Och det funkar.
    static int CalculateRandomDamage(int baseAttack)
    {
        Random rand = new Random();

        // Det här är en range mellan 80% och 120% for baseAttack
        int minDamage = (int)(baseAttack * 0.8);
        int maxDamage = (int)(baseAttack * 1.2);

        return rand.Next(minDamage, maxDamage + 1); // +1 max damage det här är.
    }

    //-----------------------------------------------------//


    static void Main()
    {

        //-----------------------------------------------------//
        //Här innan du går till Kungen och Arenan, kan jag göra att det finns choice, om du vill bara kolla, or participate.. typ choices du kan göra, för att göra spelet roligare.
        
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
            playerAttack = 30;
            playerDefense = 10;
        }
        else if (classChoice == 2)
        {
            playerClass = "Mage";
            playerAttack = 17;
            playerDefense = 5;
        }
        else if (classChoice == 3)
        {
            playerClass = "Archer";
            playerAttack = 25;
            playerDefense = 7;
        }

        //-----------------------------------------------------//

        //-----------------------------------------------------//
        //Här kommer det lite svårt..
        //Den här koden skriver först ut en rad där den visar spelarens namn och klass, till exempel "Micke the Knight enters the battlefield!".


        Console.WriteLine($"\n{playerName} the {playerClass} enters the battlefield!");
        Console.ReadLine();

        Enemy enemy = CreateRandomEnemy();

        Console.WriteLine($"\nA wild {enemy.Name} appears! ");
        Console.ReadLine();
        //Sedan skapas en slumpmässig fiende med metoden CreateRandomEnemy(). Efter det skrivs det ut att en vild fiende har dykt upp, till exempel "A wild Goblin appears!".

        //-----------------------------------------------------//
        // Sedan börjar en Fight loop som fortsätter så länge både spelaren och fienden har liv kvar. Först är det spelarens tur, där spelet skriver ut att det är spelarens tur.
        while (playerHP > 0 && enemy.HP > 0)
        {
            // Spelarens turn
            //Detta delen handlar om att man gör en slumptal när du (spelare) attackerar.
            Console.WriteLine($"\n{playerName}'s Turn:");
            Console.ReadLine();
            int rawPlayerDamage = CalculateRandomDamage(playerAttack); //Kalkulerar random damage för oss.
            int damageDealt = CalculateDamage(playerAttack, enemy.Defense); //Det här delen var lite annorlunda förrut, men för att jag addade critical hit, nu ändrade jag den så.


            //Skadan som spelaren gör beräknas genom att ta spelarens attack minus fiendens försvar..
            //..och om resultatet blir negativt sätts skadan till 0 eftersom man inte kan göra negativ skada.


            enemy.HP -= damageDealt;
            Console.WriteLine($"You deal {damageDealt} damage to {enemy.Name}! (Enemy HP: {enemy.HP})");
            Console.ReadLine();

            //Den här skadan dras sedan av från fiendens liv. Min spel skriver ut hur mycket skada spelaren gjorde och hur mycket liv fienden har kvar.

            if (enemy.HP <= 0)
            {
                Console.WriteLine($"\nYou defeated the {enemy.Name}! Victory!");
                Console.WriteLine("Press enter to continue:");
                Console.ReadLine();

                break;
            }
            //Om enemy liv blir 0 eller mindre betyder det att enemy är besegrad, och spelet skriver ut att spelaren har vunnit mot en enemy, och loopen avslutas med break.
            //-----------------------------------------------------//



            // Enemies turn att köra
            //Detta delen handlar om att man gör en slumptal när du (spelare) skyddar (blir attackerad).

            Console.WriteLine($"\n{enemy.Name}'s Turn:");
            Console.ReadLine();
            int rawEnemyDamage = CalculateRandomDamage(enemy.Attack); //Kalkulerar och ger en slumptal.
            int damageTaken = CalculateDamage(enemy.Attack, playerDefense); //Det här delen var lite annorlunda förrut, men för att jag addade critical hit, nu ändrade jag den så.
            playerHP -= damageTaken;
            Console.WriteLine($"{enemy.Name} deals {damageTaken} damage to you! (Your HP: {playerHP})");

            Console.ReadLine();


            //Fienden attackerar och skadan räknas ut som fiendens attack minus spelarens försvar, men skadan kan inte bli mindre än noll. 
            //Den dras av från spelarens liv och spelet visar hur mycket skada du fått och hur mycket liv som är kvar.

            if (playerHP <= 0)
            {
                Console.WriteLine($"\nYou have been defeated by the {enemy.Name}...");
                Console.WriteLine("Press enter to continue:");
                Console.ReadLine();

                break;

            }
        }
        //Om ditt liv når noll eller mindre, förlorar du och spelet säger att du förlorade, sedan avslutas fighten.


        //-----------------------------------------------------//

        Console.WriteLine("\nAfter the result, you are then presented with a heal potion..");
        System.Console.WriteLine("-----To be continued..-----");
        Console.ReadLine();
        //Här kan jag fortsätta med min spel..
        //..och utveckla den till t.ex: You are healed by a magician that cast a spel on you, then you go on to defeat the other enemy..
        //Eller om du vann så kan du heala din opponent, bli vänner efter, ha paths, som du väljer, om ni ska gå till aventyr tillsammans, eller du ska backstaba honom.
        //Du väljer.. jag kan utveckla den så där.  




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
        //Det här delen av koden är den sista improvement jag har gjort, som är critical damage.
        //Jag hade roligt koda den för att jag tänkte att det var lite svårt, och i det här delen fick jag chatgpt hjälpa mig.

        int CalculateDamage(int attacker, int defender)
        {
            Random rnd = new Random();
            int baseDamage = rnd.Next(attacker - 5, attacker + 6); // Addar randomness +5
            bool isCritical = rnd.NextDouble() < 0.10; // 10% chans för att göra en critical hit.

            if (isCritical)
            {
                Console.WriteLine("Critical Hit!");
                baseDamage *= 2;
            }

            int finalDamage = baseDamage - defender;
            return Math.Max(0, finalDamage);
        }

        //-----------------------------------------------------//

    }
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

