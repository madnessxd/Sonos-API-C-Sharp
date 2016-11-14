using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Sonos
{
	/// <summary>
	/// Sonos API.
	/// </summary>
	public class SonosAPI
	{
		private readonly string ip;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sonos.SonosAPI"/> class.
		/// </summary>
		/// <param name="ip">Ip.</param>
		public SonosAPI (string ip)
		{
			this.ip = ip;
		}

		private string DoWebRequest (string url, string SOAPACTION, string xmlData)
		{
			try {
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create ("http://" + ip + ":1400" + url);
				webRequest.Headers.Add ("SOAPACTION", SOAPACTION);
				webRequest.Method = "POST";
				webRequest.Timeout = 2000;
				webRequest.ContentType = "text/xml";

				string postData = xmlData;
				byte [] byteArray = Encoding.UTF8.GetBytes (postData);
				webRequest.ContentLength = byteArray.Length;

				Stream dataStream = webRequest.GetRequestStream ();
				dataStream.Write (byteArray, 0, byteArray.Length);
				dataStream.Close ();


				WebResponse response = webRequest.GetResponse ();
				dataStream = response.GetResponseStream ();
				StreamReader reader = new StreamReader (dataStream);
				string responseFromServer = reader.ReadToEnd ();

				reader.Close ();
				response.Close ();
				return responseFromServer;

			} catch (Exception e) {
				return e.ToString ();
			}
		}

		/// <summary>
		/// Pauses the sonos.
		/// </summary>
		/// <returns>The sonos.</returns>
		public string PauseSonos ()
		{
			return DoWebRequest ("/MediaRenderer/AVTransport/Control",
								 "\"urn:schemas-upnp-org:service:AVTransport:1#Pause\"",
								 "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
								"	<s:Body>\n" +
								"		<u:Pause xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
								"			<InstanceID>0</InstanceID>\n" +
								"			<Speed>1</Speed>\n" +
								"		</u:Pause>\n" +
								"	</s:Body>\n" +
								"</s:Envelope>");
		}

		/// <summary>
		/// Plaies the sonos.
		/// </summary>
		/// <returns>The sonos.</returns>
		public string PlaySonos ()
		{
			return DoWebRequest ("/MediaRenderer/AVTransport/Control",
								 "\"urn:schemas-upnp-org:service:AVTransport:1#Play\"",
								 "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
								"	<s:Body>\n" +
								"		<u:Play xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
								"			<InstanceID>0</InstanceID>\n" +
								"			<Speed>1</Speed>\n" +
								"		</u:Play>\n" +
								"	</s:Body>\n" +
								"</s:Envelope>");
		}

		/// <summary>
		/// Nexts the song.
		/// </summary>
		/// <returns>The song.</returns>
		public string NextSong ()
		{
			return DoWebRequest ("/MediaRenderer/AVTransport/Control",
								 "\"urn:schemas-upnp-org:service:AVTransport:1#Next\"",
								 "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
								"	<s:Body>\n" +
								"		<u:Next xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
								"			<InstanceID>0</InstanceID>\n" +
								"			<Speed>1</Speed>\n" +
								"		</u:Next>\n" +
								"	</s:Body>\n" +
								"</s:Envelope>");
		}

		/// <summary>
		/// Previouses the song.
		/// </summary>
		/// <returns>The song.</returns>
		public string PreviousSong ()
		{
			return DoWebRequest ("/MediaRenderer/AVTransport/Control",
								 "\"urn:schemas-upnp-org:service:AVTransport:1#Previous\"",
								 "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
								"	<s:Body>\n" +
								"		<u:Previous xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
								"			<InstanceID>0</InstanceID>\n" +
								"			<Speed>1</Speed>\n" +
								"		</u:Previous>\n" +
								"	</s:Body>\n" +
								"</s:Envelope>");
		}

		/// <summary>
		/// Gets the sonos favorites.
		/// </summary>
		/// <returns>The sonos favorites.</returns>
		public Station [] GetSonosFavorites ()
		{
			string response = DoWebRequest ("/MediaServer/ContentDirectory/Control",
								 "\"urn:schemas-upnp-org:service:ContentDirectory:1#Browse\"",
								 "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
								"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
								"   <s:Body>\n" +
								"      <u:Browse xmlns:u=\"urn:schemas-upnp-org:service:ContentDirectory:1\">\n" +
								"         <ObjectID>FV:2</ObjectID>\n" +
								"         <BrowseFlag>BrowseDirectChildren</BrowseFlag>\n" +
								"         <Filter>dc:title,res,dc:creator,upnp:artist,upnp:album,upnp:albumArtURI</Filter>\n" +
								"         <StartingIndex>0</StartingIndex>\n" +
								"         <RequestedCount>100</RequestedCount>\n" +
								"         <SortCriteria />\n" +
								"      </u:Browse>\n" +
								"   </s:Body>\n" +
								"</s:Envelope>");

			XmlDocument responseXML = new XmlDocument ();
			responseXML.LoadXml (response);

			string innerText = responseXML.SelectNodes ("//Result").Item (0).InnerText;
			responseXML.LoadXml (innerText);

			XmlNodeList stations2 = responseXML.GetElementsByTagName ("item");
			Station [] stations = new Station [stations2.Count];

			for (int i = 0; i < stations2.Count; i++) {
				XmlNode station = stations2 [i];

				string title = station.ChildNodes.Item (0).InnerText;
				string classObject = station.ChildNodes.Item (1).InnerText;
				string ordinal = station.ChildNodes.Item (2).InnerText;
				string uri = station.ChildNodes.Item (3).InnerText;
				string albumArtURI = station.ChildNodes.Item (4).InnerText;
				string type = station.ChildNodes.Item (5).InnerText;
				string description = station.ChildNodes.Item (6).InnerText;

				Station s = new Station (title, classObject, ordinal, uri, albumArtURI, type, description);
				stations [i] = s;
			}
			return stations;
		}

		/// <summary>
		/// Sets the volume.
		/// </summary>
		/// <returns>The volume.</returns>
		/// <param name="volume">Volume.</param>
		public string SetVolume (int volume)
		{
			return DoWebRequest ("/MediaRenderer/RenderingControl/Control",
								 "urn:schemas-upnp-org:service:RenderingControl:1#SetVolume",
								 "<s:Envelope \n" +
								"	xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"\n" +
								"	s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"\n" +
								"	>\n" +
								"  <s:Body>\n" +
								"    <u:SetVolume xmlns:u=\"urn:schemas-upnp-org:service:RenderingControl:1\">\n" +
								"      <InstanceID>0</InstanceID>\n" +
								"      <Channel>Master</Channel>\n" +
								"      <DesiredVolume>" + volume + "</DesiredVolume>\n" +
								"    </u:SetVolume>\n" +
								"  </s:Body>\n" +
								"</s:Envelope>");
		}

		/// <summary>
		/// Gets the current track info.
		/// </summary>
		/// <returns>The current track info.</returns>
		public Song GetCurrentTrackInfo ()
		{
			string response = DoWebRequest ("/MediaRenderer/AVTransport/Control",
								 "\"urn:schemas-upnp-org:service:AVTransport:1#GetPositionInfo\"",
								 "<s:Envelope \n" +
								"	xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"\n" +
								"	s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"\n" +
								"	>\n" +
								"  <s:Body>\n" +
								"    <u:GetPositionInfo xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
								"      <InstanceID>0</InstanceID>\n" +
								"    </u:GetPositionInfo>\n" +
								"  </s:Body>\n" +
								"</s:Envelope>\n" +
								"<!--MediaRenderer/AVTransport/Control-->");

			XmlDocument responseXML = new XmlDocument ();
			responseXML.LoadXml (response);

			string innerText = responseXML.SelectNodes ("//TrackMetaData").Item (0).InnerText;
			responseXML.LoadXml (innerText);

			string res = responseXML.GetElementsByTagName ("res").Item (0).InnerText;
			string content = responseXML.GetElementsByTagName ("r:streamContent").Item (0).InnerText;
			string title = responseXML.GetElementsByTagName ("dc:title").Item (0).InnerText;

			Song song = new Song (content, res, title);

			return song;
		}

		/// <summary>
		/// Plaies the radio station.
		/// </summary>
		/// <returns>The radio station.</returns>
		/// <param name="uri">URI.</param>
		public string PlayRadioStation (string uri)
		{
			return DoWebRequest ("/MediaRenderer/AVTransport/Control",
								 "\"urn:schemas-upnp-org:service:AVTransport:1#SetAVTransportURI\"",
								 "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n" +
								 "<s:Body>\n" +
								 "<u:SetAVTransportURI xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">\n" +
								 "<InstanceID>0</InstanceID>\n" +
								 "<CurrentURI>" + uri.Replace ("&", "&amp;") + "</CurrentURI>\n" +
								 "<CurrentURIMetaData>&lt;DIDL-Lite xmlns:dc=&quot;http://purl.org/dc/elements/1.1/&quot; xmlns:upnp=&quot;urn:schemas-upnp-org:metadata-1-0/upnp/&quot; xmlns:r=&quot;urn:schemas-rinconnetworks-com:metadata-1-0/&quot; xmlns=&quot;urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/&quot;&gt;&lt;item id=&quot;F00092020s9483&quot; parentID=&quot;F000c0008s9483&quot; restricted=&quot;true&quot;&gt;&lt;dc:title&gt;NPO Radio 2&lt;/dc:title&gt;&lt;upnp:class&gt;object.item.audioItem.audioBroadcast.sonos-favorite&lt;/upnp:class&gt;&lt;desc id=&quot;cdudn&quot; nameSpace=&quot;urn:schemas-rinconnetworks-com:metadata-1-0/&quot;&gt;SA_RINCON65031_&lt;/desc&gt;&lt;/item&gt;&lt;/DIDL-Lite&gt;</CurrentURIMetaData>\n" +
								 "</u:SetAVTransportURI>\n" +
								 "</s:Body>\n" +
								 "</s:Envelope>");
		}
	}
}