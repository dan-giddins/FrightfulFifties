using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

		private static void PrintBoard(IList<IList<IList<int>>> fourGroupCombination)
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

		private static void PrintGroup(IList<IList<int>> rowCombination)
		{
			PrintRow(rowCombination, 0);
			Console.WriteLine();
			PrintRow(rowCombination, 1);
			Console.WriteLine();
			Console.WriteLine();
		}

		private static void PrintRow(IList<IList<int>> rowCombination, int i)
		{
			foreach (var tile in rowCombination)
			{
				Console.Write(tile[i].ToString().PadRight(3));
			}
		}

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
