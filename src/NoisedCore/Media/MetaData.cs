using System;
/// <summary>
///		A media item's meta data
/// </summary>
/// //TODO: maybe it would be be better to 
///         have this class a dynamic list
///         instead of fixed properties?!
public class MetaData
{
	public string Artist {get;set;}
	public string Album {get;set;}
	public string Title {get;set;}
	public DateTime Date {get;set;}
	public string Comment {get;set;}
	public string Genrgenre {get;set;}
};
