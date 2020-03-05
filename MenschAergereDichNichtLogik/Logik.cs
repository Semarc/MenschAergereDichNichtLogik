using System;
using System.Collections.Generic;
using System.Drawing;

namespace MenschAergereDichNichtLogik
{
	public static class Logik
	{
		private static Random randomnumber = new Random();
		/// <summary>
		/// Gibt an, ob das Spiel gestartet wurde
		/// </summary>
		private static bool GameStarted = false;


		/// <summary>
		/// Gibt an, welche Punkte für welche Farbe die Eingänge zum Haus sind
		/// </summary>
		private static Dictionary<Color, ((int, int), (int, int))> HouseEntrypoints = new Dictionary<Color, ((int, int), (int, int))>()
		{
			[Color.Green] = ((5, 10), (5, 9)),
			[Color.Red] = ((10, 5), (9, 5)),
			[Color.Black] = ((5, 0), (5, 1)),
			[Color.Yellow] = ((0, 5), (1, 5)),
		};

		private static Dictionary<Color, (int, int)> HouseEndPoints = new Dictionary<Color, (int, int)>()
		{
			[Color.Green] = (5, 6),
			[Color.Red] = (6, 5),
			[Color.Black] = (5, 4),
			[Color.Yellow] = (4, 5)
		};

		/// <summary>
		/// Die Farbe, die als nächstes gesetzt werden soll
		/// </summary>
		private static Color UebergabeFarbe;

		/// <summary>
		/// Die Würfelzahl
		/// </summary>
		public static int wuerfelzahl { get; private set; }

		/// <summary>
		/// Speichert, wei oft schon gewürfelt wurde, wenn man dreimal Würelbn darf, weil alle Puppen im haus sind
		/// </summary>
		private static int AnzahlGeuerfelt;

		#region Gamestart Information
		private static List<(int, int)> StandardBoard = new List<(int, int)>
		{
			(4, 10),
			(5, 10),
			(6, 10),

			(4, 9),
			(6, 9),

			(4, 8),
			(6, 8),

			(4, 7),
			(6, 7),

			(0, 6),
			(1, 6),
			(2, 6),
			(3, 6),
			(4, 6),
			(6, 6),
			(7, 6),
			(8, 6),
			(9, 6),
			(10, 6),

			(0, 5),
			(10, 5),

			(0, 4),
			(1, 4),
			(2, 4),
			(3, 4),
			(4, 4),
			(6, 4),
			(7, 4),
			(8, 4),
			(9, 4),
			(10, 4),

			(4, 3),
			(6, 3),

			(4, 2),
			(6, 2),

			(4, 1),
			(6, 1),

			(4, 0),
			(5, 0),
			(6, 0),
		};
		private static List<((int, int), int, Color)> FinishPoints = new List<((int, int), int, Color)>()
		{
			((5, 9), 0, Color.Green),
			((5, 8), 1, Color.Green),
			((5, 7), 2, Color.Green),
			((5, 6), 3, Color.Green),

			((1, 5), 0, Color.Red),
			((2, 5), 1, Color.Red),
			((3, 5), 2, Color.Red),
			((4, 5), 3, Color.Red),

			((6, 5), 3, Color.Yellow),
			((7, 5), 2, Color.Yellow),
			((8, 5), 1, Color.Yellow),
			((9, 5), 0, Color.Yellow),

			((5, 4), 3, Color.Black),
			((5, 3), 2, Color.Black),
			((5, 2), 1, Color.Black),
			((5, 1), 0, Color.Black)
		};
		#endregion

		/// <summary>
		/// Das Spielfeld, enthält die Regulären Felder und die Ziel-Felder
		/// </summary>
		public static Field[][] Board { get; private set; } = new Field[][]
		{
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11],
			new Field[11]
		};

		private static int _CurrentPlayerIndex;
		/// <summary>
		/// Gibt an, welcher Spieler aus der Liste aktuell dran ist. Die Set-Methode geht zum nächsten Spieler
		/// </summary>
		public static int CurrentPlayerIndex
		{
			get { return _CurrentPlayerIndex; }
			private set
			{
				if (_CurrentPlayerIndex == PlayerList.Count - 1)
				{
					_CurrentPlayerIndex = 0;
				}
				else
				{
					_CurrentPlayerIndex++;
				}
			}
		}

