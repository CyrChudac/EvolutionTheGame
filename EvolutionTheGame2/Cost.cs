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
		static Cost()
		{
			int defaultTime = 15;
			int defaultAgility = Organism.DefaultStartingAgility * 2 / 9;
			DefaultCreationCost = new Cost(defaultTime, defaultAgility);
			DefaultExistenceCost = new Cost(0, defaultAgility / 3);
			DefaultDestructCost = new Cost(defaultTime / 2, defaultAgility * 3 / 4);
			DefaultUseCost = new Cost(defaultTime * 3 / 4, defaultAgility / 3);

		}
		public static readonly Cost DefaultCreationCost;
		public static readonly Cost DefaultExistenceCost;
		public static readonly Cost DefaultDestructCost;
		public static readonly Cost DefaultUseCost;
		public int Time { get; private set; }
		public int Agility { get; private set; }

		public static Cost operator *(Cost a, int b)
			=> new Cost(a.Time * b, a.Agility * b);
		public static Cost operator *(int b, Cost a)
			=> a * b;
		public static Cost operator *(Cost a, float b)
			=> new Cost((int)(a.Time * b), (int)(a.Agility * b));
		public static Cost operator *(float b, Cost a)
			=> a * b;
	}
}
