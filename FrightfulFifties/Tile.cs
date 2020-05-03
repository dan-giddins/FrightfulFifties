using System.Collections.Generic;

namespace FrightfulFifties
{
	internal class Tile
	{
		public Tile(int a, int b) =>
			Faces = new List<int> { a, b };

		public IList<int> Faces { get; }
	}
}
