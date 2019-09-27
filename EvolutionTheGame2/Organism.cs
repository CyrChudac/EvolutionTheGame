using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	public abstract class Organism
	{
		internal Organism(IEnvironment environment)
		{
			logic = new OrganismLogic(this, environment);
		}
		readonly OrganismLogic logic;

		internal void InternalUpdate() => logic.OrgUpdate();
		internal int TimeCost { get; private set; }

		protected Health Health { get; private set; }
		protected abstract void Update();
		protected T RegisterInteraction<T>() where T : Interaction, new()
			=> logic.RegisterInteraction<T>();

		struct OrganismLogic
		{
			IEnvironment environment;
			List<Interaction> interactions;
			internal OrganismLogic(Organism o, IEnvironment environment)
			{
				interactions = new List<Interaction>();
				this.environment = environment;
				Organism = o;
			}
			Organism Organism;
			public void OrgUpdate()
			{
				Organism.Update();
			}
			public T RegisterInteraction<T>() where T : Interaction, new()
			{
				T t = new T();
				t.FakeConstructor(environment);
				interactions.Add(t);
				return t;
			}
		}
	}

	
}
