using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.RFO.Inf;
using Ditec.RIS.SysFra.WinUI.Shared;
using Ditec.SysFra.DataTypes.Infrastructure;
using Ditec.WinUI.Shell.Infrastructure;
using Ditec.WinUI.Shell.Infrastructure.Mvc;
using LinFu.IoC.Configuration;

namespace Ditec.RIS.RFO.Controller
{
	[ScenePart(Scene = ModuleScenes.FOBrowse)]
	[Implements(typeof(BrowseController), ServiceName = ModuleScenes.FOBrowse)]
	public class FOBrowseController : BrowseController, IFOBrowse
	{
		public FOBrowseController(IShell shell)
            : base(shell)
        {
        }

		#region LoadData

		public void LoadData(MethodInvoker continuator)
		{
			var client = new RfoServiceReference.RfoServiceClient();
			//// zo service locatora vytiahneme servis na komunikaciu cez wcf
			var communicator = Shell.GetWcfCommunicationService();
			communicator.OperationDescription = "Načítavajú sa dáta...";
			communicator.Initialize(this, client, this.LoadDataRfoBegin, this.LoadDataRfoEnd);
			communicator.Continuator = continuator;
			communicator.Begin();
		}

		private object LoadDataRfoBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var pagingInfo = ControllerHelper.SetPagingInfo(new PagingInfo(), (this.Model as FOBrowseModel).Pager);
				var filterCriteria = (this.Model as FOBrowseModel).FilterCriteria;
				filterCriteria.PagingInfo = pagingInfo;
				return client.FyzickaOsobaListPaged((this.Model as FOBrowseModel).FilterCriteria);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		/// <summary>
		/// Ukoncenie citania dat
		/// </summary>
		/// <param name="communicator">Objekt WCF sluzby</param>
		private void LoadDataRfoEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<List<FyzickaOsoba>>)response;
					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
					}

					this.Model.Data = requestResult.Response;

					var model = (this.Model as FOBrowseModel);
					if (requestResult.PagingInfo != null && model != null)
					{
						model.Pager.CurrentPage = requestResult.PagingInfo.CurrentPage;
						model.Pager.TotalRecords = requestResult.PagingInfo.TotalRecords;
						model.Pager.RecordsPerPage = requestResult.PagingInfo.RecordsPerPage;
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

		#region OpenDetail

		public void OpenDetail()
		{
			try
			{
				var browseModel = (this.Model as FOBrowseModel);

				if (browseModel.SelectedRecordID.HasValue)
				{
					// otvorit detail pre vybrany zaznam v novom okne 
					var controller = this.Shell.GetController(this.Shell,
						typeof(DetailController),
						ModuleScenes.FODetail,
						browseModel.SelectedRecordID) as DetailController;

					var model = controller.GetModel() as FODetailModel;
					model.FilterCriteria.ID = browseModel.SelectedRecordID;

					(controller as FODetailController).Show(ModalMode.None, new EventHandler(this.UpdateData));
				}
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		#endregion

		#region UpdateData

		private void UpdateData(object sender, EventArgs e)
		{
			if ((((sender as FODetailController).GetModel() as FODetailModel).View as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.OK)
			{
				this.LoadData(null);
			}
		}

		#endregion
	}
}
