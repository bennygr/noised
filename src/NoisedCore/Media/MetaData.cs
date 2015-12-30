using System;
using Noised.Core.Media;

/// <summary>
///		A media item's meta data
/// </summary>
public class MetaData
{
    public MediaItem MediaItem{get;set;}

	/// <summary>
	///		The title
	/// </summary>
	public string Title{get;set;}

	/// <summary>
	///		A list of Artitsts (Track Artitsts) 
	/// </summary>
	public string[] Artists{get;set;}

	/// <summary>
	///		A list of Album Artists
	/// </summary>
	public string[] AlbumArtists{get;set;}

	/// <summary>
	///		A list of Composers (Songwriter, Autorship)
	/// </summary>
	public string[] Composers{get;set;}

	/// <summary>
	///		The Album 
	/// </summary>
	public string Album{get;set;}

	/// <summary>
	///		User notes and comments
	/// </summary>
	public string Comment{get;set;}

	/// <summary>
	///		A list of genres
	/// </summary>
	public string[] Genres{get;set;}

	/// <summary>
	///		Year of record
	/// </summary>
	public uint Year{get;set;}

	/// <summary>
	///		Position of the item on the corresponding Album
	/// </summary>
	public uint TrackNumber{get;set;}

	/// <summary>
	///		The amount of tracks on the Album
	/// </summary>
	public uint TrackCount{get;set;}

	/// <summary>
	///		Number of the disc (withing a box set) containing this item
	/// </summary>
	public uint Disc {get;set;}

	/// <summary>
	///		Numbers of discs in a box set
	/// </summary>
	public uint DiscCount{get;set;}

	/// <summary>
	///		Extended Grouping
	/// </summary>
	public string Grouping{get;set;}

	/// <summary>
	///		Plaintext lyrics
	/// </summary>
	public string Lyrics{get;set;}

	/// <summary>
	///		Useful for DJs who are matching songs
	/// </summary>
	public uint BeatsPerMinute{get;set;}
	
	/// <summary>
	///		Conductor of a classical track
	/// </summary>
	public string Conductor{get;set;}

	/// <summary>
	///		Copyright information
	/// </summary>
	public string Copyright{get;set;}

	public MetaData(MediaItem mediaItem)
	{
        this.MediaItem = mediaItem;
	}
};
