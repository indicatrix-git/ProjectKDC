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
	[ScenePart(Scene = ModuleScenes.OpravneCinnosti)]
	[Implements(typeof(BrowseController), ServiceName = ModuleScenes.OpravneCinnosti)]
	public class OpravneCinnostiController : BrowseController, IOpravneCinnosti
	{
		public OpravneCinnostiController(IShell shell)
            : base(shell)
        {
        }

		#region LoadOsVRis

		public void LoadOsVRis(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.LoadOsVRisDataBegin, this.LoadOsVRisDataEnd);
				communicator.Continuator = invoker;
				communicator.Begin();
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object LoadOsVRisDataBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = (OpravneCinnostiModel)this.Model;
				return client.GetFyzickaOsoba(new FyzickaOsobaFilterCriteria() { ID = model.OsVRisID });
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void LoadOsVRisDataEnd(ICommunicator communicator)
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

					((OpravneCinnostiModel)this.Model).OsVRis = requestResult.Response;
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

		#region SelectOsVRis

		public void SelectOsVRis()
		{
			//otvorit pre vybrany zaznam v novom okne 
			var controller = this.Shell.GetController(this.Shell,
				typeof(BrowseController),
				Ditec.RIS.RFO.Inf.ModuleScenes.FOBrowse) as BrowseController;

			(controller.GetModel() as Ditec.RIS.RFO.Inf.FOBrowseModel).OpenType = BrowseViewOpenType.SelectOpen;
			controller.Parent = this;
			(controller as Ditec.RIS.RFO.Controller.FOBrowseController).Show(ModalMode.MdiGlobal, setSelectedOsVRis);
		}

		/// <summary>
		/// Po vybere nadradenej sa doplni do filtrovacich podmienok
		/// </summary>
		/// <param name="NadradenaController"></param>
		/// <param name="e"></param>
		private void setSelectedOsVRis(object RFOController, EventArgs e)
		{
			var rfoModel = (Ditec.RIS.RFO.Inf.FOBrowseModel)(((BrowseController)RFOController).GetModel());
			var selRecordID = rfoModel.SelectedRecordID;
			if (selRecordID != null)
			{
				(this.Model as OpravneCinnostiModel).OsVRisID = selRecordID;
				(this.Model as OpravneCinnostiModel).OsVRisEduid = rfoModel.SelectedRecordEduId.HasValue ? rfoModel.SelectedRecordEduId.ToString() : "";// +"-" + rfoModel.SelectedRecordName;
			}
		}

		#endregion SelectOsVRis

		#region LoadZoznamOsVRfo

		public void LoadZoznamOsVRfo(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.LoadZoznamOsVRfoBegin, this.LoadZoznamOsVRfoEnd);
				communicator.Continuator = invoker;
				communicator.Begin();
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object LoadZoznamOsVRfoBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = this.Model as OpravneCinnostiModel;
				return client.VyhladanieZoznamuIFOVratFyzickeOsoby(model.VyhlOsVRfoRodneCislo, model.VyhlOsVRfoMeno, model.VyhlOsVRfoPriezvisko, model.VyhlOsVRfoRodnePriezvisko, model.VyhlOsVRfoDatumNarodenia);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void LoadZoznamOsVRfoEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<List<Osoba>>)response;

					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
						return;
					}

					var model = (this.Model as OpravneCinnostiModel);
					List<FyzickaOsoba> zoznamFo = new List<FyzickaOsoba>();
					model.ZoznamOsobVRfo = requestResult.Response;
					if (model.ZoznamOsobVRfo != null && model.ZoznamOsobVRfo.Count > 0)
					{
						foreach(Osoba o in model.ZoznamOsobVRfo)
						{
							if (o.FyzickaOsoba != null)
							{
								zoznamFo.Add(o.FyzickaOsoba);
							}
						}
					}

					this.Model.Data = zoznamFo;
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

		#region OpenDetailOsobyZRFO

		public void OpenDetailOsobyZRFO(Osoba osoba)
		{
			try
			{
				if (osoba != null)
				{
					// otvorit detail pre vybrany zaznam v novom okne 
					var controller = this.Shell.GetController(this.Shell,
						typeof(DetailController),
						ModuleScenes.RFODetail,
						osoba.FyzickaOsoba.Eduid) as DetailController;

					var model = controller.GetModel() as RFODetailModel;
					model.OsobaZRfo = osoba;
					model.FilterCriteria.ID = osoba.FyzickaOsoba.ID;

					(controller as RFODetailController).Show(ModalMode.None, null);
				}
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		#endregion

		#region NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby

		public void NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.NajdenieOsobyVRISPodlaIFOaIFOPravejOsobyBegin, this.NajdenieOsobyVRISPodlaIFOaIFOPravejOsobyEnd);
				communicator.Continuator = invoker;
				communicator.Begin();
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object NajdenieOsobyVRISPodlaIFOaIFOPravejOsobyBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = this.Model as OpravneCinnostiModel;
				return client.NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(model.OsVRfo.Ifo, model.OsVRfo.IfoPravejOsoby);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void NajdenieOsobyVRISPodlaIFOaIFOPravejOsobyEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<NajdenaOsoba>)response;

					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
						return;
					}

					var model = (this.Model as OpravneCinnostiModel);
					model.NajdenaOsoba = requestResult.Response;
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

		#region SparovatOsobuZRisSOsobouSRfo


		public void SparovatOsobuZRisSOsobouSRfo(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Načítavajú sa dáta...";
				communicator.Initialize(this, client, this.SparovatOsobuZRisSOsobouSRfoBegin, this.SparovatOsobuZRisSOsobouSRfoEnd);
				communicator.Continuator = invoker;
				communicator.Begin();
			}
			catch (Exception ex)
			{
				Shell.GetNotificationService().HandleException(ex);
			}
		}

		private object SparovatOsobuZRisSOsobouSRfoBegin(ICommunicator communicator)
		{
			try
			{
				var client = (RfoServiceReference.RfoServiceClient)communicator.Client;
				var model = this.Model as OpravneCinnostiModel;
				return client.ZapisSparovanieOsobRisRfo(model.OsVRis.FyzickaOsoba.Eduid, model.OsVRfo.Ifo, model.OsVRfo.IfoPravejOsoby, Spracovanie.Individualne, null, null);
			}
			catch (FaultException ex)
			{
				Shell.GetNotificationService().HandleWarning(ex.Message);
				return null;
			}
		}

		private void SparovatOsobuZRisSOsobouSRfoEnd(ICommunicator communicator)
		{
			try
			{
				object response = communicator.End();
				if (response != null)
				{
					var requestResult = (RequestResult<ZapisSparovanieOsobRisRfoRetVal>)response;

					if (requestResult.HasProcessingMessages(ErrorSeverity.Warning))
					{
						Shell.GetNotificationService().HandleResponse(requestResult.Messages);
						return;
					}

					var zapisSparovanieOsobRisRfoRetVal = requestResult.Response;
					if (!String.IsNullOrEmpty(zapisSparovanieOsobRisRfoRetVal.Message))
					{
						Shell.GetNotificationService().HandleInformation(zapisSparovanieOsobRisRfoRetVal.Message);
						((OpravneCinnostiModel)this.Model).OsVRis = requestResult.Response.SparovanaOsoba;
					}
					else
					{
						//Pri spárovaní osôb sa vyskytla chyba. Osoby neboli spárované.
						Shell.GetNotificationService().HandleError(this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>().GetErrorMessage(51608));
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
				communicator.Begin(null);
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
				var model = (OpravneCinnostiModel)this.Model;
				return client.StotoznenieOsobRIS(model.OsVRis.FyzickaOsoba.Eduid, model.NajdenaOsoba.EDUIDNajdene, model.OsVRfo.Ifo, model.OsVRis.FyzickaOsoba, model.NajdenaOsoba.FyzickaOsobaNajdena);
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

					var model = (OpravneCinnostiModel)this.Model;
					if (requestResult.Response == true)
					{
						//Stotožnenie osôb v RIS bolo úspešné.
						this.Shell.GetNotificationService().HandleInformation(this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>().GetErrorMessage(51604));
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
				communicator.Begin(null);
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
				var model = (OpravneCinnostiModel)this.Model;
				return client.ZrusenieOznaceniaZaujmovejOsoby(model.OsVRis.FyzickaOsoba.Ifo);
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
						var model = (OpravneCinnostiModel)this.Model;
						model.OsVRis.FyzickaOsoba.Ifo = null;
						this.FyzickaOsobaModify(communicator.Continuator);
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

		public void FyzickaOsobaModify(System.Windows.Forms.MethodInvoker invoker)
		{
			try
			{
				var client = new RfoServiceReference.RfoServiceClient();
				//// zo service locatora vytiahneme servis na komunikaciu cez wcf
				var communicator = Shell.GetWcfCommunicationService();
				communicator.OperationDescription = "Ukladajú sa dáta...";
				communicator.Initialize(this, client, this.FyzickaOsobaModifyBegin, this.FyzickaOsobaModifyEnd);
				communicator.Continuator = invoker;
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
				var model = (OpravneCinnostiModel)this.Model;
				return client.FyzickaOsobaModify(model.OsVRis.FyzickaOsoba);
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

					((OpravneCinnostiModel)this.Model).OsVRis.FyzickaOsoba = requestResult.Response;
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
