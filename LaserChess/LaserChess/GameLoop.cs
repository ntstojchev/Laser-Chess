using LaserChess.ChessBoard;
using LaserChess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

		private bool metVictoryCondition;

		public GameLoop(ChessBoard.ChessBoard chessBoard)
		{
			_chessBoard = chessBoard;

			Setup();
			EnterGameLoop();
		}

		private void Setup()
		{
			_currentGameState = GameState.PlayerTurn;

			_humanPlayerPieces = _chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Human);
			_aiPlayerPieces = _chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Ai);

			_playedEntityIDs = new List<Guid>();
		}

		private void EnterGameLoop()
		{
			metVictoryCondition = false;
			while (!metVictoryCondition)
			{
				UpdateScreen();

				if (_currentGameState == GameState.PlayerTurn)
				{
					HumanPlay();
				}
				else if (_currentGameState == GameState.AiTurn)
				{
					AiPlay();
					_aiPlayerPieces = _chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Ai);
					_currentGameState = GameState.PlayerTurn;
				}
				else if (_currentGameState == GameState.Quit)
				{
					return;
				}
			}
		}

		private void UpdateScreen()
		{
			_chessBoard.Draw();
			Console.WriteLine();
		}

		private void HumanPlay()
		{
			while (_currentGameState == GameState.PlayerTurn)
			{
				if (_playedEntityIDs.Count == _humanPlayerPieces.Count)
				{
					Console.WriteLine("You've played with all your pieces. Ending turn...");
					Console.ReadLine();

					EndTurn();
					_currentGameState = GameState.AiTurn;
					break;
				}

				#region Play with selected piece

				if (_selectedEntity != null)
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					switch (_selectedEntity.Type)
					{
						case EntityType.Grunt:
							Console.WriteLine("Grunt can move 1 space orthogonally. Attacks diagonally at any range.");
							break;
						case EntityType.Jumpship:
							Console.WriteLine(@"Jumpship moves like the knight in chess. Attack all 4 spaces orthogonally adjacent.
For attacking, specify Jumpship's current position.");
							break;
						case EntityType.Tank:
							Console.WriteLine("Tank moves like the Queen in chess, up to 3 spaces. Attacks orthogonally at any range");
							break;
					}
					Console.ResetColor();

					Console.Write("Select new cell to move (<column><row>) or 'deselect' the current piece: ");

					string inputLine = Console.ReadLine();
					if (inputLine == "deselect")
					{
						_selectedEntity = null;
						break;
					}
					else if (inputLine.Length == 2)
					{
						ChessBoardPosition newChessBoardPosition = _chessBoard.ParseChessBoardCellPosition(inputLine);
						ChessBoardPosition oldChessBoardPosition = (_humanPlayerPieces.First(p => p.EntityID == _selectedEntity.ID)).CurrentPosition;

						try
						{
							_selectedEntity.Move(_chessBoard, oldChessBoardPosition, newChessBoardPosition);
							_humanPlayerPieces = _chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Human);

							UpdateScreen();

							string inputAttack = string.Empty;
							while (inputAttack == string.Empty)
							{
								Console.Write("Select cell (<column><row>) to attack or 'skip' attack: ");
								inputAttack = Console.ReadLine();

								if (inputAttack.Length == 2)
								{
									ChessBoardPosition targetPosition = _chessBoard.ParseChessBoardCellPosition(inputAttack);

									if (targetPosition == null)
									{
										Console.WriteLine("Invalid cell.");
										inputAttack = string.Empty;
									}
									else
									{
										try
										{
											ChessBoardPosition currentPosition = (_humanPlayerPieces.First(p => p.EntityID == _selectedEntity.ID)).CurrentPosition;
											_selectedEntity.Attack(_chessBoard, currentPosition, targetPosition);

											_aiPlayerPieces = _chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Ai);
										}
										catch (Exception ex)
										{
											Console.WriteLine(ex.Message);
											inputAttack = string.Empty;
										}
									}
								}
								else if (inputAttack == "skip")
								{
									inputAttack = "skip";
								}
								else
								{
									inputAttack = string.Empty;
								}
							}

							_playedEntityIDs.Add(_selectedEntity.ID);
							_selectedEntity = null;
							break;
						}
						catch (Exception ex)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine(ex.Message);
							Console.ReadLine();
							Console.ResetColor();
							break;
						}
					}
					else
					{
						break;
					}
				}

				#endregion

				CheckVictoryConditions();
				if (metVictoryCondition)
				{
					return;
				}

				#region Select piece or end turn

				Console.Write("Select player piece (<column><row>), end turn (endturn), or quit (exit): ");

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
						_currentGameState = GameState.Quit;
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

		private void AiPlay()
		{
			DronesMoveAndAttack();

			DreadnoughtsMoveAndAttack();

			CommandUnitsMoveAndAttack();

			EndTurn();
		}

		private void DronesMoveAndAttack()
		{
			List<PlayerPiece> drones = _chessBoard.GetPlayerPiecesBasedOnEntityType(EntityType.Drone);
			if (drones.Count > 1)
			{
				drones.Shuffle();
			}

			foreach (PlayerPiece drone in drones)
			{
				Entity entity = _chessBoard.GetEntity(drone);
				entity.Move(_chessBoard, drone.CurrentPosition, null);

				UpdateScreen();
				Thread.Sleep(450);

				ChessBoardPosition newEntityPosition = _chessBoard.GetEntityPosition(drone.EntityID);
				entity.Attack(_chessBoard, newEntityPosition, null);
			}
		}

		private void DreadnoughtsMoveAndAttack()
		{
			List<PlayerPiece> dreadnoughts = _chessBoard.GetPlayerPiecesBasedOnEntityType(EntityType.Dreadnought);
			if (dreadnoughts.Count > 1)
			{
				dreadnoughts.Shuffle();
			}

			foreach (PlayerPiece dreadnought in dreadnoughts)
			{
				Entity entity = _chessBoard.GetEntity(dreadnought);
				entity.Move(_chessBoard, dreadnought.CurrentPosition, null);

				UpdateScreen();
				Thread.Sleep(450);

				ChessBoardPosition newEntityPosition = _chessBoard.GetEntityPosition(dreadnought.EntityID);
				entity.Attack(_chessBoard, newEntityPosition, null);
			}
		}

		private void CommandUnitsMoveAndAttack()
		{
			List<PlayerPiece> commandUnits = _chessBoard.GetPlayerPiecesBasedOnEntityType(EntityType.CommandUnit);

			foreach (PlayerPiece commandUnit in commandUnits)
			{

			}
		}

		private void EndTurn()
		{
			_humanPlayerPieces = _chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Human);
			_aiPlayerPieces = _chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Ai);

			_selectedEntity = null;
			_playedEntityIDs = new List<Guid>();

			CheckVictoryConditions();
		}

		private void SelectEntityDuringHumanTurn(string line)
		{
			ChessBoardPosition chessBoardPosition = _chessBoard.ParseChessBoardCellPosition(line);
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
			else if (cell.IsOccupied)
			{
				if (_playedEntityIDs.Contains(cell.Entity.ID))
				{
					Console.WriteLine($"You've already played with the piece on {line}.");
				}
				else
				{
					_selectedEntity = cell.Entity;

				}
			}
		}

		private void CheckVictoryConditions()
		{
			if (_humanPlayerPieces.Count == 0)
			{
				metVictoryCondition = true;

				Console.WriteLine("AI wins! No more human pieces!");
				Console.ReadLine();
				return;
			}

			foreach (PlayerPiece playerPiece in _aiPlayerPieces)
			{
				Entity entity = _chessBoard.GetEntity(playerPiece);
				if ((entity.Type == EntityType.Drone)
					&& (playerPiece.CurrentPosition.CurrentRow == _chessBoard.Rows - 1))
				{
					metVictoryCondition = true;

					Console.WriteLine("AI wins! Enemy drone reached 8th row! Now he's a princess!");
					Console.ReadLine();
					return;
				}
			}

			bool aliveCommandUnit = false;
			foreach (PlayerPiece playerPiece in _aiPlayerPieces)
			{
				Entity entity = _chessBoard.GetEntity(playerPiece);
				if (entity.Type == EntityType.CommandUnit)
				{
					aliveCommandUnit = true;
					return;
				}
			}

			if (!aliveCommandUnit)
			{
				metVictoryCondition = true;

				Console.WriteLine("Human wins! All command units are destroyed!");
				Console.ReadLine();
			}


		}
	}
}
