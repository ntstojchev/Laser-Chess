using LaserChess.ChessBoard;
using System;

namespace LaserChess.Entities
{
	public abstract class Entity
	{
		public string Name { get; set; }

		public Guid ID { get; set; }

		public string Icon { get; set; }

		public int HitPoints { get; set; }

		public int AttackPower { get; set; }

		public int MovementPoint { get; set; }

		public int AttackRange { get; set; }

		public EntityType Type { get; set; }

		public EntityControlType ControlType { get; set; }

		public EntityMovementType MovementType { get; set; }

		public EntitySpecialMovementPattern MovementPattern { get; set; }

		public EntityAttackType AttackType { get; set; }

		public virtual void Attack(ChessBoard.ChessBoard chessBoard, ChessBoardPosition currentPosition, ChessBoardPosition targetPosition)
		{
			throw new NotImplementedException();
		}

		public virtual void Move(ChessBoard.ChessBoard chessBoard, ChessBoardPosition oldPosition, ChessBoardPosition newPosition)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return $"{Icon}♥{HitPoints}";
		}
	}
}
