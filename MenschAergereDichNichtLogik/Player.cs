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
			_numberDiceRolls = 0;
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

		private int _numberDiceRolls;
		public int NumberDiceRolls
		{
			get
			{
				return _numberDiceRolls;
			}
			internal set
			{
				if (value != NumberDiceRolls)
				{
					_numberDiceRolls = value;
					NotifyPropertyChanged();
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

	}
}
