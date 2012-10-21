using System;

namespace DSS_2012_10
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			for (int i = 0; i == i; i++) {
			
				GameLogic gameLogic = new GameLogic();
				gameLogic.AskGameSettings();
				gameLogic.HoistLineElements();
				gameLogic.DrawGame();
				gameLogic.GameMove();
			}
		}
	}
}
