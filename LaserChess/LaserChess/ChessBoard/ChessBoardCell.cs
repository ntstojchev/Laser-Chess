using LaserChess.Entities;

namespace LaserChess.ChessBoard
{
	public class ChessBoardCell
	{
		public bool IsOccupied { get; set; }

		public Entity Entity {get; set;}

		public override string ToString()
		{
			return $" {Entity.ToString()} ";
		}
	}
}
