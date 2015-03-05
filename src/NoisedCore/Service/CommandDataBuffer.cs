using System;
using System.Text;
using System.Collections.Generic;
using Noised.Logging;
using Noised.Core.IOC;

namespace Noised.Core.Service
{
	/// <summary>
	///		A buffer which stores incoming command data 
	///		from a IService until one or more commands has been received
	/// </summary>
	internal class CommandDataBuffer 
	{
		#region Fields
		
		private List<byte> buffer = new List<byte>();
		private readonly string endTag;
		private readonly List<string> ignoreList = 
			new List<string> { "\t", "\r", "\n", "\0" };
		private int endOfFirstCommand = 0;
		
		#endregion

		#region Constrcutor
		
		/// <summary>
		///		Constructor
		/// </summary>
		internal CommandDataBuffer()
		{
			endTag = IocContainer.Get<IProtocol>().CommandEndTag;
		}

		#endregion

		/// <summary>
		///		The current command buffer as string
		/// </summary>
		private string CommandText
		{
			get
			{
				byte[] buf = this.buffer.ToArray();
				UTF8Encoding encoding = new UTF8Encoding();
				string text = encoding.GetString(buf);
				foreach (string ignoreItem in ignoreList)
				{
					text = text.Replace(ignoreItem, string.Empty);
				}
				return text;
			}
		}

		#region Methods

		/// <summary>
		///		Checks if at least one complete command is in the buffer
		/// </summary>
		/// <returns>True, if at least one complete command is in the buffer, False otherwise</returns>
		private bool CheckEnd()
		{
			return CommandText.Contains(endTag);					
		} 

		/// <summary>
		///		Gets the next command in the buffer
		/// </summary>
		/// <returns>
		///	Returns the next command in the buffer as string representation, 
		/// or string.Empty if no complete command is in the buffer
		/// </returns>
		public string PopCommand()
		{
			if(this.endOfFirstCommand > 0)
			{
				if (CheckEnd())
				{
					string cmdText = this.CommandText;
					string firstCommand = cmdText.Substring(0, endOfFirstCommand);
					byte[] newBuffer = new byte[cmdText.Length - firstCommand.Length];
					Array.Copy(this.buffer.ToArray(), endOfFirstCommand, newBuffer, 0, newBuffer.Length);
					this.buffer = new List<byte>(newBuffer);
					if(CheckEnd())
						endOfFirstCommand = CommandText.IndexOf(endTag) + endTag.Length;
					else
						endOfFirstCommand = 0;
					return firstCommand;
				} 
			}
			return string.Empty;
		}
	
		/// <summary>
		///		Adds received data to the buffer
		/// </summary>
		public bool Add(byte[] data)
		{
			foreach(byte b in data)
			{
				if(b != '\0')
					buffer.Add(b);
			}

			if(CheckEnd())	
			{
				endOfFirstCommand = CommandText.IndexOf(endTag) + endTag.Length;
				return true;
			}
			return false;
		}
	
		#endregion
	};
	
}
