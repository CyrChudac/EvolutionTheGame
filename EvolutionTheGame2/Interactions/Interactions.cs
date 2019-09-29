using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2.Interactions
{
	/// <summary>
	/// Abstract class - parent of the movement classes.
	/// </summary>
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

	/// <summary>
	/// The way how to move in the environment.
	/// </summary>
	public sealed class MoveForward : Movement
	{
		public new int UseCostForDistance(int distance)
			=> base.UseCostForDistance(distance);
		/// <returns>How many blocks did you actualy move</returns>
		public int Move(int distance)
		{
			if (distance > 0)
				return Move(distance, new Location(organism.lookingAt));
			else
				return 0;
		}
		public int Move() => Move(1);

		internal override void AntiChild() { }
	}

	/// <summary>
	/// Let's you move backward. It's always good to know when to retreat strategicaly.
	/// </summary>
	public sealed class MoveBackward : Movement
	{
		protected override float CreationModificator => 3f / 4;
		protected override float DestructModificator => 1f / 3;

		/// <returns>How many blocks did you actualy move</returns>
		public int Move()
			=> Move(1, new Location(organism.lookingAt) * (-1));

		internal override void AntiChild() { }
	}

	/// <summary>
	/// With this skill you may turn yourself in the environment.
	/// </summary>
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

		internal override void AntiChild() { }
	}

	/// <summary>
	/// Let's you bite an organism in front of you to damage it and get agility.
	/// </summary>
	public class Biting : Interaction
	{
		protected override float Modificator => 1.8f;

		public int UseCostForStrength(int strength)
			=> UseCostWithModificator(strength);

		public bool Bite(int strength)
		{
			return Use(
				() => {
					int? got = Environment.OrganismsMap[organism.Location +
						new Location(organism.lookingAt)]?.Attacked(strength, organism);
					if (got != null)
					{
						organism.Agility += (int)got;
						return true;
					}
					return false;
				},
				() => { return false; });

		}

		internal override void AntiChild() { }
	}

	/// <summary>
	/// Has actualy no agility cost. Instead it gives you half of it.
	/// </summary>
	public class Photosynthesis : DefensiveInteractions
	{
		public override int DefenseStrength => -1;
		public override int DamageReflection => -1;

		protected override float Modificator => 1;
		protected override float CreationModificator => 5;
		protected override float UseModificator => 4;
		protected override float DestructModificator => 0.3f;

		public void Photosynthesize()
		{
			organism.Agility += (UseCost * 1.5f).Agility;
			Use(() => { },
				() => { });
		}

		internal override void AntiChild() { }
	}

	/// <summary>
	/// Gives you a brief information about the surrounding environment.
	/// </summary>
	public class Eyesight : Interaction
	{
		protected override float Modificator => 1;

		/// <summary>
		/// Returns a ViewCone class. If anything goes wrong, then returns null instead.
		/// </summary>
		public ViewCone Look(int distance)
		{
			if (distance > 0)
			{
				distance += 2;
				return Use(() =>
				{
					IMapTile[,] field = new IMapTile[distance, distance];
					Location start;
					switch (organism.lookingAt)
					{
						case Direction.up:
							start = new Location(-distance / 2, -distance);
							break;
						case Direction.right:

							start = new Location(0, -distance / 2);
							break;
						case Direction.down:
							start = new Location(-distance / 2, 0);
							break;
						case Direction.left:
							start = new Location(-distance, -distance / 2);
							break;
						default: throw new NotImplementedException("Unknown direction type: " + organism.lookingAt.ToString());
					}
					for (int i = 0; i < distance; i++)
						for (int j = 0; j < distance; j++)
						{
							Location curr = new Location(organism.Location.X + start.X + i, organism.Location.Y + start.Y + j);
							if (Environment.OrganismsMap[curr] != null)
							{
								if (curr.Equals(organism.Location))
									field[i, j] = IMapTile.You;
								else if (Environment.OrganismsMap[curr].GetType() == organism.GetType())
									field[i, j] = IMapTile.AllyLife;
								else
									field[i, j] = IMapTile.EnemyLife;
							}
							else
								field[i, j] = Environment[curr];
						}

					IMapTile[,] newField = new IMapTile[distance, distance];
					for (int k = 0; k < (int)organism.lookingAt; k++)
					{
						for (int i = 0; i < distance; i++)
							for (int j = 0; j < distance; j++)
							{
								newField[i, j] = field[j, distance - i - 1];
							}
						field = newField;
					}
					return new ViewCone(field);
				},
				() => null,
				distance - 2);
			}
			else return null;
		}

		internal override void AntiChild() { }

		public class ViewCone
		{
			public ViewCone(IMapTile[,] field) 
				=> map = new Layer<IMapTile>(field);
			Layer<IMapTile> map;
			public IMapTile this [int x, int y] => map[new Location(x,y)];
		}
	}
}

