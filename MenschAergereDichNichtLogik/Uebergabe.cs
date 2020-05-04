using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace MenschAergereDichNichtLogik
{
	public static class Uebergabe
	{
		/// <summary>
		/// Enthält alle Spielstein, die verändert worden sind
		/// </summary>
		public static List<Point> GeaenderteSpielpunkte { get; set; } = new List<Point>();
		/// <summary>
		/// Gibt an, ob die Anzahl der Steine in einem der Starthäuser verändert worden sind
		/// </summary>
		public static bool Starthauserveraendert { get; set; }

		public static Color PlayerHasWon
		{
			get
			{
				for (int i = 0; i < 4; i++)
				{
					Color color = (Color)i + 1;
					Point point = Logik.HouseEndPointsDictionary[color];
					bool HasWon = true;


					FinishField current = (FinishField)Logik.Board[point.X][point.Y];
					while (current != null)
					{
						current = (FinishField)Logik.Board[current.NextField.X][current.NextField.Y];
						if (current.FinishPointColor == current.Color)
						{
							continue;
						}
						else
						{
							HasWon = false;
						}
					}
					if (HasWon)
					{
						return color;
					}
				}
				return Color.Empty;

			}
		}
	}
}
