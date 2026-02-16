using System;

class Parser
{
	// Holds all valid command words
	private readonly CommandLibrary commandLibrary; 

	// Constructor
	public Parser()
	{
		commandLibrary = new CommandLibrary();
	}

	// Ask and interpret the user input. Return a Command object.
	public Command GetCommand()
	{
		Console.Write("> "); // print prompt

		string word1 = null;
		string word2 = null;
		string word3 = null;
		string word4 = null;

		// string.Split() returns an array
		string[] words = Console.ReadLine().Split(' ');
		if (words.Length > 0) { word1 = words[0]; }
		if (words.Length > 1) { word2 = words[1]; }
		if (words.Length > 2) { word3 = words[2]; }
		if (words.Length > 3) { word4 = words[3]; }

		// Now check whether this word is known. If so, create a command with it.
		if (commandLibrary.IsValidCommandWord(word1)) {
			Console.WriteLine("");
			return new Command(word1, word2, word3, word4);
		}

		// If not, create a "null" command (for unknown command).
		Console.WriteLine("");
		return new Command(null, null, null, null);
	}

	// Prints a list of valid command words from commandLibrary.
	public void PrintValidCommands()
	{
		Console.WriteLine("Your command words are:");
		Console.WriteLine(commandLibrary.GetCommandsString(), "\n");
	}
}
