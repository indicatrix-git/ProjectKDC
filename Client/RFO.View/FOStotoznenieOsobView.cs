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
	[ScenePart(Scene = Ditec.RIS.RFO.Inf.ModuleScenes.FOStotoznenieOsob)]
	[Implements(typeof(DetailView), ServiceName = Ditec.RIS.RFO.Inf.ModuleScenes.FOStotoznenieOsob)]
	public partial class FOStotoznenieOsobView : DetailView
	{
		FOStotoznenieOsobModel model;
		private Ditec.RIS.CC.Inf.ICodeListCache codeListCache;
		private Guid? lastSelectedOsZosVRisID;
		private Guid? lastSelectedOkssID;
		private List<RfoTitul> listTitul = new List<RfoTitul>();
		private List<RfoTypTitulu> listTypTitulu = new List<RfoTypTitulu>();

		public FOStotoznenieOsobView()
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		public FOStotoznenieOsobView(IShell shell)
			: base(shell)
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		#region Methods

		private void InitializeCodeLists()
		{
			codeListCache = this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>();
			this.bsRfoNarodnost.DataSource = CCHelper.getDataSource<RfoNarodnost>(codeListCache);
			this.bsRfoRodinnyStav.DataSource = CCHelper.getDataSource<RfoRodinnyStav>(codeListCache);
			this.bsRfoObec.DataSource = CCHelper.getDataSource<RfoObec>(codeListCache);
			this.bsRfoOkres.DataSource = CCHelper.getDataSource<RfoOkres>(codeListCache);
			this.bsRfoStat.DataSource = CCHelper.getDataSource<RfoStat>(codeListCache);
			this.bsRfoTypPobytu.DataSource = CCHelper.getDataSource<RfoTypPobytu>(codeListCache);
			this.bsRfoUzemnyCelok.DataSource = CCHelper.getDataSource<RfoUzemnyCelok>(codeListCache);

			listTitul = CCHelper.getDataSource<RfoTitul>(codeListCache);
			listTypTitulu = CCHelper.getDataSource<RfoTypTitulu>(codeListCache);
		}

		private void setOsobVRisDataSource()
		{
			if(this.model.OsZosVRis != null)
			{
				this.bsOsZostVRis.DataSource = this.model.OsZosVRis.FyzickaOsoba;

				if (this.model.OsZosVRis.TitulList != null && this.model.OsZosVRis.TitulList.Count > 0)
				{
					var listTitulPred = listTitul.Where(t => listTypTitulu.Any(tt => tt.InternyKod == t.RfoTypTituluIk && (tt.Kod.Equals("1") || tt.Kod.Equals("3"))) && this.model.OsZosVRis.TitulList.Any(ft => ft.RfoTitulIk == t.InternyKod)).ToList();
					if (listTitulPred != null && listTitulPred.Count > 0)
					{
						this.model.OsZosVRisTitulPred = String.Join(" ", listTitulPred.Select(rt => rt.NazovSk));
					}

					var listTitulZa = listTitul.Where(t => listTypTitulu.Any(tt => tt.InternyKod == t.RfoTypTituluIk && tt.Kod.Equals("2")) && this.model.OsZosVRis.TitulList.Any(ft => ft.RfoTitulIk == t.InternyKod)).ToList();
					if (listTitulZa != null && listTitulZa.Count > 0)
					{
						this.model.OsZosVRisTitulZa = String.Join(" ", listTitulZa.Select(rt => rt.NazovSk));
					}
				}

				if (this.model.OsZosVRis.UdajePobytuList != null && this.model.OsZosVRis.UdajePobytuList.Count > 0)
				{
 					//UdajePobytu
					this.bsOsZosVRisUdajePobytu.DataSource = model.OsZosVRis.UdajePobytuList.FindAll(up => up.MimoSr == false);
					//UdajePobytu mimo SR
					this.bsOsZosVRisUdajePobytuMimoSR.DataSource = model.OsZosVRis.UdajePobytuList.FindAll(up => up.MimoSr == true);
				}

				if (this.model.OsZosVRis.FyzickaOsoba.ObecNarodeniaIk.HasValue)
				{
					this.lciOsZosVRis_ObecNarodenia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
					this.lciOsZosVRis_MiestoNarodeniaIne.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
				}
				else
				{
					this.lciOsZosVRis_ObecNarodenia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
					this.lciOsZosVRis_MiestoNarodeniaIne.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
				}
			}
		}

		private void setOkssDataSource()
		{
			if (this.model.Okss != null)
			{
				this.bsOkss.DataSource = this.model.Okss.FyzickaOsoba;

				if (this.model.Okss.TitulList != null && this.model.Okss.TitulList.Count > 0)
				{
					var listTitulPred = listTitul.Where(t => listTypTitulu.Any(tt => tt.InternyKod == t.RfoTypTituluIk && (tt.Kod.Equals("1") || tt.Kod.Equals("3"))) && this.model.OsZosVRis.TitulList.Any(ft => ft.RfoTitulIk == t.InternyKod)).ToList();
					if (listTitulPred != null && listTitulPred.Count > 0)
					{
						this.model.OkssTitulPred = String.Join(" ", listTitulPred.Select(rt => rt.NazovSk));
					}

					var listTitulZa = listTitul.Where(t => listTypTitulu.Any(tt => tt.InternyKod == t.RfoTypTituluIk && tt.Kod.Equals("2")) && this.model.OsZosVRis.TitulList.Any(ft => ft.RfoTitulIk == t.InternyKod)).ToList();
					if (listTitulZa != null && listTitulZa.Count > 0)
					{
						this.model.OkssTitulZa = String.Join(" ", listTitulZa.Select(rt => rt.NazovSk));
					}
				}

				if (this.model.Okss.UdajePobytuList != null && this.model.Okss.UdajePobytuList.Count > 0)
				{
					//UdajePobytu
					this.bsOkssUdajePobytu.DataSource = model.Okss.UdajePobytuList.FindAll(up => up.MimoSr == false);
					//UdajePobytu mimo SR
					this.bsOkssUdajePobytuMimoSR.DataSource = model.Okss.UdajePobytuList.FindAll(up => up.MimoSr == true);
				}

				if (this.model.Okss.FyzickaOsoba.ObecNarodeniaIk.HasValue)
				{
					this.lciOkss_ObecNarodenia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
					this.lciOkss_MiestoNarodeniaIne.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
				}
				else
				{
					this.lciOkss_ObecNarodenia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
					this.lciOkss_MiestoNarodeniaIne.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
				}
			}
		}

		private void setBtnVymenitAStotznitOsobyAccessibility()
		{
			this.btnVymenitOsoby.Enabled = this.btnStotznitOsoby.Enabled = this.model != null ? this.model.OkssID.HasValue && this.model.OsZosVRisID.HasValue : false;
		}

		private void vymenitFyzickeOsoby(Osoba osZostVRis, Osoba okss)
		{
			Osoba temp = osZostVRis;
			osZostVRis = okss;
			okss = temp;
		}

		private void zrusenieOznaceniaZaujmovejOsobyEndFunction()
		{
			(this.model.Controller as IFOStotoznenieOsob).StotoznenieOsobRIS(setOkssDataSource);
		}

		//private void stotoznenieOsobRISEndFunction()
		//{
			
		//}

		#endregion Methods

		#region Events

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.model = this.Model as FOStotoznenieOsobModel;

			this.InitializeCodeLists();

			//nastavenie lookUpEditov
			ViewHelper.SetLookUpEditsButtons(this.lcgVyhlKritOsobyZostavajucejVRIS, true, true);
			ViewHelper.SetLookUpEditsButtons(this.lcgVyhlKritOsobyKtoraSaStotoznuje, true, true);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			ViewHelper.SetReadOnlyItemsInGroupExceptSpecifiedControls(this.lcgOsZosVRis_IdentifUdajeTab, true, new Control[] { });
			ViewHelper.SetReadOnlyItemsInGroupExceptSpecifiedControls(this.lcgOsZosVRis_LokacneUdajeTab, true, new Control[] { });
			ViewHelper.SetReadOnlyItemsInGroupExceptSpecifiedControls(this.lcgOkss_IdentifUdajeTab, true, new Control[] { });
			ViewHelper.SetReadOnlyItemsInGroupExceptSpecifiedControls(this.lcgOkss_LokacneUdajeTab, true, new Control[] { });

			this.setBtnVymenitAStotznitOsobyAccessibility();
		}

		private void beOsZosVRis_EDUID_Search_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			switch (e.Button.Index)
			{
				case 0:
					(this.Model.Controller as IFOStotoznenieOsob).SelectOsZosVRis();
					break;
				case 1:
					this.model.OsZosVRisID = null;
					this.model.OsZosVRisEduid = null;
					this.model.OsZosVRis = null;
					break;
			}
		}

		private void beOsZosVRis_EDUID_Search_EditValueChanged(object sender, EventArgs e)
		{
			if (this.model != null)
			{
				this.setBtnVymenitAStotznitOsobyAccessibility();

				if (this.model.OsZosVRisID != null)
				{
					if (lastSelectedOsZosVRisID != this.model.OsZosVRisID)
					{
						(this.Model.Controller as IFOStotoznenieOsob).LoadOsZosVRis(setOsobVRisDataSource);
						lastSelectedOsZosVRisID = this.model.OsZosVRisID;
					}
				}
				else
				{
					this.lastSelectedOsZosVRisID = null;
					this.model.OsZosVRisTitulPred = null;
					this.model.OsZosVRisTitulZa = null;
					this.bsOsZostVRis.DataSource = new FyzickaOsoba();
					this.tbOsZosVRis_EDUID.Text = "";
					this.bsOsZosVRisUdajePobytu.DataSource = new List<UdajePobytu>();
					this.bsOsZosVRisUdajePobytuMimoSR.DataSource = new List<UdajePobytu>();
				}
			}
		}

		private void btnOsZosVRisDetail_Click(object sender, EventArgs e)
		{
			if (this.model.OsZosVRisID != null)
			{
				(this.model.Controller as IFOStotoznenieOsob).OpenDetailFyzickejOsoby(this.model.OsZosVRisID);
			}
		}

		private void beOkss_EDUID_Search_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			switch (e.Button.Index)
			{
				case 0:
					(this.Model.Controller as IFOStotoznenieOsob).SelectOkss();
					break;
				case 1:
					this.model.OkssID = null;
					this.model.OkssEduid = null;
					this.model.Okss = null;
					break;
			}
		}

		private void beOkss_EDUID_Search_EditValueChanged(object sender, EventArgs e)
		{
			if (this.model != null)
			{
				this.setBtnVymenitAStotznitOsobyAccessibility();

				if (this.model.OkssID != null)
				{
					if (lastSelectedOkssID != this.model.OkssID)
					{
						(this.Model.Controller as IFOStotoznenieOsob).LoadOkss(setOkssDataSource);
						lastSelectedOkssID = this.model.OkssID;
					}
				}
				else
				{
					this.lastSelectedOkssID = null;
					this.model.OkssTitulPred = null;
					this.model.OkssTitulZa = null;
					this.bsOkss.DataSource = new FyzickaOsoba();
					this.tbOkss_EDUID.Text = "";
					this.bsOkssUdajePobytu.DataSource = new List<UdajePobytu>();
					this.bsOkssUdajePobytuMimoSR.DataSource = new List<UdajePobytu>();
				}
			}
		}

		private void btnOkssDetail_Click(object sender, EventArgs e)
		{
			if (this.model.OkssID != null)
			{
				(this.model.Controller as IFOStotoznenieOsob).OpenDetailFyzickejOsoby(this.model.OkssID);
			}
		}

		private void btnZatvorit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnStotznitOsoby_Click(object sender, EventArgs e)
		{
			//Ak osoba, ktorá sa stotožňuje má vyplnené IFO (je spárovaná s osobou z RFO)
			if (!String.IsNullOrEmpty(this.model.Okss.FyzickaOsoba.Ifo))
			{
				//Systém zobrazí oznam "Stotožňovaná osoba je spárovaná s osobou z RFO. Len osoba, ktorá zostáva v RIS môže byť spárovaná s osobou v RFO. Prajete si zrušiť spárovanie stotožňovanej osoby s osobou v RFO?" - možnosť odpovede Áno/Nie
				if (Shell.GetNotificationService().HandleQuestion(codeListCache.GetErrorMessage(51601)) == System.Windows.Forms.DialogResult.Yes)
				{
					//Ak pouźívateľ odpovie Áno
					//Systém zobrazí otázku "Naozaj si prajete zrušiť spárovanie stotožňovanej osoby  s osobou z RFO?"
					if (Shell.GetNotificationService().HandleQuestion(codeListCache.GetErrorMessage(51602)) == System.Windows.Forms.DialogResult.Yes)
					{
						//Ak používateľ odpovie Áno
						//Systém zavolá zrušenie označenia záujmovej osoby v RFO pre IFO osoby z RIS 
						//Systém  osobe z RIS nastaví IFO na NULL - zruší spárovanie osoby v RIS s osobou z RFO
						(this.model.Controller as IFOStotoznenieOsob).ZrusenieOznaceniaZaujmovejOsoby(zrusenieOznaceniaZaujmovejOsobyEndFunction);
						return;
					}
					else
					{
						//Ináč 
						//Systém zobrazí oznam "Nie je možné stotožniť osoby."
						Shell.GetNotificationService().HandleInformation(codeListCache.GetErrorMessage(51603));
					}
				}
				else
				{
					//Ináč 
					//Systém zobrazí oznam "Nie je možné stotožniť osoby."
					Shell.GetNotificationService().HandleInformation(codeListCache.GetErrorMessage(51603));
				}
			}
			else
			{
				(this.model.Controller as IFOStotoznenieOsob).StotoznenieOsobRIS(setOkssDataSource);
			}
		}

		private void btnVymenitOsoby_Click(object sender, EventArgs e)
		{
			Osoba temp = this.model.OsZosVRis;
			this.model.OsZosVRis = this.model.Okss;
			this.model.Okss = temp;

			this.vymenitFyzickeOsoby(this.model.OsZosVRis, this.model.Okss);
			if (this.model.OsZosVRisID != null)
			{
				this.lastSelectedOsZosVRisID = this.model.OsZosVRis.FyzickaOsoba.ID;
				this.model.OsZosVRisID = this.model.OsZosVRis.FyzickaOsoba.ID;
				this.model.OsZosVRisEduid = this.model.OsZosVRis.FyzickaOsoba.Eduid.ToString();
				this.setOsobVRisDataSource();

			}

			if (this.model.OkssID != null)
			{
				this.lastSelectedOkssID = this.model.Okss.FyzickaOsoba.ID;
				this.model.OkssID = this.model.Okss.FyzickaOsoba.ID;
				this.model.OkssEduid = this.model.Okss.FyzickaOsoba.Eduid.ToString();
				this.setOkssDataSource();
			}
		}

		#endregion Events	

	}
}
