using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionTheGame2;
using EvolutionTheGame2.Interactions;

namespace TryingOrganisms
{
	public class Agressive : Organism
	{
		bool firstRun = true;
		Biting biting;
		Eyesight eyes;
		MoveForward move;
		Turning turn;
		protected override void Update()
		{
			if (firstRun)
			{
				biting = RegisterInteraction<Biting>();
				eyes = RegisterInteraction<Eyesight>();
				move = RegisterInteraction<MoveForward>();
				turn = RegisterInteraction<Turning>();
			}

		}
	}
}
