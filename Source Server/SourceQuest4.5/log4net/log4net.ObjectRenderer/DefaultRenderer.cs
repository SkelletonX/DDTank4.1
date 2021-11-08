using log4net.Util;
using System;
using System.Collections;
using System.IO;

namespace log4net.ObjectRenderer
{
	/// <summary>
	/// The default object Renderer.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The default renderer supports rendering objects and collections to strings.
	/// </para>
	/// <para>
	/// See the <see cref="M:log4net.ObjectRenderer.DefaultRenderer.RenderObject(log4net.ObjectRenderer.RendererMap,System.Object,System.IO.TextWriter)" /> method for details of the output.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class DefaultRenderer : IObjectRenderer
	{
		/// <summary>
		/// Render the object <paramref name="obj" /> to a string
		/// </summary>
		/// <param name="rendererMap">The map used to lookup renderers</param>
		/// <param name="obj">The object to render</param>
		/// <param name="writer">The writer to render to</param>
		/// <remarks>
		/// <para>
		/// Render the object <paramref name="obj" /> to a string.
		/// </para>
		/// <para>
		/// The <paramref name="rendererMap" /> parameter is
		/// provided to lookup and render other objects. This is
		/// very useful where <paramref name="obj" /> contains
		/// nested objects of unknown type. The <see cref="M:log4net.ObjectRenderer.RendererMap.FindAndRender(System.Object)" />
		/// method can be used to render these objects.
		/// </para>
		/// <para>
		/// The default renderer supports rendering objects to strings as follows:
		/// </para>
		/// <list type="table">
		/// 	<listheader>
		/// 		<term>Value</term>
		/// 		<description>Rendered String</description>
		/// 	</listheader>
		/// 	<item>
		/// 		<term><c>null</c></term>
		/// 		<description>
		/// 		<para>"(null)"</para>
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term><see cref="T:System.Array" /></term>
		/// 		<description>
		/// 		<para>
		/// 		For a one dimensional array this is the
		/// 		array type name, an open brace, followed by a comma
		/// 		separated list of the elements (using the appropriate
		/// 		renderer), followed by a close brace. 
		/// 		</para>
		/// 		<para>
		/// 		For example: <c>int[] {1, 2, 3}</c>.
		/// 		</para>
		/// 		<para>
		/// 		If the array is not one dimensional the 
		/// 		<c>Array.ToString()</c> is returned.
		/// 		</para>
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term><see cref="T:System.Collections.IEnumerable" />, <see cref="T:System.Collections.ICollection" /> &amp; <see cref="T:System.Collections.IEnumerator" /></term>
		/// 		<description>
		/// 		<para>
		/// 		Rendered as an open brace, followed by a comma
		/// 		separated list of the elements (using the appropriate
		/// 		renderer), followed by a close brace.
		/// 		</para>
		/// 		<para>
		/// 		For example: <c>{a, b, c}</c>.
		/// 		</para>
		/// 		<para>
		/// 		All collection classes that implement <see cref="T:System.Collections.ICollection" /> its subclasses, 
		/// 		or generic equivalents all implement the <see cref="T:System.Collections.IEnumerable" /> interface.
		/// 		</para>
		/// 		</description>
		/// 	</item>		
		/// 	<item>
		/// 		<term><see cref="T:System.Collections.DictionaryEntry" /></term>
		/// 		<description>
		/// 		<para>
		/// 		Rendered as the key, an equals sign ('='), and the value (using the appropriate
		/// 		renderer). 
		/// 		</para>
		/// 		<para>
		/// 		For example: <c>key=value</c>.
		/// 		</para>
		/// 		</description>
		/// 	</item>		
		/// 	<item>
		/// 		<term>other</term>
		/// 		<description>
		/// 		<para><c>Object.ToString()</c></para>
		/// 		</description>
		/// 	</item>
		/// </list>
		/// </remarks>
		public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
		{
			if (rendererMap == null)
			{
				throw new ArgumentNullException("rendererMap");
			}
			if (obj == null)
			{
				writer.Write(SystemInfo.NullText);
				return;
			}
			Array array = obj as Array;
			if (array != null)
			{
				RenderArray(rendererMap, array, writer);
				return;
			}
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable != null)
			{
				ICollection collection = obj as ICollection;
				if (collection != null && collection.Count == 0)
				{
					writer.Write("{}");
					return;
				}
				IDictionary dictionary = obj as IDictionary;
				if (dictionary != null)
				{
					RenderEnumerator(rendererMap, dictionary.GetEnumerator(), writer);
				}
				else
				{
					RenderEnumerator(rendererMap, enumerable.GetEnumerator(), writer);
				}
			}
			else
			{
				IEnumerator enumerator = obj as IEnumerator;
				if (enumerator != null)
				{
					RenderEnumerator(rendererMap, enumerator, writer);
					return;
				}
				if (obj is DictionaryEntry)
				{
					RenderDictionaryEntry(rendererMap, (DictionaryEntry)obj, writer);
					return;
				}
				string text = obj.ToString();
				writer.Write((text == null) ? SystemInfo.NullText : text);
			}
		}

