using System;

// TODO:
// -Create GFX
// -Set up gameflow
// -GG :P
namespace DSS_2012_10
{
	public class GameLogic
	{
		private string bigBar = "################################################################################";
		private int xCoordinateRange = 1; // how many columns are there?
		private int yCoordinateRange = 1; // how many rows are there?
		private int playerCount = 2; // how many players are there?
		private int[] xyFields; // all the possible field values are stored in here
		private string[,] playerData ; // stores the player representation marker i.e. X and O and also their set names
		private int winConditionLength; // how many fields in a line are required to win the game?
		string lineElementString = ""; // the horizontal separator line
		int lineLength = 0; // lenght of horizontal separator line so that it doiesn't need to be computed with lineElementString.Lenght

		private bool[,] fieldStates; // stores field state information

		public void AskGameSettings ()
		{
			Console.WriteLine ("You will now be prompted to input the game parameters.");
			Console.WriteLine ("Parameter 1: Board dimensions...");
			Console.WriteLine (bigBar);
			Console.WriteLine ("How many columns would you like?");
			int checkInt;
			string userInput;

			
		
			userInput = Console.ReadLine ();
			while (!(int.TryParse(userInput, out checkInt) && checkInt > 0)) {
				Console.WriteLine ("ERROR - <SetXCoordinateRange()> only accepts non-zero integers. [E:0003]");
				Console.WriteLine ("Again...How many columns would you like?");
				userInput = Console.ReadLine ();
			}
			SetXCoordinateRange (int.Parse (userInput));
			Console.WriteLine ("Column count successfully set to" + " " + "<" + checkInt + ">" + "!");
			
			Console.WriteLine ("How many rows would you like?");
			userInput = Console.ReadLine ();
			while (!(int.TryParse(userInput, out checkInt) && checkInt > 0)) {
				Console.WriteLine ("ERROR - <SetYCoordinateRange()> only accepts non-zero integers. [E:0004]");
				Console.WriteLine ("Again...How many rows would you like?");
				userInput = Console.ReadLine ();
			}
			SetYCoordinateRange (int.Parse (userInput));
			Console.WriteLine ("Row count successfully set to" + " " + "<" + checkInt + ">" + "!");
			
			Console.WriteLine ("How many players would you like?");
			userInput = Console.ReadLine ();
			while (!(int.TryParse(userInput, out checkInt) && checkInt > 1)) {
				Console.WriteLine ("ERROR - <SetPlayerCount()> only accepts integers greater than 1. [E:0005]");
				Console.WriteLine ("Again...How many players would you like?");
				userInput = Console.ReadLine ();
			}
			SetPlayerCount (int.Parse (userInput));
			Console.WriteLine ("Player count successfully set to <" + (playerCount-1) + "> !");

			Console.WriteLine ("How long must the line be in order to win the game?");
			userInput = Console.ReadLine ();
			while (!(int.TryParse(userInput, out checkInt) && checkInt > 1)) {
				Console.WriteLine ("ERROR - <SetWinCondition> only accepts integers greater than 1. [E:0006]");
				Console.WriteLine ("Again...how long shall it be?");
				userInput = Console.ReadLine ();
			}
			SetWinCondition (int.Parse (userInput));

			Console.WriteLine ("Winning condition successfully set to" + " " + "<" + winConditionLength + "> in a line" + "!");
			Console.WriteLine (bigBar);
			Console.WriteLine ("Initializing Gamespace...");
			SetXYFields ();
			Console.WriteLine ("Success! -- Boardsize:" + xCoordinateRange + "x" + yCoordinateRange + " " + "with" + " " + (playerCount - 1) + " " + "Players...");
			Console.WriteLine ("First player to get" + " " + winConditionLength + " " + "in a straight line wins!");
			Console.WriteLine (bigBar);
			Console.WriteLine ("You can type in names and markers for the participating players if you like!");
			Console.WriteLine ("Would you like to set player names and markers? <Y/N>");
			
			
			/* needs more checking...*/
			SetPlayerDataRange ();
			string inputString = Console.ReadLine ();
			while (inputString != "Y" && 
			       inputString != "y" &&
			       inputString != "N" &&
			       inputString != "n") {
				Console.WriteLine ("ERROR - <Console.ReadLine()> only listens to either Y for yes or N for no . [E:0007]");
				inputString = Console.ReadLine ();
			}
			if (inputString != "N" &&
				inputString != "n") {
				for (int yY = 1; yY < (playerCount); yY++) {
					Console.WriteLine ("Type in the name you'd like for player" + yY + " " + ":");
					SetPlayerName (yY, Console.ReadLine ());
					Console.WriteLine ("Name for player" + yY + " " + "successfully set to" + " " + "<" + playerData [(yY - 1), 0] + ">" + "!");
					Console.WriteLine ("Type in the marker you'd like for player" + yY + " " + ":");
					SetPlayerMark (yY, Console.ReadLine ());
					Console.WriteLine ("Marker for player" + yY + " " + "successfully set to" + " " + "<" + playerData [(yY - 1), 1] + ">" + "!");
				}
			} else if (inputString == "N" || inputString == "n") { 
				for (int yY = 1; yY < (playerCount); yY++) {
					Console.WriteLine ("Generic name is being assigned to player" + yY + " " + ":");
					SetPlayerName (yY, ("Player_" + yY));
					Console.WriteLine ("Name for player" + yY + " " + "successfully set to" + " " + "<" + playerData [(yY - 1), 0] + ">" + "!");
					Console.WriteLine ("Generic marker is being assigned to player" + yY + " " + ":");
					SetPlayerMark (yY, ("(P." + yY));
					Console.WriteLine ("Marker for player" + yY + " " + "successfully set to" + " " + "<" + playerData [(yY - 1), 1] + ">" + "!");
				}
			}
		}

