using System;
using System.Collections.Generic;
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

		public Color Color { get; set; }
	}

	public class FinishField : Field
	{
		internal FinishField(Color FinishPointColor, int index)
		{
			this.FinishPointColor = FinishPointColor;
			this.index = index;
		}

		public int index { get; set; }
		public Color FinishPointColor { get; set; }
	}

}
