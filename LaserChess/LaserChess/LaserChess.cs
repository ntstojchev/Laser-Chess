﻿using System;
using System.IO;

namespace LaserChess
{
	class LaserChess
	{
		private const string _quitCommand = "exit";

		static void Main(string[] args)
		{
			Console.WriteLine(Environment.NewLine);
			Console.WriteLine("Welcome to LaserChess!");
			Console.WriteLine(Environment.NewLine);

			//TODO: Get level count from the 'Levels' directory. Read the level names from thier file names.
			Console.WriteLine("Choose Chess board:");
			Console.WriteLine("1. Initial fight");
			Console.WriteLine("2. WTF?!");
			Console.WriteLine("3. Dreadnought me baby!");
			Console.WriteLine("or write 'exit' to quit LaserChess!");
			Console.WriteLine(Environment.NewLine);

			int levelPick = 0;
			while (levelPick == 0)
			{
				Console.Write("Select board (1, 2 or 3) or quit: ");

				string line = Console.ReadLine();
				if (line == _quitCommand)
				{
					return;
				}

				int userPick = 0;
				bool validInput = Int32.TryParse(line, out userPick);

				if (validInput)
				{
					if ((userPick > 0) && (userPick < 4))
					{
						levelPick = userPick;
					}
					else
					{
						Console.WriteLine($"'{line}' is not valid chess board. Please pick one of the specified chess boards.");
					}
				}
				else
				{
					Console.WriteLine($"'{line}' is not valid chess board. Please pick one of the specified chess boards.");
				}
			}

			string[] rawChessBoard;
			try
			{
				rawChessBoard = LoadLevelFromFile(levelPick);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.ReadLine();
				return;
			}

			var chessBoard = new ChessBoard.ChessBoard();
			chessBoard.LoadLevel(rawChessBoard);

			chessBoard.Draw();

			Console.WriteLine("End of program...");
			Console.ReadLine();
		}

		private static string[] LoadLevelFromFile(int levelPick)
		{
			var level = new FileInfo($"{Environment.CurrentDirectory}\\Levels\\Level{levelPick}.txt");

			string rawChessBoard = level.OpenText().ReadToEnd();
			rawChessBoard = rawChessBoard.Replace(Environment.NewLine, "|");

			if (rawChessBoard.Length != 71)
			{
				throw new FileLoadException($"Invalid level: {rawChessBoard}");
			}

			string[] chessBoard = rawChessBoard.Split('|');

			return chessBoard;
		}

		private static void DrawLevel(string[] chessBoard)
		{
		}
	}
}