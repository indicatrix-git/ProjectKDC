using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using Ditec.RIS.Client.Utils;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.RFO.Inf;
using Ditec.RIS.SysFra.WinUI.Shared;
using Ditec.WinUI.Shell.Infrastructure;
using Ditec.WinUI.Shell.Infrastructure.Mvc;
using LinFu.IoC.Configuration;
using Ditec.RIS.CC.Inf;
using Ditec.RIS.CC.Dol;

namespace Ditec.RIS.RFO.View
{
	[ScenePart(Scene = Ditec.RIS.RFO.Inf.ModuleScenes.OpravneCinnosti)]
	[Implements(typeof(BrowseView), ServiceName = Ditec.RIS.RFO.Inf.ModuleScenes.OpravneCinnosti)]
	public partial class OpravneCinnostiView : BrowseView
	{
		OpravneCinnostiModel model;
		private Ditec.RIS.CC.Inf.ICodeListCache codeListCache;

		public OpravneCinnostiView()
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		public OpravneCinnostiView(IShell shell)
			: base(shell)
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		#region Methods

		private void InitializeCodeLists()
		{
			codeListCache = this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>();
		}

		private void setOsobVRisDataSource()
		{
			if(this.model.OsVRis != null)
			{
				this.bsOsVRis.DataSource = this.model.OsVRis.FyzickaOsoba;
				this.setButtonsAccessibility();
			}
		}

		private void setButtonsAccessibility()
		{
			if (this.model != null && this.model.OsVRisID != null && this.model.OsVRis != null)
			{
				if (!String.IsNullOrEmpty(this.model.OsVRis.FyzickaOsoba.Ifo))
				{
					this.btnZrusitSparovanie.Enabled = true;
					this.btnVyhladatOsVRfo.Enabled = false;
					this.btnSparovatOsoby.Enabled = false;
					this.btnPredvyplnit.Enabled = false;
					this.btnZrusitKriteriaOsVRfo.Enabled = false;
					this.btnOsVRfoDetail.Enabled = false;
				}
				else
				{
					this.btnVyhladatOsVRfo.Enabled = true;
					this.btnPredvyplnit.Enabled = true;
					this.btnZrusitKriteriaOsVRfo.Enabled = true;
					this.btnZrusitSparovanie.Enabled = false;
					this.btnSparovatOsoby.Enabled = false;
					this.btnOsVRfoDetail.Enabled = false;
				}
			}
			else
			{
				this.btnZrusitSparovanie.Enabled = false;
				this.btnVyhladatOsVRfo.Enabled = false;
				this.btnSparovatOsoby.Enabled = false;
				this.btnPredvyplnit.Enabled = false;
				this.btnZrusitKriteriaOsVRfo.Enabled = false;
				this.btnOsVRfoDetail.Enabled = false;

				//this.bsOsVRis.DataSource = null;
			}

		}

		private void SearchOsVRfo()
		{
			(this.Model.Controller as IOpravneCinnosti).LoadZoznamOsVRfo(null);
		}

		private void bestFitGridColumns()
		{
			this.gvZoznamRFO.BeginDataUpdate();
			this.gvZoznamRFO.BestFitColumns();
			this.gvZoznamRFO.EndDataUpdate();
		}

		void zrusitVyhlKritPreOsobuvRFO()
		{
			this.model.VyhlOsVRfoMeno = null;
			this.model.VyhlOsVRfoPriezvisko = null;
			this.model.VyhlOsVRfoRodnePriezvisko = null;
			this.model.VyhlOsVRfoDatumNarodenia = null;
			this.model.VyhlOsVRfoRodneCislo = null;
			bsData.DataSource = this.model.Data = new List<FyzickaOsoba>();
		}

		void najdenieOsobyVRISPodlaIFOaIFOPravejOsobyEndFunction()
		{
			//Ak "Nájdená" = FALSE - neexistuje taká  FO v RIS , ktorá je spárovaná s osobou so zadaným IFO
			if (this.model.NajdenaOsoba.Najdena == false)
			{
				//Systém zavolá zápis individuálneho spárovania osôb RIS - RFO
				(this.model.Controller as IOpravneCinnosti).SparovatOsobuZRisSOsobouSRfo(sparovatOsobuZRisSOsobouSRfoEndFunction);
			}
			else
			{
				//V systéme RIS už existuje osoba, ktorá je spárovaná s danou osobou v RFO. Prajete si pokračovať stotožnením osôb v RIS?
				if (Shell.GetNotificationService().HandleQuestion(codeListCache.GetErrorMessage(51609)) == System.Windows.Forms.DialogResult.Yes)
				{
					(this.model.Controller as IOpravneCinnosti).StotoznenieOsobRIS(stotoznenieOsobRISEndFunction);
				}
			}
		}

		void sparovatOsobuZRisSOsobouSRfoEndFunction()
		{
			this.setOsobVRisDataSource();
			this.zrusitVyhlKritPreOsobuvRFO();
		}

		void zrusitSparovanieEndFunction()
		{
			this.setButtonsAccessibility();
			this.zrusitVyhlKritPreOsobuvRFO();
		}
		void stotoznenieOsobRISEndFunction()
		{
			this.setButtonsAccessibility();
			this.zrusitVyhlKritPreOsobuvRFO();
		}

		#endregion Methods

		#region Events

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.model = this.Model as OpravneCinnostiModel;

			this.InitializeCodeLists();

			//nastavenie lookUpEditov
			ViewHelper.SetLookUpEditsButtons(this.lcgVyhlKritOsobyZostavajucejVRIS, true, true);

