using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace NHibernateTests.Conventions
{
	public class TruncateStringConvention : AttributePropertyConvention<TruncateAttribute>
	{
		protected override void Apply(TruncateAttribute attribute, IPropertyInstance instance)
		{
			if (attribute.TruncateType != null)
				instance.CustomType(attribute.TruncateType);
		}
	}

	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	public sealed class TruncateAttribute : Attribute
	{
		/// <summary>
		/// Must be a derived class from TruncateString class
		/// </summary>
		public Type TruncateType { get; set; }
	}

	public abstract class TruncateString : AbstractStringType
	{
		public int Length { get; set; }

		protected TruncateString(int length, SqlType sqlType)
			: base(sqlType)
		{
			Length = length;
		}

		public override string Name
		{
			get { return "TruncatedString"; }
		}

		public override void Set(System.Data.IDbCommand cmd, object value, int index)
		{
			var str = (string)value;
			if (str.Length > Length)
				str = str.Substring(0, Length);
			base.Set(cmd, str, index);
		}
	}

	public class TruncateToFiftyString : TruncateString
	{
		private const int MaximumLength = 5;

		public TruncateToFiftyString() : base(MaximumLength, new StringSqlType(MaximumLength)) { }
	}
}