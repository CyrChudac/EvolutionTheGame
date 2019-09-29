using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	class BackgroundEnvironment
	{
		public Layer<IMapTile> Map { get; }
		public Layer<Organism> OrganismsMap { get; }
		public List<List<Organism>> OrganismsList { get; }
	}
}
