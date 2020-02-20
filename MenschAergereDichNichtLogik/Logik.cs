using System;
using System.Collections.Generic;
using System.Drawing;

namespace MenschAergereDichNichtLogik
{
	public static class Logik
	{
        private static  Random randomnumber = new Random();
        public static  int wuerfelzahl { get; private set; }

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

		public static Player CurrentPlayer { get; private set}

		private static Queue<Player> PlayerQueue = new Queue<Player>();

		public static void FieldClick(int X, int Y)
		{

		}

		public static void DiceKlick()
		{
          wuerfelzahl =  randomnumber.Next(1, 6);
            


		}

		public static void HomeClick()
		{

		}

		public static void GameStart()
		{

			foreach (Point point in StandardBoard)
			{
				Board[point.X][point.Y] = new Field(); 
			}
			


		}




	}
}
