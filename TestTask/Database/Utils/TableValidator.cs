using System.Collections.Generic;
using TestTask.Database.Entity;
using TestTask.Database.DAO;

namespace TestTask.Database.Util
{
	public class TableValidator<T, TId> where T : AEntity
	{
		private ADAOEntity<T, TId> adao;

		public TableValidator(ADAOEntity<T,TId> adao)
		{
			this.adao = adao;
		}

		public bool EmptyTable()
		{
			IList<T> data = adao.FindAll();
			return data == null || data.Count == 0;
		}

		public void GenerateTableContent(GenerationStrategy<T> generationStrategy)
		{
			IList<T> content = generationStrategy();
			foreach (T elem in content)
			{
				adao.Create(elem);
			}
		}
	}
}
