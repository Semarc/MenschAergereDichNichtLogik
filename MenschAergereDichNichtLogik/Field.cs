using System;
using System.Runtime.CompilerServices;

namespace MenschAergereDichNichtLogik
{
	public enum Color
	{
		Empty = 0,
		Red = 1,
		Black = 2,
		Yellow = 3,
		Green = 4
	}

	public static class HelperClass
	{
		public static string ColorToProperString(this Color color)
		{
			return null;
		}
	}



	public struct Point : IEquatable<Point>
	{
		public Point(int intX, int intY)
		{
			InternalX = intX;
			InternalY = intY;
		}

		//Sicherstellen, dass der Typ Immulatable ist
		private readonly int InternalX;
		private readonly int InternalY;
		public int X { get { return InternalX; } }
		public int Y { get { return InternalY; } }


		public override bool Equals(object obj)
		{
			// If parameter cannot be cast to Point return false.
			//Because Point is a Struct, Nullchecking is included
			if(obj is Point p)
			{
				return Equals(p);

			}
			else
			{
				return false;
			}
		}

		public bool Equals(Point p)
		{
			// Return true if the fields match:
			return (X == p.X) && (Y == p.Y);
		}

		public override int GetHashCode()
		{
			int hashCode = 1861411795;
			hashCode = hashCode * -1521134295 + X.GetHashCode();
			hashCode = hashCode * -1521134295 + Y.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(Point left, Point right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right)
		{
			return !(left == right);
		}

	}

	public class Field
	{
		internal Field()
		{

		}

		public Color Color { get; internal set; }
		public Point NextField { get; internal set; } = new Point(-1, -1);

		internal bool IsUrsprung { get; set; } = false;
		public bool IsAusgewaehlt { get; internal set; } = false;
	}

	public class FinishField : Field
	{
		internal FinishField(Color FinishPointColor, int Index) : base()
		{
			this.FinishPointColor = FinishPointColor;
			this.Index = Index;
		}

		public int Index { get; internal set; }
		public Color FinishPointColor { get; internal set; }
	}

}
