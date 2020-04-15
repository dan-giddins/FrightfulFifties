using System;
using System.Collections.Generic;
using System.Text;

namespace FrightfulFifties
{
	internal class RowNode
	{
		public RowNode(IList<StatefulTile> statefulTileSet)
		{
			Row = statefulTileSet;
			UniqueEdges = new List<RowNode>();
		}

		public IList<StatefulTile> Row { get; set; }
		public IList<RowNode> UniqueEdges { get; set; }

		internal bool Unique(RowNode otherRow)
		{
			if (this == otherRow)
			{
				return false;
			}
			foreach (var tile in Row)
			{
				foreach (var otherTile in otherRow.Row)
				{
					if (tile.Tile == otherTile.Tile)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
