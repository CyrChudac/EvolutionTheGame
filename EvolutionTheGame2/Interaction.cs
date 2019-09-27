using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
    public abstract class Interaction
    {
		public Interaction()
		{
			CreationCost *= CreationModificator;
			ExistenceCost *= ExistenceModificator;
			DestructCost *= DestructModificator;
		}
		internal void FakeConstructor(IEnvironment environment, Organism o)
		{
			this.Environment = environment;
			this.organism = o;
		}

		protected Organism organism;
		internal IEnvironment Environment { get; private set; }
		protected virtual void DuringFakeCtor() { }
		public Cost CreationCost { get; protected set; } = Cost.DefaultCreationCost;
		public Cost ExistenceCost { get; protected set; } = Cost.DefaultExistenceCost;
		public Cost DestructCost { get; protected set; } = Cost.DefaultDestructCost;
		public abstract float CreationModificator { get; }
		public abstract float ExistenceModificator { get; }
		public abstract float DestructModificator { get; }

	}

	public sealed class MoveForward : Interaction
	{
		public override float CreationModificator => 1;
		public override float ExistenceModificator => 1;
		public override float DestructModificator => 2f / 3;

		public int CostForDistance(int distance)
			=> Organism.DefaultStartingAgility / 7 + (distance * Organism.DefaultStartingAgility / 15);

		/// <returns>How many blocks did you actualy moved</returns>
		public int Move(int distance)
		{
			if (organism.Agility < CostForDistance(distance))
			{

			}
			else
				return 0;
		}
	}
}
