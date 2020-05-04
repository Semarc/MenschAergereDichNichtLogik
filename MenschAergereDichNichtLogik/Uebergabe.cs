using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MenschAergereDichNichtLogik
{
	public static class Uebergabe
	{
		public static Color PlayerHasWon
		{
			get
			{


				for (int i = 1; i < 5; i++)
				{
					bool HatGewonnen = true;
					foreach (var item in Logik.FinishPoints)
					{
						if((Logik.Board[item.Item1.X][item.Item1.Y].Color == (Color)i) == false)
						{
							HatGewonnen = false;
						}
					}
					if (HatGewonnen)
					{
						return (Color)i;
					}
				}
				return Color.Empty;


				//for (int i = 0; i < Logik.PlayerList.Count; i++)
				//{
				//	Color color = (Color)i + 1;
				//	Point point = Logik.HouseEndPointsDictionary[color];
				//	bool HasWon = true;


				//	FinishField current = (FinishField)Logik.Board[point.X][point.Y];
				//	while (current != null)
				//	{
				//		if (current.FinishPointColor == current.Color)
				//		{
				//			current = null;
				//		}
				//		else
				//		{
				//			HasWon = false;
				//			current = current.NextField != new Point(-1, -1) ? (FinishField)Logik.Board[current.NextField.X][current.NextField.Y] : null;
				//		}
				//	}
				//	if (HasWon)
				//	{
				//		return color;
				//	}
				//}
				//return Color.Empty;

			}
		}
	}
}
