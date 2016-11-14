namespace Sonos
{
	/// <summary>
	/// Song.
	/// </summary>
	public class Song
	{
		string content;
		string res;
		string title;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sonos.Song"/> class.
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="res">Res.</param>
		/// <param name="title">Title.</param>
		public Song (string content, string res, string title)
		{
			this.content = content;
			this.res = res;
			this.title = title;
		}

		/// <summary>
		/// Tos the string.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString ()
		{
			return "Song{" + "content=" + content + ", res=" + res + ", title=" + title + '}';
		}

		/// <summary>
		/// Content this instance.
		/// </summary>
		public string Content ()
		{
			return content;
		}

		/// <summary>
		/// Res this instance.
		/// </summary>
		public string Res ()
		{
			return res;
		}

		/// <summary>
		/// Title this instance.
		/// </summary>
		public string Title ()
		{
			return title;
		}
	}
}
