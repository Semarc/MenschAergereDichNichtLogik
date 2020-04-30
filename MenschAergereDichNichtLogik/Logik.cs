﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MenschAergereDichNichtLogik
{
	public static class Logik
	{
		private static readonly Random randomnumber = new Random();
		/// <summary>
		/// Gibt an, ob das Spiel gestartet wurde
		/// </summary>
		private static bool GameStarted = false;


		/// <summary>
		/// Gibt an, welche Punkte für welche Farbe die Eingänge zum Haus sind
		/// </summary>
		public static readonly ReadOnlyDictionary<Color, (Point, Point)> HouseEntrypointsDictionary = new ReadOnlyDictionary<Color, (Point, Point)>(new Dictionary<Color, (Point, Point)>
		{
			[Color.Green] = (new Point(5, 10), new Point(5, 9)),
			[Color.Red] = (new Point(10, 5), new Point(9, 5)),
			[Color.Black] = (new Point(5, 0), new Point(5, 1)),
			[Color.Yellow] = (new Point(0, 5), new Point(1, 5)),
		});

		public static readonly ReadOnlyDictionary<Color, Point> HouseEndPointsDictionary = new ReadOnlyDictionary<Color, Point>(new Dictionary<Color, Point>
		{
			[Color.Green] = new Point(5, 6),
			[Color.Red] = new Point(6, 5),
			[Color.Black] = new Point(5, 4),
			[Color.Yellow] = new Point(4, 5)
		});

		/// <summary>
		/// Gibt die Felder an, auf die neue Steine Kommen, nachdem sie aus dem Haus gekommen sind
		/// </summary>
		public static readonly ReadOnlyDictionary<Color, Point> StartPointsDictionary = new ReadOnlyDictionary<Color, Point>(new Dictionary<Color, Point>
		{
			[Color.Green] = new Point(6, 10),
			[Color.Red] = new Point(10, 4),
			[Color.Black] = new Point(4, 0),
			[Color.Yellow] = new Point(0, 6)
		});

		/// <summary>
		/// Die Farbe, die als nächstes gesetzt werden soll
		/// </summary>
		private static Color UebergabeFarbe;

		/// <summary>
		/// Die Würfelzahl
		/// </summary>
		public static int Wuerfelzahl { get; private set; }

		/// <summary>
		/// Speichert, wei oft schon gewürfelt wurde, wenn man dreimal Würelbn darf, weil alle Puppen im haus sind
		/// </summary>
		private static int AnzahlGeuerfelt;

		#region Gamestart Information
		private static readonly ReadOnlyCollection<Point> StandardBoard = new ReadOnlyCollection<Point>
		(new List<Point> {
			new Point(4, 10),
			new Point(5, 10),
			new Point(6, 10),

			new Point(4, 9),
			new Point(6, 9),

			new Point(4, 8),
			new Point(6, 8),

			new Point(4, 7),
			new Point(6, 7),

			new Point(0, 6),
			new Point(1, 6),
			new Point(2, 6),
			new Point(3, 6),
			new Point(4, 6),
			new Point(6, 6),
			new Point(7, 6),
			new Point(8, 6),
			new Point(9, 6),
			new Point(10, 6),

			new Point(0, 5),
			new Point(10, 5),

			new Point(0, 4),
			new Point(1, 4),
			new Point(2, 4),
			new Point(3, 4),
			new Point(4, 4),
			new Point(6, 4),
			new Point(7, 4),
			new Point(8, 4),
			new Point(9, 4),
			new Point(10, 4),

			new Point(4, 3),
			new Point(6, 3),

			new Point(4, 2),
			new Point(6, 2),

			new Point(4, 1),
			new Point(6, 1),

			new Point(4, 0),
			new Point(5, 0),
			new Point(6, 0),
		});
		private static readonly ReadOnlyCollection<(Point, int, Color)> FinishPoints = new ReadOnlyCollection<(Point, int, Color)>
		(new List<(Point, int, Color)>{
			(new Point(5, 9), 0, Color.Black),
			(new Point(5, 8), 1, Color.Black),
			(new Point(5, 7), 2, Color.Black),
			(new Point(5, 6), 3, Color.Black),

			(new Point(1, 5), 0, Color.Yellow),
			(new Point(2, 5), 1, Color.Yellow),
			(new Point(3, 5), 2, Color.Yellow),
			(new Point(4, 5), 3, Color.Yellow),

			(new Point(6, 5), 3, Color.Red),
			(new Point(7, 5), 2, Color.Red),
			(new Point(8, 5), 1, Color.Red),
			(new Point(9, 5), 0, Color.Red),

			(new Point(5, 4), 3, Color.Green),
			(new Point(5, 3), 2, Color.Green),
			(new Point(5, 2), 1, Color.Green),
			(new Point(5, 1), 0, Color.Green)
		});
		#endregion

		/// <summary>
		/// Das Spielfeld, enthält die Regulären Felder und die Ziel-Felder
		/// </summary>
		private static Field[][] BoardInternal { get; set; } = new Field[][]
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

		private static ReadOnlyCollection<ReadOnlyCollection<Field>> BoardCache;

		public static ReadOnlyCollection<ReadOnlyCollection<Field>> Board
		{
			get
			{
				if (BoardCache == null)
				{
					List<ReadOnlyCollection<Field>> temp2 = new List<ReadOnlyCollection<Field>>();

					for (int i = 0; i < BoardInternal.Length; i++)
					{
						temp2.Add(new ReadOnlyCollection<Field>(BoardInternal[i]));
					}
					BoardCache = temp2.AsReadOnly();
				}
				return BoardCache;
			}
		}

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
		public static ReadOnlyCollection<Player> PlayerList
		{
			get
			{
				if(PlayerListCache is null)
				{
					PlayerListCache = PlayerListInternal.AsReadOnly();
				}
				return PlayerListCache;
			}
		}

		private static ReadOnlyCollection<Player> PlayerListCache;
		private static readonly List<Player> PlayerListInternal = new List<Player>();

		#region Fieldclick
		/// <summary>
		/// Methode, die aufgerufen werden soll, wenn ein Spielfeld geklickt wurde
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <returns><see langword="true"/>, wenn ein Zug möglich ist. <see langword="false"/>, wenn kein Zug möglich ist</returns>
		public static bool FieldClick(int X, int Y)
		{
			if (GameStarted && BoardInternal[X][Y] != null)
			{
				if (BoardInternal[X][Y] is FinishField)
				{
					//Wenn eine Figur gesetzt werden soll
					if (BoardInternal[X][Y].IsAusgewaehlt && UebergabeFarbe != Color.Empty)
					{
						SetField(X, Y);
						return true;
					}
					else if (BoardInternal[X][Y].Color == PlayerList[CurrentPlayerIndex].Color)
					{
						AusgewaehltZuruecksetzen();
						return HausPfadesucher(Wuerfelzahl, X, Y);
					}
				}
				else if (BoardInternal[X][Y] is Field)
				{
					//Wenn eine Figur gesetzt werden soll
					if (BoardInternal[X][Y].IsAusgewaehlt && UebergabeFarbe != Color.Empty)
					{
						SetField(X, Y);
						return true;
					}
					//Wenn eine (andere) Figur des gleichen Spielers ausgewählt werden soll
					else if (BoardInternal[X][Y].Color == PlayerList[CurrentPlayerIndex].Color)
					{
						AusgewaehltZuruecksetzen();
						return Pfadesucher(Wuerfelzahl, X, Y);
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
				BoardInternal[X][Y].IsAusgewaehlt = true;
				return true;
			}
			else if (Wuerfel == Wuerfelzahl)
			{
				AusgewaehltZuruecksetzen();
				BoardInternal[X][Y].IsUrsprung = true;
			}
			if (HouseEntrypointsDictionary[PlayerList[CurrentPlayerIndex].Color].Item1.X == X && HouseEntrypointsDictionary[PlayerList[CurrentPlayerIndex].Color].Item1.Y == Y)
			{
				return HausPfadesucher(Wuerfel - 1, HouseEntrypointsDictionary[PlayerList[CurrentPlayerIndex].Color].Item2.X, HouseEntrypointsDictionary[PlayerList[CurrentPlayerIndex].Color].Item2.Y);
			}
			return Pfadesucher(Wuerfel - 1, BoardInternal[X][Y].NextField.X, BoardInternal[X][Y].NextField.Y);

		}

		private static bool HausPfadesucher(int Wuerfel, int X, int Y)
		{
			if (Wuerfel == 0)
			{
				BoardInternal[X][Y].IsAusgewaehlt = true;
				return true;
			}
			else if (Wuerfel == Wuerfelzahl)
			{
				AusgewaehltZuruecksetzen();
				BoardInternal[X][Y].IsUrsprung = true;
			}
			if (BoardInternal[X][Y].Color != Color.Empty || BoardInternal[X][Y].NextField == new Point(-1, -1))
			{
				return false;
			}
			else
			{
				return HausPfadesucher(Wuerfel - 1, BoardInternal[X][Y].NextField.X, BoardInternal[X][Y].NextField.Y);
			}
		}

		private static void SetField(int X, int Y)
		{
			//Wenn eine Figur geschlagen wird
			if (BoardInternal[X][Y].Color != Color.Empty)
			{
				foreach (Player player in PlayerList)
				{
					if (player.Color == BoardInternal[X][Y].Color)
					{
						player.NumberHome++;
						Uebergabe.Starthauserveraendert = true;
						break;
					}
				}
			}

			BoardInternal[X][Y].Color = UebergabeFarbe;
			Uebergabe.GeaenderteSpielpunkte.Add(new Point(X, Y));
			UebergabeFarbe = Color.Empty;

			//Wenn keine Sechs -> Nächster Spieler ist dran
			if (Wuerfelzahl != 6)
			{
				CurrentPlayerIndex--;
			}
			Wuerfelzahl = 0;

			AusgewaehltZuruecksetzen();
		}

		private static void AusgewaehltZuruecksetzen()
		{
			//Setzt die Spielsteine Zurück
			for (int i = 0; i < BoardInternal.Length; i++)
			{
				for (int j = 0; j < BoardInternal[i].Length; j++)
				{
					if (BoardInternal[i][j] != null)
					{
						BoardInternal[i][j].IsUrsprung = false;
						if (BoardInternal[i][j].IsAusgewaehlt == true)
						{
							Uebergabe.GeaenderteSpielpunkte.Add(new Point(i, j));
							BoardInternal[i][j].IsAusgewaehlt = false;
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
			if (GameStarted)
			{
				if (Wuerfelzahl == 0 || PlayerList[CurrentPlayerIndex].NumberHome == 4 && Wuerfelzahl != 6)
				{ 
					Wuerfelzahl = randomnumber.Next(1, 7);
					if (PlayerList[CurrentPlayerIndex].NumberHome < 4)
					{
						
						AnzahlGeuerfelt = 0;
						//Prüfen, ob ein Zug möglich ist
						for (int i = 0; i < BoardInternal.Length; i++)
						{
							for (int j = 0; j < BoardInternal[i].Length; j++)
							{
								if (BoardInternal[i][j] != null && BoardInternal[i][j].Color == PlayerList[CurrentPlayerIndex].Color)
								{
									if (Pfadesucher(Wuerfelzahl, i, j))
									{
										AusgewaehltZuruecksetzen();
										return true;
									}
								}
							}
						}
					}
					else if(Wuerfelzahl == 6)
					{
						return true;
					}
					else
					{
						AnzahlGeuerfelt++;

						if (AnzahlGeuerfelt >= 3)
						{
							AnzahlGeuerfelt = 0;
							CurrentPlayerIndex++;
							return false;
						}
					}
				}
				AusgewaehltZuruecksetzen();
			}
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
					if (Wuerfelzahl == 6)
					{
						if (PlayerList[CurrentPlayerIndex].NumberHome > 0)
						{
							PlayerList[CurrentPlayerIndex].NumberHome--;
							UebergabeFarbe = PlayerList[CurrentPlayerIndex].Color;
							SetField(StartPointsDictionary[PlayerList[CurrentPlayerIndex].Color].X, StartPointsDictionary[PlayerList[CurrentPlayerIndex].Color].Y);
							Uebergabe.Starthauserveraendert = true;
						}
					}
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
				if (PlayerNames is null)
				{
					throw new NullReferenceException("Die Liste darf nicht null sein");
				}
				else if (PlayerNames.Count > 4 || PlayerNames.Count < 1)
				{
					throw new ArgumentOutOfRangeException("PlayerNames", PlayerNames, "Die Liste muss zwischen 1 und 4 Namen enthalten");
				}
				else
				{
					//Hinzufügen der Normalen Punkte zum Brett
					foreach (Point point in StandardBoard)
					{
						BoardInternal[point.X][point.Y] = new Field();
					}

					//Hinzufügen der ZielPunkte zum Brett
					foreach ((Point, int, Color) point in FinishPoints)
					{
						BoardInternal[point.Item1.X][point.Item1.Y] = new FinishField(point.Item3, point.Item2);
					}

					//Zuweisung des Nächsten Feldes für das erste Feld
					BoardInternal[StandardBoard[0].X][StandardBoard[0].Y].NextField = new Point(StandardBoard[1].X, StandardBoard[1].Y);

					NextFieldFinder(StandardBoard[1].X, StandardBoard[1].Y);
					HouseNextFieldFinder();

					for (int i = 0; i < PlayerNames.Count; i++)
					{
						PlayerListInternal.Add(new Player(PlayerNames[i], (Color)i + 1));
					}
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
			if (BoardInternal[X][Y].NextField == new Point(-1, -1))
			{
				if (X - 1 >= 0)
				{
					if (BoardInternal[X - 1][Y] != null && BoardInternal[X - 1][Y].NextField == new Point(X, Y) == false && BoardInternal[X - 1][Y] is FinishField == false)
					{
						BoardInternal[X][Y].NextField = new Point(X - 1, Y);
						NextFieldFinder(X - 1, Y);
						return;
					}
				}
				if (X + 1 <= 10)
				{
					if (BoardInternal[X + 1][Y] != null && BoardInternal[X + 1][Y].NextField == new Point(X, Y) == false && BoardInternal[X + 1][Y] is FinishField == false)
					{
						BoardInternal[X][Y].NextField = new Point(X + 1, Y);
						NextFieldFinder(X + 1, Y);
						return;

					}

				}
				if (Y - 1 >= 0)
				{
					if (BoardInternal[X][Y - 1] != null && BoardInternal[X][Y - 1].NextField == new Point(X, Y) == false && BoardInternal[X][Y - 1] is FinishField == false)
					{
						BoardInternal[X][Y].NextField = new Point(X, Y - 1);
						NextFieldFinder(X, Y - 1);
						return;

					}

				}
				if (Y + 1 <= 10)
				{
					if (BoardInternal[X][Y + 1] != null && BoardInternal[X][Y + 1].NextField == new Point(X, Y) == false && BoardInternal[X][Y + 1] is FinishField == false)
					{
						BoardInternal[X][Y].NextField = new Point(X, Y + 1);
						NextFieldFinder(X, Y + 1);
						return;

					}

				}

				return;
			}

		}

		private static void HouseNextFieldFinder()
		{

			foreach (KeyValuePair<Color, (Point, Point)> valuePair in HouseEntrypointsDictionary)
			{
				HouseNextFieldfinder_Inner(valuePair.Value.Item2.X, valuePair.Value.Item2.Y);
			}
		}

		private static void HouseNextFieldfinder_Inner(int X, int Y)
		{
			if (BoardInternal[X][Y].NextField == new Point(-1, -1))
			{
				if (X - 1 >= 0)
				{
					if (BoardInternal[X - 1][Y] != null && BoardInternal[X - 1][Y].NextField == new Point(X, Y) == false && BoardInternal[X - 1][Y] is FinishField)
					{
						BoardInternal[X][Y].NextField = new Point(X - 1, Y);

						foreach (KeyValuePair<Color, Point> valuepair in HouseEndPointsDictionary)
						{
							if (valuepair.Value.X == X - 1 && valuepair.Value.Y == Y)
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
					if (BoardInternal[X + 1][Y] != null && BoardInternal[X + 1][Y].NextField == new Point(X, Y) == false && BoardInternal[X + 1][Y] is FinishField)
					{
						BoardInternal[X][Y].NextField = new Point(X + 1, Y);


						foreach (KeyValuePair<Color, Point> valuepair in HouseEndPointsDictionary)
						{
							if (valuepair.Value.X == X + 1 && valuepair.Value.Y == Y)
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
					if (BoardInternal[X][Y - 1] != null && BoardInternal[X][Y - 1].NextField == new Point(X, Y) == false && BoardInternal[X][Y - 1] is FinishField)
					{
						BoardInternal[X][Y].NextField = new Point(X, Y - 1);


						foreach (KeyValuePair<Color, Point> valuepair in HouseEndPointsDictionary)
						{
							if (valuepair.Value.X == X && valuepair.Value.Y == Y - 1)
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
					if (BoardInternal[X][Y + 1] != null && BoardInternal[X][Y + 1].NextField == new Point(X, Y) == false && BoardInternal[X][Y + 1] is FinishField)
					{
						BoardInternal[X][Y].NextField = new Point(X, Y + 1);


						foreach (KeyValuePair<Color, Point> valuepair in HouseEndPointsDictionary)
						{
							if (valuepair.Value.X == X && valuepair.Value.Y == Y + 1)
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
	}
}