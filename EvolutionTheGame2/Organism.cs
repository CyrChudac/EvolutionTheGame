using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionTheGame2.Interactions;

namespace EvolutionTheGame2
{
	public abstract class Organism
	{
		protected bool hasBeenAttacked { get; private set; } = false;
		public static readonly int DefaultStartingAgility = 25;
		internal void FakeConstructor(IEnvironment environment)
		{
			logic = new OrganismLogic(this, environment);
		}
		OrganismLogic logic;

		internal Direction lookingAt;
		internal Location Location;
		internal void InternalUpdate() => logic.OrgUpdate();
		internal int TimeCost { get; private set; }

		protected internal int Agility { get; internal set; } = DefaultStartingAgility;
		protected Health Health { get; private set; }
		protected abstract void Update();
		protected bool TryRegisterInteraction<T>(out T result) where T : Interaction, new()
			=> logic.TryRegisterInteraction<T>(out result);
		protected T RegisterInteraction<T>() where T : Interaction, new()
			=> logic.RegisterInteraction<T>();
		internal int Attacked(int strength, Organism attacker)
			=> logic.Attacked(strength, attacker);

		struct OrganismLogic
		{
			IEnvironment environment;
			List<Interaction> interactions;
			internal OrganismLogic(Organism o, IEnvironment environment)
			{
				interactions = new List<Interaction>();
				this.environment = environment;
				Organism = o;
				Organism.lookingAt = (Direction)new Random().Next(Enum.GetNames(typeof(Direction)).Length);
			}
			Organism Organism;
			public void OrgUpdate()
			{
				Organism.Update();
				Organism.hasBeenAttacked = false;
				foreach (Interaction i in interactions)
				{
					Organism.Agility -= i.ExistenceCost.Agility;
					Organism.TimeCost -= i.ExistenceCost.Time;
				}
			}
			public bool TryRegisterInteraction<T>(out T result) where T : Interaction, new()
			{
				result = new T();
				if (Organism.Agility > result.CreationCost.Agility)
				{
					SetInteraction(result);
					interactions.Add(result);
					return true;
				}
				else return false;
			}
			public T RegisterInteraction<T>() where T : Interaction, new()
			{
				T result = new T();
				if (Organism.Agility > result.CreationCost.Agility)
				{
					SetInteraction(result);
					return result;
				}
				else return null;
			}
			void SetInteraction(Interaction i)
			{
				i.FakeConstructor(environment, Organism);
				Organism.Agility -= i.CreationCost.Agility;
				Organism.TimeCost -= i.CreationCost.Time;
				interactions.Add(i);
			}
			public int Attacked(int strength, Organism attacker)
			{
				Organism.hasBeenAttacked = true;
				int defense = 0;
				int reflect = 0;
				foreach (var interaction in interactions)
					if (interaction is DefensiveInteractions)
					{
						defense += ((DefensiveInteractions)interaction).DefenseStrength;
						reflect += ((DefensiveInteractions)interaction).DamageReflection;
					}
				int damage = Math.Max(0, DefaultStartingAgility * (strength - defense) / 2);
				Organism.Agility -= damage;
				return damage - (DefaultStartingAgility * reflect / 4); 
			}
		}
	}

	
}
