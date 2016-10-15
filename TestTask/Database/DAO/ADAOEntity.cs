using System.Collections.Generic;
using NHibernate;
using TestTask.Database.Entity;

namespace TestTask.Database.DAO
{
	public abstract class ADAOEntity<T, TId> where T : AEntity
	{
		protected readonly ISession currentSession;

		protected ADAOEntity(ISession session) {
			currentSession = session;
		}

		#region Abstract Methods
		public abstract bool ContainsProperty(string propertyName);

		public abstract IList<T> FindByProperty<P>(string propertyName, P keyValue);

		public abstract IList<T> UpdateByProperty<P>(T newEntity, string propertyName, P keyValue);

		public abstract void DeleteByProperty<P>(string propertyName, P keyValue);

		public abstract void DeleteAll();
		#endregion

		#region Default Methods
		public void CloseSession() {
			if (currentSession.IsOpen)
			{
				currentSession.Close();
			}
		}

		public T Create(T entity) {
			using (ITransaction transaction = currentSession.BeginTransaction())
			{
				try
				{
					currentSession.Save(entity);
					currentSession.Flush();
					transaction.Commit();
					return entity;
				}
				catch {
					transaction.Rollback();
					throw;
				}
			}
		}

		public T FindByID(TId id) {
			return currentSession.Get<T>(id);
		}

		public IList<T> FindAll() {
			return currentSession.CreateCriteria(typeof(T)).List<T>();
		}

		public T Update(T entity) {
			using (ITransaction transaction = currentSession.BeginTransaction())
			{
				try
				{
					currentSession.Update(entity);
					transaction.Commit();
					return entity;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public void Delete(T entity) {
			using (ITransaction transaction = currentSession.BeginTransaction())
			{
				try
				{
					currentSession.Delete(entity);
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}
		#endregion
	}
}
