using System;
using System.Collections.Generic;
using System.Drawing;

namespace MenschAergereDichNichtLogik
{
	public static class Logik
	{
		static Logik()
		{
			HouseEntrypoints.Add(Color.Green, (5, 10));
			HouseEntrypoints.Add(Color.Red, (10, 5));
			HouseEntrypoints.Add(Color.Black, (5, 0));
			HouseEntrypoints.Add(Color.Yellow, (0, 5));
		}
		private static Random randomnumber = new Random();
		public static int wuerfelzahl { get; private set; }
		private static bool GameStarted = false;
		private static Dictionary<Color, (int, int)> HouseEntrypoints = new Dictionary<Color, (int, int)>();

		#region Gamestart Information
		private static List<(int, int)> StandardBoard = new List<(int, int)>
		{
			(4, 10),
			(5, 10),
			(6, 10),

			(4, 9),
			(6, 9),

			(4, 8),
			(6, 8),

			(4, 7),
			(6, 7),

			(0, 6),
			(1, 6),
			(2, 6),
			(3, 6),
			(4, 6),
			(6, 6),
			(7, 6),
			(8, 6),
			(9, 6),
			(10, 6),

			(0, 5),
			(10, 5),

			(0, 4),
			(1, 4),
			(2, 4),
			(3, 4),
			(4, 4),
			(6, 4),
			(7, 4),
			(8, 4),
			(9, 4),
			(10, 4),

			(4, 3),
			(6, 3),

			(4, 2),
			(6, 2),

			(4, 1),
			(6, 1),

			(4, 0),
			(5, 0),
			(6, 0),
		};
		private static List<((int, int), int, Color)> FinishPoints = new List<((int, int), int, Color)>()
		{
			((5, 9), 0, Color.Green),
			((5, 8), 1, Color.Green),
			((5, 7), 2, Color.Green),
			((5, 6), 3, Color.Green),

			((1, 5), 0, Color.Red),
			((2, 5), 1, Color.Red),
			((3, 5), 2, Color.Red),
			((4, 5), 3, Color.Red),

			((6, 5), 3, Color.Yellow),
			((7, 5), 2, Color.Yellow),
			((8, 5), 1, Color.Yellow),
			((9, 5), 0, Color.Yellow),

			((5, 4), 3, Color.Black),
			((5, 3), 2, Color.Black),
			((5, 2), 1, Color.Black),
			((5, 1), 0, Color.Black)
		};
		#endregion

		public static Field[][] Board { get; private set; } = new Field[][]
		{
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11]
		};

		public static Player CurrentPlayer { get; private set; }

		private static Queue<Player> PlayerQueue = new Queue<Player>();

		public static void FieldClick(int X, int Y)
		{
			if (GameStarted)
			{
				if (Board[X][Y] is null)
				{
					return;
				}
				else
				{

				}
			}
		}

		public static void DiceKlick()
		{
			if (GameStarted)
			{


				if (wuerfelzahl == 0)
				{
					wuerfelzahl = randomnumber.Next(1, 7);
				}

			}
		}

		public static void HomeClick(Color HausColor)
		{

			if (GameStarted)
			{


				if (HausColor == CurrentPlayer.Color)
				{

				}
				else
				{
					return;
				}
			}
		}

		public static void GameStart(List<string> PlayerNames)
		{
			if (GameStarted == false)
			{
				if (PlayerNames.Count > 4 || PlayerNames.Count < 1)
				{
					throw new ArgumentOutOfRangeException("PlayerNames", PlayerNames, "Die Liste muss zwischen 1 und 4 Namen enthalten");
				}
				else if (PlayerNames is null)
				{
					throw new NullReferenceException("Die Liste darf nicht null sein");
				}
				else
				{





					foreach ((int, int) point in StandardBoard)
					{
						Board[point.Item1][point.Item2] = new Field();
					}

					foreach (((int, int), int, Color) Point in FinishPoints)
					{
						Board[Point.Item1.Item1][Point.Item1.Item2] = new FinishField(Point.Item3, Point.Item2);
					}

					Board[StandardBoard[0].Item1][StandardBoard[0].Item2].NextField = (StandardBoard[1].Item1, StandardBoard[1].Item2);
					NextFieldFinder(StandardBoard[1].Item1, StandardBoard[1].Item2);

					GameStarted = true;
				}
			}
		}

		private static void NextFieldFinder(int X, int Y)
		{
			if (TupleEqual(Board[X][Y].NextField, (-1, -1)))
			{
				if (X - 1 >= 0)
				{
					if (Board[X - 1][Y] != null && TupleEqual(Board[X - 1][Y].NextField, (X, Y)) == false && Board[X - 1][Y] is FinishField == false)
					{
						Board[X][Y].NextField = (X - 1, Y);
						NextFieldFinder(X - 1, Y);
						return;
					}
				}
				if (X + 1 <= 10)
				{
					if (Board[X + 1][Y] != null && TupleEqual(Board[X + 1][Y].NextField, (X, Y)) == false && Board[X + 1][Y] is FinishField == false)
					{
						Board[X][Y].NextField = (X + 1, Y);
						NextFieldFinder(X + 1, Y);
						return;

					}

				}
				if (Y - 1 >= 0)
				{
					if (Board[X][Y - 1] != null && TupleEqual(Board[X][Y -1 ].NextField, (X, Y)) == false && Board[X][Y - 1] is FinishField == false)
					{
						Board[X][Y].NextField = (X, Y - 1);
						NextFieldFinder(X, Y - 1);
						return;

					}

				}
				if (Y + 1 <= 10)
				{
					if (Board[X][Y + 1] != null && TupleEqual(Board[X][Y + 1].NextField, (X, Y)) == false && Board[X][Y + 1] is FinishField == false)
					{
						Board[X][Y].NextField = (X, Y + 1);
						NextFieldFinder(X, Y + 1);
						return;

					}

				}

				return;
			}
		}
		private static bool TupleEqual((int, int) tuple1, (int, int) tuple2)
		{
			return tuple1.Item1 == tuple2.Item1 && tuple1.Item2 == tuple2.Item2;
		}
	}
}