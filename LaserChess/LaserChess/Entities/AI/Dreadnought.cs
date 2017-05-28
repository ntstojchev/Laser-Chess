using LaserChess.ChessBoard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities.AI
{
	public class Dreadnought : Entity
	{
		public const string _name = "Dreadnought";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "N";
		public const int _hitPoints = 5;
		public const int _attackPower = 2;
		public const int _movementPoint = 1;
		public const int _attackRange = 1;
		public const EntityType _type = EntityType.Dreadnought;
		public const EntityControlType _controlType = EntityControlType.Ai;
		public const EntityMovementType _movementType = EntityMovementType.OmniDirection;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.None;
		public const EntityAttackType _attackType = EntityAttackType.OmniDirection;

		public Dreadnought()
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
			var validAttackPosition = new List<ChessBoardPosition>();

			ChessBoardPosition upLeftPosition = chessBoard.CalculatePosition(currentPosition, 1, 1, true);
			ChessBoardPosition upRight = chessBoard.CalculatePosition(currentPosition, 1, -1, true);
			ChessBoardPosition downLeftPosition = chessBoard.CalculatePosition(currentPosition, -1, 1, true);
			ChessBoardPosition downRightPosition = chessBoard.CalculatePosition(currentPosition, -1, -1, true);
			ChessBoardPosition upPosition = chessBoard.CalculatePosition(currentPosition, 1, 0, true);
			ChessBoardPosition downPosition = chessBoard.CalculatePosition(currentPosition, -1, 0, true);
			ChessBoardPosition leftPosition = chessBoard.CalculatePosition(currentPosition, 0, 1, true);
			ChessBoardPosition rightPosition = chessBoard.CalculatePosition(currentPosition, 0, -1, true);

			validAttackPosition.Add(upLeftPosition);
			validAttackPosition.Add(upRight);
			validAttackPosition.Add(downLeftPosition);
			validAttackPosition.Add(downRightPosition);
			validAttackPosition.Add(upPosition);
			validAttackPosition.Add(downPosition);
			validAttackPosition.Add(leftPosition);
			validAttackPosition.Add(rightPosition);

			validAttackPosition.RemoveAll(p => p == null);

			foreach (ChessBoardPosition position in validAttackPosition)
			{
				ChessBoardCell target = chessBoard.GetCell(position);
				if (target.IsOccupied
					&& target.Entity != null
					&& target.Entity.ControlType == EntityControlType.Human)
				{
					target.Entity.HitPoints = target.Entity.HitPoints - AttackPower;

					Console.WriteLine($"Hit on enemy {target.Entity.Name} for {AttackPower} damage!");
					if (target.Entity.HitPoints < 1)
					{
						Console.WriteLine($"Enemy {target.Entity.Name} is destroyed!");
						chessBoard.EmptyCell(position);
					}

					Console.ReadLine();
				}
			}
		}

		public override void Move(ChessBoard.ChessBoard chessBoard, ChessBoardPosition oldPosition, ChessBoardPosition newPosition)
		{
			List<PlayerPiece> playerPieces = chessBoard.GetPlayerPiecesBasedOnControlType(EntityControlType.Human);

			var validMoves = new List<KeyValuePair<int, List<Point>>>();
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
				validMoves.Add(new KeyValuePair<int, List<Point>>(nodes.Count, nodes));
			}

			var paths = new List<int>();
			foreach (KeyValuePair<int, List<Point>> pair in validMoves)
			{
				paths.Add(pair.Key);
			}

			paths.Sort();
			KeyValuePair<int, List<Point>> nextPosition = new KeyValuePair<int, List<Point>>(0, null);
			if (paths.Count > 0)
			{
				nextPosition = validMoves.Find(m => m.Key == paths[0]);
			}

			if (paths.Count > 0 && nextPosition.Key != 0)
			{
				newPosition = new ChessBoardPosition
				{
					CurrentColumn = nextPosition.Value[0].Y,
					CurrentRow = nextPosition.Value[0].X,
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
	}
}
