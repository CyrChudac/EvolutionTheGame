using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	interface IEnvironment
	{
		IMapTile[,] Map { get; }
		Organism[,] OrganismsMap { get; }
		void AddOrganism(Organism o, int aftermiliseconds);
	}

	class Environment : IEnvironment
	{
		public Environment(BackgroundEnvironment environment)
		{
			Map = environment.Map;
			OrganismsMap = environment.OrganismsMap;
			Organisms = new List<Organism>();
			environment.OrganismsList.Add(Organisms);
		}

		List<Organism> Organisms;

		public IMapTile[,] Map { get; private set; }

		public Organism[,] OrganismsMap { get; private set; }

		public void AddOrganism(Organism o, int aftermiliseconds)
		{
			throw new NotImplementedException();
		}
	}
}
