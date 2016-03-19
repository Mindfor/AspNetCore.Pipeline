namespace Microsoft.AspNetCore.Builder
{
	/// <summary>
	/// Options used by <see cref="Mindfor.AspNetCore.BasicAuthenticationHandler{TUser}"/>.
	/// </summary>
	public class BasicAuthenticationOptions : HeaderAuthenticationOptions
	{
		/// <summary>
		/// Authentication scheme for basic authentication.
		/// </summary>
		public const string BasicAuthenticationScheme = "Basic";

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public BasicAuthenticationOptions()
		{
			AuthenticationScheme = BasicAuthenticationScheme;
		}
	}
}