using System.Reflection;
using NHibernate;
using NHibernate.Cfg;

namespace TestTask.Database.Util
{
	public class NHibernateUtil
	{
		private static ISessionFactory factory;

		static NHibernateUtil()
		{
			Configuration cfg = new Configuration();
			cfg.AddAssembly(Assembly.GetExecutingAssembly());
			cfg.Configure();
			Factory = cfg.BuildSessionFactory();
		}

		public static ISessionFactory Factory
		{
			get { return factory; }
			private set { factory = value; }
		}
	}
}
