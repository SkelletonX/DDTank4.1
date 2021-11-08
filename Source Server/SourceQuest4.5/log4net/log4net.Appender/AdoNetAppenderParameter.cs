using log4net.Core;
using log4net.Layout;
using System;
using System.Data;

namespace log4net.Appender
{
	/// <summary>
	/// Parameter type used by the <see cref="T:log4net.Appender.AdoNetAppender" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class provides the basic Game.Service parameter properties
	/// as defined by the <see cref="T:System.Data.IDbDataParameter" /> interface.
	/// </para>
	/// <para>This type can be subclassed to provide Game.Service specific
	/// functionality. The two methods that are called externally are
	/// <see cref="M:log4net.Appender.AdoNetAppenderParameter.Prepare(System.Data.IDbCommand)" /> and <see cref="M:log4net.Appender.AdoNetAppenderParameter.FormatValue(System.Data.IDbCommand,log4net.Core.LoggingEvent)" />.
	/// </para>
	/// </remarks>
	public class AdoNetAppenderParameter
	{
		/// <summary>
		/// The name of this parameter.
		/// </summary>
		private string m_parameterName;

		/// <summary>
		/// The Game.Service type for this parameter.
		/// </summary>
		private DbType m_dbType;

		/// <summary>
		/// Flag to infer type rather than use the DbType
		/// </summary>
		private bool m_inferType = true;

		/// <summary>
		/// The precision for this parameter.
		/// </summary>
		private byte m_precision;

		/// <summary>
		/// The scale for this parameter.
		/// </summary>
		private byte m_scale;

		/// <summary>
		/// The size for this parameter.
		/// </summary>
		private int m_size;

		/// <summary>
		/// The <see cref="T:log4net.Layout.IRawLayout" /> to use to render the
		/// logging event into an object for this parameter.
		/// </summary>
		private IRawLayout m_layout;

		/// <summary>
		/// Gets or sets the name of this parameter.
		/// </summary>
		/// <value>
		/// The name of this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The name of this parameter. The parameter name
		/// must match up to a named parameter to the SQL stored procedure
		/// or prepared statement.
		/// </para>
		/// </remarks>
		public string ParameterName
		{
			get
			{
				return m_parameterName;
			}
			set
			{
				m_parameterName = value;
			}
		}

		/// <summary>
		/// Gets or sets the Game.Service type for this parameter.
		/// </summary>
		/// <value>
		/// The Game.Service type for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The Game.Service type for this parameter. This property should
		/// be set to the Game.Service type from the <see cref="P:log4net.Appender.AdoNetAppenderParameter.DbType" />
		/// enumeration. See <see cref="P:System.Data.IDataParameter.DbType" />.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the type from the value.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:System.Data.IDataParameter.DbType" />
		public DbType DbType
		{
			get
			{
				return m_dbType;
			}
			set
			{
				m_dbType = value;
				m_inferType = false;
			}
		}

		/// <summary>
		/// Gets or sets the precision for this parameter.
		/// </summary>
		/// <value>
		/// The precision for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The maximum number of digits used to represent the Value.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the precision from the value.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:System.Data.IDbDataParameter.Precision" />
		public byte Precision
		{
			get
			{
				return m_precision;
			}
			set
			{
				m_precision = value;
			}
		}

		/// <summary>
		/// Gets or sets the scale for this parameter.
		/// </summary>
		/// <value>
		/// The scale for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The number of decimal places to which Value is resolved.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the scale from the value.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:System.Data.IDbDataParameter.Scale" />
		public byte Scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				m_scale = value;
			}
		}

		/// <summary>
		/// Gets or sets the size for this parameter.
		/// </summary>
		/// <value>
		/// The size for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The maximum size, in bytes, of the data within the column.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the size from the value.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:System.Data.IDbDataParameter.Size" />
		public int Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="T:log4net.Layout.IRawLayout" /> to use to 
		/// render the logging event into an object for this 
		/// parameter.
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Layout.IRawLayout" /> used to render the
		/// logging event into an object for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Layout.IRawLayout" /> that renders the value for this
		/// parameter.
		/// </para>
		/// <para>
		/// The <see cref="T:log4net.Layout.RawLayoutConverter" /> can be used to adapt
		/// any <see cref="T:log4net.Layout.ILayout" /> into a <see cref="T:log4net.Layout.IRawLayout" />
		/// for use in the property.
		/// </para>
		/// </remarks>
		public IRawLayout Layout
		{
			get
			{
				return m_layout;
			}
			set
			{
				m_layout = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.AdoNetAppenderParameter" /> class.
		/// </summary>
		/// <remarks>
		/// Default constructor for the AdoNetAppenderParameter class.
		/// </remarks>
		public AdoNetAppenderParameter()
		{
			m_precision = 0;
			m_scale = 0;
			m_size = 0;
		}

		/// <summary>
		/// Prepare the specified Game.Service command object.
		/// </summary>
		/// <param name="command">The command to prepare.</param>
		/// <remarks>
		/// <para>
		/// Prepares the Game.Service command object by adding
		/// this parameter to its collection of parameters.
		/// </para>
		/// </remarks>
		public virtual void Prepare(IDbCommand command)
		{
			IDbDataParameter dbDataParameter = command.CreateParameter();
			dbDataParameter.ParameterName = m_parameterName;
			if (!m_inferType)
			{
				dbDataParameter.DbType = m_dbType;
			}
			if (m_precision != 0)
			{
				dbDataParameter.Precision = m_precision;
			}
			if (m_scale != 0)
			{
				dbDataParameter.Scale = m_scale;
			}
			if (m_size != 0)
			{
				dbDataParameter.Size = m_size;
			}
			command.Parameters.Add(dbDataParameter);
		}

		/// <summary>
		/// Renders the logging event and set the parameter value in the command.
		/// </summary>
		/// <param name="command">The command containing the parameter.</param>
		/// <param name="loggingEvent">The event to be rendered.</param>
		/// <remarks>
		/// <para>
		/// Renders the logging event using this parameters layout
		/// object. Sets the value of the parameter on the command object.
		/// </para>
		/// </remarks>
		public virtual void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
		{
			IDbDataParameter dbDataParameter = (IDbDataParameter)command.Parameters[m_parameterName];
			object obj = Layout.Format(loggingEvent);
			if (obj == null)
			{
				obj = DBNull.Value;
			}
			dbDataParameter.Value = obj;
		}
	}
}
