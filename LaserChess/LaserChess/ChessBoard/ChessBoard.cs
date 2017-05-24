using LaserChess.Entities;
using LaserChess.Entities.AI;
using LaserChess.Entities.Human;
using System;
using System.Collections.Generic;
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

			Console.WriteLine();
			Console.WriteLine("   +___+___+___+___+___+___+___+___+");

			for (int row = 0; row < Rows; row++)
			{
				Console.WriteLine("   |   |   |   |   |   |   |   |   |");

				var rowLine = new StringBuilder();
				//rowLine.Append($" {Rows - row} |");
				Console.Write($" {Rows - row} |");

				bool invertEmptySpace = row % 2 != 0;
				for (int column = 0; column < Columns; column++)
				{
					ChessBoardCell cell = ChessBoardCells[row, column];
					if (cell.IsOccupied && cell.Entity != null)
					{
						//rowLine.Append($"{cell}|");
						if (cell.Entity.ControlType == Entities.EntityControlType.Human)
						{
							Console.ForegroundColor = ConsoleColor.Cyan;
						}
						else if (cell.Entity.ControlType == Entities.EntityControlType.Ai)
						{
							Console.ForegroundColor = ConsoleColor.Red;
						}

						Console.Write($"{cell}");
						Console.ResetColor();
						Console.Write("|");
					}
					else
					{
						if ((column % 2) == 0)
						{
							if (invertEmptySpace)
							{
								//rowLine.Append($"{OddCellSymbol}|");
								Console.Write($"{OddCellSymbol}|");
							}
							else
							{
								//rowLine.Append($"{EvenCellSymbol}|");
								Console.Write($"{EvenCellSymbol}|");
							}
						}
						else
						{
							if (invertEmptySpace)
							{
								Console.Write($"{EvenCellSymbol}|");
								//rowLine.Append($"{EvenCellSymbol}|");
							}
							else
							{
								Console.Write($"{OddCellSymbol}|");
								//rowLine.Append($"{OddCellSymbol}|");
							}
						}
					}
				}

				//Console.WriteLine(rowLine.ToString());
				Console.WriteLine();
				Console.WriteLine("   |___|___|___|___|___|___|___|___|");
			}

			Console.WriteLine("     A   B   C   D   E   F   G   H  ");
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

		public List<PlayerPiece> GetPlayerPieces(EntityControlType controlType)
		{
			var playerPieces = new List<PlayerPiece>();
			
			for (int row = 0; row < Rows; row++)
			{
				for (int column = 0; column < Columns; column++)
				{
					if (ChessBoardCells[row, column].IsOccupied)
					{
						Entity entity = ChessBoardCells[row, column].Entity;
						if (entity.ControlType == controlType)
						{
							var playerPiece = new PlayerPiece
							{
								EntityID = entity.ID,
							};

							playerPiece.CurrentPosition = new ChessBoardPosition
							{
								CurrentRow = row,
								CurrentColumn = column,
							};

							playerPieces.Add(playerPiece);
						}
					}
				}
			}

			return playerPieces;
		}

		public ChessBoardPosition ParseChessBoardCellPosition(string line)
		{
			int column = -1;
			switch (line[0])
			{
				case 'A':
					column = 0;
					break;
				case 'B':
					column = 1;
					break;
				case 'C':
					column = 2;
					break;
				case 'D':
					column = 3;
					break;
				case 'E':
					column = 4;
					break;
				case 'F':
					column = 5;
					break;
				case 'G':
					column = 6;
					break;
				case 'H':
					column = 7;
					break;
				default:
					column = -1;
					Console.WriteLine("Invalid chess board column.");
					break;
			}

			if (column < 0)
			{
				return null;
			}

			int inputRow;
			bool validRow = Int32.TryParse(line[1].ToString(), out inputRow);

			int row = -1;
			if (validRow)
			{
				if (inputRow > 0 && inputRow < 9)
				{
					row = Rows - inputRow;
				}
			}

			if (row < 0)
			{
				return null;
			}

			var chessBoardPosition = new ChessBoardPosition
			{
				CurrentRow = row,
				CurrentColumn = column,
			};

			return chessBoardPosition;
		}

		public ChessBoardCell GetCell(ChessBoardPosition chessBoardPosition)
		{
			return ChessBoardCells[chessBoardPosition.CurrentRow, chessBoardPosition.CurrentColumn];
		}

		public void SetCell(ChessBoardPosition chessBoardPosition, ChessBoardCell cell)
		{
			ChessBoardCells[chessBoardPosition.CurrentRow, chessBoardPosition.CurrentColumn] = cell;
		}

		public void EmptyCell(ChessBoardPosition chessBoardPosition)
		{
			var cell = new ChessBoardCell
			{
				Entity = null,
				IsOccupied = false,
			};

			SetCell(chessBoardPosition, cell);
		}
	}
}
