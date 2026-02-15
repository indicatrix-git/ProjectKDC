using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.RFO.Inf;
using Ditec.RIS.Client.Utils;
using Ditec.RIS.SysFra.WinUI.Shared;
using Ditec.WinUI.Shell.Infrastructure;
using Ditec.WinUI.Shell.Infrastructure.Mvc;
using LinFu.IoC.Configuration;
using Ditec.RIS.CC.Inf;
using ModuleScenes = Ditec.RIS.RFO.Inf.ModuleScenes;
using Ditec.RIS.CC.Dol;
using DevExpress.XtraLayout;
using Ditec.SysFra.DataTypes.Infrastructure;

namespace Ditec.RIS.RFO.View
{
	[ScenePart(Scene = ModuleScenes.FODetail)]
	[Implements(typeof(DetailView), ServiceName = ModuleScenes.FODetail)]
	public partial class FODetailView : DetailView
	{
		public FODetailView()
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		public FODetailView(IShell shell)
			: base(shell)
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		#region Atributtes

		FODetailModel model;
		private Ditec.RIS.CC.Inf.ICodeListCache codeListCache;
		private List<RfoTitul> listTitul = new List<RfoTitul>();
		private List<RfoTypTitulu> listTypTitulu = new List<RfoTypTitulu>();

		#endregion Atributtes

		#region Methods

		private void InitializeUI()
		{
			//nastavim vsetko na readOnly
			ViewHelper.SetReadOnlyItemsInGroupExceptSpecifiedControls(this.lcgRFOIdentifikacneUdaje, true, new Control[] { this.gcStatnaPrislusnost, this.gcObmPozbavPravnejSposobilosti, this.gcUdajeStotoznenia, this.btnZatvorit });
			ViewHelper.SetReadOnlyItemsInGroupExceptSpecifiedControls(this.lcgRFOLokacneUdaje, true, new Control[] { this.gcPobytVSR, this.gcPobytMimoSR, this.gcUdajeOZakazePobytu });

			Tools.SetNumberOfRowsInLookUpEdit(this.lcgRFOIdentifikacneUdaje);
		}

		/// <summary>
		/// Nacitanie ciselnikov na model
		/// </summary>
		private void InitializeCodeLists()
		{
			codeListCache = this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>();

			this.bsRfoPohlavie.DataSource = CCHelper.getDataSource<RfoPohlavie>(codeListCache);

			this.bsRfoNarodnost.DataSource = CCHelper.getDataSource<RfoNarodnost>(codeListCache);

			this.bsRfoRodinnyStav.DataSource = CCHelper.getDataSource<RfoRodinnyStav>(codeListCache);

			listTitul = CCHelper.getDataSource<RfoTitul>(codeListCache);
			listTypTitulu = CCHelper.getDataSource<RfoTypTitulu>(codeListCache);
		}

		#endregion Methods

		#region Events

