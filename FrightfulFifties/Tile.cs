using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace FrightfulFifties
{
	internal class Tile
	{
		public Tile(int a, int b) =>
			Faces = new List<int> { a, b };

		public IList<int> Faces { get; }
	}
}
