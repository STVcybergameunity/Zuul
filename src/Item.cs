
class Item
{
    //fields
    public int Weight { get; }
    public string Description { get; }

    //constructor

    // Makes a item from the information given at Game
    public Item(int weight, string description)
    {
        Weight = weight;
        Description = description;
    }
}