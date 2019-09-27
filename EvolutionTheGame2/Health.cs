using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	public struct Health
	{
		public Health(int maximal)
		{
			Current = maximal;
			Max = maximal;
		}
		public Health(int maximal, int current)
		{
			Max = maximal;
			Current = current;
		}
		public int Current { get; private set; }
		public int Max { get; private set; }
	}
}
