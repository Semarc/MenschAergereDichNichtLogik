using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MenschAergereDichNichtLogik
{
	public class Player : INotifyPropertyChanged
	{
		private int _numberDiceRolls;
		private int _numberHome;

		internal Player() { }


		public Color Color { get; internal set; }
		public string Name { get; internal set; }
		public int NumberHome { get => _numberHome; internal set => _numberHome = value; }
		public int NumberDiceRolls { get => _numberDiceRolls; internal set => _numberDiceRolls = value; }

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
