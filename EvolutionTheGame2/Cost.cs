using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	public struct Cost
	{
		public Cost(int time, int agility)
		{
			Time = time;
			Agility = agility;
		}
		public int Time { get; private set; }
		public int Agility { get; private set; }
	}
}
