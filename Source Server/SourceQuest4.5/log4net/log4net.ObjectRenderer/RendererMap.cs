using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace log4net.ObjectRenderer
{
	/// <summary>
	/// Map class objects to an <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Maintains a mapping between types that require special
	/// rendering and the <see cref="T:log4net.ObjectRenderer.IObjectRenderer" /> that
	/// is used to render them.
	/// </para>
	/// <para>
	/// The <see cref="M:log4net.ObjectRenderer.RendererMap.FindAndRender(System.Object)" /> method is used to render an
	/// <c>object</c> using the appropriate renderers defined in this map.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class RendererMap
	{
		private Hashtable m_map;

		private Hashtable m_cache = new Hashtable();

		private static IObjectRenderer s_defaultRenderer = new DefaultRenderer();

		/// <summary>
		/// Get the default renderer instance
		/// </summary>
		/// <value>the default renderer</value>
		/// <remarks>
		/// <para>
		/// Get the default renderer
		/// </para>
		/// </remarks>
		public IObjectRenderer DefaultRenderer => s_defaultRenderer;

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default constructor.
		/// </para>
		/// </remarks>
		public RendererMap()
		{
			m_map = Hashtable.Synchronized(new Hashtable());
		}

		/// <summary>
		/// Render <paramref name="obj" /> using the appropriate renderer.
		/// </summary>
		/// <param name="obj">the object to render to a string</param>
		/// <returns>the object rendered as a string</returns>
		/// <remarks>
		/// <para>
		/// This is a convenience method used to render an object to a string.
		/// The alternative method <see cref="M:log4net.ObjectRenderer.RendererMap.FindAndRender(System.Object,System.IO.TextWriter)" />
		/// should be used when streaming output to a <see cref="T:System.IO.TextWriter" />.
		/// </para>
		/// </remarks>
		public string FindAndRender(object obj)
		{
			string text = obj as string;
			if (text != null)
			{
				return text;
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			FindAndRender(obj, stringWriter);
			return stringWriter.ToString();
		}

		/// <summary>
		/// Render <paramref name="obj" /> using the appropriate renderer.
		/// </summary>
		/// <param name="obj">the object to render to a string</param>
		/// <param name="writer">The writer to render to</param>
		/// <remarks>
		/// <para>
		/// Find the appropriate renderer for the type of the
		/// <paramref name="obj" /> parameter. This is accomplished by calling the
		/// <see cref="M:log4net.ObjectRenderer.RendererMap.Get(System.Type)" /> method. Once a renderer is found, it is
		/// applied on the object <paramref name="obj" /> and the result is returned
		/// as a <see cref="T:System.String" />.
		/// </para>
		/// </remarks>
		public void FindAndRender(object obj, TextWriter writer)
		{
			if (obj == null)
			{
				writer.Write(SystemInfo.NullText);
				return;
			}
			string text = obj as string;
			if (text != null)
			{
				writer.Write(text);
			}
			else
			{
				try
				{
					Get(obj.GetType()).RenderObject(this, obj, writer);
				}
				catch (Exception ex)
				{
					LogLog.Error("RendererMap: Exception while rendering object of type [" + obj.GetType().FullName + "]", ex);
					string str = "";
					if (obj != null && (object)obj.GetType() != null)
					{
						str = obj.GetType().FullName;
					}
					writer.Write("<log4net.Error>Exception rendering object type [" + str + "]");
					if (ex != null)
					{
						string str2 = null;
						try
						{
							str2 = ex.ToString();
						}
						catch
						{
						}
						writer.Write("<stackTrace>" + str2 + "</stackTrace>");
					}
					writer.Write("</log4net.Error>");
				}
			}
		}

		/// <summary>
		/// Gets the renderer for the specified object type
		/// </summary>
		/// <param name="obj">the object to lookup the renderer for</param>
		/// <returns>the renderer for <paramref name="obj" /></returns>
		/// <remarks>
		/// <param>
		/// Gets the renderer for the specified object type.
		/// </param>
		/// <param>
		/// Syntactic sugar method that calls <see cref="M:log4net.ObjectRenderer.RendererMap.Get(System.Type)" /> 
		/// with the type of the object parameter.
		/// </param>
		/// </remarks>
		public IObjectRenderer Get(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			return Get(obj.GetType());
		}

		/// <summary>
		/// Gets the renderer for the specified type
		/// </summary>
		/// <param name="type">the type to lookup the renderer for</param>
		/// <returns>the renderer for the specified type</returns>
		/// <remarks>
		/// <para>
		/// Returns the renderer for the specified type.
		/// If no specific renderer has been defined the
		/// <see cref="P:log4net.ObjectRenderer.RendererMap.DefaultRenderer" /> will be returned.
		/// </para>
		/// </remarks>
		public IObjectRenderer Get(Type type)
		{
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			IObjectRenderer objectRenderer = null;
			objectRenderer = (IObjectRenderer)m_cache[type];
			if (objectRenderer == null)
			{
				Type type2 = type;
				while ((object)type2 != null)
				{
					objectRenderer = SearchTypeAndInterfaces(type2);
					if (objectRenderer != null)
					{
						break;
					}
					type2 = type2.BaseType;
				}
				if (objectRenderer == null)
				{
					objectRenderer = s_defaultRenderer;
				}
				m_cache[type] = objectRenderer;
			}
			return objectRenderer;
		}

		/// <summary>
		/// Internal function to recursively search interfaces
		/// </summary>
		/// <param name="type">the type to lookup the renderer for</param>
		/// <returns>the renderer for the specified type</returns>
		private IObjectRenderer SearchTypeAndInterfaces(Type type)
		{
			IObjectRenderer objectRenderer = (IObjectRenderer)m_map[type];
			if (objectRenderer != null)
			{
				return objectRenderer;
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				objectRenderer = SearchTypeAndInterfaces(type2);
				if (objectRenderer != null)
				{
					return objectRenderer;
				}
			}
			return null;
		}

		/// <summary>
		/// Clear the map of renderers
		/// </summary>
		/// <remarks>
		/// <para>
		/// Clear the custom renderers defined by using
		/// <see cref="M:log4net.ObjectRenderer.RendererMap.Put(System.Type,log4net.ObjectRenderer.IObjectRenderer)" />. The <see cref="P:log4net.ObjectRenderer.RendererMap.DefaultRenderer" />
		/// cannot be removed.
		/// </para>
		/// </remarks>
		public void Clear()
		{
			m_map.Clear();
			m_cache.Clear();
		}

		/// <summary>
		/// Register an <see cref="T:log4net.ObjectRenderer.IObjectRenderer" /> for <paramref name="typeToRender" />. 
		/// </summary>
		/// <param name="typeToRender">the type that will be rendered by <paramref name="renderer" /></param>
		/// <param name="renderer">the renderer for <paramref name="typeToRender" /></param>
		/// <remarks>
		/// <para>
		/// Register an object renderer for a specific source type.
		/// This renderer will be returned from a call to <see cref="M:log4net.ObjectRenderer.RendererMap.Get(System.Type)" />
		/// specifying the same <paramref name="typeToRender" /> as an argument.
		/// </para>
		/// </remarks>
		public void Put(Type typeToRender, IObjectRenderer renderer)
		{
			m_cache.Clear();
			if ((object)typeToRender == null)
			{
				throw new ArgumentNullException("typeToRender");
			}
			if (renderer == null)
			{
				throw new ArgumentNullException("renderer");
			}
			m_map[typeToRender] = renderer;
		}
	}
}
