using System.Collections.Generic;

namespace FrightfulFifties
{
	internal class StatefulTile
	{
		public StatefulTile(Tile tile, IList<int> state)
		{
			Tile = tile;
			State = state;
			RowNodes = new List<RowNode>();
		}

		public Tile Tile { get; }
		public IList<int> State { get; }
		public StatefulTile StatefulTileTwin { get; set; }
		public IList<RowNode> RowNodes { get; }

		public int GetFaceValue(int i) =>
			Tile.Faces[State[i]];
	}
}