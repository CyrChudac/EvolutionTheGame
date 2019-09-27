using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
    public abstract class Interaction
    {
		internal abstract void FakeConstructor(IEnvironment environment);
		public abstract Cost CreationCost { get; protected set; }
		public abstract Cost ExistenceCost { get; protected set; }
		public abstract Cost DestructCost { get; protected set; }
	}
}
