using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	struct Location
	{
		public Location (int x, int y)
		{
			X = x;
			Y = y;
		}
		public Location(Direction d)
		{
			switch (d)
			{
				case Direction.up:
					X = 0;
					Y = -1;
						break;
				case Direction.down:
					X = 0;
					Y = 1;
					break;
				case Direction.left:
					X = -1;
					Y = 0;
					break;
				case Direction.right:
					X = 1;
					Y = 0;
					break;
				default: throw new NotImplementedException("Direction of unknown type casted to Location : " + d.ToString());
			}
		}
		public int X { get; private set; }
		public int Y { get; private set; }

		public static Location operator +(Location a, Location b)
			=> new Location(a.X + b.X, a.Y + b.Y);
		public static Location operator *(Location a, int b)
			=> new Location(a.X * b, a.Y * b);
		public static Location operator *(int b, Location a)
			=> a * b;
	}
}
