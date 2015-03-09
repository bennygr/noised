using System.Collections.Generic;
namespace Noised.Core.Commands
{
	interface ICommandFactory
	{
		IEnumerable<AbstractCommand> CreateCommands(CommandMetaDataContainer commandMetaDataContainer);
	};
}
