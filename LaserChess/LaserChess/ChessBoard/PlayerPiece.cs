using System;

namespace LaserChess.ChessBoard
{
	public class PlayerPiece
	{
		public Guid EntityID { get; set; }

		public ChessBoardPosition CurrentPosition { get; set; }
	}
}
