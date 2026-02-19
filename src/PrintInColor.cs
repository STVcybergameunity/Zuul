
class PrintInColor
{
    public void Red<T>(T printed)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(printed);
		Console.ForegroundColor = ConsoleColor.White;
	}

	public void Green<T>(T printed)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(printed);
		Console.ForegroundColor = ConsoleColor.White;
	}
}