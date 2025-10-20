using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi {
	// Every move has a coördinate that is clicked and as amount of enemy pieces captured
	// The points variable is a algoritme used for a bot
	public struct Move {
		public int X, Y;
		public int Captures, Points;
		public Move(int x, int y, int captures, int points) : this() {
			X = x;
			Y = y;
			Captures = captures;
			Points = points;
		}
	}

	public struct PlayedMove {
		public int X, Y;
		public Player P;
		public PlayedMove(int x, int y, Player p) : this() {
			X = x;
			Y = y;
			P = p;
		}
	}

	public enum Player { Red = 1, Blue = 2, Tie = 3 } // Possible Players, just to make it more readable 
	public enum Direction {
		Up, Down, Left, Right, LeftUp, RightUp, LeftDown, RightDown
	} // Just to make it easier with names, number codes get confusing

	public class Board {
		private int[,] Tiles;
		private int Size;
		private readonly Dictionary<Direction, (int, int)> Directions = new Dictionary<Direction, (int, int)> {
			{ Direction.Up, (0 , -1) },
			{ Direction.Down, (0, 1) },
			{ Direction.Left, (-1, 0) },
			{ Direction.Right, (1, 0) },
			{ Direction.LeftUp, (-1, -1) },
			{ Direction.LeftDown, (-1, 1)},
			{ Direction.RightUp, (1, -1) },
			{ Direction.RightDown, (1, 1) }
		}; // Possible directions the stones can move
		  
		// Keep track of all the moves, for the move export function 
		public List<PlayedMove> PlayedMoves { get; private set; }

		public int GetSize { get { return Size; } }
		public int[,] GetTiles { get { return Tiles; } }
		public bool GameOver {
			get {
				if (RedStones + BlueStones == Size * Size) return true;
				return false;
			}
		}

		public int RedStones {
			get { return Tiles.Cast<int>().Count(Tile => Tile == (int)Player.Red); }
		}

		public int BlueStones {
			get { return Tiles.Cast<int>().Count(Tile => Tile == (int)Player.Blue); }
		}

		public Board() {
			Size = 6;
			InitBoard();
		}
		// Create a new bord, and set the middle pieces
		private void InitBoard() {
			Tiles = new int[Size, Size];
			Tiles[Size / 2, Size / 2] = (int)Player.Red;
			Tiles[Size / 2 - 1, Size / 2] = (int)Player.Blue;
			Tiles[Size / 2, Size / 2 - 1] = (int)Player.Blue;
			Tiles[Size / 2 - 1, Size / 2 - 1] = (int)Player.Red;
			PlayedMoves = new List<PlayedMove>() { };
		}
		// Chnage board size, this means we start again
		public void SetSize(int InputSize) {
			Size = InputSize;
			InitBoard();
		}
		// Convert coördinate to player or empty
		private int GetTileContent(Point t) {
			return Tiles[t.Y, t.X];
		}
		// Every tile is awarded points based on its location, eg corners are better since they can't be taken
		// But you don't want to give the corners away, so the tiles around it are negative
		private int GetTilePoints(Point t) {
			int x = t.X;
			int y = t.Y;
			if (new[] { (0, 0), (0, Size - 1), (Size - 1, 0), (Size - 1, Size - 1) }.Contains((x, y))) return 100; // Corners
			if (x == 1 && new[] { 0, 1, Size - 1, Size - 2 }.Contains(y)) return -50; // Next to corner
			if (x == Size - 2 && new[] { 0, 1, Size - 1, Size - 2 }.Contains(y)) return -50; // Next to corner
			if (y == 1 && new[] { 0, 1, Size - 1, Size - 2 }.Contains(x)) return -50; // Next to corner
			if (y == Size - 2 && new[] { 0, 1, Size - 1, Size - 2 }.Contains(x)) return -50; // Next to corner
			if (x == 0 || x == Size - 1 || y == 0 || y == Size - 1) return 30; // Edge pieces
			if (x == 1 || x == Size - 2 || y == 1 || y == Size - 2) return -10; // Pieces adjacent to edge pieces
			return 10; // Normal tile
		}
		// Check if the coördinates are in the board, otherwise we are checking a invalid direction
		private bool InBounds(Point p) {
			if (p.X < 0 || p.X >= Size) return false;
			if (p.Y < 0 || p.Y >= Size) return false;
			return true;
		}
		// Check the coördinates of a direction
		private Point DirCheck(Point p, (int, int) dir) {
			return new Point(p.X + dir.Item1, p.Y + dir.Item2);
		}

		// Is called for every direction, checks if the stone can be moved this way
		private void GetDirectionMoves((int, int) Offset, Point Pos, List<Move> result, Player p) {
			int OppositePlayer = (int)((p == Player.Red) ? Player.Blue : Player.Red); // The enemy player
			int Captures = 0; int Points = 0; // Statistics used for bots and algorithms
			Point iter = DirCheck(Pos, Offset); // Start from the next tile
			while (InBounds(iter)) { // As long as we are in the board keep going
				int content = GetTileContent(iter);
				// You can't capture yourself so this is not a valid move
				if (content == (int)p) break;
				// Enemy player seen 
				if (content == OppositePlayer) {
					Points += GetTilePoints(iter);
					Captures++;
				}
				// An empty space, the piece can move here if we capture atleast on piece
				else if (content == 0 && Captures != 0) {
					Points += GetTilePoints(iter);
					result.Add(new Move(iter.X, iter.Y, Captures, Points));
					break;
				}
				// If the space is empty and we can't capture an enemy piece, this direction has no valid move
				else if (content == 0 && Captures == 0) {
					break;
				}
				iter = DirCheck(iter, Offset);
			}
		}

		// For every tile, we check if it is a piece of the player, 
		// If it is we iterate thru every direction to see if it is a valid move
		public List<Move> GetPossibleMoves(Player p) {
			List<Move> Result = new List<Move>();

			ParallelLoopResult res = Parallel.For(0, Size, j => {
				for (int i = 0; i < Size; i++) {
					if (Tiles[j, i] != (int)p) continue;
					foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
						Directions.TryGetValue(dir, out (int, int) Offset);
						GetDirectionMoves(Offset, new Point(i, j), Result, p);
					}
				}
			});
			return Result.GroupBy(m => new { m.X, m.Y })
				.Select(g => new Move(g.Key.X, g.Key.Y, g.Sum(m => m.Captures), g.Sum(m => m.Points)))
				.ToList();
			// Return all the directions, since some moves can be duplicate, we need to combine them
		}
		// Looks a bit like GetDirectionMoves(), iterate thru every direction
		public void Move(int X, int Y, Player p) {
			PlayedMoves.Add(new PlayedMove(X, Y, p));
			List<Point> Changes = new List<Point>();
			int OppositePlayer = (int)((p == Player.Red) ? Player.Blue : Player.Red);
			foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
				List<Point> Temp = new List<Point>();
				Directions.TryGetValue(dir, out (int, int) Offset);
				Point iter = DirCheck(new Point(X, Y), Offset);
				int OppSeen = 0;
				while (InBounds(iter)) {
					int Content = GetTileContent(iter);
					if (Content == (int)p && OppSeen == 1) {
						Changes.AddRange(Temp);
						break;
					}
					if (Content == OppositePlayer) {
						OppSeen = 1;
						Temp.Add(iter);
					}
					else if (Content == 0) {
						break;
					}
					iter = DirCheck(iter, Offset);
				}
			}
			foreach (Point Change in Changes) {
				Tiles[Change.Y, Change.X] = (int)p;
			}
			Tiles[Y, X] = (int)p;
		}
		// Defined in the class, so we don't make a new one every time
		// It also prevents same seed instantes
		public Random rnd = new Random();
		public void ComputerMove(int Level, Player p) {
			List<Move> moves = GetPossibleMoves(p);
			if (moves.Count == 0) return;
			switch (Level) {
				case 0: // Best move based on points
					List<Move> movesPoints = moves.OrderByDescending(moveP => moveP.Points).ToList();
					Move(movesPoints[0].X, movesPoints[0].Y, p);
					break;
				case 1: // Best move based on captures
					List<Move> movesCaptures = moves.OrderByDescending(moveC => moveC.Captures).ToList();
					Move(movesCaptures[0].X, movesCaptures[0].Y, p);
					break;
				case 2:
				default: // Random move 
					int move = rnd.Next(moves.Count);
					Move(moves[move].X, moves[move].Y, p);
					break;
			}
		}
	}
}
