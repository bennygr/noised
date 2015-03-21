using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Noised.Logging;
using Noised.Core.Commands; 

namespace Noised.Core.Service.Protocols.JSON
{
	internal class JSONProtocol : IProtocol
	{
		private const string endTag = "{NOISEDEOC}";
		private ILogging logging;
		private ICommandFactory commandFactory;

		/// <summary>
		///		Constrcutor
		/// </summary>
		/// <param name="logging">Logging object</param>
		public JSONProtocol(ILogging logging,
							ICommandFactory commandFactory) 
		{
			this.logging = logging;
			this.commandFactory = commandFactory;
		}

		#region IProtocol
		
		public string CommandEndTag
		{
			get{return endTag;}
		}

		public AbstractCommand ParseCommand(ServiceConnectionContext context, string commandData)
		{
			commandData = commandData.Replace(endTag,string.Empty);
			logging.Debug("Parsing command: " + commandData);

			CommandMetaData commandMetaData = 
				JsonConvert.DeserializeObject<CommandMetaData>(commandData);

			//Injection the context as first argument
			List<object> parameters = new List<object>();
			parameters.Add(context);
			if(commandMetaData.Parameters != null)
				parameters.AddRange(commandMetaData.Parameters);
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
