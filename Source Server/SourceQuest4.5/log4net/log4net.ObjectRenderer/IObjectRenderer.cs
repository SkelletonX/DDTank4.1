using System.IO;

namespace log4net.ObjectRenderer
{
	/// <summary>
	/// Implement this interface in order to render objects as strings
	/// </summary>
	/// <remarks>
	/// <para>
	/// Certain types require special case conversion to
	/// string form. This conversion is done by an object renderer.
	/// Object renderers implement the <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />
	/// interface.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IObjectRenderer
	{
		/// <summary>
		/// Render the object <paramref name="obj" /> to a string
		/// </summary>
		/// <param name="rendererMap">The map used to lookup renderers</param>
		/// <param name="obj">The object to render</param>
		/// <param name="writer">The writer to render to</param>
		/// <remarks>
		/// <para>
		/// Render the object <paramref name="obj" /> to a 
		/// string.
		/// </para>
		/// <para>
		/// The <paramref name="rendererMap" /> parameter is
		/// provided to lookup and render other objects. This is
		/// very useful where <paramref name="obj" /> contains
		/// nested objects of unknown type. The <see cref="M:log4net.ObjectRenderer.RendererMap.FindAndRender(System.Object,System.IO.TextWriter)" />
		/// method can be used to render these objects.
		/// </para>
		/// </remarks>
		void RenderObject(RendererMap rendererMap, object obj, TextWriter writer);
	}
}
