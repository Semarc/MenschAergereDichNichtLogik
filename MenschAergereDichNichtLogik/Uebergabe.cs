﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MenschAergereDichNichtLogik
{
	public static class Uebergabe
	{
		/// <summary>
		/// Enthält alle Spielstein, die verändert worden sind
		/// </summary>
		public static List<(int, int)> GeaenderteSpielpunkte = new List<(int, int)>();
		/// <summary>
		/// Gibt an, ob die Anzahl der Steine in einem der Starthäuser verändert worden sind
		/// </summary>
		public static bool Starthauserveraendert;
	}
}