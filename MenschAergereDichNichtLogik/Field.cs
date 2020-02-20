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
}
