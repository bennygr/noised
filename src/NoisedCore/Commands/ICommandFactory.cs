namespace Noised.Core.Commands
{
    interface ICommandFactory
    {
        AbstractCommand CreateCommand(CommandMetaData commandMetaData);
    };
}
