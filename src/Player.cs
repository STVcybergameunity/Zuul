using System.Runtime.InteropServices;

class Player
{
    //fields
    protected Inventory backpack;
    private int health;
    private PrintInColor printincolor = null;
    // auto property
    public Room CurrentRoom { get; set; }
    public int enemyAttack = 0;
    public Dictionary<string,Item> craftingshit = new Dictionary<string, Item>();
    public Random rndnum = new Random();
    public int tel = 0;
    
    // Makes a player with HP and a Inventory
    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backpack = new Inventory(100);
    }
    // Methods
    public Inventory getBackpack()
    {
        return backpack;
    }

    public Room GetRoom()
    {
        return CurrentRoom;
    }

    public int getHealth()
    {
        return health;
    }

    public Item Place(string itemName)
    {
        // A temp item object
        Item temp = null;

        if(backpack.getItems().ContainsKey(itemName))
        {
            // Item exists in Inventory?
            // Secure item
            temp = backpack.getItems()[itemName];

            // Remove from dictionary
            backpack.getItems().Remove(itemName);
        }

        // Return saved item
        return temp;
    }

    public bool isAlive()
    {
        if (health <= 0)
        {
            return false;
        }
        return true;
    }

    // A method that allows you to take damage
    // Can later be inplemented for on a hit taken /Maybe with random int
    public void Damage(int damage)
	{
		health -=damage;
	}

    // Heals the player but only if u can heal with a amount of hp u choose
    // Else print feedback
    public void Heal(string hptotstr)
	{
        if (hptotstr != null)
        {
            int hptot=Int32.Parse(hptotstr);
            if (health <= 100-hptot)
            {
                health += hptot;
                printincolor.Green($"You healed! Your health is now: {health}HP");
            }
            else
            {
                printincolor.Red("You aren't all that injured are you?");
            }
        }
        else
        {
            printincolor.Red("Please add a valid number...");
        }
	}

    // Allows you to hurt yourself for a amount that you typ in
    public void damageNum(string hptotstr)
	{
        if (hptotstr != null)
        {
            int hptot=Int32.Parse(hptotstr);
            if (health >= hptot)
            {
                health -= hptot;
                printincolor.Red($"You took {hptot} damage! Your health is now: {health}HP");
            }
            else
            {
                Console.WriteLine("That would kill u.");
            }
        }
        else
        {
            Console.WriteLine("Please add a valid number...");
        }
	}

    // If the player is low show a message
	// If you are very low show a diffrent message
	public void LowHp()
	{
		if (health <= 40 && health >= 30)
		{
			printincolor.Red($"You feel hurt.");
		}
		else if(health <= 20)
        {
			printincolor.Red($"You feel miserable. U should heal!");
		}
	}

    // Shows the players HP
	public void SeeHealth()
    {
        Console.WriteLine($"Your health is: {health}HP\n");
    }
    
    // Allows the player to see how much they are carrying and the space left
	public void checkWeight()
	{
		Console.Write("Total used weight is: ");
		Console.WriteLine(getBackpack().TotalWeight());
		Console.Write("You have: ");
		Console.WriteLine($"{getBackpack().FreeWeight()} weight left\n");
	}

    // Kill urself usefull to test on death triggers
    public void Sepuccu()
	{
		health = 0;
	}

    // Allows the enemy to attack you in game
    public int EnemyAttack(int num, int num1)
    {
		int dodgenr = rndnum.Next(1,11);
		if (dodgenr == 1)
		{
			health -= enemyAttack;
            return enemyAttack;
		}
		else
		{
			enemyAttack = rndnum.Next(num,num1);
            health -= enemyAttack;
			return enemyAttack;
		}
    }

    // Allows the player to attack the enemy in game
    public int PlayerAttack(int num, int num1)
	{	
		int playerAttack = 0;
		int dodgenrplayer = rndnum.Next(1,11);
		if (dodgenrplayer == 1)
		{
			CurrentRoom.enemy.DamageEnemy(playerAttack);
            printincolor.Red("You missed\n");
            return playerAttack;
		}

		else
		{
			playerAttack = rndnum.Next(num,num1);
			CurrentRoom.enemy.DamageEnemy(playerAttack);
            return playerAttack;
		}
    }
    // Allows u to take a item from a room
    public bool TakeFromChest(string itemName)
    {
		Item testtemp = CurrentRoom.Chest.Peek(itemName);
        if (testtemp.Weight > backpack.FreeWeight())
        {
            printincolor.Red($"The item is to heavy u need {testtemp.Weight - backpack.FreeWeight()} more space\n");
            return false; 
        }

        if (testtemp != null)
        {
            // Remove itemName from chest and save it
            Item item = CurrentRoom.Chest.Get(itemName);

            // Add the item we took to the backpack
            getBackpack().Put(itemName, item);

            Console.WriteLine($"You took {itemName} from the chest.");
            // Return true
            return true;
        }
        // If empty return false
        Console.WriteLine($"The item {itemName} doesn't exist here.");
        return false;
    }

    // Allows you to add items to a room
	public bool PutInChest(string itemName)
    {
        if (getBackpack() != null)
        {
            // Remove itemName from backpack and save it
            Item item = Place(itemName);

            // Add the item we took to the inventory
            CurrentRoom.Chest.Put(itemName, item);

            // Return true
            return true;
        }

        // If empty return false
        return false;
    }

    // Uses a item
    public void Use(Command command)
    {
        if(!command.HasSecondWord())
        {
            Console.WriteLine("What do u want to use?\n");
            return;
        }

        string itemName = command.SecondWord;
        Item item = backpack.Peek(itemName);
        backpack.Get(itemName);
        if (item == null)
        {
            printincolor.Red("You don't have that as a item.\n");
            Console.WriteLine(CurrentRoom.GetLongDescription());
            return;
        }
        
        // Checks what item it is then uses it
        switch(itemName)
        {
            case "bandage":
                if (health <= 100-40)
                {
                    health += 40;
                    Console.WriteLine("You used the bandage +20HP");
                    printincolor.Green($"You healed! Your health is now: {health}HP\n");
                }

                else
                {
                    Console.WriteLine("I'm not all that hurt, so I have no need for this now.\n");
                }
                
                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;

            case "medkit":
                if (health <= 100-50)
                {
                    Console.WriteLine($"U used the medkit +{100-health}HP");
                    health = 100;
                    printincolor.Green($"You healed! Your health is now: {health}HP\n");
                }

                else
                {
                    Console.WriteLine("It would be a waste to use this right now.\n");
                }

                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;

            case "meatpack":
                getBackpack().GetMaxWeight(900);
                Console.WriteLine($"You equiped the meatpack your carrying capacity increased to {getBackpack().GetMaxWeight(0)}.\n");
                return;

            case "key":

                if (!command.HasThirdWord())
                {
                    return;
                }

                Room lockedRoom = CurrentRoom.GetExit(command.ThirdWord);

                if (lockedRoom == null)
                {
                    printincolor.Red("No room in that direction.\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                else if (!lockedRoom.GetLock())
                {
                    printincolor.Red("Room isn't locked.\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                lockedRoom.RemoveLock();
                printincolor.Green("The door is now unlocked.\n");
                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;

            case "hydraulics":
                
                Room nurgleLockedRoom = CurrentRoom.GetExit(command.ThirdWord);

                if (nurgleLockedRoom == null)
                {
                    printincolor.Red("No room in that direction.\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                if (!nurgleLockedRoom.GetNurgleLock())
                {
                    printincolor.Red("I can't use that here\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                nurgleLockedRoom.RemoveNurgleLock();
                printincolor.Green("It's open now the smell is horrible\n");
                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;
            
            case "nurgling":

                if (!command.HasThirdWord())
                {
                    Console.WriteLine("Where shall I use this?\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                Room nurgleDrop = CurrentRoom.GetExit(command.ThirdWord);

                if (!nurgleDrop.GetNurgleDrop())
                {
                    printincolor.Red("I can't use that here\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                else if (nurgleDrop.GetNurgleLock())
                {
                    printincolor.Red("It's mouth is still closed maybe I can open it.\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                }

                else 
                {
                    backpack.Put("bookofmeat", new Item(0,"bookofmeat"));
                    printincolor.Green("The nurgling was devoured and a book remained. You gained the bookofmeat.\n");
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                }
                break;
            
            default:
                Console.WriteLine($"You can't use {command.SecondWord}.");
                break;
        }
    }

    // Allows the usage of spells if you have the item bookofmeat
    public void Spells(Command command)
    {
        if(!command.HasSecondWord())
        {
            Console.WriteLine("What do u want to use?\n");
            return;
        }

        if(!command.HasThirdWord())
        {
            Console.WriteLine("What spell do u want to use?\n");
        }

        if (CurrentRoom.enemy == null)
		{
	        printincolor.Red("There is nothing here to attack.\n");
			return;
		}
        
        string itemName = command.SecondWord;
        Item item  = backpack.Peek(itemName);
        if (item == null)
        {
            printincolor.Red("You don't have that as a item.\n");
            Console.WriteLine(CurrentRoom.GetLongDescription());
            return;
        }

        if (tel != 0)
        {
            printincolor.Red("You can only use one spell per turn\n");
            return;
        }

        switch(itemName)
        {
            case "bookofmeat":
                if (SpellsHelp(command) != 0)
                {
                    tel += 1;
                }
                break;

            default:
                break;
        }
        
        if (CurrentRoom.enemy.GetEnemyCurrentHealth() > 0)
        {
            printincolor.Red($"The enemy has {CurrentRoom.enemy.GetEnemyCurrentHealth()}HP remaining\n");
        }
        
        else
		{
			printincolor.Green("You slayed the thing.\n");
            Console.WriteLine(CurrentRoom.GetLongDescription());
		}

        if (!CurrentRoom.enemy.EnemyIsAlive())
		{
			CurrentRoom.enemy = null;
		}
    }

    // Assists the spells

    public int SpellsHelp(Command command)
    {
        int damageSpell = 0;
        string spellName = command.ThirdWord;
        switch(spellName)
        {
            case "fireball":
                damageSpell = PlayerAttack(45,61);
                printincolor.Green($"You used {spellName} you dealt {damageSpell} damage");
                return damageSpell;

            default:
                printincolor.Red($"You don't have the spell {command.ThirdWord}.");
                break;
        }
        return damageSpell;
    }

    // Allows the crafting of the hydrolics item if the correct items are called
    public string Craft(Command command)
    {
        if  (!command.HasSecondWord() || !command.HasThirdWord())
        {
            printincolor.Red("What do u want to use in crafting.\n");
            return null;
        }

        else if (!command.HasFourthWord())
        {
            printincolor.Red("I need one more thing.\n");
            return null;
        }

        Item craftItem = backpack.Peek(command.SecondWord);
        Item craftItem2 = backpack.Peek(command.ThirdWord);
        Item craftItem3 = backpack.Peek(command.FourthWord);

        if (craftItem == null || craftItem2 == null || craftItem3 == null)
        {
            printincolor.Red("You don't have that item.");
            return null;
        }

        craftingshit.Add(command.SecondWord,craftItem);
        craftingshit.Add(command.ThirdWord,craftItem2);
        craftingshit.Add(command.FourthWord,craftItem3);

        if (!craftingshit.ContainsKey("metalrod") || !craftingshit.ContainsKey("piston") || !craftingshit.ContainsKey("ducttape"))
        {
            printincolor.Red("Those weren't the correct items.\n");
            return null;
        }

        else
        {
            return "hydraulics";
        }
    }
}
