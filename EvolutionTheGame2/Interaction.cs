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
			UseCost *= UseModificator;
			CreationCost *= CreationModificator;
			ExistenceCost *= ExistenceModificator;
			DestructCost *= DestructModificator;
		}
		internal void FakeConstructor(IEnvironment environment, Organism o)
		{
			this.Environment = environment;
			this.organism = o;
			DuringFakeCtor();
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

		protected int UseCostWithModificator(int i)
			=> UseCost.Agility + (i * UseCost.Agility / 2);

		protected void Use(Action use, Action defaultAction, int useCostModificator = 1)
		 => Use(ActionToFunc.ToFunc(use), ActionToFunc.ToFunc(defaultAction), useCostModificator);

		protected T Use<T>(Func<T> useFunction, Func<T> defaultFunc, int useCostModificator = 1)
		{
			if (organism.Agility >= UseCostWithModificator(useCostModificator))
			{
				organism.Agility -= UseCostWithModificator(useCostModificator);
				return useFunction();
			}
			return defaultFunc();
		}
	}

	public abstract class Movement : Interaction
	{
		internal Movement() { }
		protected override float Modificator => 1;

		protected int UseCostForDistance(int distance)
			=> UseCostWithModificator(distance);

		/// <returns>How many blocks did you actualy move</returns>
		internal int Move(int distance, Location l)
		{
			return Use(
				() =>
				{
					int moved = 0;
					Location newLoc = organism.Location;
					while (Environment[newLoc] == IMapTile.Nothing)
					{
						newLoc += l;
						moved++;
					}

					organism.Location = newLoc;
					return moved;
				},
				() => 0,
				distance);
		}
	}

	public sealed class MoveForward : Movement
	{
		public new int UseCostForDistance(int distance)
			=> base.UseCostForDistance(distance);
		/// <returns>How many blocks did you actualy move</returns>
		public int Move(int distance)
			=> Move(distance, new Location(organism.lookingAt));
	}

	public sealed class MoveBackward : Movement
	{
		protected override float CreationModificator => 3f / 4;
		protected override float DestructModificator => 1f / 3;

		/// <returns>How many blocks did you actualy move</returns>
		public int Move()
			=> Move(1, new Location(organism.lookingAt) * (-1));
	}

	public class Turning : Interaction
	{
		protected override float Modificator => 1f / 2;

		void Turn(int shift)
		{
			Use(() =>
			   organism.lookingAt = (Direction)((int)organism.lookingAt - shift).Modulo(Enum.GetNames(typeof(Direction)).Length),
			   () => Direction.down);
		}

		public void TurnLeft()
			=> Turn(-1);
		public void TurnRight()
			=> Turn(1);
	}

	public class Biting : Interaction
	{
		protected override float Modificator => 1.8f;

		public int UseCostForStrength(int strength)
			=> UseCostWithModificator(strength);

		public bool Bite(int strength)
		{
			return Use(
				() => {
					Environment.OrganismsMap[organism.Location + new Location(organism.lookingAt)]?.Attacked(strength);
					return true;
				},
				() => { return false; });

		}
	}

	static class ActionToFunc
	{
		public static Func<bool> ToFunc(Action a)
		 => new Func() { a = a }.Use;

		struct Func
		{
			public Action a;
			public bool Use()
			{
				a();
				return true;
			}
		}

		public static Action ToAction<T>(Func<T> f)
			=> () => f();
	}
}
