using System;
using System.Collections.Generic;
using System.Text;

namespace MenschAergereDichNichtLogik
{
	public enum Farbe
	{
		Rot,
		Schwarz,
		Gelb,
		Gruen,
		Leer
	}

	public class Field
	{
		internal Field()
		{

		}

		public Farbe Farbe { get; set; }
	}
}
