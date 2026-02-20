class PrintInInputColor
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
    public void Color<T>(T printed, string color)
	{
		if (Enum.TryParse<ConsoleColor>(color, true, out var colorValue))
		{
			Console.ForegroundColor = colorValue;
			Standard(printed);
			Console.ForegroundColor = ConsoleColor.White;
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Invalid color specified. Defaulting to white.");
			Standard(printed);
		}
	}
}