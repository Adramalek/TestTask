using System.Collections.Generic;
using TestTask.Database.Entity;

namespace TestTask.Database
{
	public delegate IList<E> GenerationStrategy<E>() where E : AEntity;
}