		public void SetPlayerDataRange ()
		{
			playerData = new string[playerCount, 2];
		}

		public void SetFieldStatesRange ()
		{
			fieldStates = new bool[(xyFields.Length), (playerCount)];
		}

		public void ClearFieldForPlayer (int fieldNumber, int playerNumber) // clears a field for a player. 0 is for system, 1 is player 1, 2 is player 2 etc
		{
			fieldStates [(fieldNumber - 1), playerNumber] = false; // zero field is field 1 thus the "-1" argument
		}

		public void MarkFieldForPlayer (int fieldNumber, int playerNumber) // marks a field for a player. 0 is system, 1 is player 1 , 2 is player 2 etc.
		{
			fieldStates [(fieldNumber -1), playerNumber] = true;
			fieldStates [(fieldNumber -1), 0] = false;
		}

		public void SetXCoordinateRange (int x) // sets the number of columns
		{
			xCoordinateRange = x;
		}
		
		public void SetYCoordinateRange (int y) // sets the number of rows
		{
			yCoordinateRange = y;
		}

		public void SetPlayerCount (int p) // sets the number of players
		{
			playerCount = (p + 1);
		}

		public bool SetPlayerName (int playerNumber, string nameString)
		{
			if (playerNumber > 0 && 
				playerNumber <= (playerCount + 1)) {
				playerData [playerNumber - 1, 0] = nameString;
				return true;
			} else {
				Console.WriteLine ("ERROR - <playerData[" + playerNumber + ",0]>" + " " + "is not within range of <playerData[]>. [E:0001]");
				return false;
			}
		} // sets the player names

		public bool SetPlayerMark (int playerNumber, string markString)
		{
			if (playerNumber > 0 && playerNumber < (playerCount + 2)) {
				playerData [playerNumber - 1, 1] = markString;
				return true;
			} else {
				Console.WriteLine ("ERROR - <playerData[" + playerNumber + ",1]>" + " " + "is not within range of <playerData[]>. [E:0002]");
				return false;
			}
		} // sets the player markers

		public void SetXYFields ()
		{
			xyFields = new int[(xCoordinateRange * yCoordinateRange)];
			fieldStates = new bool[(xyFields.Length), (playerCount + 1)];
			for (int i = 0; i < (xyFields.Length); i++) {
				xyFields [i] = i + 1;
				fieldStates [i, 0] = true; // initializes the array with 0 owning all fields (meaning all fields are free)
				for (int p = 1; p <= playerCount; p++) {

					fieldStates [i, p] = false; // initializes the array with no players owning any fields
				}
			}
		} // initializes the gamespace

