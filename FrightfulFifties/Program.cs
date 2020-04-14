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
			var allTiles = new List<IList<int>>
			{
				new List<int> {22, 8},
				new List<int> {9, 20},
				new List<int> {10, 8},
				new List<int> {7, 12},
				new List<int> {18, 13},
				new List<int> {5, 16},
				new List<int> {17, 15},
				new List<int> {19, 11},
				new List<int> {20, 21},
				new List<int> {19, 23},
				new List<int> {4, 5},
				new List<int> {14, 6},
				new List<int> {17, 3},
				new List<int> {6, 2},
				new List<int> {1, 18},
				new List<int> {24, 7}
			};
			//BruteForce(allTiles);
			GroupsOfFour(allTiles);
		}

		private static void GroupsOfFour(IList<IList<int>> allTiles)
		{
			var fourGroups = new List<IList<IList<int>>>();
			foreach (var rowCombination in GetCombinations(allTiles, 4))
			{
				for (var i = 0; i < Math.Pow(2, 4); i++)
				{
					var tileSet = GetTileSet(rowCombination, i);
					if (FourCheck(tileSet))
					{
						fourGroups.Add(tileSet);
					}
				}
			}
			foreach (var fourGroupCombination in GetCombinations(fourGroups, 4))
			{
				if (CheckUniqueTiles(fourGroupCombination))
				{
					PrintBoard(fourGroupCombination);
				}
			}
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

		private static bool FourCheck(IList<IList<int>> rowCombination) =>
			rowCombination.Select(x => x[0]).Sum() == 50 && rowCombination.Select(x => x[1]).Sum() == 50;

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

		private static void BruteForce(IList<IList<int>> allTiles)
		{
			for (var i = 0; i < Math.Pow(2, 16); i++)
			{
				var tileSet = GetTileSet(allTiles, i);
				foreach (var permutation in GetPermutations(tileSet, 0, tileSet.Count - 1))
				{
					if (BruteCheck(permutation))
					{
						Console.WriteLine("Found a Solution!");
						PrintBruteBoard(permutation);
					}
				}
			}
		}

		private static IList<IList<int>> GetTileSet(IList<IList<int>> allTiles, int i)
		{
			var tileSet = new List<IList<int>>();
			for (var j = 0; j < allTiles.Count; j++)
			{
				tileSet.Add(new List<int> { allTiles[j][(i & (1 << j)) >> j], allTiles[j][(~i & (1 << j)) >> j] });
			}
			return tileSet;
		}

		private static void PrintBruteBoard(IList<IList<int>> list)
		{
			for (var i = 0; i < list.Count() / 4; i++)
			{
				foreach (var value in list.Skip(i * 4).Take(4).Select(x => x.First()))
				{
					Console.Write(value.ToString().PadRight(3));
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		private static bool BruteCheck(IList<IList<int>> list)
		{
			for (var i = 0; i < list.Count() / 4; i++)
			{
				// rows
				if (list.Skip(i * 4).Take(4).Select(x => x.First()).Sum() != 50)
				{
					return false;
				}
				// columns
				var columnSum = 0;
				for (var j = i; j < list.Count() / 4; j++)
				{
					columnSum += list[j].First();
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
				leftDiagSum += list[i].First();
			}
			if (leftDiagSum != 50)
			{
				return false;
			}
			var rightDiagSum = 0;
			for (var i = 3; i < list.Count(); i += 3)
			{
				rightDiagSum += list[i].First();
			}
			return rightDiagSum == 50;
		}

		private static IEnumerable<IList<IList<int>>> GetPermutations(IList<IList<int>> list, int pointer, int max)
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

		private static void Swap(IList<int> a, IList<int> b)
		{
			var temp = a;
			a = b;
			b = temp;
		}

		private static long GetFactoral(int n) =>
			Enumerable.Range(1, n).Aggregate(1L, (x, y) => x * y);
	}
}
