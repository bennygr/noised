using System;
namespace Noised.Core
{
	/// <summary>
	///		A user using noised
	/// </summary>
	public class User
	{
		#region Properties
		
		public String Name { get; private set; }
		
		#endregion

		#region Constructors
		
		public User (String name)
		{
			this.Name = name;
		}
		
		#endregion

		public override String ToString()
		{
			return Name;
		}
	};
}
