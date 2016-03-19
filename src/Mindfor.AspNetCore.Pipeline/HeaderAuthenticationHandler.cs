using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Mindfor.AspNetCore
{
	/// <summary>
	/// Handles authentication via "Authorization" header.
	/// </summary>
	/// <typeparam name="TOptions">Type of authentication options.</typeparam>
	/// <typeparam name="TUser">Type of identity user.</typeparam>
	public abstract class HeaderAuthenticationHandler<TOptions, TUser> : AuthenticationHandler<TOptions>
		where TOptions : HeaderAuthenticationOptions
		where TUser : class
	{
		public const string AuthorizationHeader = "Authorization";
		public const string WWWAuthenticateHeader = "WWW-Authenticate";

		/// <inheritdoc />
		protected override sealed async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			// get authorization header
			string auth = Request.Headers[AuthorizationHeader];
			if (auth == null)
				return AuthenticateResult.Skip();

			// confirm authorization scheme
			string[] authParts = auth.Split(' ');
			if (authParts.Length != 2)
				return AuthenticateResult.Fail("Invalid authorization header");
			else if (!ShouldHandleScheme(authParts[0], Options.AutomaticAuthenticate))
				return AuthenticateResult.Fail($"Authorization scheme \"{authParts[0]}\" is not supported");

			// extract user and password
			string base64 = authParts[1];
			string authValue;
			try
			{
				byte[] bytes = Convert.FromBase64String(base64);
				authValue = Encoding.ASCII.GetString(bytes);
			}
			catch
			{
				authValue = null;
			}
			if (string.IsNullOrEmpty(authValue))
				return AuthenticateResult.Fail("Invalid authorization header base64 value");

			// find and validate user
			var user = await HandleAuthenticateHeaderAsync(authValue);
			if (user == null)
				return AuthenticateResult.Fail("User was not found");

			string validationResult = await ValidateUserAsync(user);
			if (validationResult != null)
				return AuthenticateResult.Fail(validationResult);

			// create authentication ticket
			var ticket = await CreateAuthenticationTicketAsync(user);
			return AuthenticateResult.Success(ticket);
		}

		/// <inheritdoc />
		protected override Task<bool> HandleForbiddenAsync(ChallengeContext context)
		{
			Response.Headers[WWWAuthenticateHeader] = $"{Options.AuthenticationScheme} realm=\"{Options.Realm}\"";
			return base.HandleForbiddenAsync(context);
		}

		/// <summary>
		/// Validates that user is active and can be signed in.
		/// </summary>
		/// <param name="user">User to validate.</param>
		/// <returns><c>Null</c> is user is valid and can be signed in; error string message if user is invalid.</returns>
		protected virtual async Task<string> ValidateUserAsync(TUser user)
		{
			var userManager = Context.RequestServices.GetRequiredService<UserManager<TUser>>();
			if (await userManager.IsLockedOutAsync(user))
				return "User is locked out";
			return null;
		}

		/// <summary>
		/// Creates authentication ticket for specified user.
		/// </summary>
		/// <param name="user">Validated user.</param>
		/// <returns>Authentication ticket for user.</returns>
		protected virtual async Task<AuthenticationTicket> CreateAuthenticationTicketAsync(TUser user)
		{
			var signInManager = Context.RequestServices.GetRequiredService<SignInManager<TUser>>();
			var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(user);
			var props = new AuthenticationProperties();
			return new AuthenticationTicket(claimsPrincipal, props, Options.AuthenticationScheme);
		}

		/// <summary>
		/// Tries to authenticate user from header value.
		/// </summary>
		/// <param name="authValue">Value converted from base64 Authenticate header.</param>
		/// <returns>User if authentication header matches; otherwise <c>null</c>.</returns>
		protected abstract Task<TUser> HandleAuthenticateHeaderAsync(string authValue);
	}
}