		private void RFODetailView_Shown(object sender, EventArgs e)
		{
			this.InitializeUI();

			if ((this.Model as DetailModel).Identifier != null)
			{
				(this.Model.Controller as IFODetail).LoadData(null);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.InitializeCodeLists();

			//nastavim si property gridu
			ViewHelper.SetGridViewProperties(this.gvStatnaPrislusnost);
			ViewHelper.SetGridViewProperties(this.gvObmPozbavPravnejSposobilosti);
			ViewHelper.SetGridViewProperties(this.gvUdajeStotoznenia);
		}

		protected override void OnDataChanged()
		{
			base.OnDataChanged();

			this.model = this.Model as FODetailModel;
			Osoba modelData = (Osoba)this.model.Data;

			//Udaje o fyzickej osobe
			if (modelData != null)
			{
				this.bsRfoFyzickaOsoba.DataSource = (FyzickaOsoba)modelData.FyzickaOsoba;

				if (modelData.TitulList != null && modelData.TitulList.Count > 0)
				{
					var listTitulPred = listTitul.Where(t => listTypTitulu.Any(tt => tt.InternyKod == t.RfoTypTituluIk && (tt.Kod.Equals("1") || tt.Kod.Equals("3"))) && modelData.TitulList.Any(ft => ft.RfoTitulIk == t.InternyKod)).ToList();
					if (listTitulPred != null && listTitulPred.Count > 0)
					{
						this.model.FyzickaOsobaTitulPred = String.Join(" ", listTitulPred.Select(rt => rt.NazovSk));
					}

					var listTitulZa = listTitul.Where(t => listTypTitulu.Any(tt => tt.InternyKod == t.RfoTypTituluIk && tt.Kod.Equals("2")) && modelData.TitulList.Any(ft => ft.RfoTitulIk == t.InternyKod)).ToList();
					if (listTitulZa != null && listTitulZa.Count > 0)
					{
						this.model.FyzickaOsobaTitulZa = String.Join(" ", listTitulZa.Select(rt => rt.NazovSk));
					}
				}

				this.bsRfoStat.DataSource = CCHelper.getDataSource<RfoStat>(codeListCache);
				this.bsRfoOkres.DataSource = CCHelper.getDataSource<RfoOkres>(codeListCache);
				this.bsRfoObec.DataSource = CCHelper.getDataSource<RfoObec>(codeListCache);
				this.bsRfoSposobilostPravUkon.DataSource = CCHelper.getDataSource<RfoSposobilostPravUkon>(codeListCache);
				this.bsRfoTypRoleVRodiVztahu.DataSource = CCHelper.getDataSource<RfoTypRoleVRodiVztahu>(codeListCache);
				this.bsRfoTypPobytu.DataSource = CCHelper.getDataSource<RfoTypPobytu>(codeListCache);
				this.bsRfoUzemnyCelok.DataSource = CCHelper.getDataSource<RfoUzemnyCelok>(codeListCache);

				if (modelData.StatnaPrislusnostList != null && modelData.StatnaPrislusnostList.Count > 0)
				{
					var statList = CCHelper.getDataSource<RfoStat>(codeListCache);

					//vyberiem len tie staty, s ktorymi ma dana osoba statnu prislusnost
					this.bsRfoStatnaPrislusnost.DataSource = statList.FindAll(s => modelData.StatnaPrislusnostList.Select(sp => sp.RfoStatIk).ToList().Contains((Guid)s.InternyKod));
				}

				if (modelData.PravnaSposobilostObmedzenieList != null && modelData.PravnaSposobilostObmedzenieList.Count > 0)
				{
					//PravnaSposobilostObmedzenie
					this.bsPravnaSposobilostObmedzenie.DataSource = modelData.PravnaSposobilostObmedzenieList;
				}

				if (modelData.StotoznenaFyzOsobaList != null && modelData.StotoznenaFyzOsobaList.Count > 0)
				{
					//StotoznenaFyzOsoba
					this.bsUdajeStotoznenia.DataSource = modelData.StotoznenaFyzOsobaList;
				}

				if (modelData.UdajePobytuList != null && modelData.UdajePobytuList.Count > 0)
				{
					//UdajePobytu
					this.bsUdajePobytu.DataSource = modelData.UdajePobytuList.FindAll(up => up.MimoSr == false);
					//UdajePobytu mimo SR
					this.bsUdajePobytuMimoSR.DataSource = modelData.UdajePobytuList.FindAll(up => up.MimoSr == true);
				}

				if (modelData.ZakazPobytuList != null && modelData.ZakazPobytuList.Count > 0)
				{
					//ZakazPobytu
					this.bsZakazPobytu.DataSource = modelData.ZakazPobytuList;
				}
			}
		}

		private void btnZatvorit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void bsRfoFyzickaOsoba_CurrentChanged(object sender, EventArgs e)
		{
			if (this.bsRfoFyzickaOsoba.Current != null)
			{
				var currentRfoFyzickaOsoba = (this.bsRfoFyzickaOsoba.Current as FyzickaOsoba);
				if (currentRfoFyzickaOsoba.ObecNarodeniaIk.HasValue)
				{
					this.lciObecNarodenia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
					this.lciMiestoNarodeniaIne.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
				}
				else
				{
					this.lciObecNarodenia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
					this.lciMiestoNarodeniaIne.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
				}

				if (currentRfoFyzickaOsoba.RfoOkresNarodeniaIk.HasValue)
				{
					this.lciOkresNarodeniaIk.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
					this.lciOkresNarodeniaMimoCiselnik.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
				}
				else
				{
					this.lciOkresNarodeniaIk.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
					this.lciOkresNarodeniaMimoCiselnik.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
				}

				if (currentRfoFyzickaOsoba.RfoUzemnyCelokUmrtiaIk.HasValue)
				{
					this.lciMiestoUmrtiaIk.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
					this.lciMiestoUmrtiaMimoCiselnik.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
				}
				else
				{
					this.lciMiestoUmrtiaIk.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
					this.lciMiestoUmrtiaMimoCiselnik.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
				}
			}
		}

		private void btnZatvorit_Click_1(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion Event
	}
}
