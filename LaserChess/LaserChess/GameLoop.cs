using LaserChess.ChessBoard;
using LaserChess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess
{
	public class GameLoop
	{
		private ChessBoard.ChessBoard _chessBoard;
		private GameState _currentGameState;

		private List<PlayerPiece> _humanPlayerPieces;
		private List<PlayerPiece> _aiPlayerPieces;

		private List<Guid> _playedEntityIDs;
		private Entity _selectedEntity;

		public GameLoop(ChessBoard.ChessBoard chessBoard)
		{
			_chessBoard = chessBoard;

			Setup();
			EnterGameLoop();
		}

		private void Setup()
		{
			_currentGameState = GameState.PlayerTurn;

			_humanPlayerPieces = _chessBoard.GetPlayerPieces(EntityControlType.Human);
			_aiPlayerPieces = _chessBoard.GetPlayerPieces(EntityControlType.Ai);

			_playedEntityIDs = new List<Guid>();
		}

		private void EnterGameLoop()
		{


			bool metVictoryCondition = false;
			while (!metVictoryCondition)
			{
				UpdateScreen();

				if (_currentGameState == GameState.PlayerTurn)
				{
					while (_currentGameState == GameState.PlayerTurn)
					{
						#region Play with selected piece

						if (_selectedEntity != null)
						{
							switch (_selectedEntity.Type)
							{
								case EntityType.Grunt:
									Console.WriteLine("Grunt can move 1 space orthogonally.");
									break;
								case EntityType.Jumpship:
									Console.WriteLine("Jumpship moves like the knight in chess.");
									break;
								case EntityType.Tank:
									Console.WriteLine("Tank moves like the Queen in chess, up to 3 spaces.");
									break;
							}

							Console.Write("Select new cell (<row><column>) or deselect the current piece: ");

							string inputLine = Console.ReadLine();
							ChessBoardPosition newChessBoardPosition = ParseChessBoardCellPosition(inputLine);
							ChessBoardPosition oldChessBoardPosition = (_humanPlayerPieces.First(p => p.EntityID == _selectedEntity.ID)).CurrentPosition;

							//_selectedEntity.Move(_chessBoard, oldChessBoardPosition, newChessBoardPosition);

							///TO DO implement Grunt Move and attack

							_playedEntityIDs.Add(_selectedEntity.ID);
							_selectedEntity = null;
							break;
						}

						#endregion

						#region Select piece or end turn

						Console.Write("Select player piece (<row><column>), end turn (endturn), or quit (exit): ");

						string line = Console.ReadLine();
						if (line != string.Empty)
						{
							if (line == "endturn")
							{
								EndTurn();
								_currentGameState = GameState.AiTurn;
							}
							else if (line == "exit")
							{
								return;
							}
							else if (line.Length == 2)
							{
								SelectEntityDuringHumanTurn(line);
							}
						}

						#endregion
					}
				}
				else if (_currentGameState == GameState.AiTurn)
				{
					AiPlay();
				}

				//add victory conditions
			}
		}

		private void UpdateScreen()
		{
			_chessBoard.Draw();
			Console.WriteLine();
		}

		private void EndTurn()
		{

		}

		private void AiPlay()
		{

		}

		private void SelectEntityDuringHumanTurn(string line)
		{
			ChessBoardPosition chessBoardPosition = ParseChessBoardCellPosition(line);
			if (chessBoardPosition == null)
			{
				Console.WriteLine("Invalid cell.");
				return;
			}

			ChessBoardCell cell = _chessBoard.GetCell(chessBoardPosition);
			if (cell == null)
			{
				Console.WriteLine($"Specified cell {line} is empty.");
			}
			else if (_playedEntityIDs.Contains(cell.Entity.ID))
			{
				Console.WriteLine($"You've already played with the piece on {line}.");
			}
			else
			{
				_selectedEntity = cell.Entity;
			}
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
					row = _chessBoard.Rows - inputRow;
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
	}
}
