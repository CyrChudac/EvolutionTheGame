using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionTheGame2
{
	static class IntExtensions
	{
		public static int Modulo(this int first, int second)
		{
			int result = first - ((first / second) * second);
			if (result < 0)
				return result + second;
			return result;
		}
	}
}
