class Inventory
{
    //fields
    private int maxWeight;
    public int weight { get; }
    private Dictionary<string, Item> items;

    //constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    //methods
    public bool Put(string itemName, Item item)
    {
        Console.WriteLine($"Weight is {item.Weight}");
        if(item.Weight < maxWeight)
        {
            items.Add(item.Description, item);
            return true;
        }
        return false;
    }

    public Item Get(string itemName)
    {
        if(items.ContainsKey(itemName))
        {
            items.Remove(itemName);
        }
        return null;
    }

    public int TotalWeight()
    {
        int total = 0;
        foreach(dynamic x in items)
        {
            total += weight;
        }
        return total;
    }
    
    public int FreeWeight()
    {
       int  freeWeight = maxWeight - TotalWeight();
       return freeWeight;
    }
}
