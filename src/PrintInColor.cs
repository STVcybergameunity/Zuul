class PrintInColor
{
	public void Standard<T>(T printed)
	{
		if(typeof(T).IsClass)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Class can't be colored");
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