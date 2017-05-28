using LaserChess.ChessBoard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserChess.Entities.Human
{
	public class Grunt : Entity
	{
		public const string _name = "Grunt";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "G";
		public const int _hitPoints = 2;
		public const int _attackPower = 1;
		public const int _movementPoint = 1;
		public const int _attackRange = 0;
		public const EntityType _type = EntityType.Grunt;
		public const EntityControlType _controlType = EntityControlType.Human;
		public const EntityMovementType _movementType = EntityMovementType.Orthogonally;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.None;
		public const EntityAttackType _attackType = EntityAttackType.Diagonally;

		public Grunt()
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
			var validPositions = GetValidAttackPositions(chessBoard, currentPosition);
			validPositions.RemoveAll(p => p == null);

			if (targetPosition == null)
			{
				throw new Exception("The specified cell is outside the chess board field.");
			}

			if (validPositions.Any(p => p.CurrentRow == targetPosition.CurrentRow
								&& p.CurrentColumn == targetPosition.CurrentColumn))
			{
				ChessBoardCell cell = chessBoard.GetCell(targetPosition);
				if (cell.IsOccupied && cell.Entity != null)
				{
					cell.Entity.HitPoints = cell.Entity.HitPoints - AttackPower;

					Console.WriteLine($"Hit on enemy {cell.Entity.Name} for {AttackPower} damage!");
					if (cell.Entity.HitPoints < 1)
					{
						Console.WriteLine($"Enemy {cell.Entity.Name} is destroyed!");
						chessBoard.EmptyCell(targetPosition);
					}

					Console.ReadLine();
				}
			}
			else
			{
				throw new Exception("Invalid attack position. Specified cell is not in the attack range.");
			}
		}

		public override void Move(ChessBoard.ChessBoard chessBoard, ChessBoardPosition oldPosition, ChessBoardPosition newPosition)
		{
			var validPositions = GetValidMovePositions(chessBoard, oldPosition);
			validPositions.RemoveAll(p => p == null);

			if (newPosition == null)
			{
				throw new Exception("The specified cell is outside the chess board field.");
			}

			if (validPositions.Any(p => p.CurrentRow == newPosition.CurrentRow 
								&& p.CurrentColumn == newPosition.CurrentColumn))
			{
				ChessBoardCell cell = chessBoard.GetCell(oldPosition);
				chessBoard.SetCell(newPosition, cell);

				chessBoard.EmptyCell(oldPosition);
			}
			else
			{
				throw new Exception("Invalid move. Specified cell is not a valid move.");
			}
		}

		private List<ChessBoardPosition> GetValidMovePositions(ChessBoard.ChessBoard chessBoard, ChessBoardPosition currentPosition)
		{
			var validPositions = new List<ChessBoardPosition>();

			ChessBoardPosition upPosition = chessBoard.CalculatePosition(currentPosition, MovementPoint, 0);
			ChessBoardPosition downPosition = chessBoard.CalculatePosition(currentPosition, -MovementPoint, 0);
			ChessBoardPosition leftPosition = chessBoard.CalculatePosition(currentPosition, 0, MovementPoint);
			ChessBoardPosition rightPosition = chessBoard.CalculatePosition(currentPosition, 0, -MovementPoint);

			validPositions.Add(upPosition);
			validPositions.Add(downPosition);
			validPositions.Add(leftPosition);
			validPositions.Add(rightPosition);

			return validPositions;
		}

		private List<ChessBoardPosition> GetValidAttackPositions(ChessBoard.ChessBoard chessBoard, ChessBoardPosition currentPosition)
		{
			var validPositions = new List<ChessBoardPosition>();

			for (int diagonalCorner = 1; diagonalCorner < chessBoard.Rows; diagonalCorner++)
			{
				ChessBoardPosition upLeftPosition = chessBoard.CalculatePosition(currentPosition, diagonalCorner, diagonalCorner, true);
				ChessBoardPosition upRight = chessBoard.CalculatePosition(currentPosition, diagonalCorner, -diagonalCorner, true);
				ChessBoardPosition downLeftPosition = chessBoard.CalculatePosition(currentPosition, -diagonalCorner, diagonalCorner, true);
				ChessBoardPosition downRightPosition = chessBoard.CalculatePosition(currentPosition, -diagonalCorner, -diagonalCorner, true);

				validPositions.Add(upLeftPosition);
				validPositions.Add(upRight);
				validPositions.Add(downLeftPosition);
				validPositions.Add(downRightPosition);
			}

			return validPositions;
		}
	}
}
