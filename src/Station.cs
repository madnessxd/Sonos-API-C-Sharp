namespace Sonos
{
	/// <summary>
	/// Station.
	/// </summary>
	public class Station
	{
		string title;
		string classObject;
		string ordinal;
		string uri;
		string albumArtURI;
		string type;
		string description;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sonos.Station"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="classObject">Class object.</param>
		/// <param name="ordinal">Ordinal.</param>
		/// <param name="uri">URI.</param>
		/// <param name="albumArtURI">Album art URI.</param>
		/// <param name="type">Type.</param>
		/// <param name="description">Description.</param>
		public Station (string title, string classObject, string ordinal, string uri, string albumArtURI, string type, string description)
		{
			this.title = title;
			this.classObject = classObject;
			this.ordinal = ordinal;
			this.uri = uri;
			this.albumArtURI = albumArtURI;
			this.type = type;
			this.description = description;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Sonos.Station"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Sonos.Station"/>.</returns>
		public override string ToString ()
		{
			return "Station{" + "title=" + title + ", classObject=" + classObject + ", ordinal=" + ordinal + ", uri=" + uri + ", albumArtURI=" + albumArtURI + ", type=" + type + ", description=" + description + '}';
		}

		/// <summary>
		/// Title this instance.
		/// </summary>
		public string Title ()
		{
			return title;
		}

		/// <summary>
		/// Classes the object.
		/// </summary>
		/// <returns>The object.</returns>
		public string ClassObject ()
		{
			return classObject;
		}

		/// <summary>
		/// Ordinal this instance.
		/// </summary>
		public string Ordinal ()
		{
			return ordinal;
		}

		/// <summary>
		/// URI this instance.
		/// </summary>
		public string Uri ()
		{
			return uri;
		}

		/// <summary>
		/// Albums the art URI.
		/// </summary>
		/// <returns>The art URI.</returns>
		public string AlbumArtURI ()
		{
			return albumArtURI;
		}

		/// <summary>
		/// Type this instance.
		/// </summary>
		public string Type ()
		{
			return type;
		}

		/// <summary>
		/// Description this instance.
		/// </summary>
		public string Description ()
		{
			return description;
		}
	}
}
