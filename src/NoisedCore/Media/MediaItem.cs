using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Noised.Core.Media
{
	/// <summary>
	///		A media item which can be played
	/// </summary>
	public class MediaItem
	{
		/// <summary>
		///		Numeric id of an item
		/// </summary>
		public Int64 Id{get; set;}

		/// <summary>
		///		Uri of the media item
		/// </summary>
		public Uri Uri{get; set;}	

		/// <summary>
		///		The meta data of the media item
		/// </summary>
		public List<MetaData> MetaData{get;set;} 

		public MediaItem(Uri uri)
		{
			Uri = uri;
			MetaData = new List<MetaData>();
		}

		/// <summary>
		///		The protocol 
		/// </summary>
		/// <remarks>
		///		For example file://, or spotify://
		/// </remarks>
		public string Protocol 
		{
			get
            {
                string protocolRegex = @"^(.*:\/\/)";
                var regex = new Regex(protocolRegex); 
                var match = regex.Match(Uri.ToString());
                if (match.Success)
                {
                    return match.Groups[0].Value;
                }
                throw new ArgumentException("Invalid protocol in URI");
            }
		}

		public void AddMetaData(String name,String metaValue)
		{
			MetaData.Add(new MetaData(this,name,metaValue));
		}
	};
}
