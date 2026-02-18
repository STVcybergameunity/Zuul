using System.Collections;

class Inventory
{
    //fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    //constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    public Dictionary<string, Item> getItems()
    {
        return items;
    }

    //methods

    // allows a item to be placed in a room
    public bool Put(string _, Item item)
    {
        if(item.Weight <= FreeWeight())
        {
            items.Add(item.Description, item);
            return true;
        }
        Console.WriteLine($"You can't carry anymore\nWeight left {FreeWeight()} the item weights is {item.Weight}");
        return false;
    }

    public int GetMaxWeight(int weight)
    {
        maxWeight = maxWeight += weight;
        return this.maxWeight;
    }

    // Used to help assist add to backpack
    public Item Get(string itemName)
    {
        // Temp item creation
        Item temp = null;

        if(items.ContainsKey(itemName))
        {
            // If item exists
            // Put it in the temp
            temp = items[itemName];

            // Remove item from inventory
            items.Remove(itemName);
        }

        // Return item
        return temp;
    }

    public Item Peek(string itemName)
    {
        // Temp item creation
        Item temp = null;

        if(items.ContainsKey(itemName))
        {
            // If item extists
            // Put it in the temp
            temp = items[itemName];
        }
        return temp;
    }

    // Adds up all of the weight of the items in the inventory
    public int TotalWeight()
    {
        int total = 0;

        foreach(var (key, item) in items)
        {
            total += item.Weight;
        }

        return total;
    }
    
    // See how much space is left in the Inventory
    public int FreeWeight()
    {
       int  freeWeight = maxWeight - TotalWeight();
       return freeWeight;
    }
}
