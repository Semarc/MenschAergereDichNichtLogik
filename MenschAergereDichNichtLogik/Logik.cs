using System;
using System.Collections.Generic;
using System.Drawing;

namespace MenschAergereDichNichtLogik
{
	public static class Logik
	{
		private static Random randomnumber = new Random();
		public static int wuerfelzahl { get; private set; }
		private static bool GameStarted = false;

		private static List<Point> StandardBoard = new List<Point>
		{
			new Point(4, 10),
			new Point(5, 10),
			new Point(6, 10),

			new Point(4, 9),
			new Point(6, 9),

			new Point(4, 8),
			new Point(6, 8),

			new Point(4, 7),
			new Point(6, 7),

			new Point(0, 6),
			new Point(1, 6),
			new Point(2, 6),
			new Point(3, 6),
			new Point(4, 6),
			new Point(6, 6),
			new Point(7, 6),
			new Point(8, 6),
			new Point(9, 6),
			new Point(10, 6),

			new Point(0, 5),
			new Point(10, 5),

			new Point(0, 4),
			new Point(1, 4),
			new Point(2, 4),
			new Point(3, 4),
			new Point(4, 4),
			new Point(6, 4),
			new Point(7, 4),
			new Point(8, 4),
			new Point(9, 4),
			new Point(10, 4),

			new Point(4, 3),
			new Point(6, 3),

			new Point(4, 2),
			new Point(6, 2),

			new Point(4, 1),
			new Point(6, 1),

			new Point(4, 0),
			new Point(5, 0),
			new Point(6, 0),
		};

		private static List<(Point, int, Color)> FinishPoints = new List<(Point, int, Color)>()
		{
			(new Point(5, 9), 0, Color.Green),
			(new Point(5, 8), 1, Color.Green),
			(new Point(5, 7), 2, Color.Green),
			(new Point(5, 6), 3, Color.Green),

			(new Point(1, 5), 0, Color.Red),
			(new Point(2, 5), 1, Color.Red),
			(new Point(3, 5), 2, Color.Red),
			(new Point(4, 5), 3, Color.Red),

			(new Point(6, 5), 3, Color.Yellow),
			(new Point(7, 5), 2, Color.Yellow),
			(new Point(8, 5), 1, Color.Yellow),
			(new Point(9, 5), 0, Color.Yellow),

			(new Point(5, 4), 3, Color.Black),
			(new Point(5, 3), 2, Color.Black),
			(new Point(5, 2), 1, Color.Black),
			(new Point(5, 1), 0, Color.Black)
		};

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
				if (PlayerNames.Count > 4 || PlayerNames.Count < 1 || PlayerNames is null)
				{
					throw new ArgumentOutOfRangeException("PlayerNames", PlayerNames, "Die Liste muss zwischen 1 und 4 Namen enthalten");
				}





				foreach (Point point in StandardBoard)
				{
					Board[point.X][point.Y] = new Field();
				}

				foreach ((Point, int, Color) Point in FinishPoints)
				{
					Board[Point.Item1.X][Point.Item1.Y] = new FinishField(Point.Item3, Point.Item2);
				}


				GameStarted = true;
			}
		}




	}
}
