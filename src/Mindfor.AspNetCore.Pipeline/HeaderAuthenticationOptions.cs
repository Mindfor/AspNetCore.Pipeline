namespace Microsoft.AspNetCore.Builder
{
	/// <summary>
	/// Options for <see cref="Mindfor.AspNetCore.HeaderAuthenticationHandler{TOptions, TUser}"/>.
	/// </summary>
	public class HeaderAuthenticationOptions : AuthenticationOptions
	{
		/// <summary>
		/// Default authentication realm.
		/// </summary>
		public const string DefaultRealm = "app";

		/// <summary>
		/// Gets or sets protection space name. Realms allow the protected resources on
		/// a server to be partitioned into a set of protection spaces, each with its own
		/// authentication scheme and/or authorization database.
		/// </summary>
		public string Realm { get; set; } = DefaultRealm;

		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public HeaderAuthenticationOptions()
		{
			AutomaticAuthenticate = true;
		}
	}
}