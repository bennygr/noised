using System;
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
		public JSONProtocol(ILogging logging,ICommandFactory commandFactory) 
		{
			this.logging = logging;
			this.commandFactory = commandFactory;
		}

		#region IProtocol
		
		public string CommandEndTag
		{
			get{return endTag;}
		}

		public AbstractCommand Parse(string commandData)
		{
			commandData = commandData.Replace(endTag,string.Empty);
			logging.Debug("Parsing " + commandData);
			CommandMetaDataContainer test = new CommandMetaDataContainer{
				ProtocolVersion = 2.0f,
				Commands = new List<CommandMetaData>()
				{
					new CommandMetaData
					{
						Name = "Test",
						Parameters = new List<Object>{"test"}
					},
					new CommandMetaData
					{
						Name = "Ping",
						Parameters = new List<Object>{"test",
													new List<int>{1,2,3}}
					}
				}
			};

			string json = JsonConvert.SerializeObject(test,Formatting.Indented);
			Console.WriteLine(json);

			var t = JsonConvert.DeserializeObject<CommandMetaDataContainer>(json);
			IEnumerable<AbstractCommand> cs = commandFactory.CreateCommands(t);


			Console.WriteLine(cs.Count());
			//foreach(var commandItem in t.Commands)
			//{
			//	Console.WriteLine(commandItem.Name);	
			//	foreach(var param in commandItem.Parameters)
			//	{
			//		Console.WriteLine(param);
			//		Console.WriteLine(param.GetType());
			//	}
			//}

			//var command = JsonConvert.DeserializeAnonymousType(commandData, definition);
			//logging.Debug(command.Name);
			return null;
		}
		
		#endregion
	};
}
