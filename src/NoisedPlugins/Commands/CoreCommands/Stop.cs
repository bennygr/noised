using System;
using Noised.Core.Commands;
using Noised.Core.Service;
using Noised.Core.IOC;
using Noised.Core.Media;

namespace Noised.Commands.Core
{
	public class Stop : AbstractCommand
	{
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="context">Connection context</param>
		public Stop (ServiceConnectionContext context)
			: base(context) { }

		#region AbstractCommand
	
		protected override void Execute()
		{
			var mediaManager = IocContainer.Get<IMediaManager>();
			mediaManager.Stop();
		}
	
		#endregion
	};
}
