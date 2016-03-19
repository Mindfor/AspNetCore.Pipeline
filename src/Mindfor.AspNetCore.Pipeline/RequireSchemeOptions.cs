namespace Microsoft.AspNetCore.Builder
{
	/// <summary>
	/// Provides options for <see cref="Mindfor.AspNetCore.RequireSchemeMiddleware"/>.
	/// </summary>
	public class RequireSchemeOptions
	{
		/// <summary>
		/// Gets or sets host name to redirect to if scheme does not match.
		/// If null then redirects to current host name.
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// Gets or sets port number to redirect to if scheme does not match.
		/// If null then redirects to the default port.
		/// </summary>
		public int? Port { get; set; }
	}
}