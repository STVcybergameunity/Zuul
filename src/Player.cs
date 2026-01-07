class Player
{
    private Game game;
    //fields
    public int health;
    // auto property
    public Room CurrentRoom { get; set; }
    // constructor
    public Player()
    {
        CurrentRoom = null;
        health = 100;
    }
    //methods
    void Heal()
    {
        health = 100;
    }
}
