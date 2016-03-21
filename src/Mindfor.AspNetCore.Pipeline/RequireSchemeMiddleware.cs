using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Mindfor.AspNetCore
{
	/// <summary>
	/// Validates request scheme. Can redirect HTTP => HTTPS or HTTPS => HTTP.
	/// </summary>
	public class RequireSchemeMiddleware
	{
		readonly RequestDelegate _next;
		readonly RequireScheme _requiredScheme;
		readonly RequireSchemeOptions _options;

		/// <summary>
		/// Create new middleware instance.
		/// </summary>
		/// <param name="next">Pipeline next handler.</param>
		/// <param name="requiredScheme">
		/// Required request scheme. If <see cref="RequireScheme.Https"/> then
		/// HTTP requests will be redirected to HTTPs and vice versa.
		/// </param>
		/// <param name="options">Redirect options.</param>
		public RequireSchemeMiddleware(RequestDelegate next, RequireScheme requiredScheme, IOptions<RequireSchemeOptions> options)
		{
			if (next == null)
				throw new ArgumentNullException(nameof(next));
			if (options == null)
				throw new ArgumentNullException(nameof(options));
			_next = next;
			_requiredScheme = requiredScheme;
			_options = options.Value;
		}

		/// <summary>
		/// Invokes middleware that validated request scheme.
		/// </summary>
		public async Task Invoke(HttpContext context)
		{
			var request = context.Request;
			if (_requiredScheme == RequireScheme.Https && !request.IsHttps ||
				_requiredScheme == RequireScheme.Http && request.IsHttps)
			{
				// only redirect for GET and HEAD requests
				// otherwise the browser might not propagate the verb and request body correctly
				if (!string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase) &&
					!string.Equals(request.Method, "HEAD", StringComparison.OrdinalIgnoreCase))
				{
					context.Response.StatusCode = StatusCodes.Status403Forbidden;
				}
				else
				{
					RedirectToScheme(context);
				}
				return;
			}

			// process request
			await _next(context);
		}

		/// <summary>
		/// Redirects current request to required in options scheme.
		/// </summary>
		/// <param name="context"></param>
		void RedirectToScheme(HttpContext context)
		{
			var request = context.Request;

			// generate destination host string
			string host = request.Host.Value;
			if (!string.IsNullOrEmpty(_options?.Host))
			{
				int portIndex = host.LastIndexOf(":");
				if (portIndex == -1)
					host = _options.Host;
				else
					host = _options.Host + host.Substring(portIndex);
			}

			if (_options?.Port != null)
			{
				int portIndex = host.LastIndexOf(":");
				if (portIndex != -1)
					host = host.Substring(0, portIndex);
				host += ":" + _options.Port;
			}

			// redirect to destination protocol version of page
			var newUrl = string.Concat(
				_requiredScheme.ToString().ToLowerInvariant(),
				"://",
				host,
				request.PathBase,
				request.Path,
				request.QueryString);
			context.Response.Redirect(newUrl, permanent: true);
		}
	}

	/// <summary>
	/// Required scheme for <see cref="Mindfor.AspNetCore.RequireSchemeMiddleware"/>.
	/// </summary>
	public enum RequireScheme
	{
		Http,
		Https
	}
}