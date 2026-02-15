using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.RFO.Inf;
using Ditec.SysFra.DataTypes.Infrastructure;
using Ditec.WinUI.Shell.Infrastructure;
using Ditec.WinUI.Shell.Infrastructure.Mvc;
using LinFu.IoC.Configuration;

namespace Ditec.RIS.RFO.Controller
{
	[ScenePart(Scene = ModuleScenes.FODetail)]
	[Implements(typeof(DetailController), ServiceName = ModuleScenes.FODetail)]
	public class FODetailController : DetailController, IFODetail
	{
		public FODetailController(IShell shell)
            : base(shell)
        {
        }

		#region LoadData

		public void LoadData(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.LoadDataBegin, this.LoadDataEnd);
				communicator.Continuator = invoker;
				communicator.Begin(((DetailModel)this.Model).Identifier);
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object LoadDataBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = (FODetailModel)this.Model;
				return client.GetFyzickaOsoba(model.FilterCriteria);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void LoadDataEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<Osoba>)response;
					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
						return;
					}

					var model = (FODetailModel)this.Model;

					model.Data = requestResult.Response;

					if(model.Data != null)
					{
						var osoba = (Osoba)model.Data;
						if(osoba.FyzickaOsoba != null && osoba.FyzickaOsoba.Zneplatnena)
						{
							using (var client = new RfoServiceReference.RfoServiceClient())
							{
								var eduIDPravejOsoby = client.GetEduIdPravejOsoby(osoba.FyzickaOsoba);
								if (eduIDPravejOsoby != null && eduIDPravejOsoby.Response != null)
								{
									model.EDUIDPravejOsoby = eduIDPravejOsoby.Response;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
			finally
			{
				//nastavim si zaciatocny stav prehladu
				if (communicator.Continuator != null)
				{
					communicator.InvokeContinuator();
				}
			}
		}

		#endregion
	}
}
