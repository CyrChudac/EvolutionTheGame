using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	class BackgroundEnvironment
	{
		public IMapTile[,] Map { get; }
		public Organism[,] OrganismsMap { get; }
		public List<List<Organism>> OrganismsList { get; }
	}
}
