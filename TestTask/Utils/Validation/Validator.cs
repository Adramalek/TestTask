using System;
using TestTask.Util;

namespace TestTask.Util.Validation
{
	public class Validator<T> where T : class
	{
		public event FailEvent onFail = delegate { };
		public event AcceptEvent onAccept = delegate { };

		public bool Validate(T arg, params T[] correctVariants)
		{
			bool correct = false;
			int var = 0;
			if (correctVariants != null && correctVariants.Length != 0)
			{
				for (int i = 0; i < correctVariants.Length; i++)
				{
					if (arg.Equals(correctVariants[i]))
					{
						correct = true;
						var = i;
						break;
					}
				}
			}
			else correct = true;
			if (!correct) onFail();
			else onAccept(var);
			return correct;
		}
	}
}
