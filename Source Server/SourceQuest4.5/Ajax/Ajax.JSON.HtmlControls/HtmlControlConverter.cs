using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Ajax.JSON.HtmlControls
{
	internal class HtmlControlConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => "AjaxHtmlControl";

		public virtual Type[] SupportedTypes => new Type[1]
		{
			typeof(HtmlControl)
		};

		public bool IncludeSubclasses => false;

		public void RenderClientScript(ref StringBuilder sb)
		{
			sb.Append("function HtmlControl(id) {\r\n\tvar ele = null;\r\n\tif(typeof(id) == 'object') ele = id; else ele = document.getElementById(id);\r\n\tif(ele == null) return null;\r\n\tvar _o = ele.cloneNode(true);\r\n\tvar _op = document.createElement('SPAN');\r\n\t_op.appendChild(_o);\t\r\n\tthis._source = _op.innerHTML;\r\n}\r\nHtmlControl.prototype.toString = function(){ return this._source; }\r\n\r\nfunction HtmlControlUpdate(func, parentId) {\r\nvar f,i,ff,fa='';\r\nvar ele = document.getElementById(parentId);\r\nif(ele == null) return;\r\nvar args = [];\r\nfor(i=0; i<HtmlControlUpdate.arguments.length; i++)\r\n\targs[args.length] = HtmlControlUpdate.arguments[i];\r\nif(args.length > 2)\r\n\tfor(i=2; i<args.length; i++){fa += 'args[' + i + ']';if(i < args.length -1){ fa += ','; }}\r\nf = '{\"invoke\":function(args){return ' + func + '(' + fa + ');}}';\r\nff = null;eval('ff=' + f + ';');\r\nif(ff != null && typeof(ff.invoke) == 'function')\r\n{\r\n\tvar res = ff.invoke(args);\r\n\tif(res.error != null){alert(res.error);return;}\r\n\tele.innerHTML = res.value;\r\n}\r\n}\r\n");
		}

		public object FromString(string s, Type t)
		{
			return AjaxHtmlControlConverter.HtmlToHtmlControl(s, t);
		}

		public void ToJSON(ref StringBuilder sb, object o)
		{
			DefaultConverter.ToJSON(ref sb, AjaxHtmlControlConverter.ControlToString((Control)o));
		}
	}
}
