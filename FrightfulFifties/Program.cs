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
			var allTiles = new List<List<int>>
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
			BruteForce(allTiles);
		}

		private static void BruteForce(IList<List<int>> allTiles)
		{
			for (var i = 0; i < Math.Pow(2, 16); i++)
			{
				var tileSet = new int[allTiles.Count];
				for (var j = 0; j < tileSet.Length; j++)
				{
					tileSet[j] = allTiles[j][(i & (1 << j)) >> j];
				}
				foreach (var permutation in GetPermutations(tileSet, 0, tileSet.Length - 1))
				{
					if (Check(permutation))
					{
						Console.WriteLine("Found a Solution!");
						PrintBoard(permutation);
					}
				}
			}
		}

		private static void PrintBoard(IList<int> list)
		{
			for (var i = 0; i < list.Count() / 4; i++)
			{
				list.Skip(i * 4).Take(4).ToList().ForEach(delegate (int value)
				{
					Console.Write(value.ToString().PadRight(3));
				});
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		private static bool Check(IList<int> list)
		{
			for (var i = 0; i < list.Count() / 4; i++)
			{
				// rows
				if (list.Skip(i * 4).Take(4).Sum() != 50)
				{
					return false;
				}
				// columns
				var columnSum = 0;
				for (var j = i; j < list.Count() / 4; j++)
				{
					columnSum += list[j];
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
				leftDiagSum += list[i];
			}
			if (leftDiagSum != 50)
			{
				return false;
			}
			var rightDiagSum = 0;
			for (var i = 3; i < list.Count(); i += 3)
			{
				rightDiagSum += list[i];
			}
			return rightDiagSum == 50;
		}

		private static IEnumerable<IList<int>> GetPermutations(int[] list, int pointer, int max)
		{
			if (pointer == max)
			{
				yield return list;
			}
			else
			{
				for (var i = pointer; i <= max; i++)
				{
					Swap(ref list[pointer], ref list[i]);
					foreach (var result in GetPermutations(list, pointer + 1, max))
					{
						yield return result;
					}
					Swap(ref list[pointer], ref list[i]);
				}
			}
		}

		private static void Swap(ref int a, ref int b)
		{
			var temp = a;
			a = b;
			b = temp;
		}

		private static long GetFactoral(int n) =>
			Enumerable.Range(1, n).Aggregate(1L, (x, y) => x * y);
	}
}
