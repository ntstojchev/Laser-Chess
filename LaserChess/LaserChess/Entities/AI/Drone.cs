using LaserChess.ChessBoard;
using System;
using System.Collections.Generic;

namespace LaserChess.Entities.AI
{
	public class Drone : Entity
	{
		public const string _name = "Drone";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "D";
		public const int _hitPoints = 2;
		public const int _attackPower = 1;
		public const int _movementPoint = 1;
		public const int _attackRange = 0;
		public const EntityType _type = EntityType.Drone;
		public const EntityControlType _controlType = EntityControlType.Ai;
		public const EntityMovementType _movementType = EntityMovementType.Orthogonally;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.Pawn;
		public const EntityAttackType _attackType = EntityAttackType.Diagonally;

		public Drone()
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

			var validTargets = new List<ChessBoardPosition>();
			foreach (ChessBoardPosition validPosition in validPositions)
			{
				ChessBoardCell cell = chessBoard.GetCell(validPosition);

				if (cell.IsOccupied && cell.Entity.ControlType == EntityControlType.Human)
				{
					validTargets.Add(validPosition);
				}
			}

			if (validTargets.Count > 0)
			{
				validTargets.Shuffle();

				ChessBoardCell target = chessBoard.GetCell(validTargets[0]);
				if (target.IsOccupied && target.Entity != null)
				{
					target.Entity.HitPoints = target.Entity.HitPoints - AttackPower;

					Console.WriteLine($"Hit on enemy {target.Entity.Name}!");
					if (target.Entity.HitPoints < 1)
					{
						Console.WriteLine($"Enemy {target.Entity.Name} is destroyed!");
						chessBoard.EmptyCell(validTargets[0]);
					}

					Console.ReadLine();
				}
			}
		}

		public override void Move(ChessBoard.ChessBoard chessBoard, ChessBoardPosition oldPosition, ChessBoardPosition newPosition)
		{
			if ((oldPosition.CurrentRow + 1) < 8)
			{
				newPosition = new ChessBoardPosition
				{
					CurrentColumn = oldPosition.CurrentColumn,
					CurrentRow = oldPosition.CurrentRow + 1,
				};

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
