using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MenschAergereDichNichtLogik
{
	public class Player 
	{ 

		internal Player(string Name, Color Color)
		{
			this.Name = Name;
			this.Color = Color;
			_numberHome = 4;
		}


		public Color Color { get; internal set; }
		public string Name { get; internal set; }

		private int _numberHome;
		public int NumberHome
		{
			get
			{
				return _numberHome;
			}
			internal set
			{
				if (value != _numberHome)
				{
					_numberHome = value;
				}
			}
		}
	}
}
