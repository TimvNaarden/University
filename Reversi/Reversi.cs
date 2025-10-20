using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Reversi {
	public partial class Reversi : Form {
		// Board Size, Use in width calculations!
		private static Size S = new Size(400, 400);
		private Label BoardDisplay;

		// Alternating Player (Red, Blue)
		private Player ActivePlayer = Player.Red;

		// For the log, display the winner
		private Player LastWinner = Player.Red;

		// Game board, has it's own methods for functions
		private readonly Board B = new Board();

		// Current Settings
		private int Mode = 3;
		// Selected options for next game
		private int NextMode = 3;
		private int NextSize = 6;

		// When help is clicked turn to 1
		// Possible moves are then rendered in the paint function
		private int HelpClicked = 0;

		private int BlueWins = 0;
		private int RedWins = 0;
		private int Ties = 0;

		// Inputs, Only one of each can be checked
		List<CheckBox> Opponents;
		List<CheckBox> Sizes;
		private void Init() {
			// VS Designer
			InitializeComponent();

			// Checkboxes 
			Opponents = new List<CheckBox>() { Level1CheckBox, Level2CheckBox, Level3CheckBox, MultiplayerCheckBox };
			Sizes = new List<CheckBox>() { SizeCheckBox6, SizeCheckBox8, SizeCheckBox10, SizeCheckBoxC };
			foreach (CheckBox x in Opponents) {
				x.CheckedChanged += OpponentChanged;
			}
			foreach (CheckBox x in Sizes) {
				x.CheckedChanged += SizeInputChanged;
			}
			// Button functions 
			NewGameButton.Click += NewGameClicked;
			HelpButton.Click += FormHelpButtonClicked;

			ShowCaptures.Click += (object o, EventArgs ea) => { BoardDisplay.Invalidate(); };

			// Label or 'display'
			BoardDisplay = new Label();
			Controls.Add(BoardDisplay);
			BoardDisplay.Size = S;
			BoardDisplay.Paint += DisplayPaintEvent;
			BoardDisplay.MouseDown += DisplayClickEvent;
			BoardDisplay.Location = new Point(100, 150);

			// Draw the initial state
			BoardDisplay.Invalidate();
		}
		public Reversi() {
			Init();
			// Logic to test moves, debug only
			/*
			for (int i = 0; i < 15; i++) {
				b.ComputerMove(1, Player.Red);
				b.ComputerMove(1, Player.Blue);
			}
			b.ComputerMove(1, Player.Blue);
			BoardDisplay.Invalidate();
			*/
		}
		#region EventHandlers
		public void OpponentChanged(object sender, EventArgs ea) {
			foreach (CheckBox x in Opponents) {
				// Set every checkbox to false except the one clicked
				if (x != sender) {
					// Setting .Checked properties creates an event, which we don't want
					x.CheckedChanged -= OpponentChanged;
					x.Checked = false;
					x.CheckedChanged += OpponentChanged;
				}
				else NextMode = Opponents.IndexOf(x);
			}
		}
		public void SizeInputChanged(object sender, EventArgs ea) {
			// Every Checkbox has an other value, can't work with index
			if (sender == SizeCheckBox6) NextSize = 6;
			if (sender == SizeCheckBox8) NextSize = 8;
			if (sender == SizeCheckBox10) NextSize = 10;
			// Set every checkbox to false except the one clicked
			foreach (CheckBox x in Sizes) {
				if (x != sender) {
					// Setting .Checked properties creates an event, which we don't want
					x.CheckedChanged -= SizeInputChanged;
					x.Checked = false;
					x.CheckedChanged += SizeInputChanged;
				}
			}
		}
		public void NewGameClicked(object sender, EventArgs ea) {
			ActivePlayer = Player.Red;
			// Custom size input
			if (SizeCheckBoxC.Checked) {
				try {
					if (!int.TryParse(SizeTextBox.Text, out int InputSize)) {
						throw new Exception("Geen geldig nummer");
					}
					else if (InputSize % 2 != 0) {
						throw new Exception("Nummer moet een veelvoud van 2 zijn");
					}
					B.SetSize(InputSize); // Size, this also recreates the board
				}
				catch (Exception e) {
					MessageBox.Show(e.Message, "Er is iets foutgegaan");
				}
			}
			else {
				B.SetSize(NextSize); // Size, this also recreates the board
			}
			Mode = NextMode;
			BoardDisplay.Invalidate();
		}

		// Little bit weird name, but HelpButtonClicked is a function name for the form itself
		public void FormHelpButtonClicked(Object sender, EventArgs ea) {
			HelpClicked = (HelpClicked == 1) ? 0 : 1;
			BoardDisplay.Invalidate();
		}

		private void DisplayPaintEvent(object sender, PaintEventArgs pea) {
			// Basic draw functions to display the bord
			DrawRaster(pea.Graphics);
			DrawTiles(pea.Graphics, B.GetTiles);
			// Show the stones every player has and which player's mpve it is
			RedStones.Text = B.RedStones.ToString() + " Stenen";
			BlueStones.Text = B.BlueStones.ToString() + " Stenen";
			string Speler = (ActivePlayer == Player.Red) ? "Rode" : "Blauwe";
			TurnLabel.Text = Speler + " speler aan zet";
			TurnLabel.ForeColor = (ActivePlayer == Player.Red) ? Color.Red : Color.Blue;
			// Good gramer is important
			if (B.PlayedMoves.Count() == 1) MovesPlayed.Text = B.PlayedMoves.Count().ToString() + " zet gedaan";
			else MovesPlayed.Text = B.PlayedMoves.Count().ToString() + " zetten gedaan";

			// Suggestion Moves
			if (HelpClicked == 1) DrawHelp(pea.Graphics);
		}
		private void PlayerMove(int TileX, int TileY) {
			if (B.GetPossibleMoves(ActivePlayer).Count(m => m.X == TileX && m.Y == TileY) > 0) {
				B.Move(TileX, TileY, ActivePlayer);
				ActivePlayer = (ActivePlayer == Player.Red) ? Player.Blue : Player.Red; // Switch players
				if (!B.GameOver && B.GetPossibleMoves(ActivePlayer).Count() == 0) { // Check if next player can move
					ActivePlayer = (ActivePlayer == Player.Red) ? Player.Blue : Player.Red; // Otherwise switch players again
					if (B.GetPossibleMoves(ActivePlayer).Count() == 0) { // If the other player also has no moves, the game ends 
						CheckGameOver(true);
					}
				}
			}
		}

		private bool ComputerMove() {
			B.ComputerMove(Mode, Player.Blue); // Computer is always the second player, so blue
			ActivePlayer = Player.Red; // Switch players after move, since computer is always blue, we now it is red now
			BoardDisplay.Refresh(); // Update the bord before checking for an end 
			CheckGameOver();
			if (!B.GameOver && B.GetPossibleMoves(ActivePlayer).Count() == 0) { // Check if player can do a move 
				ActivePlayer = Player.Blue; // Player can't move so we check for the other player(computer)
				if (B.GetPossibleMoves(ActivePlayer).Count() == 0) {
					CheckGameOver(true);
				}
				return true; // Computer needs to do a set
			}
			return false;
		}

		private async void DisplayClickEvent(object sender, MouseEventArgs mea) {
			// Player tried to do computer move
			if (Mode != 3 && ActivePlayer == Player.Blue) return;
			// Calculate the tile clicked
			var Rect = BoardDisplay.ClientRectangle;
			float TileSize = (float)Rect.Width / B.GetSize;
			int TileX = (int)(mea.X / TileSize);
			int TileY = (int)(mea.Y / TileSize);
			PlayerMove(TileX, TileY); // Try the move
			BoardDisplay.Refresh(); // Show it first, before checking for a possible game end
			CheckGameOver();

			// Computer logic
			if (Mode != 3 && ActivePlayer == Player.Blue) { // Mode 3 is the only not computer mode
				do {
					await Task.Delay(1000); // Wait a littlie bit, it is very confusing if the computer plays instant
				}
				while (ComputerMove()); // Do first since we want to have the delay first 
			}
		}
		#endregion
		#region Drawfunctions
		// Draw the basic bord based on the board size, should be pretty simple
		public void DrawRaster(Graphics g) {
			var Rect = BoardDisplay.ClientRectangle;
			float TileSize = (float)Rect.Width / B.GetSize;

			using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
				Rect, Color.DarkGreen, Color.ForestGreen, 45f)) {
				g.FillRectangle(brush, Rect);
			}

			using (var p = new Pen(Color.Black, 1f)) {
				for (int i = 0; i <= B.GetSize; i++) {
					float pos = i * TileSize;
					g.DrawLine(p, Rect.Left + pos, Rect.Top, Rect.Left + pos, Rect.Top + Rect.Height);
					g.DrawLine(p, Rect.Left, Rect.Top + pos, Rect.Left + Rect.Width, Rect.Top + pos);
				}
				g.DrawLine(p, Rect.Right - 1, Rect.Top, Rect.Right - 1, Rect.Bottom);
				g.DrawLine(p, Rect.Left, Rect.Bottom - 1, Rect.Right, Rect.Bottom - 1);
			}
		}
		private void DrawHelp(Graphics g) {
			Pen p = (ActivePlayer == Player.Red) ? Pens.Red : Pens.Blue; // Draw with color of the active player
			var Rect = BoardDisplay.ClientRectangle;
			float TileSize = (float)Rect.Width / B.GetSize;
			float Diameter = TileSize * 0.8f;
			foreach (Move m in B.GetPossibleMoves(ActivePlayer)) {
				float cx = Rect.Left + m.X * TileSize + TileSize / 2f;
				float cy = Rect.Top + m.Y * TileSize + TileSize / 2f;
				float left = cx - Diameter / 2f;
				float top = cy - Diameter / 2f;
				g.DrawEllipse(p, left, top, Diameter, Diameter); // Draw not fill, possible moves are not filled in
				if (ShowCaptures.Checked) {
					float scale = (TileSize * 0.7f) / g.MeasureString(m.Captures.ToString(), DefaultFont).Width; // Measure how much bigger it needs to be
					RectangleF Tile = new RectangleF((left + TileSize * 0.05f) / scale, top / scale, TileSize * 0.7f, TileSize * 0.7f); // The size of thre rectangle with some margins and padding
					g.ScaleTransform(scale, scale); // X and Y are scaled equally since it is a square
					g.DrawString(m.Captures.ToString(), DefaultFont, Brushes.Black, Tile);
					g.ResetTransform(); // Reset to original, otherwise the other drawings still use this scale
				}
			}
		}
		public void DrawTiles(Graphics g, int[,] tiles) {
			var Rect = BoardDisplay.ClientRectangle;
			float TileSize = (float)Rect.Width / B.GetSize;
			float Diameter = TileSize * 0.8f;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Makes the circles better, in my opinion

			for (int y = 0; y < B.GetSize; y++) {
				for (int x = 0; x < B.GetSize; x++) {
					int tile = tiles[y, x];
					if (tile == 0) continue; // Empty tile
					float cx = Rect.Left + x * TileSize + TileSize / 2f;
					float cy = Rect.Top + y * TileSize + TileSize / 2f;
					float left = cx - Diameter / 2f;
					float top = cy - Diameter / 2f;
					using (var brush = new SolidBrush(tile == (int)Player.Red ? Color.Red : Color.Blue)) { // Player stones are colored in, so brush
						g.FillEllipse(brush, left, top, Diameter, Diameter);
					}
				}
			}
		}
		#endregion
		private void CheckGameOver(bool OverrideCheck = false) {
			if (B.GameOver || OverrideCheck) {
				if (B.RedStones > B.BlueStones) {
					LastWinner = Player.Red;
					RedWins++;
					ResultBox(Player.Red, 0);
					//Speler rood heeft gewonnen
				}
				else if (B.RedStones < B.BlueStones) {
					LastWinner = Player.Blue;
					BlueWins++;
					ResultBox(Player.Blue, 0);
					//Speler blauw heeft gewonnen
				}
				else {
					LastWinner = Player.Tie;
					Ties++;
					ResultBox(Player.Red, 1);
					//Gelijkspel
				}
			}
		}
		// Ask if they want to play another game
		private void ResultBox(Player Winner, int Tie) {
			// Update Scoreboard
			// Only needs to happen after a result
			// So it is better to do it here
			RedWinsLabel.Text = "Rood Gewonnen: " + RedWins.ToString();
			BlueWinsLabel.Text = "Blauw Gewonnen: " + BlueWins.ToString();
			TieLabel.Text = "Gelijkspel: " + Ties.ToString();
			DialogResult res;
			if (Tie == 1) res = MessageBox.Show("Gelijkspel\nNog een potje?", "Spel afgelopen", MessageBoxButtons.YesNo);
			else {
				string Win = (Winner == Player.Red) ? "Rode" : "Blauwe";
				res = MessageBox.Show(Win + " speler heeft gewonnen.\nNog een potje?", "Spel afgelopen", MessageBoxButtons.YesNo);
				BoardDisplay.Invalidate();
			}
			if (res == DialogResult.Yes) {
				B.SetSize(6);
				BoardDisplay.Invalidate();
			}
		}

		private void ExportLogButtonClick(object sender, EventArgs e) {
			SaveFileDialog f = new SaveFileDialog() {
				Filter = "Text file| *.txt",
				Title = "Exporteer Spelverslag"
			};
			f.ShowDialog();

			if (f.FileName != "") {
				StreamWriter ws = new StreamWriter(Path.GetFullPath(f.FileName));
				string Opponent = (Mode != 3) ? Opponents[Mode].Text : "medespeler";
				ws.WriteLine($"Speler tegen {Opponent}");
				foreach (PlayedMove m in B.PlayedMoves) {
					string Speler = (m.P == Player.Blue) ? "Blauwe Speler" : "Rode Speler";
					ws.WriteLine($"{Speler} heeft {{{m.X},{m.Y}}} ingenomen");
				}
				string Winner = (LastWinner == Player.Red) ? "Rode Speler" : "Blauwe Speler";
				if (LastWinner == Player.Tie) {
					ws.WriteLine($"Het spel is geëindigd in een gelijkspel. Blauwe stenen: {B.BlueStones}, Rode stenen: {B.RedStones}");
				}
				else ws.WriteLine($"{Winner} Heeft gewonnen. Blauwe stenen: {B.BlueStones}, Rode stenen: {B.RedStones}");
				ws.Close();
				MessageBox.Show("Spelverslag succesvol geëxporteerd", "Exporteer Spelverslag");
			}
		}
	}
}