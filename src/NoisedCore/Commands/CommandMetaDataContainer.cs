using System.Collections.Generic;
namespace Noised.Core.Commands
{
	public class CommandMetaDataContainer
	{
		public float ProtocolVersion{get;set;}
		public List<CommandMetaData> Commands{get;set;}
	};
}
