class Enemy
{
    private int HealthEnemy;
    private int CurrentHealthEnemy;
    private bool Alive
    {
        get { return CurrentHealthEnemy > 0; }
    }
    private int AttackDamage;
    private string DescriptionEnemy;
    public Room currentroom;

    public Enemy(int maxhealth, string desc, int attackdamage, Room room = null)
    {
        HealthEnemy = maxhealth;
        CurrentHealthEnemy = HealthEnemy;
        AttackDamage = attackdamage;
        DescriptionEnemy = desc;
        currentroom = room;
    }
    public void DamageEnemy(int damage)
    {
        CurrentHealthEnemy -= damage;
    }
    public bool EnemyIsAlive()
    {
        return this.Alive;
    }
    public string GetEnemyDesc()
    {
        return this.DescriptionEnemy;
    }
    public int GetEnemyDamage()
    {
        return this.AttackDamage;
    }
    public int GetEnemyCurrentHealth()
    {
        return this.CurrentHealthEnemy;
    }
}
