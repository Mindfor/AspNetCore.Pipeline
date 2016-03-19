using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mindfor.AspNetCore
{
	/// <summary>
	/// Provides authentication via HTTP "Authorization" header.
	/// </summary>
	/// <typeparam name="THandler">Type of authentication handler.</typeparam>
	/// <typeparam name="TUser">Type of identity user.</typeparam>
	/// <typeparam name="TOptions">Type of authentication handler options.</typeparam>
	public class HeaderAuthenticationMiddleware<THandler, TOptions, TUser> : AuthenticationMiddleware<TOptions>
		where THandler : HeaderAuthenticationHandler<TOptions, TUser>, new()
		where TOptions : HeaderAuthenticationOptions, new()
		where TUser : class
	{
		/// <summary>
		/// Creates new middleware instance.
		/// </summary>
		public HeaderAuthenticationMiddleware(RequestDelegate next,
			IOptions<TOptions> options,
			ILoggerFactory loggerFactory,
			UrlEncoder urlEncoder)
			: base(next, options, loggerFactory, urlEncoder)
		{
		}

		/// <inheritdoc/>
		protected override AuthenticationHandler<TOptions> CreateHandler()
		{
			return new THandler();
		}
	}
}