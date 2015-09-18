using System.Collections;
using FluentNHibernate.Mapping;
using NHibernateTests.Conventions;

namespace NHibernateTests.Mappings
{
	public class Actor
	{
		public virtual int Id { get; set; }
		[Truncate(TruncateType = typeof(TruncateToFiftyString), Length = 3)]
		public virtual string Name { get; set; }
		public virtual IList Movies { get; set; }
	}

	public class ActorMap : ClassMap<Actor>
	{
		public ActorMap()
		{
			Id(x => x.Id);
			Map(x => x.Name).Length(3).CustomType<TruncateStringType>();

			HasManyToMany<Movie>(x => x.Movies)
				.Table("Cast")
				.ParentKeyColumn("ActorId")
				.ChildKeyColumn("MovieId")
				.LazyLoad()
				.Cascade.All();
		}
	}
}