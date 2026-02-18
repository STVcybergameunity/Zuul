
class PrintInColor
{
    public void Red(string printed)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(printed);
		Console.ForegroundColor = ConsoleColor.White;
	}

	public void Green(string printed)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(printed);
		Console.ForegroundColor = ConsoleColor.White;
	}
}