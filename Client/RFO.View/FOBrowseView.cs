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
	[ScenePart(Scene = Ditec.RIS.RFO.Inf.ModuleScenes.FOBrowse)]
	[Implements(typeof(BrowseView), ServiceName = Ditec.RIS.RFO.Inf.ModuleScenes.FOBrowse)]
	public partial class FOBrowseView : BrowseView
	{
		FOBrowseModel model;
		private Ditec.RIS.CC.Inf.ICodeListCache codeListCache;
		List<SparovanieSRFO> sparovanieSRFOList = new List<SparovanieSRFO>() { new SparovanieSRFO() { Nazov = "Spárované s RFO", Hodnota = true }, new SparovanieSRFO() { Nazov = "Nespárované s RFO", Hodnota = false }};

		public FOBrowseView()
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		public FOBrowseView(IShell shell)
			: base(shell)
		{
			Localizer.Active = new MyLocalizer();
			InitializeComponent();
		}

		#region Methods

		private void InitializeCodeLists()
		{
			codeListCache = this.Shell.GetServiceLocator().GetInstance<Ditec.RIS.CC.Inf.ICodeListCache>();

			Tools.SetNumberOfRowsInLookUpEdit(this.lcgSearch);
		}

		private void Search()
		{
			(this.Model.Controller as IFOBrowse).LoadData(null);
		}

		private void bestFitGridColumns()
		{
			this.gvZoznamRFO.BeginDataUpdate();
			this.gvZoznamRFO.BestFitColumns();
			this.gvZoznamRFO.EndDataUpdate();
		}

		/// <summary>
		/// Nastavia sa visibility controlov
		/// </summary>
		private void SetVisibility()
		{
			if (this.model.OpenType == BrowseViewOpenType.SelectOpen)
			{
				this.lciVybrat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
				this.lciDetail.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			}
			else
			{
				this.lciVybrat.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
				this.lciDetail.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
			}
		}

		#endregion Methods

		#region Events

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.model = this.Model as FOBrowseModel;

			this.InitializeCodeLists();
			
			//nastavim si binding na model
			this.model.FilterCriteria.Zneplatnena = false;
			this.bsFilterCriteria.DataSource = this.model.FilterCriteria;

			//nastavenie masiek
			Tools.SetMasksByDataSource(this.lcgSearch, typeof(FyzickaOsobaFilterCriteria));
			Tools.SetMaskInt(this.tbEDUID, 9);
			Tools.SetMaskTextLength(this.tbRodneCislo, 11);

			//nastavenie lookUpEditov
			ViewHelper.SetLookUpEditsButtons(this.lcgSearch, true, true);

			//nastavim si property gridu
			ViewHelper.SetGridViewProperties(this.gvZoznamRFO);

			//nastavim si, aby sa prazdny string bral ako null
			ViewHelper.SetTextCorrectEvent(this.lcgSearch);

			this.SetVisibility();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.bsSparovanieSRFO.DataSource = sparovanieSRFOList.ToList();
			//this.btnVyhladat_Click(null, e);
		}
		
		private void PagerMain_PageChangeRequested(object sender, PageNavigationEventArgs e)
		{
			this.model.Pager.CurrentPage = e.RequiredPage;
			this.model.Pager.RecordsPerPage = e.RequiredRecordsPerPage;
			this.Search();
		}

		private void btnVyhladat_Click(object sender, EventArgs e)
		{
			this.model.Pager.CurrentPage = 1;
			this.Search();
		}

		private void btnZrusitKriteria_Click(object sender, EventArgs e)
		{
			this.model.FilterCriteria = new FyzickaOsobaFilterCriteria();
			this.bsFilterCriteria.DataSource = this.model.FilterCriteria;
			this.model.Data = new List<FyzickaOsoba>();
			this.model.Pager.CurrentPage = 0;
			this.model.Pager.TotalRecords = 0;
		}

		private void btnDetail_Click(object sender, EventArgs e)
		{
			switch (this.model.OpenType)
			{
				case BrowseViewOpenType.SelectOpen:
					{
						this.btnVybrat_Click(sender, null);
						break;
					}
				default:
					{
						(this.model.Controller as IFOBrowse).OpenDetail();
						break;
					}
			}
		}

		private void btnVybrat_Click(object sender, EventArgs e)
		{
			if (this.lciVybrat.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
			{
				this.gcZoznamRFO.RefreshDataSource();
				this.model.SelectedRecordID = ((FyzickaOsoba)bsData.Current).ID;
				this.model.SelectedRecordEduId = ((FyzickaOsoba)bsData.Current).Eduid;
				this.model.SelectedRecord = (FyzickaOsoba)bsData.Current;
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void btnZatvorit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void bsData_CurrentChanged(object sender, EventArgs e)
		{
			if (this.model.OpenType == BrowseViewOpenType.StandardOpen)
			{
				if (this.bsData.Current != null)
				{
					this.model.SelectedRecordID = (this.bsData.Current as FyzickaOsoba).ID;
					this.model.SelectedRecord = (FyzickaOsoba)this.bsData.Current;
				}
			}
		}

		#endregion Events
	}
}