		public int GetYCoordinateFromField (int xyFieldNumber)
		{
			double field = xyFieldNumber;
			return Convert.ToInt32((Math.Ceiling ((field/xCoordinateRange))));

		} // retrieves the row-number of a given field

		public int GetFieldNumberFromXY (int x, int y)
		{
			return (x * y);
		} // retrieves the field-number of the field with the given X and Y coordinates



		public int GetXCoordinateFromField (int xyFieldNumber)
		{
			return (xyFieldNumber%xCoordinateRange);
		} // retrieves the column-number of a given field

		public void SetWinCondition (int number)
		{
			winConditionLength = number;
		} // sets the winConditionLength

		public bool CheckWinCondition (int playerNumber, int fieldNumber)
		{

			int fieldY = GetYCoordinateFromField (fieldNumber);
			int fieldX = GetXCoordinateFromField (fieldNumber);

			Console.WriteLine (fieldX + "|" + fieldY);

			int testInt = 0;

			// =====================    designating fields to search line elements on.
			// | o | 1 | 2 | 3 | 2 |	I determine the parse space by reaching out from
			// =====================	the last active field (3 in this graphic) and
			// | o | o | 2 | 2 | 2 |	then I go up, down, left and right to determine
			// =====================	the maximum distance for fields to still connect
			// | o | 1 | o | 1 | o |	into a winning condition with the last active field
			// =====================	(need 3 in a row to win in this graphic)
			// | o | o | o | o | o |	I then check all the fields in their directions
			//							respectively as long as they return "true" for 
			//							their owner to be the last active player and then
			//							check if their sum matches(or exceeds) the win
			//							conditions or not.

			for (int i = 0; testInt < 1; i++) {
				testInt = (fieldNumber - (xCoordinateRange * (winConditionLength - (1 + i))));
			}

			int yDownStart = testInt;

			for (int i = 0; testInt < 1; i++) {
				testInt = (fieldNumber - (yCoordinateRange * (winConditionLength - (1 + i))));
			}
			
			int xLeftStart = testInt;

			for (int i = 0; testInt < 1; i++) {
				testInt = (fieldNumber + (xCoordinateRange * (winConditionLength - (1 + i))));
			}
			
			int yDownEnd = testInt;

			for (int i = 0; testInt < 1; i++) {
				testInt = (fieldNumber + (yCoordinateRange * (winConditionLength - (1 + i))));
			}
			
			int xLeftEnd = testInt;

			int trueCount = 0;

			for (int i = 1; i <= (yDownEnd-fieldY); i++) {

				if (fieldStates [(GetFieldNumberFromXY (fieldX, (fieldY + i))), playerNumber] == true) { // go one down
					trueCount++;
				}
			}
			for (int i = 1; i <= (fieldY - yDownStart); i++) {
				
				if (fieldStates [(GetFieldNumberFromXY (fieldX, (fieldY - i))), playerNumber] == true) { // go one up
					trueCount++;
				}
			} 
			if (trueCount >= (winConditionLength - 1)) { // would mean a vertical line meeting the victory condition was found
				return true;
				//		break;
			} else {
				trueCount = 0;
			}
			for (int i = 1; i <= (xLeftEnd-fieldX); i++) {
				
				if (fieldStates [(GetFieldNumberFromXY (fieldY, (fieldX + i))), playerNumber] == true) { // go one right
					trueCount++;
				}
			}
			for (int i = 1; i <= (fieldY - yDownStart); i++) {
				
				if (fieldStates [(GetFieldNumberFromXY (fieldY, (fieldX - i))), playerNumber] == true) { // go one left
					trueCount++;
				}
			} 
			if (trueCount >= (winConditionLength - 1)) { // would mean a horizontal line meeting the victory condition was found
				return true;
				//		break;
			} else {
				trueCount = 0;
			}
			for (int i = 1; i <= (xLeftEnd-fieldX)&& i <=(yDownEnd-fieldY); i++) {
				Console.WriteLine(xLeftEnd + " " + fieldX + " " + yDownEnd + " " + fieldY); 
				if (fieldStates [(GetFieldNumberFromXY ((fieldY + i), (fieldX + i))), playerNumber] == true) { // go one right & one down
					trueCount++;
				}
			}
			for (int i = 1; i <= (fieldY - yDownStart)&& i <= (fieldY - yDownStart); i++) {
				
				if (fieldStates [(GetFieldNumberFromXY ((fieldY - i), (fieldX - i))), playerNumber] == true) { // go one left & one up
					trueCount++;
				}
			} 
			if (trueCount >= (winConditionLength - 1)) { // would mean a downwards diagonal line meeting the victory condition was found
				return true;
				//		break;
			} else {
				trueCount = 0;
			}
			for (int i = 1; i <= (fieldY - yDownStart)&& i <=(yDownEnd-fieldY); i++) {
				
				if (fieldStates [(GetFieldNumberFromXY ((fieldY + i), (fieldX - i))), playerNumber] == true) { // go one left & one down
					trueCount++;
				}
			}
			for (int i = 1; i <= (xLeftEnd-fieldX)&& i <= (fieldY - yDownStart); i++) {
				
				if (fieldStates [(GetFieldNumberFromXY ((fieldY - i), (fieldX + i))), playerNumber] == true) { // go one right & one up
					trueCount++;
				}
			} 
			if (trueCount >= (winConditionLength - 1)) { // would mean a upwards diagolnal line meeting the victory condition was found
				return true;
				//		break;
			} else {
				trueCount = 0;
			}
			return false;

			

		} // checks if capturing the current fieldNumber field generates a victory for the capturing player


