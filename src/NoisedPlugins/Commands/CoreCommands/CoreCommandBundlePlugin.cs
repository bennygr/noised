using System;
using Noised.Core.Plugins.Commands;
public class CoreCommandBundlePlugin : ICommandBundle
{

		#region IDisposable

		public void Dispose(){}

		#endregion

		#region IPlugin
		
		public String Name
		{
			get
			{
				return  "CoreCommandBundle";
			}
		}

		public String Description
		{
			get
			{
				return  "The noised core commands";
			}
		}
		
		public String AuthorName
		{
			get
			{
				return "Benjamin Grüdelbach";
			}
		}

		public String AuthorContact
		{
			get
			{
				return "nocontact@availlable.de";
			}
		}

		public Version Version
		{
			get
			{
				return new Version(1,0);
			}
		}

		public DateTime CreationDate
		{
			get
			{
				return DateTime.Parse("04.03.2015");
			}
		}
		
		#endregion
};
