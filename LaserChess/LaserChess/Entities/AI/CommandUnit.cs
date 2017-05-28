using LaserChess.ChessBoard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities.AI
{
	public class CommandUnit : Entity
	{
		public const string _name = "CommandUnit";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "C";
		public const int _hitPoints = 5;
		public const int _attackPower = 0;
		public const int _movementPoint = 1;
		public const int _attackRange = 0;
		public const EntityType _type = EntityType.CommandUnit;
		public const EntityControlType _controlType = EntityControlType.Ai;
		public const EntityMovementType _movementType = EntityMovementType.Orthogonally;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.None;
		public const EntityAttackType _attackType = EntityAttackType.None;

		public CommandUnit()
		{
			Name = _name;
			ID = _id;
			Icon = _icon;
			HitPoints = _hitPoints;
			AttackPower = _attackPower;
			MovementPoint = _movementPoint;
			AttackRange = _attackRange;
			Type = _type;
			ControlType = _controlType;
			MovementType = _movementType;
			MovementPattern = _movementPattern;
			AttackType = _attackType;
		}

		public override void Attack(ChessBoard.ChessBoard chessBoard, ChessBoardPosition currentPosition, ChessBoardPosition targetPosition)
		{
			//This unit does not attack.
		}

		public override void Move(ChessBoard.ChessBoard chessBoard, ChessBoardPosition oldPosition, ChessBoardPosition newPosition)
		{
			List<PlayerPiece> playerPieces = chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Human);
			CommandUnitMoveState moveState = CommandUnitMoveState.StayStill;

			var possibleAttacks = new List<KeyValuePair<int, List<Point>>>();
			foreach (PlayerPiece playerPiece in playerPieces)
			{
				bool[,] chessBoardGrid = chessBoard.GetPathfinderGrid();
				chessBoardGrid[oldPosition.CurrentRow, oldPosition.CurrentColumn] = true;
				chessBoardGrid[playerPiece.CurrentPosition.CurrentRow, playerPiece.CurrentPosition.CurrentColumn] = true;

				Point startLocation = new Point(oldPosition.CurrentRow, oldPosition.CurrentColumn);
				Point endLocation = new Point(playerPiece.CurrentPosition.CurrentRow, playerPiece.CurrentPosition.CurrentColumn);

				SearchParameters sp = new SearchParameters(startLocation, endLocation, chessBoardGrid);
				PathFinder pathfinder = new PathFinder(sp);

				List<Point> nodes = pathfinder.FindPath();
				possibleAttacks.Add(new KeyValuePair<int, List<Point>>(nodes.Count, nodes));
			}

			var paths = new List<int>();
			foreach (KeyValuePair<int, List<Point>> pair in possibleAttacks)
			{
				paths.Add(pair.Key);
			}

			paths.Sort();
			KeyValuePair<int, List<Point>> nearestEnemy = new KeyValuePair<int, List<Point>>(0, null);
			if (paths.Count > 0)
			{
				nearestEnemy = possibleAttacks.Find(m => m.Key == paths[0]);
			}

			if (paths.Count > 0 && nearestEnemy.Key != 0)
			{
				if (nearestEnemy.Key == 1 || nearestEnemy.Key == 2)
				{
					if (nearestEnemy.Value[nearestEnemy.Value.Count - 1].X >= oldPosition.CurrentRow)
					{
						moveState = CommandUnitMoveState.MoveLeft;
					}
					else if ((nearestEnemy.Value[nearestEnemy.Value.Count - 1].X <= oldPosition.CurrentRow))
					{
						moveState = CommandUnitMoveState.MoveRight;
					}
				}
				else
				{
					bool safeUpLeft = false;
					for (int radius = 0; radius < chessBoard.Columns; radius++)
					{
						ChessBoardPosition upLeftPosition = chessBoard.CalculatePosition(oldPosition, radius, radius, true);
						if (upLeftPosition != null)
						{
							ChessBoardCell cell = chessBoard.GetCell(upLeftPosition);
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Human)
							{
								safeUpLeft = false;
							}
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Ai)
							{
								safeUpLeft = true;
							}
						}
					}

					bool safeUpRight = false;
					for (int radius = 0; radius < chessBoard.Columns; radius++)
					{
						ChessBoardPosition upRightPosition = chessBoard.CalculatePosition(oldPosition, radius, -radius, true);
						if (upRightPosition != null)
						{
							ChessBoardCell cell = chessBoard.GetCell(upRightPosition);
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Human)
							{
								safeUpRight = false;
							}
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Ai)
							{
								safeUpRight = true;
							}
						}
					}

					bool safeDownLeft = false;
					for (int radius = 0; radius < chessBoard.Columns; radius++)
					{
						ChessBoardPosition downLeftPosition = chessBoard.CalculatePosition(oldPosition, -radius, radius, true);
						if (downLeftPosition != null)
						{
							ChessBoardCell cell = chessBoard.GetCell(downLeftPosition);
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Human)
							{
								safeDownLeft = false;
							}
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Ai)
							{
								safeDownLeft = true;
							}
						}
					}

					bool safeDownRight = false;
					for (int radius = 0; radius < chessBoard.Columns; radius++)
					{
						ChessBoardPosition downRightPosition = chessBoard.CalculatePosition(oldPosition, -radius, -radius, true);
						if (downRightPosition != null)
						{
							ChessBoardCell cell = chessBoard.GetCell(downRightPosition);
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Human)
							{
								safeDownRight = false;
							}
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Ai)
							{
								safeDownRight = true;
							}
						}
					}

					bool safeUp = false;
					for (int radius = 0; radius < chessBoard.Columns; radius++)
					{
						ChessBoardPosition upPosition = chessBoard.CalculatePosition(oldPosition, radius, 0, true);
						if (upPosition != null)
						{
							ChessBoardCell cell = chessBoard.GetCell(upPosition);
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Human)
							{
								safeUp = false;
							}
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Ai)
							{
								safeUp = true;
							}
						}
					}

					bool safeDown = false;
					for (int radius = 0; radius < chessBoard.Columns; radius++)
					{
						ChessBoardPosition downPosition = chessBoard.CalculatePosition(oldPosition, -radius, 0, true);
						if (downPosition != null)
						{
							ChessBoardCell cell = chessBoard.GetCell(downPosition);
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Human)
							{
								safeDown = false;
							}
							if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Ai)
							{
								safeDown = true;
							}
						}
					}

					if (safeUpLeft && safeUpRight && safeDownLeft && safeDownRight)
					{
						moveState = CommandUnitMoveState.StayStill;
					}

					if (!safeUpLeft || !safeDownLeft || !safeUp)
					{
						moveState = CommandUnitMoveState.MoveRight;
					}

					if (!safeUpRight || !safeDownRight || !safeDown)
					{
						moveState = CommandUnitMoveState.MoveLeft;
					}
				}
			}

			if (moveState == CommandUnitMoveState.MoveLeft)
			{
				newPosition = new ChessBoardPosition
				{
					CurrentColumn = oldPosition.CurrentColumn - 1,
					CurrentRow = oldPosition.CurrentRow,
				};
			}
			else if (moveState == CommandUnitMoveState.MoveRight)
			{
				newPosition = new ChessBoardPosition
				{
					CurrentColumn = oldPosition.CurrentColumn + 1,
					CurrentRow = oldPosition.CurrentRow,
				};
			}
			else if (moveState == CommandUnitMoveState.StayStill)
			{
				return;
			}

			ChessBoardCell newCell = chessBoard.GetCell(newPosition);
			if (newCell.IsOccupied)
			{
				return;
			}
			else
			{
				ChessBoardCell cell = chessBoard.GetCell(oldPosition);
				chessBoard.SetCell(newPosition, cell);

				chessBoard.EmptyCell(oldPosition);
			}
		}
	}
}