		/// <summary>
		/// Die Liste aller Spieler
		/// </summary>
		public static List<Player> PlayerList { get; private set; } = new List<Player>();

		#region Fieldclick
		/// <summary>
		/// Methode, die aufgerufen werden soll, wenn ein Spielfeld geklickt wurde
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		public static bool FieldClick(int X, int Y)
		{
			if (GameStarted && Board[X][Y] != null)
			{
				if (Board[X][Y] is FinishField)
				{
					//Wenn eine Figur gesetzt werden soll
					if (Board[X][Y].IsAusgewaehlt && UebergabeFarbe != Color.Empty)
					{
						SetField(X, Y);
						return true;
					}
					else if (Board[X][Y].Color == PlayerList[CurrentPlayerIndex].Color)
					{
						AusgewaehltZuruecksetzen();
						return HausPfadesucher(wuerfelzahl, X, Y);
					}
				}
				else if (Board[X][Y] is Field)
				{
					//Wenn eine Figur gesetzt werden soll
					if (Board[X][Y].IsAusgewaehlt && UebergabeFarbe != Color.Empty)
					{
						SetField(X, Y);
						return true;
					}
					//Wenn eine (andere) Figur des gleichen Spielers ausgewählt werden soll
					else if (Board[X][Y].Color == PlayerList[CurrentPlayerIndex].Color)
					{
						AusgewaehltZuruecksetzen();
						return Pfadesucher(wuerfelzahl, X, Y);
					}
				}
				return false;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Geht rekursiv durch die Anzahl des Würfel an feldern durch
		/// </summary>
		private static bool Pfadesucher(int Wuerfel, int X, int Y)
		{
			if (Wuerfel == 0)
			{
				Board[X][Y].IsAusgewaehlt = true;
				return true;
			}
			else if (Wuerfel == wuerfelzahl)
			{
				AusgewaehltZuruecksetzen();
				Board[X][Y].IsUrsprung = true;
			}
			if (HouseEntrypoints[PlayerList[CurrentPlayerIndex].Color].Item1.Item1 == X && HouseEntrypoints[PlayerList[CurrentPlayerIndex].Color].Item1.Item2 == Y)
			{
				return HausPfadesucher(Wuerfel - 1, HouseEntrypoints[PlayerList[CurrentPlayerIndex].Color].Item2.Item1, HouseEntrypoints[PlayerList[CurrentPlayerIndex].Color].Item2.Item2);
			}
			return Pfadesucher(Wuerfel - 1, Board[X][Y].NextField.Item1, Board[X][Y].NextField.Item2);

		}

		private static bool HausPfadesucher(int Wuerfel, int X, int Y)
		{
			if (Wuerfel == 0)
			{
				Board[X][Y].IsAusgewaehlt = true;
				return true;
			}
			else if (Wuerfel == wuerfelzahl)
			{
				AusgewaehltZuruecksetzen();
				Board[X][Y].IsUrsprung = true;
			}
			if (Board[X][Y].Color != Color.Empty || TupleEqual(Board[X][Y].NextField, (-1, -1)))
			{
				return false;
			}
			else
			{
				return HausPfadesucher(Wuerfel - 1, Board[X][Y].NextField.Item1, Board[X][Y].NextField.Item2);
			}
		}

		private static void SetField(int X, int Y)
		{
			//Wenn eine Figur geschlagen wird
			if (Board[X][Y].Color != Color.Empty)
			{
				foreach (Player player in PlayerList)
				{
					if (player.Color == Board[X][Y].Color)
					{
						player.NumberHome++;
						Uebergabe.Starthauserveraendert = true;
						break;
					}
				}
			}

			Board[X][Y].Color = UebergabeFarbe;
			Uebergabe.GeaenderteSpielpunkte.Add((X, Y));
			UebergabeFarbe = Color.Empty;

			//Wenn keine Sechs -> Nächster Spieler ist dran
			if (wuerfelzahl != 6)
			{
				CurrentPlayerIndex--;
			}
			wuerfelzahl = 0;

			AusgewaehltZuruecksetzen();
		}

		private static void AusgewaehltZuruecksetzen()
		{
			//Setzt die Spielsteine Zurück
			for (int i = 0; i < Board.Length; i++)
			{
				for (int j = 0; j < Board[i].Length; j++)
				{
					if (Board[i][j] != null)
					{
						Board[i][j].IsUrsprung = false;
						if (Board[i][j].IsAusgewaehlt == true)
						{
							Uebergabe.GeaenderteSpielpunkte.Add((i, j));
							Board[i][j].IsAusgewaehlt = false;
						}
					}
				}
			}

			UebergabeFarbe = Color.Empty;
		}
		#endregion

		#region Diceclick
		/// <summary>
		/// Methode, die zum aufrufen des Würfels aufgerufen werden soll
		/// </summary>
		public static bool DiceKlick()
		{
			if (GameStarted && wuerfelzahl == 0)
			{
				wuerfelzahl = randomnumber.Next(1, 7);

				if (PlayerList[CurrentPlayerIndex].NumberHome < 4)
				{
					AnzahlGeuerfelt = 0;
					//Prüfen, ob ein Zug möglich ist
					for (int i = 0; i < Board.Length; i++)
					{
						for (int j = 0; j < Board[i].Length; j++)
						{
							if (Board[i][j] != null && Board[i][j].Color == PlayerList[CurrentPlayerIndex].Color)
							{
								if (Pfadesucher(wuerfelzahl, i, j))
								{
									AusgewaehltZuruecksetzen();
									return true;
								}
							}
						}
					}
				}
				else
				{
					AnzahlGeuerfelt++;

					if(AnzahlGeuerfelt >= 3)
					{
						AnzahlGeuerfelt = 0;
						CurrentPlayerIndex++;
						return false;
					}
				}
			}
			AusgewaehltZuruecksetzen();
			return false;
		}

		#endregion

		#region Homeclick
		/// <summary>
		/// Methode, die beim Klick des Hauses aufgerufen werden soll
		/// </summary>
		/// <param name="HausColor"></param>
		public static void HomeClick(Color HausColor)
		{

			if (GameStarted)
			{


				if (HausColor == PlayerList[CurrentPlayerIndex].Color)
				{

				}
				else
				{
					return;
				}
			}
		}
		#endregion

		#region Gamestart
		/// <summary>
		/// Methode, die bei Spielbegin aufgerufen werden soll
		/// </summary>
		/// <param name="PlayerNames"></param>
		public static void GameStart(List<string> PlayerNames)
		{
			if (GameStarted == false)
			{
				//Validierung der Liste
				if (PlayerNames.Count > 4 || PlayerNames.Count < 1)
				{
					throw new ArgumentOutOfRangeException("PlayerNames", PlayerNames, "Die Liste muss zwischen 1 und 4 Namen enthalten");
				}
				else if (PlayerNames is null)
				{
					throw new NullReferenceException("Die Liste darf nicht null sein");
				}
				else
				{
					//Hinzufügen der Normalen Punkte zum Brett
					foreach ((int, int) point in StandardBoard)
					{
						Board[point.Item1][point.Item2] = new Field();
					}

					//Hinzufügen der ZielPunkte zum Brett
					foreach (((int, int), int, Color) Point in FinishPoints)
					{
						Board[Point.Item1.Item1][Point.Item1.Item2] = new FinishField(Point.Item3, Point.Item2);
					}

					//Zuweisung des Nächsten Feldes für das erste Feld
					Board[StandardBoard[0].Item1][StandardBoard[0].Item2].NextField = (StandardBoard[1].Item1, StandardBoard[1].Item2);

					NextFieldFinder(StandardBoard[1].Item1, StandardBoard[1].Item2);
					HouseNextFieldFinder();

					GameStarted = true;
				}
			}
		}

		/// <summary>
		/// Sucht das nächste Feld, damit jedes Feld sein nächstes kennt. 
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		private static void NextFieldFinder(int X, int Y)
		{
			if (TupleEqual(Board[X][Y].NextField, (-1, -1)))
			{
				if (X - 1 >= 0)
				{
					if (Board[X - 1][Y] != null && TupleEqual(Board[X - 1][Y].NextField, (X, Y)) == false && Board[X - 1][Y] is FinishField == false)
					{
						Board[X][Y].NextField = (X - 1, Y);
						NextFieldFinder(X - 1, Y);
						return;
					}
				}
				if (X + 1 <= 10)
				{
					if (Board[X + 1][Y] != null && TupleEqual(Board[X + 1][Y].NextField, (X, Y)) == false && Board[X + 1][Y] is FinishField == false)
					{
						Board[X][Y].NextField = (X + 1, Y);
						NextFieldFinder(X + 1, Y);
						return;

					}

				}
				if (Y - 1 >= 0)
				{
					if (Board[X][Y - 1] != null && TupleEqual(Board[X][Y - 1].NextField, (X, Y)) == false && Board[X][Y - 1] is FinishField == false)
					{
						Board[X][Y].NextField = (X, Y - 1);
						NextFieldFinder(X, Y - 1);
						return;

					}

				}
				if (Y + 1 <= 10)
				{
					if (Board[X][Y + 1] != null && TupleEqual(Board[X][Y + 1].NextField, (X, Y)) == false && Board[X][Y + 1] is FinishField == false)
					{
						Board[X][Y].NextField = (X, Y + 1);
						NextFieldFinder(X, Y + 1);
						return;

					}

				}

				return;
			}

		}

		private static void HouseNextFieldFinder()
		{

			foreach (KeyValuePair<Color, ((int, int), (int, int))> valuePair in HouseEntrypoints)
			{
				HouseNextFieldfinder_Inner(valuePair.Value.Item2.Item1, valuePair.Value.Item2.Item2);
			}
		}

		private static void HouseNextFieldfinder_Inner(int X, int Y)
		{
			if (TupleEqual(Board[X][Y].NextField, (-1, -1)))
			{
				if (X - 1 >= 0)
				{
					if (Board[X - 1][Y] != null && TupleEqual(Board[X - 1][Y].NextField, (X, Y)) == false && Board[X - 1][Y] is FinishField)
					{
						Board[X][Y].NextField = (X - 1, Y);

						foreach (KeyValuePair<Color, (int, int)> valuepair in HouseEndPoints)
						{
							if (valuepair.Value.Item1 == X - 1 && valuepair.Value.Item2 == Y)
							{
								return;
							}
						}
						HouseNextFieldfinder_Inner(X - 1, Y);
						return;
					}
				}
				if (X + 1 <= 10)
				{
					if (Board[X + 1][Y] != null && TupleEqual(Board[X + 1][Y].NextField, (X, Y)) == false && Board[X + 1][Y] is FinishField)
					{
						Board[X][Y].NextField = (X + 1, Y);


						foreach (KeyValuePair<Color, (int, int)> valuepair in HouseEndPoints)
						{
							if (valuepair.Value.Item1 == X + 1 && valuepair.Value.Item2 == Y)
							{
								return;
							}
						}
						HouseNextFieldfinder_Inner(X + 1, Y);
						return;

					}

				}
				if (Y - 1 >= 0)
				{
					if (Board[X][Y - 1] != null && TupleEqual(Board[X][Y - 1].NextField, (X, Y)) == false && Board[X][Y - 1] is FinishField)
					{
						Board[X][Y].NextField = (X, Y - 1);


						foreach (KeyValuePair<Color, (int, int)> valuepair in HouseEndPoints)
						{
							if (valuepair.Value.Item1 == X && valuepair.Value.Item2 == Y - 1)
							{
								return;
							}
						}
						HouseNextFieldfinder_Inner(X, Y - 1);
						return;
					}

				}
				if (Y + 1 <= 10)
				{
					if (Board[X][Y + 1] != null && TupleEqual(Board[X][Y + 1].NextField, (X, Y)) == false && Board[X][Y + 1] is FinishField)
					{
						Board[X][Y].NextField = (X, Y + 1);


						foreach (KeyValuePair<Color, (int, int)> valuepair in HouseEndPoints)
						{
							if (valuepair.Value.Item1 == X && valuepair.Value.Item2 == Y + 1)
							{
								return;
							}
						}
						HouseNextFieldfinder_Inner(X, Y + 1);
						return;

					}

				}
			}
		}

		#endregion

		/// <summary>
		/// Prüft, ob zwei Tupel von Typ (int, int) gleich sind
		/// </summary>
		/// <param name="tuple1"></param>
		/// <param name="tuple2"></param>
		/// <returns></returns>
		private static bool TupleEqual((int, int) tuple1, (int, int) tuple2)
		{
			return tuple1.Item1 == tuple2.Item1 && tuple1.Item2 == tuple2.Item2;
		}
	}
}