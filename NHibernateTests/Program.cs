using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernateTests.Conventions;
using NHibernateTests.Mappings;

namespace NHibernateTests
{
	class Program
	{
		static void Main(string[] args)
		{
			var session = SqlLiteSessionFactory.OpenSession();

			var movie = new Movie { Name = "Nea Marin Miliardar"};
			var actor = new Actor {Name = "Amza Pelea", Movies = new[]{movie}};

			using (var transaction = session.BeginTransaction())
			{
				session.Save(actor);
				session.Save(new Actor {Name = "Sergiu Nicolaescu"});
				session.Save(new Actor {Name = "Jean Constantin"});

				transaction.Commit();
			}

			session.Flush();
			var myactor = session.Load<Actor>(1);

			Console.ReadKey();
		}
	}

	public static class SqlLiteSessionFactory
	{
		private static ISessionFactory _sessionFactory;
		private static ISessionFactory SessionFactory
		{
			get
			{
				return _sessionFactory ?? (_sessionFactory = Fluently.Configure()
					.Database(SQLiteConfiguration
						.Standard
						.InMemory()
						.UsingFile("facultydata.db")
						.ShowSql
					)
					.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Actor>())
					.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Movie>())
					//.Mappings(m => m.FluentMappings.Conventions.Add<TruncateStringConvention>())					
					.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
					.BuildSessionFactory());
			}
		}
		public static ISession OpenSession()
		{
			return SessionFactory.OpenSession();
		}
	}

}
