using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2.Interactions
{
	public abstract class DefensiveInteractions : Interaction
	{
		public abstract int DefenseStrength { get; }
		public abstract int DamageReflection { get; }
	}

	public sealed class Shell : DefensiveInteractions
	{
		internal override void AntiChild() { }
		public override int DefenseStrength => 1;
		public override int DamageReflection => 0;
		protected override float Modificator => 1;
	}

	public sealed class Spines : DefensiveInteractions
	{
		internal override void AntiChild() { }
		public override int DefenseStrength => 0;
		public override int DamageReflection => 1;
		protected override float Modificator => 0.75f;
	}
	public sealed class Crust : DefensiveInteractions
	{
		internal override void AntiChild() { }
		public override int DefenseStrength => 2;
		public override int DamageReflection => 0;
		protected override float Modificator => 1.5f;
	}

	public sealed class ThornyCrust : DefensiveInteractions
	{
		internal override void AntiChild() { }
		public override int DefenseStrength => 2;
		public override int DamageReflection => 2;
		protected override float Modificator => 2;
	}
}