		/// <summary>
		/// Render the array argument into a string
		/// </summary>
		/// <param name="rendererMap">The map used to lookup renderers</param>
		/// <param name="array">the array to render</param>
		/// <param name="writer">The writer to render to</param>
		/// <remarks>
		/// <para>
		/// For a one dimensional array this is the
		/// array type name, an open brace, followed by a comma
		/// separated list of the elements (using the appropriate
		/// renderer), followed by a close brace. For example:
		/// <c>int[] {1, 2, 3}</c>.
		/// </para>
		/// <para>
		/// If the array is not one dimensional the 
		/// <c>Array.ToString()</c> is returned.
		/// </para>
		/// </remarks>
		private void RenderArray(RendererMap rendererMap, Array array, TextWriter writer)
		{
			if (array.Rank != 1)
			{
				writer.Write(array.ToString());
				return;
			}
			writer.Write(array.GetType().Name + " {");
			int length = array.Length;
			if (length > 0)
			{
				rendererMap.FindAndRender(array.GetValue(0), writer);
				for (int i = 1; i < length; i++)
				{
					writer.Write(", ");
					rendererMap.FindAndRender(array.GetValue(i), writer);
				}
			}
			writer.Write("}");
		}

		/// <summary>
		/// Render the enumerator argument into a string
		/// </summary>
		/// <param name="rendererMap">The map used to lookup renderers</param>
		/// <param name="enumerator">the enumerator to render</param>
		/// <param name="writer">The writer to render to</param>
		/// <remarks>
		/// <para>
		/// Rendered as an open brace, followed by a comma
		/// separated list of the elements (using the appropriate
		/// renderer), followed by a close brace. For example:
		/// <c>{a, b, c}</c>.
		/// </para>
		/// </remarks>
		private void RenderEnumerator(RendererMap rendererMap, IEnumerator enumerator, TextWriter writer)
		{
			writer.Write("{");
			if (enumerator != null && enumerator.MoveNext())
			{
				rendererMap.FindAndRender(enumerator.Current, writer);
				while (enumerator.MoveNext())
				{
					writer.Write(", ");
					rendererMap.FindAndRender(enumerator.Current, writer);
				}
			}
			writer.Write("}");
		}

		/// <summary>
		/// Render the DictionaryEntry argument into a string
		/// </summary>
		/// <param name="rendererMap">The map used to lookup renderers</param>
		/// <param name="entry">the DictionaryEntry to render</param>
		/// <param name="writer">The writer to render to</param>
		/// <remarks>
		/// <para>
		/// Render the key, an equals sign ('='), and the value (using the appropriate
		/// renderer). For example: <c>key=value</c>.
		/// </para>
		/// </remarks>
		private void RenderDictionaryEntry(RendererMap rendererMap, DictionaryEntry entry, TextWriter writer)
		{
			rendererMap.FindAndRender(entry.Key, writer);
			writer.Write("=");
			rendererMap.FindAndRender(entry.Value, writer);
		}
	}
}
