using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	interface IEnvironment
	{
		Layer<IMapTile> Map { get; }
		Layer<Organism> OrganismsMap { get; }
		void AddOrganism(Organism o, int aftermiliseconds);
		IMapTile this[Location l] { get; }
		IMapTile this[int x, int y] { get; }
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

		public IMapTile this[int x, int y] => Map[x,y];
		public IMapTile this [Location l] => Map[l];

		List<Organism> Organisms;

		public Layer<IMapTile> Map { get; private set; }

		public Layer<Organism> OrganismsMap { get; private set; }

		public void AddOrganism(Organism o, int aftermiliseconds)
		{
			throw new NotImplementedException();
		}
	}

	struct Layer<T>
	{
		public Layer(T[,] field)
			=> layer = field;
		T[,] layer { get; }
		public T this[int x, int y]
		{
			get => layer[x.Modulo(layer.GetLength(0)), y.Modulo(layer.GetLength(1))];
			set => layer[x.Modulo(layer.GetLength(0)), y.Modulo(layer.GetLength(1))] = value;
		}
		public T this [Location l]
		{
			get => this[l.X, l.Y];
			set => this[l.X, l.Y] = value;
		}
	}
}
