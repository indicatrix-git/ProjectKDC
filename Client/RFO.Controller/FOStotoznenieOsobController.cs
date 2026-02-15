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
	[ScenePart(Scene = ModuleScenes.FOStotoznenieOsob)]
	[Implements(typeof(DetailController), ServiceName = ModuleScenes.FOStotoznenieOsob)]
	public class FOStotoznenieOsobController : DetailController, IFOStotoznenieOsob
	{
		public FOStotoznenieOsobController(IShell shell)
            : base(shell)
        {
        }

		#region LoadOsZosVRis

		public void LoadOsZosVRis(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.LoadOsZosVRisDataBegin, this.LoadOsZosVRisDataEnd);
				communicator.Continuator = invoker;
				communicator.Begin(((DetailModel)this.Model).Identifier);
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object LoadOsZosVRisDataBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = (FOStotoznenieOsobModel)this.Model;
				return client.GetFyzickaOsoba(new FyzickaOsobaFilterCriteria() { ID = model.OsZosVRisID });
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void LoadOsZosVRisDataEnd(ICommunicator communicator)
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

					((FOStotoznenieOsobModel)this.Model).OsZosVRis = requestResult.Response;
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

		#region SelectOsZosVRis

		public void SelectOsZosVRis()
		{
			//otvorit detail pre vybrany zaznam v novom okne 
			var controller = this.Shell.GetController(this.Shell,
				typeof(BrowseController),
				Ditec.RIS.RFO.Inf.ModuleScenes.FOBrowse) as BrowseController;

			(controller.GetModel() as Ditec.RIS.RFO.Inf.FOBrowseModel).OpenType = BrowseViewOpenType.SelectOpen;
			controller.Parent = this;
			(controller as Ditec.RIS.RFO.Controller.FOBrowseController).Show(ModalMode.MdiGlobal, setSelectedOsZosVRis);
		}

		/// <summary>
		/// Po vybere nadradenej sa doplni do filtrovacich podmienok
		/// </summary>
		/// <param name="NadradenaController"></param>
		/// <param name="e"></param>
		private void setSelectedOsZosVRis(object RFOController, EventArgs e)
		{
			var rfoModel = (Ditec.RIS.RFO.Inf.FOBrowseModel)(((BrowseController)RFOController).GetModel());
			var selRecordID = rfoModel.SelectedRecordID;
			if (selRecordID != null)
			{
				(this.Model as FOStotoznenieOsobModel).OsZosVRisID = selRecordID;
				(this.Model as FOStotoznenieOsobModel).OsZosVRisEduid = rfoModel.SelectedRecordEduId.HasValue ? rfoModel.SelectedRecordEduId.ToString() : "";// +"-" + rfoModel.SelectedRecordName;
			}
		}

		#endregion SelectOsZosVRis

		#region LoadOkss

		public void LoadOkss(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.LoadOkssDataBegin, this.LoadOkssDataEnd);
				communicator.Continuator = invoker;
				communicator.Begin(((DetailModel)this.Model).Identifier);
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object LoadOkssDataBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = (FOStotoznenieOsobModel)this.Model;
				return client.GetFyzickaOsoba(new FyzickaOsobaFilterCriteria() { ID = model.OkssID });
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void LoadOkssDataEnd(ICommunicator communicator)
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

					((FOStotoznenieOsobModel)this.Model).Okss = requestResult.Response;
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

		#region SelectOkss

		public void SelectOkss()
		{
			//otvorit detail pre vybrany zaznam v novom okne 
			var controller = this.Shell.GetController(this.Shell,
				typeof(BrowseController),
				Ditec.RIS.RFO.Inf.ModuleScenes.FOBrowse) as BrowseController;

			(controller.GetModel() as Ditec.RIS.RFO.Inf.FOBrowseModel).OpenType = BrowseViewOpenType.SelectOpen;
			controller.Parent = this;
			(controller as Ditec.RIS.RFO.Controller.FOBrowseController).Show(ModalMode.MdiGlobal, setSelectedOkss);
		}

		/// <summary>
		/// Po vybere nadradenej sa doplni do filtrovacich podmienok
		/// </summary>
		/// <param name="NadradenaController"></param>
		/// <param name="e"></param>
		private void setSelectedOkss(object RFOController, EventArgs e)
		{
			var rfoModel = (Ditec.RIS.RFO.Inf.FOBrowseModel)(((BrowseController)RFOController).GetModel());
			var selRecordID = rfoModel.SelectedRecordID;
			if (selRecordID != null)
			{
				(this.Model as FOStotoznenieOsobModel).OkssID = selRecordID;
				(this.Model as FOStotoznenieOsobModel).OkssEduid = rfoModel.SelectedRecordEduId.HasValue ? rfoModel.SelectedRecordEduId.ToString() : "";
			}
		}

		#endregion

		#region OpenDetailFyzickejOsoby

		public void OpenDetailFyzickejOsoby(Guid? selectedRecordID)
		{
			try
			{
				if (selectedRecordID.HasValue)
				{
					// otvorit detail pre vybrany zaznam v novom okne 
					var controller = this.Shell.GetController(this.Shell,
						typeof(DetailController),
						ModuleScenes.FODetail,
						selectedRecordID) as DetailController;

					var model = controller.GetModel() as FODetailModel;
					model.FilterCriteria.ID = selectedRecordID;

					(controller as FODetailController).Show(ModalMode.None, null);
				}
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		#endregion

		#region StotoznenieOsobRIS

		public void StotoznenieOsobRIS(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.StotoznenieOsobRISBegin, this.StotoznenieOsobRISEnd);
				communicator.Continuator = invoker;
				communicator.Begin(((DetailModel)this.Model).Identifier);
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object StotoznenieOsobRISBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = (FOStotoznenieOsobModel)this.Model;
				return client.StotoznenieOsobRIS(model.OsZosVRis.FyzickaOsoba.Eduid, model.Okss.FyzickaOsoba.Eduid, model.Okss.FyzickaOsoba.Ifo, model.OsZosVRis.FyzickaOsoba, model.Okss.FyzickaOsoba);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void StotoznenieOsobRISEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<Boolean>)response;
					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
						return;
					}

					var model = (FOStotoznenieOsobModel)this.Model;
					if (requestResult.Response == true)
					{
						//"Stotožnenie osôb v RIS bolo úspešné."
						this.Shell.GetNotificationService().HandleInformation(this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>().GetErrorMessage(51604));
						((FOStotoznenieOsobModel)this.Model).Okss = null;
						((FOStotoznenieOsobModel)this.Model).OkssID = null;
						((FOStotoznenieOsobModel)this.Model).OkssEduid = null;
					}
					else
					{
						//Počas stotžnenia osôb sa vyskytla chyba.
						this.Shell.GetNotificationService().HandleInformation(this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>().GetErrorMessage(51605));
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

		#region ZrusenieOznaceniaZaujmovejOsoby

		public void ZrusenieOznaceniaZaujmovejOsoby(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.ZrusenieOznaceniaZaujmovejOsobyBegin, this.ZrusenieOznaceniaZaujmovejOsobyEnd);
				communicator.Continuator = invoker;
				communicator.Begin(((DetailModel)this.Model).Identifier);
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object ZrusenieOznaceniaZaujmovejOsobyBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = (FOStotoznenieOsobModel)this.Model;
				return client.ZrusenieOznaceniaZaujmovejOsoby(model.Okss.FyzickaOsoba.Ifo);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void ZrusenieOznaceniaZaujmovejOsobyEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<Boolean>)response;
					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
						return;
					}
					if (requestResult.Response == true)
					{
						var model = (FOStotoznenieOsobModel)this.Model;
						model.Okss.FyzickaOsoba.Ifo = null;
						this.FyzickaOsobaModify();
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

		#region FyzickaOsobaModify

		public void FyzickaOsobaModify()
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				//// zo service locatora vytiahneme servis na komunikaciu cez wcf
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Ukladajú sa dáta...";
				communicator.Initialize(this, client, this.FyzickaOsobaModifyBegin, this.FyzickaOsobaModifyEnd);
				communicator.Begin();
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object FyzickaOsobaModifyBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = (FOStotoznenieOsobModel)this.Model;
				return client.FyzickaOsobaModify(model.Okss.FyzickaOsoba);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void FyzickaOsobaModifyEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<FyzickaOsoba>)response;
					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
						return;
					}

					((FOStotoznenieOsobModel)this.Model).Okss.FyzickaOsoba = requestResult.Response;
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
