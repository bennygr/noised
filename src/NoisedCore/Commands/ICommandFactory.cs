using System.Collections.Generic;
namespace Noised.Core.Commands
{
	interface ICommandFactory
	{
		AbstractCommand CreateCommand(CommandMetaData commandMetaData);
	};
}
