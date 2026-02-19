
class PrintInColor
{
	public void Standard<T>(T printed)
	{
		if(typeof(T).IsClass && typeof(T) == typeof(Player))
		{
			Player player = printed as Player;
			player.checkWeight();
		} 
		else
		{
			Console.WriteLine(printed);
		}
		Console.ForegroundColor = ConsoleColor.White;
	}
    public void Red<T>(T printed)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Standard(printed);
	}

	public void Green<T>(T printed)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Standard(printed);
	}
}