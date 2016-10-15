using System;

namespace TestTask.Database.Entity
{
	public abstract class AEntity
	{
		protected int id;

		public virtual int ID { get; set; }
	}
}
