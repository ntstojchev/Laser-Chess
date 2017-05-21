using System;

namespace LaserChess.Entities
{
	[Flags]
	public enum EntityMovementType
	{
		None = 0,
		Orthogonally = 1,
		Diagonally = 2,
		OmniDirection = Orthogonally | Diagonally,
	}
}
