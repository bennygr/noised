using System;
using Noised.Core.Media;
/// <summary>
///		A media item's meta data
/// </summary>
public class MetaData
{
    public MediaItem MediaItem{get;set;}
	public string Name{get;set;}
	public string Value{get;set;}

	public MetaData(MediaItem mediaItem, string name, string dataValue)
	{
        this.MediaItem = mediaItem;
		this.Name = name;
		this.Value = dataValue;
	}

	public int GetAsInt()
	{
		return Int32.Parse(Value);
	}

	public long GetAsLong()
	{
		return Int64.Parse(Value);
	}

	public float GetAsFloat()
	{
		return float.Parse(Value);
	}

	public double GetAsDouble()
	{
		return Double.Parse(Value);
	}
};
