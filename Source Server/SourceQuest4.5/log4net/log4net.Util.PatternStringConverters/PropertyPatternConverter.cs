using System.IO;

namespace log4net.Util.PatternStringConverters
{
	/// <summary>
	/// Property pattern converter
	/// </summary>
	/// <remarks>
	/// <para>
	/// This pattern converter reads the thread and global properties.
	/// The thread properties take priority over global properties.
	/// See <see cref="P:log4net.ThreadContext.Properties" /> for details of the 
	/// thread properties. See <see cref="P:log4net.GlobalContext.Properties" /> for
	/// details of the global properties.
	/// </para>
	/// <para>
	/// If the <see cref="P:log4net.Util.PatternConverter.Option" /> is specified then that will be used to
	/// lookup a single property. If no <see cref="P:log4net.Util.PatternConverter.Option" /> is specified
	/// then all properties will be dumped as a list of key value pairs.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class PropertyPatternConverter : PatternConverter
	{
		/// <summary>
		/// Write the property value to the output
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="state">null, state is not set</param>
		/// <remarks>
		/// <para>
		/// Writes out the value of a named property. The property name
		/// should be set in the <see cref="P:log4net.Util.PatternConverter.Option" />
		/// property.
		/// </para>
		/// <para>
		/// If the <see cref="P:log4net.Util.PatternConverter.Option" /> is set to <c>null</c>
		/// then all the properties are written as key value pairs.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, object state)
		{
			CompositeProperties compositeProperties = new CompositeProperties();
			PropertiesDictionary properties = LogicalThreadContext.Properties.GetProperties(create: false);
			if (properties != null)
			{
				compositeProperties.Add(properties);
			}
			PropertiesDictionary properties2 = ThreadContext.Properties.GetProperties(create: false);
			if (properties2 != null)
			{
				compositeProperties.Add(properties2);
			}
			compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());
			if (Option != null)
			{
				PatternConverter.WriteObject(writer, null, compositeProperties[Option]);
			}
			else
			{
				PatternConverter.WriteDictionary(writer, null, compositeProperties.Flatten());
			}
		}
	}
}
