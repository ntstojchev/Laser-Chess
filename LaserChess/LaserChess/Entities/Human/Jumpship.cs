using LaserChess.ChessBoard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserChess.Entities.Human
{
	public class Jumpship : Entity
	{
		public const string _name = "Jumpship";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "J";
		public const int _hitPoints = 2;
		public const int _attackPower = 2;
		public const int _movementPoint = 0;
		public const int _attackRange = 1;
		public const EntityType _type = EntityType.Jumpship;
		public const EntityControlType _controlType = EntityControlType.Human;
		public const EntityMovementType _movementType = EntityMovementType.Orthogonally;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.Knight;
		public const EntityAttackType _attackType = EntityAttackType.Orthogonally;

		public Jumpship()
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

			foreach (ChessBoardPosition position in validPositions)
			{
				ChessBoardCell cell = chessBoard.GetCell(position);
				if (cell.IsOccupied && cell.Entity != null)
				{
					cell.Entity.HitPoints = cell.Entity.HitPoints - AttackPower;

					Console.WriteLine($"Hit on enemy {cell.Entity.Name} for {AttackPower} damage!");
					if (cell.Entity.HitPoints < 1)
					{
						Console.WriteLine($"Enemy {cell.Entity.Name} is destroyed!");
						chessBoard.EmptyCell(position);
					}

					Console.ReadLine();
				}
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

			ChessBoardPosition upLeftPosition = chessBoard.CalculatePosition(currentPosition, 2, 1);
			ChessBoardPosition upRightPosition = chessBoard.CalculatePosition(currentPosition, 2, -1);

			ChessBoardPosition downLeftPosition = chessBoard.CalculatePosition(currentPosition, -2, 1);
			ChessBoardPosition downRightPosition = chessBoard.CalculatePosition(currentPosition, -2, -1);

			ChessBoardPosition leftUpPosition = chessBoard.CalculatePosition(currentPosition, 1, 2);
			ChessBoardPosition leftDownPosition = chessBoard.CalculatePosition(currentPosition, -1, 2);

			ChessBoardPosition rightUpPosition = chessBoard.CalculatePosition(currentPosition, 1, -2);
			ChessBoardPosition rightDownPosition = chessBoard.CalculatePosition(currentPosition, -1, -2);

			validPositions.Add(upLeftPosition);
			validPositions.Add(upRightPosition);
			validPositions.Add(downLeftPosition);
			validPositions.Add(downRightPosition);
			validPositions.Add(leftUpPosition);
			validPositions.Add(leftDownPosition);
			validPositions.Add(rightUpPosition);
			validPositions.Add(rightDownPosition);

			return validPositions;
		}

		private List<ChessBoardPosition> GetValidAttackPositions(ChessBoard.ChessBoard chessBoard, ChessBoardPosition currentPosition)
		{
			var validPositions = new List<ChessBoardPosition>();

			ChessBoardPosition upPosition = chessBoard.CalculatePosition(currentPosition, 1, 0, true);
			ChessBoardPosition downPosition = chessBoard.CalculatePosition(currentPosition, -1, 0, true);
			ChessBoardPosition leftPosition = chessBoard.CalculatePosition(currentPosition, 0, 1, true);
			ChessBoardPosition rightPosition = chessBoard.CalculatePosition(currentPosition, 0, -1, true);

			validPositions.Add(upPosition);
			validPositions.Add(downPosition);
			validPositions.Add(leftPosition);
			validPositions.Add(rightPosition);

			return validPositions;
		}
	}
}
