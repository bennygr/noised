using Noised.Core.Commands; 
namespace Noised.Core.Service
{
	internal class JSONProtocol : IProtocol
	{
		private const string endTag = "{NOISEDEOC}";

		/// <summary>
		///		Constrcutor
		/// </summary>
		public JSONProtocol() { }

		#region IProtocol
		
		public string CommandEndTag
		{
			get{return endTag;}
		}

		public AbstractCommand Parse(string commandData)
		{
			//TODO
			return null;
		}
		
		#endregion
	};
}
