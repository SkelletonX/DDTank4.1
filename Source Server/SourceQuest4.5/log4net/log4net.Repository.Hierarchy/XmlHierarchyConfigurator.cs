using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Xml;

namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Initializes the log4net environment using an XML DOM.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Configures a <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> using an XML DOM.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class XmlHierarchyConfigurator
	{
		private enum ConfigUpdateMode
		{
			Merge,
			Overwrite
		}

		private const string CONFIGURATION_TAG = "log4net";

		private const string RENDERER_TAG = "renderer";

		private const string APPENDER_TAG = "appender";

		private const string APPENDER_REF_TAG = "appender-ref";

		private const string PARAM_TAG = "param";

		private const string CATEGORY_TAG = "category";

		private const string PRIORITY_TAG = "priority";

		private const string LOGGER_TAG = "logger";

		private const string NAME_ATTR = "name";

		private const string TYPE_ATTR = "type";

		private const string VALUE_ATTR = "value";

		private const string ROOT_TAG = "root";

		private const string LEVEL_TAG = "level";

		private const string REF_ATTR = "ref";

		private const string ADDITIVITY_ATTR = "additivity";

		private const string THRESHOLD_ATTR = "threshold";

		private const string CONFIG_DEBUG_ATTR = "configDebug";

		private const string INTERNAL_DEBUG_ATTR = "debug";

		private const string CONFIG_UPDATE_MODE_ATTR = "update";

		private const string RENDERING_TYPE_ATTR = "renderingClass";

		private const string RENDERED_TYPE_ATTR = "renderedClass";

		private const string INHERITED = "inherited";

		/// <summary>
		/// key: appenderName, value: appender.
		/// </summary>
		private Hashtable m_appenderBag;

		/// <summary>
		/// The Hierarchy being configured.
		/// </summary>
		private readonly Hierarchy m_hierarchy;

		/// <summary>
		/// Construct the configurator for a hierarchy
		/// </summary>
		/// <param name="hierarchy">The hierarchy to build.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.XmlHierarchyConfigurator" /> class
		/// with the specified <see cref="T:log4net.Repository.Hierarchy.Hierarchy" />.
		/// </para>
		/// </remarks>
		public XmlHierarchyConfigurator(Hierarchy hierarchy)
		{
			m_hierarchy = hierarchy;
			m_appenderBag = new Hashtable();
		}

		/// <summary>
		/// Configure the hierarchy by parsing a DOM tree of XML elements.
		/// </summary>
		/// <param name="element">The root element to parse.</param>
		/// <remarks>
		/// <para>
		/// Configure the hierarchy by parsing a DOM tree of XML elements.
		/// </para>
		/// </remarks>
		public void Configure(XmlElement element)
		{
			if (element == null || m_hierarchy == null)
			{
				return;
			}
			string localName = element.LocalName;
			if (localName != "log4net")
			{
				LogLog.Error("XmlHierarchyConfigurator: Xml element is - not a <log4net> element.");
				return;
			}
			if (!LogLog.InternalDebugging)
			{
				string attribute = element.GetAttribute("debug");
				LogLog.Debug("XmlHierarchyConfigurator: debug attribute [" + attribute + "].");
				if (attribute.Length > 0 && attribute != "null")
				{
					LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute, defaultValue: true);
				}
				else
				{
					LogLog.Debug("XmlHierarchyConfigurator: Ignoring debug attribute.");
				}
				string attribute2 = element.GetAttribute("configDebug");
				if (attribute2.Length > 0 && attribute2 != "null")
				{
					LogLog.Warn("XmlHierarchyConfigurator: The \"configDebug\" attribute is deprecated.");
					LogLog.Warn("XmlHierarchyConfigurator: Use the \"debug\" attribute instead.");
					LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute2, defaultValue: true);
				}
			}
			ConfigUpdateMode configUpdateMode = ConfigUpdateMode.Merge;
			string attribute3 = element.GetAttribute("update");
			if (attribute3 != null && attribute3.Length > 0)
			{
				try
				{
					configUpdateMode = (ConfigUpdateMode)OptionConverter.ConvertStringTo(typeof(ConfigUpdateMode), attribute3);
				}
				catch
				{
					LogLog.Error("XmlHierarchyConfigurator: Invalid update attribute value [" + attribute3 + "]");
				}
			}
			LogLog.Debug("XmlHierarchyConfigurator: Configuration update mode [" + configUpdateMode.ToString() + "].");
			if (configUpdateMode == ConfigUpdateMode.Overwrite)
			{
				m_hierarchy.ResetConfiguration();
				LogLog.Debug("XmlHierarchyConfigurator: Configuration reset before reading config.");
			}
			foreach (XmlNode childNode in element.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					XmlElement xmlElement = (XmlElement)childNode;
					if (xmlElement.LocalName == "logger")
					{
						ParseLogger(xmlElement);
					}
					else if (xmlElement.LocalName == "category")
					{
						ParseLogger(xmlElement);
					}
					else if (xmlElement.LocalName == "root")
					{
						ParseRoot(xmlElement);
					}
					else if (xmlElement.LocalName == "renderer")
					{
						ParseRenderer(xmlElement);
					}
					else if (!(xmlElement.LocalName == "appender"))
					{
						SetParameter(xmlElement, m_hierarchy);
					}
				}
			}
			string attribute4 = element.GetAttribute("threshold");
			LogLog.Debug("XmlHierarchyConfigurator: Hierarchy Threshold [" + attribute4 + "]");
			if (attribute4.Length > 0 && attribute4 != "null")
			{
				Level level = (Level)ConvertStringTo(typeof(Level), attribute4);
				if (level != null)
				{
					m_hierarchy.Threshold = level;
				}
				else
				{
					LogLog.Warn("XmlHierarchyConfigurator: Unable to set hierarchy threshold using value [" + attribute4 + "] (with acceptable conversion types)");
				}
			}
		}

		/// <summary>
		/// Parse appenders by IDREF.
		/// </summary>
		/// <param name="appenderRef">The appender ref element.</param>
		/// <returns>The instance of the appender that the ref refers to.</returns>
		/// <remarks>
		/// <para>
		/// Parse an XML element that represents an appender and return 
		/// the appender.
		/// </para>
		/// </remarks>
		protected IAppender FindAppenderByReference(XmlElement appenderRef)
		{
			string attribute = appenderRef.GetAttribute("ref");
			IAppender appender = (IAppender)m_appenderBag[attribute];
			if (appender != null)
			{
				return appender;
			}
			XmlElement xmlElement = null;
			if (attribute != null && attribute.Length > 0)
			{
				foreach (XmlElement item in appenderRef.OwnerDocument.GetElementsByTagName("appender"))
				{
					if (item.GetAttribute("name") == attribute)
					{
						xmlElement = item;
						break;
					}
				}
			}
			if (xmlElement == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: No appender named [" + attribute + "] could be found.");
				return null;
			}
			appender = ParseAppender(xmlElement);
			if (appender != null)
			{
				m_appenderBag[attribute] = appender;
			}
			return appender;
		}

		/// <summary>
		/// Parses an appender element.
		/// </summary>
		/// <param name="appenderElement">The appender element.</param>
		/// <returns>The appender instance or <c>null</c> when parsing failed.</returns>
		/// <remarks>
		/// <para>
		/// Parse an XML element that represents an appender and return
		/// the appender instance.
		/// </para>
		/// </remarks>
		protected IAppender ParseAppender(XmlElement appenderElement)
		{
			string attribute = appenderElement.GetAttribute("name");
			string attribute2 = appenderElement.GetAttribute("type");
			LogLog.Debug("XmlHierarchyConfigurator: Loading Appender [" + attribute + "] type: [" + attribute2 + "]");
			try
			{
				IAppender appender = (IAppender)Activator.CreateInstance(SystemInfo.GetTypeFromString(attribute2, throwOnError: true, ignoreCase: true));
				appender.Name = attribute;
				foreach (XmlNode childNode in appenderElement.ChildNodes)
				{
					if (childNode.NodeType == XmlNodeType.Element)
					{
						XmlElement xmlElement = (XmlElement)childNode;
						if (xmlElement.LocalName == "appender-ref")
						{
							string attribute3 = xmlElement.GetAttribute("ref");
							IAppenderAttachable appenderAttachable = appender as IAppenderAttachable;
							if (appenderAttachable != null)
							{
								LogLog.Debug("XmlHierarchyConfigurator: Attaching appender named [" + attribute3 + "] to appender named [" + appender.Name + "].");
								IAppender appender2 = FindAppenderByReference(xmlElement);
								if (appender2 != null)
								{
									appenderAttachable.AddAppender(appender2);
								}
							}
							else
							{
								LogLog.Error("XmlHierarchyConfigurator: Requesting attachment of appender named [" + attribute3 + "] to appender named [" + appender.Name + "] which does not implement log4net.Core.IAppenderAttachable.");
							}
						}
						else
						{
							SetParameter(xmlElement, appender);
						}
					}
				}
				(appender as IOptionHandler)?.ActivateOptions();
				LogLog.Debug("XmlHierarchyConfigurator: Created Appender [" + attribute + "]");
				return appender;
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlHierarchyConfigurator: Could not create Appender [" + attribute + "] of type [" + attribute2 + "]. Reported error follows.", exception);
				return null;
			}
		}

		/// <summary>
		/// Parses a logger element.
		/// </summary>
		/// <param name="loggerElement">The logger element.</param>
		/// <remarks>
		/// <para>
		/// Parse an XML element that represents a logger.
		/// </para>
		/// </remarks>
		protected void ParseLogger(XmlElement loggerElement)
		{
			string attribute = loggerElement.GetAttribute("name");
			LogLog.Debug("XmlHierarchyConfigurator: Retrieving an instance of log4net.Repository.Logger for logger [" + attribute + "].");
			Logger logger = m_hierarchy.GetLogger(attribute) as Logger;
			lock (logger)
			{
				bool flag = OptionConverter.ToBoolean(loggerElement.GetAttribute("additivity"), defaultValue: true);
				LogLog.Debug("XmlHierarchyConfigurator: Setting [" + logger.Name + "] additivity to [" + flag + "].");
				logger.Additivity = flag;
				ParseChildrenOfLoggerElement(loggerElement, logger, isRoot: false);
			}
		}

		/// <summary>
		/// Parses the root logger element.
		/// </summary>
		/// <param name="rootElement">The root element.</param>
		/// <remarks>
		/// <para>
		/// Parse an XML element that represents the root logger.
		/// </para>
		/// </remarks>
		protected void ParseRoot(XmlElement rootElement)
		{
			Logger root = m_hierarchy.Root;
			lock (root)
			{
				ParseChildrenOfLoggerElement(rootElement, root, isRoot: true);
			}
		}

		/// <summary>
		/// Parses the children of a logger element.
		/// </summary>
		/// <param name="catElement">The category element.</param>
		/// <param name="log">The logger instance.</param>
		/// <param name="isRoot">Flag to indicate if the logger is the root logger.</param>
		/// <remarks>
		/// <para>
		/// Parse the child elements of a &lt;logger&gt; element.
		/// </para>
		/// </remarks>
		protected void ParseChildrenOfLoggerElement(XmlElement catElement, Logger log, bool isRoot)
		{
			log.RemoveAllAppenders();
			foreach (XmlNode childNode in catElement.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					XmlElement xmlElement = (XmlElement)childNode;
					if (xmlElement.LocalName == "appender-ref")
					{
						IAppender appender = FindAppenderByReference(xmlElement);
						string attribute = xmlElement.GetAttribute("ref");
						if (appender != null)
						{
							LogLog.Debug("XmlHierarchyConfigurator: Adding appender named [" + attribute + "] to logger [" + log.Name + "].");
							log.AddAppender(appender);
						}
						else
						{
							LogLog.Error("XmlHierarchyConfigurator: Appender named [" + attribute + "] not found.");
						}
					}
					else if (xmlElement.LocalName == "level" || xmlElement.LocalName == "priority")
					{
						ParseLevel(xmlElement, log, isRoot);
					}
					else
					{
						SetParameter(xmlElement, log);
					}
				}
			}
			(log as IOptionHandler)?.ActivateOptions();
		}

		/// <summary>
		/// Parses an object renderer.
		/// </summary>
		/// <param name="element">The renderer element.</param>
		/// <remarks>
		/// <para>
		/// Parse an XML element that represents a renderer.
		/// </para>
		/// </remarks>
		protected void ParseRenderer(XmlElement element)
		{
			string attribute = element.GetAttribute("renderingClass");
			string attribute2 = element.GetAttribute("renderedClass");
			LogLog.Debug("XmlHierarchyConfigurator: Rendering class [" + attribute + "], Rendered class [" + attribute2 + "].");
			IObjectRenderer objectRenderer = (IObjectRenderer)OptionConverter.InstantiateByClassName(attribute, typeof(IObjectRenderer), null);
			if (objectRenderer == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: Could not instantiate renderer [" + attribute + "].");
			}
			else
			{
				try
				{
					m_hierarchy.RendererMap.Put(SystemInfo.GetTypeFromString(attribute2, throwOnError: true, ignoreCase: true), objectRenderer);
				}
				catch (Exception exception)
				{
					LogLog.Error("XmlHierarchyConfigurator: Could not find class [" + attribute2 + "].", exception);
				}
			}
		}

		/// <summary>
		/// Parses a level element.
		/// </summary>
		/// <param name="element">The level element.</param>
		/// <param name="log">The logger object to set the level on.</param>
		/// <param name="isRoot">Flag to indicate if the logger is the root logger.</param>
		/// <remarks>
		/// <para>
		/// Parse an XML element that represents a level.
		/// </para>
		/// </remarks>
		protected void ParseLevel(XmlElement element, Logger log, bool isRoot)
		{
			string text = log.Name;
			if (isRoot)
			{
				text = "root";
			}
			string attribute = element.GetAttribute("value");
			LogLog.Debug("XmlHierarchyConfigurator: Logger [" + text + "] Level string is [" + attribute + "].");
			if ("inherited" == attribute)
			{
				if (isRoot)
				{
					LogLog.Error("XmlHierarchyConfigurator: Root level cannot be inherited. Ignoring directive.");
					return;
				}
				LogLog.Debug("XmlHierarchyConfigurator: Logger [" + text + "] level set to inherit from parent.");
				log.Level = null;
			}
			else
			{
				log.Level = log.Hierarchy.LevelMap[attribute];
				if (log.Level == null)
				{
					LogLog.Error("XmlHierarchyConfigurator: Undefined level [" + attribute + "] on Logger [" + text + "].");
				}
				else
				{
					LogLog.Debug("XmlHierarchyConfigurator: Logger [" + text + "] level set to [name=\"" + log.Level.Name + "\",value=" + log.Level.Value + "].");
				}
			}
		}

		/// <summary>
		/// Sets a parameter on an object.
		/// </summary>
		/// <param name="element">The parameter element.</param>
		/// <param name="target">The object to set the parameter on.</param>
		/// <remarks>
		/// The parameter name must correspond to a writable property
		/// on the object. The value of the parameter is a string,
		/// therefore this function will attempt to set a string
		/// property first. If unable to set a string property it
		/// will inspect the property and its argument type. It will
		/// attempt to call a static method called <c>Parse</c> on the
		/// type of the property. This method will take a single
		/// string argument and return a value that can be used to
		/// set the property.
		/// </remarks>
		protected void SetParameter(XmlElement element, object target)
		{
			string text = element.GetAttribute("name");
			if (element.LocalName != "param" || text == null || text.Length == 0)
			{
				text = element.LocalName;
			}
			Type type = target.GetType();
			Type type2 = null;
			PropertyInfo propertyInfo = null;
			MethodInfo methodInfo = null;
			propertyInfo = type.GetProperty(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if ((object)propertyInfo != null && propertyInfo.CanWrite)
			{
				type2 = propertyInfo.PropertyType;
			}
			else
			{
				propertyInfo = null;
				methodInfo = FindMethodInfo(type, text);
				if ((object)methodInfo != null)
				{
					type2 = methodInfo.GetParameters()[0].ParameterType;
				}
			}
			if ((object)type2 == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: Cannot find Property [" + text + "] to set object on [" + target.ToString() + "]");
				return;
			}
			string text2 = null;
			if (element.GetAttributeNode("value") != null)
			{
				text2 = element.GetAttribute("value");
			}
			else if (element.HasChildNodes)
			{
				foreach (XmlNode childNode in element.ChildNodes)
				{
					if (childNode.NodeType == XmlNodeType.CDATA || childNode.NodeType == XmlNodeType.Text)
					{
						text2 = ((text2 != null) ? (text2 + childNode.InnerText) : childNode.InnerText);
					}
				}
			}
			if (text2 != null)
			{
				try
				{
					text2 = OptionConverter.SubstituteVariables(text2, Environment.GetEnvironmentVariables());
				}
				catch (SecurityException)
				{
					LogLog.Debug("XmlHierarchyConfigurator: Security exception while trying to expand environment variables. Error Ignored. No Expansion.");
				}
				Type type3 = null;
				string attribute = element.GetAttribute("type");
				if (attribute != null && attribute.Length > 0)
				{
					try
					{
						Type typeFromString = SystemInfo.GetTypeFromString(attribute, throwOnError: true, ignoreCase: true);
						LogLog.Debug("XmlHierarchyConfigurator: Parameter [" + text + "] specified subtype [" + typeFromString.FullName + "]");
						if (!type2.IsAssignableFrom(typeFromString))
						{
							if (OptionConverter.CanConvertTypeTo(typeFromString, type2))
							{
								type3 = type2;
								type2 = typeFromString;
							}
							else
							{
								LogLog.Error("XmlHierarchyConfigurator: Subtype [" + typeFromString.FullName + "] set on [" + text + "] is not a subclass of property type [" + type2.FullName + "] and there are no acceptable type conversions.");
							}
						}
						else
						{
							type2 = typeFromString;
						}
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlHierarchyConfigurator: Failed to find type [" + attribute + "] set on [" + text + "]", exception);
					}
				}
				object obj = ConvertStringTo(type2, text2);
				if (obj != null && (object)type3 != null)
				{
					LogLog.Debug("XmlHierarchyConfigurator: Performing additional conversion of value from [" + obj.GetType().Name + "] to [" + type3.Name + "]");
					obj = OptionConverter.ConvertTypeTo(obj, type3);
				}
				if (obj != null)
				{
					if ((object)propertyInfo != null)
					{
						LogLog.Debug("XmlHierarchyConfigurator: Setting Property [" + propertyInfo.Name + "] to " + obj.GetType().Name + " value [" + obj.ToString() + "]");
						try
						{
							propertyInfo.SetValue(target, obj, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
						}
						catch (TargetInvocationException ex2)
						{
							LogLog.Error(string.Concat("XmlHierarchyConfigurator: Failed to set parameter [", propertyInfo.Name, "] on object [", target, "] using value [", obj, "]"), ex2.InnerException);
						}
					}
					else if ((object)methodInfo != null)
					{
						LogLog.Debug("XmlHierarchyConfigurator: Setting Collection Property [" + methodInfo.Name + "] to " + obj.GetType().Name + " value [" + obj.ToString() + "]");
						try
						{
							methodInfo.Invoke(target, BindingFlags.InvokeMethod, null, new object[1]
							{
								obj
							}, CultureInfo.InvariantCulture);
						}
						catch (TargetInvocationException ex2)
						{
							LogLog.Error(string.Concat("XmlHierarchyConfigurator: Failed to set parameter [", text, "] on object [", target, "] using value [", obj, "]"), ex2.InnerException);
						}
					}
				}
				else
				{
					LogLog.Warn(string.Concat("XmlHierarchyConfigurator: Unable to set property [", text, "] on object [", target, "] using value [", text2, "] (with acceptable conversion types)"));
				}
				return;
			}
			object obj2 = null;
			if ((object)type2 == typeof(string) && !HasAttributesOrElements(element))
			{
				obj2 = "";
			}
			else
			{
				Type defaultTargetType = null;
				if (IsTypeConstructible(type2))
				{
					defaultTargetType = type2;
				}
				obj2 = CreateObjectFromXml(element, defaultTargetType, type2);
			}
			if (obj2 == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: Failed to create object to set param: " + text);
			}
			else if ((object)propertyInfo != null)
			{
				LogLog.Debug(string.Concat("XmlHierarchyConfigurator: Setting Property [", propertyInfo.Name, "] to object [", obj2, "]"));
				try
				{
					propertyInfo.SetValue(target, obj2, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
				}
				catch (TargetInvocationException ex2)
				{
					LogLog.Error(string.Concat("XmlHierarchyConfigurator: Failed to set parameter [", propertyInfo.Name, "] on object [", target, "] using value [", obj2, "]"), ex2.InnerException);
				}
			}
			else if ((object)methodInfo != null)
			{
				LogLog.Debug(string.Concat("XmlHierarchyConfigurator: Setting Collection Property [", methodInfo.Name, "] to object [", obj2, "]"));
				try
				{
					methodInfo.Invoke(target, BindingFlags.InvokeMethod, null, new object[1]
					{
						obj2
					}, CultureInfo.InvariantCulture);
				}
				catch (TargetInvocationException ex2)
				{
					LogLog.Error(string.Concat("XmlHierarchyConfigurator: Failed to set parameter [", methodInfo.Name, "] on object [", target, "] using value [", obj2, "]"), ex2.InnerException);
				}
			}
		}

		/// <summary>
		/// Test if an element has no attributes or child elements
		/// </summary>
		/// <param name="element">the element to inspect</param>
		/// <returns><c>true</c> if the element has any attributes or child elements, <c>false</c> otherwise</returns>
		private bool HasAttributesOrElements(XmlElement element)
		{
			foreach (XmlNode childNode in element.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Attribute || childNode.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Test if a <see cref="T:System.Type" /> is constructible with <c>Activator.CreateInstance</c>.
		/// </summary>
		/// <param name="type">the type to inspect</param>
		/// <returns><c>true</c> if the type is creatable using a default constructor, <c>false</c> otherwise</returns>
		private static bool IsTypeConstructible(Type type)
		{
			if (type.IsClass && !type.IsAbstract)
			{
				ConstructorInfo constructor = type.GetConstructor(new Type[0]);
				if ((object)constructor != null && !constructor.IsAbstract && !constructor.IsPrivate)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Look for a method on the <paramref name="targetType" /> that matches the <paramref name="name" /> supplied
		/// </summary>
		/// <param name="targetType">the type that has the method</param>
		/// <param name="name">the name of the method</param>
		/// <returns>the method info found</returns>
		/// <remarks>
		/// <para>
		/// The method must be a public instance method on the <paramref name="targetType" />.
		/// The method must be named <paramref name="name" /> or "Add" followed by <paramref name="name" />.
		/// The method must take a single parameter.
		/// </para>
		/// </remarks>
		private MethodInfo FindMethodInfo(Type targetType, string name)
		{
			string strB = "Add" + name;
			MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo[] array = methods;
			foreach (MethodInfo methodInfo in array)
			{
				if (!methodInfo.IsStatic && (string.Compare(methodInfo.Name, name, ignoreCase: true, CultureInfo.InvariantCulture) == 0 || string.Compare(methodInfo.Name, strB, ignoreCase: true, CultureInfo.InvariantCulture) == 0))
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 1)
					{
						return methodInfo;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Converts a string value to a target type.
		/// </summary>
		/// <param name="type">The type of object to convert the string to.</param>
		/// <param name="value">The string value to use as the value of the object.</param>
		/// <returns>
		/// <para>
		/// An object of type <paramref name="type" /> with value <paramref name="value" /> or 
		/// <c>null</c> when the conversion could not be performed.
		/// </para>
		/// </returns>
		protected object ConvertStringTo(Type type, string value)
		{
			if ((object)typeof(Level) == type)
			{
				Level level = m_hierarchy.LevelMap[value];
				if (level == null)
				{
					LogLog.Error("XmlHierarchyConfigurator: Unknown Level Specified [" + value + "]");
				}
				return level;
			}
			return OptionConverter.ConvertStringTo(type, value);
		}

		/// <summary>
		/// Creates an object as specified in XML.
		/// </summary>
		/// <param name="element">The XML element that contains the definition of the object.</param>
		/// <param name="defaultTargetType">The object type to use if not explicitly specified.</param>
		/// <param name="typeConstraint">The type that the returned object must be or must inherit from.</param>
		/// <returns>The object or <c>null</c></returns>
		/// <remarks>
		/// <para>
		/// Parse an XML element and create an object instance based on the configuration
		/// data.
		/// </para>
		/// <para>
		/// The type of the instance may be specified in the XML. If not
		/// specified then the <paramref name="defaultTargetType" /> is used
		/// as the type. However the type is specified it must support the
		/// <paramref name="typeConstraint" /> type.
		/// </para>
		/// </remarks>
		protected object CreateObjectFromXml(XmlElement element, Type defaultTargetType, Type typeConstraint)
		{
			Type type = null;
			string attribute = element.GetAttribute("type");
			if (attribute == null || attribute.Length == 0)
			{
				if ((object)defaultTargetType == null)
				{
					LogLog.Error("XmlHierarchyConfigurator: Object type not specified. Cannot create object of type [" + typeConstraint.FullName + "]. Missing Value or Type.");
					return null;
				}
				type = defaultTargetType;
			}
			else
			{
				try
				{
					type = SystemInfo.GetTypeFromString(attribute, throwOnError: true, ignoreCase: true);
				}
				catch (Exception exception)
				{
					LogLog.Error("XmlHierarchyConfigurator: Failed to find type [" + attribute + "]", exception);
					return null;
				}
			}
			bool flag = false;
			if ((object)typeConstraint != null && !typeConstraint.IsAssignableFrom(type))
			{
				if (!OptionConverter.CanConvertTypeTo(type, typeConstraint))
				{
					LogLog.Error("XmlHierarchyConfigurator: Object type [" + type.FullName + "] is not assignable to type [" + typeConstraint.FullName + "]. There are no acceptable type conversions.");
					return null;
				}
				flag = true;
			}
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(type);
			}
			catch (Exception ex)
			{
				LogLog.Error("XmlHierarchyConfigurator: Failed to construct object of type [" + type.FullName + "] Exception: " + ex.ToString());
			}
			foreach (XmlNode childNode in element.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					SetParameter((XmlElement)childNode, obj);
				}
			}
			(obj as IOptionHandler)?.ActivateOptions();
			if (flag)
			{
				return OptionConverter.ConvertTypeTo(obj, typeConstraint);
			}
			return obj;
		}
	}
}
