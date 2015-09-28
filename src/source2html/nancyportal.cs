using System;
using System.Web;
using System.Net;

using Nancy.Responses;

namespace source2html
{
	// https://source2html.apphb.com?lexer=csharp&linenos=1&ashtml=1&codeurl=https://gist.githubusercontent.com/ralfw/42b0071e49ce12c9ecd4/raw/c07b073ceb9fbdac8f9904766e4d171b2a7ed8b4/01%2520program.cs
	// http://127.0.0.1:8080/?lexer=csharp&linenos=1&ashtml=1&codeurl=https://gist.githubusercontent.com/ralfw/42b0071e49ce12c9ecd4/raw/c07b073ceb9fbdac8f9904766e4d171b2a7ed8b4/01%2520program.cs

	// http://hilite.me/api
	// lexers: http://pygments.org/docs/lexers/, csharp, fsharp, js, java, cpp
	// styles: http://pygments.org/docs/styles/#builtin-styles, colorful

	public class Nancyportal : Nancy.NancyModule
	{
		public Nancyportal ()
		{
			// usage: ...?codeurl=...&lexer=...&linenos=1&ashtml=1
			Get ["/"] = x => {
				var codeurl = Request.Query["codeurl"];
				if (!codeurl.HasValue)
					return "Usage: ...?codeurl=...&linenos=1&lexer=csharp";

				var code = "";
				using(var wc = new WebClient()) {
					code = wc.DownloadString(codeurl);
					code = HttpUtility.UrlEncode(code);
				}

				var lexer = Request.Query["lexer"];
				var linenos = Request.Query["linenos"];
				var ashtml = Request.Query["ashtml"];

				var hiliteurl = string.Format("http://hilite.me/api?code={0}&lexer={1}&linenos={2}", code, lexer, linenos);
				var html = "";
				using(var wc = new WebClient()) {
					wc.Encoding = System.Text.Encoding.UTF8;
					html = wc.DownloadString(hiliteurl); // in html ist ein umlaut korrekt enthalten
				}


				// ab hier passiert etwas, so dass umlaute u.u. nicht korrekt beim empfänger ankommen :-(
				// lokal funktioniert es zwar, aber wenn hosted by appharbor, dann irgendwie nicht.
				var htmlbytes = System.Text.Encoding.UTF8.GetBytes(html);

				return new Nancy.Response {
					ContentType = ashtml.HasValue ? "text/html; charset=utf-8" : "text/plain; charset=utf-8",
					Contents = s => s.Write(htmlbytes, 0, htmlbytes.Length)
				};
			};
		}
	}
}