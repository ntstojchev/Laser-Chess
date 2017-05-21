using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.ChessBoard
{
	public class ChessBoard
	{
		private const string _evenCellSymbol = "///";
		private const string _oddCellSymbol = "|||";

		private ChessBoardCell[] _chessBoardCells;

		public int Rows { get; set; }

		public int Columns { get; set; }

		public string EvenCellSymbol
		{
			get { return _evenCellSymbol; }
		}

		public string OddCellSymbol
		{
			get { return _oddCellSymbol; }
		}

		public ChessBoardCell[] ChessBoardCells
		{
			get { return _chessBoardCells; }
		}

		/// <summary>
		/// 
		/// </summary>
		public void Draw()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		public void LoadLevel(string levelPath)
		{
			throw new NotImplementedException();
		}
	}
}
