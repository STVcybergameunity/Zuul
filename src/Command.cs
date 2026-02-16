class Command
{
	public string CommandWord { get; init; }
	public string SecondWord { get; init; }
	public string ThirdWord { get; init; }
	public string FourthWord { get; init; }
	
	// Create a command object. First and second word must be supplied, but
	// either one (or both) can be null. See Parser.GetCommand()
	public Command(string first, string second, string third, string fourth)
	{
		CommandWord = first;
		SecondWord = second;
		ThirdWord = third;
		FourthWord = fourth;
	}

	
	// Return true if this command was not understood.
	public bool IsUnknown()
	{
		return CommandWord == null;
	}

	
	// Return true if the command has a second word.
	public bool HasSecondWord()
	{
		return SecondWord != null;
	}

	public bool HasThirdWord()
	{
		return ThirdWord != null;
	}
	public bool HasFourthWord()
	{
		return FourthWord != null;
	}
}