		public bool CheckWinCondition (int playerNumber)
		{

			int trueCount = 0;
			for (int i = 0; i < xyFields.Length+1; i++) { // will check all fieldStates[]

				if (fieldStates [i, playerNumber] == true) { // listens for player ownership of the checked fields

// will check if the player-owned field is within parse area. Field that are too close to the field borders are ignored as they would automatically 
// be checked upon detection of a player-owned field within winConditionLength's range. Without doing this the check would crash the program
// as it would try to access fieldStates[] outside it's defined range... so, this is not for cosmetics, it's required for the program to run ;)

					if (GetYCoordinateFromField (i) < winConditionLength && GetYCoordinateFromField (i) < (xyFields.Length - (winConditionLength - 2)) && GetXCoordinateFromField (i) < winConditionLength && GetXCoordinateFromField (i) < (xyFields.Length - (winConditionLength - 2))) {

						for (int c = 1; c < winConditionLength; c++) {
/* down */
							if (fieldStates [(i + (c * xCoordinateRange)), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}
						for (int c = 1; c < winConditionLength; c++) {
/* up */
							if (fieldStates [(i + (c * xCoordinateRange)), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}
/* right */
						for (int c = 1; c < winConditionLength; c++) {
							if (fieldStates [(i + c), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}
/* left */
						for (int c = 1; c < winConditionLength; c++) {
							if (fieldStates [(i - c), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}
						for (int c = 1; c < winConditionLength; c++) {
/* right down */
							if (fieldStates [(i + (c + xCoordinateRange)), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}
						for (int c = 1; c < winConditionLength; c++) {
/* left down */
							if (fieldStates [(i + (xCoordinateRange - c)), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}
						for (int c = 1; c < winConditionLength; c++) {
/* right up */
							if (fieldStates [(i - (xCoordinateRange - c)), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}
						for (int c = 1; c < winConditionLength; c++) {
/* left up */
							if (fieldStates [(i - (xCoordinateRange + c)), playerNumber] == true) {
								trueCount++;
							} else {
								trueCount = 0;
							}
						}



					}

				

				}


			}
			if (trueCount > 0) {  // unless win conditions are met the trueCount variable will always be reset to zero - it being non-zero means at least one win condition was met
				return true;
			} else {
				return false;
			}
			

		} // REDUNDANT: secondary parse of whole gamespace -- currently unused & incomplete!!!

		public int CountIntDigits (int number)
		{
			double logCheckDouble = (number + 1);
			
			return (Convert.ToInt32(Math.Ceiling(Math.Log10(logCheckDouble))));
			
		} // counts the digits of a given integer, i.e. a field-number

		public void HoistLineElements ()
		{
			lineElementString = "";
			int lineLength = 0;
			for (int i = 1; i <= (CountIntDigits (xyFields.Length)); i++) {
				lineElementString += "="; // one for each digit of the maximum digits to be represented in the grid
				lineLength++;
			}

			for (int i = 1; i <= xCoordinateRange; i++) {
				lineElementString += "====="; // one for each  field separator and one as the spacing on each side of the field number/marker plus one for the marker/number separators
				lineLength += 5;
			}
			lineElementString += "=="; //  +one for the closing separator
			lineLength ++;
		} // hoists the horizontal line separator string for the graphical representation of the gamespace

		public string GetFieldString (int fieldNumber)
		{
	
			if (fieldStates [(fieldNumber), 0] == true) {

				return (fieldNumber+1).ToString();
			}
			for (int p = 0; p < playerCount; p++) {
				if (fieldStates [(fieldNumber), p] == true) {
					return playerData [p, 1];
				}
			}
			Console.WriteLine("ERROR -- Unable to get string for player! [E:0008]");
			return "";
		}  // retrieves the correct string to paint onto a field on the graphical representation of the gamespace

		public string DrawFieldElements (int f)
		{
			string fieldElementsString = "";
			int checkInt;

			checkInt = GetFieldString (f).Length;
			checkInt = CountIntDigits (xyFields.Length) - checkInt;
			for (int n = 0; n < checkInt; n++) {
				fieldElementsString += "0";
			}
			fieldElementsString += GetFieldString (f);

	
			return fieldElementsString;
		}

		public void DrawGame ()
		{
			int nN = 0;
			for (int i = 1; i<= ((yCoordinateRange*2)+1); i++)
				if (i % 2 != 0) { // line 1, 3, 5, 7 etc.
					Console.WriteLine ("\n" + lineElementString);

				} else {
			
					for (int n = 0; n < xCoordinateRange; n++) {
						Console.Write ("|" + " " + DrawFieldElements (nN+n) + " ");
					}

				nN+=xCoordinateRange;
				}

		} // draws the gamespace into the console

		public bool GameMove()
		{
			int checkInt;
			string userInput;
			int whosTurn = -1;
			for (int j = 0; j < xyFields.Length; j++)
			{
			for ( int i = 1; i <= playerCount; i++)
			{

				Console.WriteLine ("Round" + i );


				int activePlayer = ((i) % (playerCount-1));
				Console.Write (" " + playerData[(activePlayer), 0] + "'s turn!");
				Console.Write ("Please enter the field's number you wish to capture!");
				userInput = Console.ReadLine ();

				while (!(int.TryParse(userInput, out checkInt) && 
				         checkInt > 0 && 
				         (fieldStates[(checkInt-1),0] = true))) { // field cannot be captured unless it's a free field (owned by player zero "the system")

					Console.WriteLine ("fieldStates["+checkInt+",0] = "+fieldStates[checkInt,0]);

					Console.WriteLine ("ERROR - This field is not available!. [E:0007]");
					Console.WriteLine ("Again...which field would you like to capture??");
					userInput = Console.ReadLine ();

				}
				MarkFieldForPlayer(checkInt,activePlayer);
				Console.WriteLine ("Field" + checkInt + " " + "has been assigned to Player" + " " + playerData[(activePlayer),0]);

				if ( CheckWinCondition(activePlayer ,checkInt))
				{
					Console.WriteLine("Gratulations" + " " + playerData[(activePlayer),0] + ", you won!");
					Console.WriteLine(bigBar);
					Console.WriteLine("The Game will now re-initialize!"); // I'll consider putting a replay option here...but I cba right now
					Console.WriteLine(bigBar);
					Console.WriteLine("THANK YOU FOR PLAYING >> TIC TAC TOE PLUS PLUS << SEE YOU ANOTHER TIME!");
					Console.WriteLine(bigBar);
					Console.WriteLine(bigBar);
					return true;


				}
				DrawGame();
				}


			}
			Console.WriteLine("It's a DRAW! Try again later!(All fields are full, slap me if this is a false alarm!)");
			return false;
		}

		public GameLogic ()
		{
		} // constructor
	}
}

