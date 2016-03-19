using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Mindfor.AspNetCore
{
	/// <summary>
	/// Handles basic authentication via "Authorization" header.
	/// </summary>
	/// <typeparam name="TUser">Type of identity user.</typeparam>
	public class BasicAuthenticationHandler<TUser> : HeaderAuthenticationHandler<BasicAuthenticationOptions, TUser>
		where TUser : class
	{
		/// <inheritdoc/>
		protected override async Task<TUser> HandleAuthenticateHeaderAsync(string value)
		{
			// extract username and password
			string userName;
			string password;
			int sepIndex = value.IndexOf(':');
			if (sepIndex == -1)
			{
				userName = value;
				password = null;
			}
			else
			{
				userName = value.Substring(0, sepIndex);
				password = value.Substring(sepIndex + 1);
			}

			// find user and validate password
			var userManager = Context.RequestServices.GetRequiredService<UserManager<TUser>>();
			var user = await userManager.FindByNameAsync(userName);
			if (user == null)
				user = await userManager.FindByEmailAsync(userName);
			if (user == null || !await userManager.CheckPasswordAsync(user, password))
				return null;
			return user;
		}
	}
}