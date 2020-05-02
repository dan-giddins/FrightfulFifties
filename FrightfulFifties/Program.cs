using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FrightfulFifties
{
	internal class Program
	{
		private static void Main()
		{
			var allTiles = new List<Tile>
			{
				new Tile(22, 8),
				new Tile(9, 20),
				new Tile(10, 8),
				new Tile(7, 12),
				new Tile(18, 13),
				new Tile(5, 16),
				new Tile(17, 15),
				new Tile(19, 11),
				new Tile(20, 21),
				new Tile(19, 23),
				new Tile(4, 5),
				new Tile(14, 6),
				new Tile(17, 3),
				new Tile(6, 2),
				new Tile(1, 18),
				new Tile(24, 7)
			};
			GroupsOfFour(allTiles);
		}

		private static void GroupsOfFour(IList<Tile> allTiles)
		{
			Console.WriteLine("Getting all stateful tiles...");
			var allStatefullTiles = GetAllStatefulTiles(allTiles);
			Console.WriteLine("Got all stateful tiles.");

			Console.WriteLine("Getting rows...");
			var rows = new List<RowNode>();
			foreach (var statefulTileCombination in new Combinations<StatefulTile>(allStatefullTiles, 4))
			{
				if (ValidRow(statefulTileCombination))
				{
					var rowNode = new RowNode(statefulTileCombination);
					rows.Add(rowNode);
					foreach (var tile in rowNode.Row)
					{
						tile.RowNodes.Add(rowNode);
					}
				}
			}
			Console.WriteLine($"Got {rows.Count} rows.");

			//Console.WriteLine($"Computing uniqueness graph...");
			//foreach (var row in rows)
			//{
			//	foreach (var otherRow in rows)
			//	{
			//		if (row.Unique(otherRow))
			//		{
			//			row.UniqueEdges.Add(otherRow);
			//		}
			//	}
			//}
			//Console.WriteLine($"Computed uniqueness graph.");

			//PrintRowCount(allStatefullTiles);

			Console.WriteLine("Finding valid grids...");
			foreach (var rowA in rows)
			{
				var tileA = rowA.Row[0];
				var validColumnAs = tileA.RowNodes.Where(x =>
					x != rowA && !x.Row.Any(y =>
						y != tileA && rowA.Row.Any(z =>
							z != tileA && IsSameTile(y, z))));
				foreach (var columnA in validColumnAs)
				{
					var tileB = rowA.Row[1];
					var validColumnBs = tileB.RowNodes.Where(x =>
						x != rowA && !x.Row.Any(y =>
							y != tileB && rowA.Row.Any(z =>
								z != tileB && IsSameTile(y, z))
							|| columnA.Row.Any(z =>
								IsSameTile(y, z))));
					foreach (var columnB in validColumnBs)
					{
						var tileC = rowA.Row[2];
						var validColumnCs = tileC.RowNodes.Where(x =>
							x != rowA && !x.Row.Any(y =>
								y != tileC && rowA.Row.Any(z =>
									z != tileC && IsSameTile(y, z))
								|| columnA.Row.Any(z =>
									IsSameTile(y, z))
								|| columnB.Row.Any(z =>
									IsSameTile(y, z))));
						foreach (var columnC in validColumnCs)
						{
							var tileD = rowA.Row[3];
							var validColumnDs = tileD.RowNodes.Where(x =>
								x != rowA && !x.Row.Any(y =>
									y != tileD && rowA.Row.Any(z =>
										z != tileD && IsSameTile(y, z))
									|| columnA.Row.Any(z =>
										IsSameTile(y, z))
									|| columnB.Row.Any(z =>
										IsSameTile(y, z))
									|| columnC.Row.Any(z =>
										IsSameTile(y, z))));
							foreach (var columnD in validColumnDs)
							{
								// create new objects and remove row A from the colums
								var columnARowB = CopyRow(columnA);
								columnARowB.Row.Remove(tileA);
								var columnBRowB = CopyRow(columnB);
								columnBRowB.Row.Remove(tileB);
								var columnCRowB = CopyRow(columnC);
								columnCRowB.Row.Remove(tileC);
								var columnDRowB = CopyRow(columnD);
								columnDRowB.Row.Remove(tileD);
								var validRowBs = rows.Except(new List<RowNode>
								{ rowA, columnA, columnB, columnC, columnD })
									.Where(x =>
										columnARowB.Row.Any(y => x.Row.Contains(y))
										&& columnBRowB.Row.Any(y => x.Row.Contains(y))
										&& columnCRowB.Row.Any(y => x.Row.Contains(y))
										&& columnDRowB.Row.Any(y => x.Row.Contains(y)));
								foreach (var validRowB in validRowBs)
								{
									var columnARowC = CopyRow(columnARowB);
									var columnBRowC = CopyRow(columnBRowB);
									var columnCRowC = CopyRow(columnCRowB);
									var columnDRowC = CopyRow(columnDRowB);
									foreach (var tile in validRowB.Row)
									{
										columnARowC.Row.Remove(tile);
										columnBRowC.Row.Remove(tile);
										columnCRowC.Row.Remove(tile);
										columnDRowC.Row.Remove(tile);
									}
									var validRowCs = rows.Except(new List<RowNode>
									{ rowA, validRowB, columnA, columnB, columnC, columnD })
										.Where(x =>
										columnARowC.Row.Any(y => x.Row.Contains(y))
										&& columnBRowC.Row.Any(y => x.Row.Contains(y))
										&& columnCRowC.Row.Any(y => x.Row.Contains(y))
										&& columnDRowC.Row.Any(y => x.Row.Contains(y)));
									foreach (var validRowC in validRowCs)
									{
										var columnARowD = CopyRow(columnARowC);
										var columnBRowD = CopyRow(columnBRowC);
										var columnCRowD = CopyRow(columnCRowC);
										var columnDRowD = CopyRow(columnDRowC);
										foreach (var tile in validRowC.Row)
										{
											columnARowD.Row.Remove(tile);
											columnBRowD.Row.Remove(tile);
											columnCRowD.Row.Remove(tile);
											columnDRowD.Row.Remove(tile);
										}
										var validRowD = rows.Except(new List<RowNode>
										{ rowA, validRowB, validRowC, columnA, columnB, columnC, columnD })
											.SingleOrDefault(x =>
												columnARowD.Row.Any(y => x.Row.Contains(y))
												&& columnBRowD.Row.Any(y => x.Row.Contains(y))
												&& columnCRowD.Row.Any(y => x.Row.Contains(y))
												&& columnDRowD.Row.Any(y => x.Row.Contains(y)));
										if (validRowD != null)
										{
											//PrintBoard(new List<RowNode> { rowA, validRowB, validRowC, validRowD });
											// create and organise board as 2D array
											var possibleDiagonalAs = rows.Except(new List<RowNode>
										{ rowA, validRowB, validRowC, validRowD, columnA, columnB, columnC, columnD });
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private static RowNode CopyRow(RowNode columnA) =>
			new RowNode(columnA.Row.ToList());
		private static bool IsSameTile(StatefulTile a, StatefulTile b) =>
			a == b || a == b.StatefulTileTwin;

		private static void PrintRowCount(IList<StatefulTile> allStatefullTiles)
		{
			foreach (var tile in allStatefullTiles)
			{
				PrintFace(tile, 0);
				Console.Write("| ");
				PrintFace(tile, 1);
				Console.WriteLine($": {tile.RowNodes.Count} Rows");
			}
		}

		private static IList<StatefulTile> GetAllStatefulTiles(IList<Tile> allTiles)
		{
			var allStatefulTiles = new List<StatefulTile>();
			foreach (var tile in allTiles)
			{
				var a = new StatefulTile(tile, new List<int> { 0, 1 });
				var b = new StatefulTile(tile, new List<int> { 1, 0 });
				a.StatefulTileTwin = b;
				b.StatefulTileTwin = a;
				allStatefulTiles.Add(a);
				allStatefulTiles.Add(b);
			}
			return allStatefulTiles;
		}

		private static void PrintBoard(IList<RowNode> fourGroupCombination)
		{
			foreach (var fourGroup in fourGroupCombination)
			{
				PrintRow(fourGroup, 0);
				Console.Write("| ");
				PrintRow(fourGroup, 1);
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		private static bool CheckUniqueTiles(IList<IList<IList<int>>> fourGroupCombination)
		{
			foreach (var fourGroup in fourGroupCombination)
			{
				foreach (var tile in fourGroup)
				{
					var tempFourGroupCombination = fourGroupCombination.ToList();
					tempFourGroupCombination.Remove(fourGroup);
					foreach (var tempFourGroup in tempFourGroupCombination)
					{
						foreach (var tempTile in tempFourGroup)
						{
							if (tile[0] == tempTile[0] && tile[1] == tempTile[1]
								|| tile[0] == tempTile[1] && tile[1] == tempTile[0])
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		private static bool ValidRow(IList<StatefulTile> tileSet) =>
			tileSet.Any(x => tileSet.Any(y => x.StatefulTileTwin == y))
			? false
			: tileSet.Select(x => x.GetFaceValue(0)).Sum() == 50
				&& tileSet.Select(x => x.GetFaceValue(1)).Sum() == 50;

		private static void PrintGroup(RowNode rowNode)
		{
			PrintRow(rowNode, 0);
			Console.WriteLine();
			PrintRow(rowNode, 1);
			Console.WriteLine();
			Console.WriteLine();
		}

		private static void PrintRow(RowNode rowNode, int i)
		{
			foreach (var tile in rowNode.Row)
			{
				PrintFace(tile, i);
			}
		}

		private static void PrintFace(StatefulTile tile, int i) =>
			Console.Write(tile.GetFaceValue(i).ToString().PadRight(3));

		static IEnumerable<IList<T>> GetCombinations<T>(IList<T> list, int k) =>
			k == 1
			? list.Select(x => new List<T> { x })
			: list.SelectMany((e, i) =>
				GetCombinations(list.Skip(i + 1).ToList(), k - 1).Select(c => (new[] { e }).Concat(c).ToList()));

		private static int GetState(int i, int j) =>
			(i & (1 << j)) >> j;

		private static void PrintBruteBoard(IList<Tile> list)
		{
			for (var i = 0; i < list.Count() / 4; i++)
			{
				foreach (var value in list.Skip(i * 4).Take(4).Select(x => x.Faces.First()))
				{
					Console.Write(value.ToString().PadRight(3));
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		private static bool BruteCheck(IList<Tile> list)
		{
			for (var i = 0; i < list.Count() / 4; i++)
			{
				// rows
				if (list.Skip(i * 4).Take(4).Select(x => x.Faces.First()).Sum() != 50)
				{
					return false;
				}
				// columns
				var columnSum = 0;
				for (var j = i; j < list.Count() / 4; j++)
				{
					columnSum += list[j].Faces.First();
				}
				if (columnSum != 50)
				{
					return false;
				}
			}
			// diag
			var leftDiagSum = 0;
			for (var i = 0; i < list.Count(); i += 5)
			{
				leftDiagSum += list[i].Faces.First();
			}
			if (leftDiagSum != 50)
			{
				return false;
			}
			var rightDiagSum = 0;
			for (var i = 3; i < list.Count(); i += 3)
			{
				rightDiagSum += list[i].Faces.First();
			}
			return rightDiagSum == 50;
		}

		private static IEnumerable<IList<Tile>> GetPermutations(IList<Tile> list, int pointer, int max)
		{
			if (pointer == max)
			{
				yield return list;
			}
			else
			{
				for (var i = pointer; i <= max; i++)
				{
					Swap(list[pointer], list[i]);
					foreach (var result in GetPermutations(list, pointer + 1, max))
					{
						yield return result;
					}
					Swap(list[pointer], list[i]);
				}
			}
		}

		private static void Swap(Tile a, Tile b)
		{
			var temp = a;
			a = b;
			b = temp;
		}

		private static long GetFactoral(int n) =>
			Enumerable.Range(1, n).Aggregate(1L, (x, y) => x * y);
	}
}
