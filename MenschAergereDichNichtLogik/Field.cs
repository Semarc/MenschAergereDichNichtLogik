using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MenschAergereDichNichtLogik
{
	public enum Color
	{
		Red,
		Black,
		Yellow,
		Green,
		Empty
	}

	public class Field
	{
		internal Field()
		{

		}

		public Color Color { get; internal set; }
		public (int, int) NextField { get; internal set; } = (-1, -1);

		internal bool IsUrsprung { get; set; } = false;
		public bool IsAusgewaehlt { get; internal set; } = false;
	}

	public class FinishField : Field
	{
		internal FinishField(Color FinishPointColor, int index) : base()
		{
			this.FinishPointColor = FinishPointColor;
			this.index = index;
		}

		public int index { get; internal set; }
		public Color FinishPointColor { get; internal set; }
	}

}
