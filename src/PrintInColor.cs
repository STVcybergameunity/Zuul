class PrintInColor
{
	public void Standard<T>(T printed)
	{
		if(typeof(T).IsClass && typeof(T) != typeof(string))
		{
			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = ConsoleColor.Red;
			Console.WriteLine("Class can't be colored");
			Console.BackgroundColor = ConsoleColor.Black;
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

	public void Yellow<T>(T printed)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Standard(printed);
	}
}