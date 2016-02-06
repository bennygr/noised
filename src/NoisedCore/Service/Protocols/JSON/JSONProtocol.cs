using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Noised.Logging;
using Noised.Core.Commands;

namespace Noised.Core.Service.Protocols.JSON
{
    class JSONProtocol : IProtocol
    {
        private const string endTag = "{NOISEDEOC}";
        private ILogging logging;
        private ICommandFactory commandFactory;

        /// <summary>
        ///		Constrcutor
        /// </summary>
        /// <param name="logging">Logging object</param>
        /// <param name = "commandFactory">The command factory</param>
        public JSONProtocol(ILogging logging,
                            ICommandFactory commandFactory)
        {
            this.logging = logging;
            this.commandFactory = commandFactory;
        }

        #region IProtocol

        public string CommandEndTag
        {
            get { return endTag; }
        }

        public AbstractCommand ParseCommand(ServiceConnectionContext context, string commandData)
        {
            commandData = commandData.Replace(endTag, string.Empty);
            logging.Debug("Parsing command: " + commandData);

            CommandMetaData commandMetaData =
                JsonConvert.DeserializeObject<CommandMetaData>(commandData);

            //Injection the context as first argument
            var parameters = new List<object>();
            parameters.Add(context);
			//injection the other arguments
			foreach(object parameter in commandMetaData.Parameters)
			{
				var arrayParameter = parameter as JArray;
				if(arrayParameter != null)
				{
					parameters.Add(arrayParameter.ToObject<List<Object>>());
				}
				else
				{
					parameters.Add(parameter);
				}
			}
            commandMetaData.Parameters = parameters;

            return commandFactory.CreateCommand(commandMetaData);
        }

        public byte[] CreateResponse(ResponseMetaData responseData)
        {
            string json =
                JsonConvert.SerializeObject(responseData, Formatting.Indented);
            return Encoding.UTF8.GetBytes(json);
        }
        #endregion
    };
}
