using System.Collections;
using FluentNHibernate.Mapping;

namespace NHibernateTests.Mappings
{
	public class Movie
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IList Actors { get; set; }
	}

	public class MovieMap : ClassMap<Movie>
	{
		public MovieMap()
		{
			Id(x => x.Id);
			Map(x => x.Name);

			HasManyToMany<Actor>(x => x.Actors)
				.Table("Cast")
				.ParentKeyColumn("ActorId")
				.ChildKeyColumn("MovieId")
				.Inverse();
		}
	}
}