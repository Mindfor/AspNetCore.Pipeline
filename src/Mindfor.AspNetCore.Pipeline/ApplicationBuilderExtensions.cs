using System;
using Microsoft.Extensions.Options;
using Mindfor.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
	/// <summary>
	/// Extension methods for <see cref="IApplicationBuilder"/> to add Mindfor middlewares to the request execution pipeline.
	/// </summary>
	public static class PipelineApplicationBuilderExtensions
	{
		/// <summary>
		/// Adds middleware to require HTTPS connections only.
		/// All HTTP connections will be redirected to HTTPS.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		public static IApplicationBuilder UseRequireHttps(this IApplicationBuilder app)
		{
			return app.UseMiddleware<RequireSchemeMiddleware>(RequireScheme.Https);
		}

		/// <summary>
		/// Adds middleware to require HTTPS connections only.
		/// All HTTP connections will be redirected to HTTPS.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		/// <param name="options">Options for <see cref="RequireSchemeMiddleware"/>.</param>
		public static IApplicationBuilder UseRequireHttps(this IApplicationBuilder app, RequireSchemeOptions options)
		{
			if (options == null)
				throw new ArgumentNullException(nameof(options));
			return app.UseMiddleware<RequireSchemeMiddleware>(RequireScheme.Https, Options.Create(options));
		}

		/// <summary>
		/// Adds middleware to require HTTP connections only.
		/// All HTTPS connections will be redirected to HTTP.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		public static IApplicationBuilder UseRequireHttp(this IApplicationBuilder app)
		{
			return app.UseMiddleware<RequireSchemeMiddleware>(RequireScheme.Http);
		}

		/// <summary>
		/// Adds middleware to require HTTP connections only.
		/// All HTTPS connections will be redirected to HTTP.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		/// <param name="options">Options for <see cref="RequireSchemeMiddleware"/>.</param>
		public static IApplicationBuilder UseRequireHttp(this IApplicationBuilder app, RequireSchemeOptions options)
		{
			if (options == null)
				throw new ArgumentNullException(nameof(options));
			return app.UseMiddleware<RequireSchemeMiddleware>(RequireScheme.Http, Options.Create(options));
		}

		/// <summary>
		/// Adds basic authentication middleware to the application pipeline.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		/// <typeparam name="TUser">Type of identity user.</typeparam>
		/// <returns>The original app parameter</returns>
		public static IApplicationBuilder UseBasicAuthentication<TUser>(this IApplicationBuilder app)
			where TUser : class
		{
			return app.UseHeaderAuthentication<BasicAuthenticationHandler<TUser>, BasicAuthenticationOptions, TUser>();
		}

		/// <summary>
		/// Adds a basic authentication middleware to your web application pipeline.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		/// <param name="options">Options for <see cref="BasicAuthenticationHandler{User}"/>.</param>
		/// <typeparam name="TUser">Type of identity user.</typeparam>
		/// <returns>The original app parameter</returns>
		public static IApplicationBuilder UseBasicAuthentication<TUser>(this IApplicationBuilder app, BasicAuthenticationOptions options)
			where TUser : class
		{
			if (options == null)
				throw new ArgumentNullException(nameof(options));
			return app.UseHeaderAuthentication<BasicAuthenticationHandler<TUser>, BasicAuthenticationOptions, TUser>(options);
		}

		/// <summary>
		/// Adds a header authentication middleware to your web application pipeline.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		/// <typeparam name="THandler">Type of header authentication handler.</typeparam>
		/// <typeparam name="TOptions">Type of header authentication options.</typeparam>
		/// <typeparam name="TUser">Type of identity user.</typeparam>
		/// <returns>The original app parameter</returns>
		public static IApplicationBuilder UseHeaderAuthentication<THandler, TOptions, TUser>(this IApplicationBuilder app)
			where THandler : HeaderAuthenticationHandler<TOptions, TUser>, new()
			where TOptions : HeaderAuthenticationOptions, new()
			where TUser : class
		{
			return app.UseMiddleware<HeaderAuthenticationMiddleware<THandler, TOptions, TUser>>();
		}

		/// <summary>
		/// Adds a header authentication middleware to your web application pipeline.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
		/// <param name="options">Options for <see cref="HeaderAuthenticationHandler{TOptions,User}"/>.</param>
		/// <typeparam name="THandler">Type of header authentication handler.</typeparam>
		/// <typeparam name="TOptions">Type of header authentication options.</typeparam>
		/// <typeparam name="TUser">Type of identity user.</typeparam>
		/// <returns>The original app parameter</returns>
		public static IApplicationBuilder UseHeaderAuthentication<THandler, TOptions, TUser>(this IApplicationBuilder app, TOptions options)
			where THandler : HeaderAuthenticationHandler<TOptions, TUser>, new()
			where TOptions : HeaderAuthenticationOptions, new()
			where TUser : class
		{
			if (options == null)
				throw new ArgumentNullException(nameof(options));
			return app.UseMiddleware<HeaderAuthenticationMiddleware<THandler, TOptions, TUser>>(Options.Create(options));
		}
	}
}