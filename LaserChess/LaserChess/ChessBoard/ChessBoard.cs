﻿using LaserChess.Entities.AI;
using LaserChess.Entities.Human;
using System;
using System.Text;

namespace LaserChess.ChessBoard
{
	public class ChessBoard
	{
		private const string _evenCellSymbol = " * ";
		private const string _oddCellSymbol = " # ";
		private const int _rows = 8;
		private const int _columns = 8;

		private ChessBoardCell[,] _chessBoardCells;

		public int Rows { get; set; }

		public int Columns { get; set; }

		public string EvenCellSymbol
		{
			get { return _evenCellSymbol; }
		}

		public string OddCellSymbol
		{
			get { return _oddCellSymbol; }
		}

		public ChessBoardCell[,] ChessBoardCells
		{
			get { return _chessBoardCells; }
		}

		public ChessBoard()
		{
			Rows = _rows;
			Columns = _columns;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Draw()
		{
			///("_ +___+___+___+___+___+___+___+___+");
			///("  |   |   |   |   |   |   |   |   |");
			///("A |   |   |   |   |   |   |   |   |");
			///("  |___|___|___|___|___|___|___|___|");
			///("  |   |   |   |   |   |   |   |   |");
			///("B |   |   |   |   |   |   |   |   |");
			///("  |___|___|___|___|___|___|___|___|");

			Console.Clear();

			Console.WriteLine("  +___+___+___+___+___+___+___+___+");

			for (int row = 0; row < Rows; row++)
			{
				Console.WriteLine("  |   |   |   |   |   |   |   |   |");

				var rowLine = new StringBuilder();
				rowLine.Append($"{Rows - row} |");

				bool invertEmptySpace = row % 2 != 0;
				for (int column = 0; column < Columns; column++)
				{
					ChessBoardCell cell = ChessBoardCells[row, column];
					if (cell.IsOccupied && cell.Entity != null)
					{
						rowLine.Append($"{cell}|");
					}
					else
					{
						if ((column % 2) == 0)
						{
							if (invertEmptySpace)
							{
								rowLine.Append($"{OddCellSymbol}|");
							}
							else
							{
								rowLine.Append($"{EvenCellSymbol}|");
							}
						}
						else
						{
							if (invertEmptySpace)
							{
								rowLine.Append($"{EvenCellSymbol}|");
							}
							else
							{
								rowLine.Append($"{OddCellSymbol}|");
							}
						}
					}
				}

				Console.WriteLine(rowLine.ToString());
				Console.WriteLine("  |___|___|___|___|___|___|___|___|");
			}

			Console.WriteLine("  + A   B   C   D   E   F   G   H +");
		}

		/// <summary>
		/// 
		/// </summary>
		public void LoadLevel(string[] levelPath)
		{
			_chessBoardCells = new ChessBoardCell[Rows, Columns];

			for (int row = 0; row < Rows; row++)
			{
				for (int column = 0; column < Columns; column++)
				{
					char cell = (levelPath[row])[column];

					var chessBoardCell = new ChessBoardCell();
					switch(cell)
					{
						case 'G':
							chessBoardCell.Entity = new Grunt();
							chessBoardCell.IsOccupied = true;
							break;
						case 'J':
							chessBoardCell.Entity = new Jumpship();
							chessBoardCell.IsOccupied = true;
							break;
						case 'T':
							chessBoardCell.Entity = new Tank();
							chessBoardCell.IsOccupied = true;
							break;
						case 'D':
							chessBoardCell.Entity = new Drone();
							chessBoardCell.IsOccupied = true;
							break;
						case 'N':
							chessBoardCell.Entity = new Dreadnought();
							chessBoardCell.IsOccupied = true;
							break;
						case 'C':
							chessBoardCell.Entity = new CommandUnit();
							chessBoardCell.IsOccupied = true;
							break;
						case '0':
							chessBoardCell.Entity = null;
							chessBoardCell.IsOccupied = false;
							break;
					}

					_chessBoardCells[row, column] = chessBoardCell;
				}
			}
		}
	}
}