			Tools.SetMaskTextLength(this.tbOsVRis_RodneCislo, 11);
			Tools.SetMaskTextLength(this.tbOsVRfo_RodneCislo, 11);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			ViewHelper.SetReadOnlyItemsInGroupExceptSpecifiedControls(this.lcgOsVRisVyhlaOsobaVRis, true, new Control[] { });
			this.setButtonsAccessibility();
		}

		private void beOsVRis_EDUID_Search_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			switch (e.Button.Index)
			{
				case 0:
					(this.Model.Controller as IOpravneCinnosti).SelectOsVRis();
					break;
				case 1:
					this.model.OsVRisID = null;
					this.model.OsVRisEduid = null;
					this.model.OsVRis = null;
					this.btnZrusitKriteriaOsVRfo_Click(null, null);
					break;
			}
		}

		private void beOsVRis_EDUID_Search_EditValueChanged(object sender, EventArgs e)
		{
			if (this.model != null)
			{
				this.setButtonsAccessibility();

				if (this.model.OsVRisID != null)
				{
					(this.Model.Controller as IOpravneCinnosti).LoadOsVRis(setOsobVRisDataSource);
					this.btnZrusitKriteriaOsVRfo_Click(null, null);
				}
				else
				{	
					this.bsOsVRis.DataSource = new FyzickaOsoba();
					this.tbOsVRis_EDUID.Text = "";
				}
			}
		}

		private void btnOsVRisDetail_Click(object sender, EventArgs e)
		{
			if (this.model.OsVRisID != null)
			{
				(this.model.Controller as IOpravneCinnosti).OpenDetailFyzickejOsoby(this.model.OsVRisID);
			}
		}

		private void btnOsVRfoDetail_Click(object sender, EventArgs e)
		{
			if (bsData.Current != null)
			{
				if (this.model.ZoznamOsobVRfo != null && this.model.ZoznamOsobVRfo.Count > 0)
				{
					//var openedForms = (this.Shell as Form).MdiChildren;

					var osoba = this.model.ZoznamOsobVRfo.FirstOrDefault(o => o.FyzickaOsoba.Eduid == ((FyzickaOsoba)this.bsData.Current).Eduid);
					(this.model.Controller as IOpravneCinnosti).OpenDetailOsobyZRFO(osoba);
				}
			}
		}

		private void btnPredvyplnit_Click(object sender, EventArgs e)
		{
			if (this.model != null && this.model.OsVRis != null)
			{
				this.model.VyhlOsVRfoMeno = this.model.OsVRis.FyzickaOsoba.MenoZobrazovane;
				this.model.VyhlOsVRfoPriezvisko = this.model.OsVRis.FyzickaOsoba.PriezviskoZobrazovane;
				this.model.VyhlOsVRfoRodnePriezvisko = this.model.OsVRis.FyzickaOsoba.RodneMenoZobrazovane;
				this.model.VyhlOsVRfoDatumNarodenia = this.model.OsVRis.FyzickaOsoba.DatumNarodenia;
				this.model.VyhlOsVRfoRodneCislo = this.model.OsVRis.FyzickaOsoba.RodneCislo;
			}
		}

		private void bsData_CurrentChanged(object sender, EventArgs e)
		{
			if (this.bsData != null && this.bsData.Current != null)
			{
				(this.Model as OpravneCinnostiModel).OsVRfoID = (this.bsData.Current as FyzickaOsoba).ID;
				(this.Model as OpravneCinnostiModel).OsVRfo = (this.bsData.Current as FyzickaOsoba);

				this.btnZrusitSparovanie.Enabled = true;
				this.btnSparovatOsoby.Enabled = true;
				this.btnOsVRfoDetail.Enabled = true;
			}
			else
			{
				this.btnZrusitSparovanie.Enabled = false;
				this.btnSparovatOsoby.Enabled = false;
				this.btnOsVRfoDetail.Enabled = false;
			}
		}

		private void btnSparovatOsoby_Click(object sender, EventArgs e)
		{
			if (this.model.OsVRfo != null)
			{
				//Prajete si spárovať osobu z RIS s osobou z RFO?
				if (Shell.GetNotificationService().HandleQuestion(codeListCache.GetErrorMessage(51607)) == System.Windows.Forms.DialogResult.Yes)
				{
					(this.model.Controller as IOpravneCinnosti).NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(this.najdenieOsobyVRISPodlaIFOaIFOPravejOsobyEndFunction);
				}
			}
		}

		private void btnZrusitSparovanie_Click(object sender, EventArgs e)
		{
			//Prajete si zrušiť spárovanie osoby z RIS s osobou z RFO?
			if (Shell.GetNotificationService().HandleQuestion(codeListCache.GetErrorMessage(51610)) == System.Windows.Forms.DialogResult.Yes)
			{
				(this.model.Controller as IOpravneCinnosti).ZrusenieOznaceniaZaujmovejOsoby(this.zrusitSparovanieEndFunction);
			}
		}

		private void btnVyhladatOsVRfo_Click(object sender, EventArgs e)
		{
			this.SearchOsVRfo();
		}

		private void btnZrusitKriteriaOsVRfo_Click(object sender, EventArgs e)
		{
			this.zrusitVyhlKritPreOsobuvRFO();
			this.btnZrusitSparovanie.Enabled = false;
			this.btnSparovatOsoby.Enabled = false;
			this.btnOsVRfoDetail.Enabled = false;
		}

		private void btnOsVRis_ZrisUdajeVyhladanejOsoby_Click(object sender, EventArgs e)
		{
			this.model.OsVRisID = null;
			this.model.OsVRisEduid = null;
			this.model.OsVRis = null;
			this.btnZrusitKriteriaOsVRfo_Click(null, null);
		}

		private void btnZatvorit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion Events	
	}
}
