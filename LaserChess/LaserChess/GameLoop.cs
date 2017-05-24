﻿using LaserChess.ChessBoard;
using LaserChess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
							Console.ForegroundColor = ConsoleColor.Blue;
							switch (_selectedEntity.Type)
							{
								case EntityType.Grunt:
									Console.WriteLine("Grunt can move 1 space orthogonally. Attacks diagonally at any range.");
									break;
								case EntityType.Jumpship:
									Console.WriteLine("Jumpship moves like the knight in chess.");
									break;
								case EntityType.Tank:
									Console.WriteLine("Tank moves like the Queen in chess, up to 3 spaces.");
									break;
							}
							Console.ResetColor();

							Console.Write("Select new cell to move (<row><column>) or 'deselect' the current piece: ");

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
									_humanPlayerPieces = _chessBoard.GetPlayerPieces(EntityControlType.Human);

									UpdateScreen();

									string inputAttack = string.Empty;
									while (inputAttack == string.Empty)
									{
										Console.Write("Select cell (<row><column>) to attack or 'skip' attack: ");
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
							///TO DO implement Grunt attack
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
	}
}
