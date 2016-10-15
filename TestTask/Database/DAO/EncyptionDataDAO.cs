using System;
using System.Collections.Generic;
using TestTask.Database.Entity;
using NHibernate;

namespace TestTask.Database.DAO
{
	public class EncyptionDataDAO : ADAOEntity<EncryptionData, int>
	{
		public EncyptionDataDAO(ISession session) : base (session){}

		private string GenerateQuery(QueryType type, string propertyName) {
			if (ContainsProperty(propertyName) == false) throw new ArgumentException();
			string query = "";
			switch (type) {
				case QueryType.SELECT:
					query = "select * from EncryptionData where "+propertyName+"=:p1";
					break;
				case QueryType.UPDATE:
					query = "Update EncryptionData set oldSymbol=:p1, newSymbol=:p2 where "+propertyName+"=:p3";
					break;
				case QueryType.DELETE:
					query = "delete from EncryptionData where "+propertyName+"=:p1";
					break;
			}
			return query;
		}

		#region Override
		public override bool ContainsProperty(string propertyName) {
			return propertyName.Equals("oldSymbol") || propertyName.Equals("newSymbol");
		}

		public override IList<EncryptionData> FindByProperty<P>(string propertyName, P keyValue)
		{
			return currentSession.CreateQuery(GenerateQuery(QueryType.SELECT, propertyName))
								  .SetParameter("p1", keyValue)
			                      .List<EncryptionData>();
		}

		public override IList<EncryptionData> UpdateByProperty<P>(EncryptionData newEntity, string propertyName, P keyValue)
		{
			
			using (ITransaction transaction = currentSession.BeginTransaction()) {
				try
				{
					var result = currentSession.CreateQuery(GenerateQuery(QueryType.UPDATE, propertyName))
										 .SetParameter("p1", newEntity.OldSymbol)
										 .SetParameter("p2", newEntity.NewSymbol)
										 .SetParameter("p3", keyValue)
										 .List<EncryptionData>();
					transaction.Commit();
					return result;
				}
				catch 
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public override void DeleteByProperty<P>(string propertyName, P keyValue)
		{
			using (ITransaction transaction = currentSession.BeginTransaction())
			{
				try
				{
					currentSession.CreateQuery(GenerateQuery(QueryType.DELETE, propertyName))
								  .SetParameter("p1", keyValue).ExecuteUpdate();
					transaction.Commit();
				}
				catch 
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public override void DeleteAll()
		{
			using (ITransaction transaction = currentSession.BeginTransaction())
			{
				currentSession.Delete("delete from EncryptionData");
			}
		}
		#endregion
	}
}
