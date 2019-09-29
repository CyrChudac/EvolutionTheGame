using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	/// <summary>
	/// Interactions let's you, obviously, to interact with the environment. 
	/// </summary>
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

		internal abstract void AntiChild();

		internal IEnvironment Environment { get; private set; }

		protected Organism organism;
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
			=> UseCost.Agility + ((i - 1) * UseCost.Agility / 2);

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