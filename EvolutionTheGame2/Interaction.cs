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
		public Cost UseCost { get; protected set; } = Cost.DefaultUseCost; 
		public Cost CreationCost { get; protected set; } = Cost.DefaultCreationCost;
		public Cost ExistenceCost { get; protected set; } = Cost.DefaultExistenceCost;
		public Cost DestructCost { get; protected set; } = Cost.DefaultDestructCost;
		protected virtual float UseModificator => Modificator;
		protected virtual float CreationModificator => Modificator;
		protected virtual float ExistenceModificator => Modificator;
		protected virtual float DestructModificator => Modificator;
		protected abstract float Modificator { get; }

	}

	public abstract class Movement : Interaction
	{
		protected override float Modificator => 1;

		protected int CostForDistance(int distance)
			=> Organism.DefaultStartingAgility / 7 + (distance * Organism.DefaultStartingAgility / 15);

		/// <returns>How many blocks did you actualy move</returns>
		internal int Move(int distance, Location l)
		{
			int moved = 0;
			if (organism.Agility < CostForDistance(distance))
			{
				Location newLoc = organism.Location;
				while (Environment[newLoc] == IMapTile.Nothing)
				{
					newLoc += l;
					moved++;
				}

				organism.Agility -= CostForDistance(distance);
				organism.Location = newLoc;
			}
			return moved;
		}
	}

	public sealed class MoveForward : Movement
	{
		public new int CostForDistance(int distance)
			=> base.CostForDistance(distance);
		/// <returns>How many blocks did you actualy move</returns>
		public int Move(int distance)
			=> Move(distance, new Location(organism.lookingAt));
	}

	public sealed class MoveBackward : Movement
	{
		protected override float CreationModificator => 3f / 4;
		protected override float DestructModificator => 1f / 3;

		public int Cost() => CostForDistance(1);

		/// <returns>How many blocks did you actualy move</returns>
		public int Move()
			=> Move(1, new Location(organism.lookingAt) * (-1));
	}

	public class Turning : Interaction
	{
		protected override float Modificator => 1f / 2;

		public void TurnLeft()
			=> organism.lookingAt = (Direction)((int)organism.lookingAt - 1).Modulo(Enum.GetNames(typeof(Direction)).Length);
		public void TurnRight()
			=> organism.lookingAt = (Direction)((int)organism.lookingAt + 1).Modulo(Enum.GetNames(typeof(Direction)).Length);
	}
}
