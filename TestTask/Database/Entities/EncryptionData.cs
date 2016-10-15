using System;

namespace TestTask.Database.Entity
{
	public class EncryptionData : AEntity
	{
		private string oldSymbol;
		private string newSymbol;

		public virtual string OldSymbol { get; set; }
		public virtual string NewSymbol { get; set; }
	}
}
