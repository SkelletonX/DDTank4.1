using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
	public delegate IEnumerable<KeyValuePair<object, object>> ExtensionDataGetter(object o);
}
