using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bbv.Common.EventBroker;
using Ditec.RIS.Client.Utils;
using Ditec.RIS.SysFra.WinUI.Shared;
using Ditec.WinUI.Shell.Infrastructure;
using Ditec.WinUI.Shell.Infrastructure.Mvc;
using LinFu.IoC.Configuration;

namespace Ditec.RIS.RFO.Inf
{
    public partial class ModuleMenu : Form, IModule
    {

        public ModuleMenu()
        {
            InitializeComponent();
        }

        public ModuleMenu(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        [Inject]
        public IShell Shell
        {
            get;
            set;
        }

        /// <summary>
        /// event pri spusteni
        /// </summary>
        public void OnLoad()
        {
        }

        /// <summary>
        /// Skontroluje, ci formular nie je uz otvoreny, ak ano, tak ho aktivuje
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        private bool IsfOpenedForm(string scene)
        {
            var openedForms = (this.Shell as Form).MdiChildren;

            foreach (var openedForm in openedForms)
            {
                // ak okno je uz raz otvorene, da hu do popredia
				if ((openedForm as GenericView).GetSceneName() == scene && (openedForm as Form).Enabled)
                {
                    openedForm.Activate();
                    return true;
                }
				else if ((openedForm as GenericView).GetSceneName() == scene && !(openedForm as Form).Enabled)
				{
					return true;
				}
            }
            return false;
        }

        public DevExpress.XtraBars.Bar GetMenuBar()
        {
			if (!Permission.FO_Read)
				this.bbiRFO.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

			if (!Permission.FO_Modify)
			{
				this.bsiOpravneCinnosti.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
				this.bbiOpravneCinnostiRISRFO.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
			}

            return miMainMenu;
        }

        private void bbiRFO_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.IsfOpenedForm(ModuleScenes.FOBrowse))
                return;

			var controller = this.Shell.GetController(this, typeof(BrowseController), ModuleScenes.FOBrowse) as BrowseController;
			(controller.GetModel() as FOBrowseModel).OpenType = BrowseViewOpenType.StandardOpen;
            controller.Show(ModalMode.None);
        }

		private void bbiOpravneCinnostiRISRFO_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if (this.IsfOpenedForm(ModuleScenes.OpravneCinnosti))
				return;

			var controller = this.Shell.GetController(this, typeof(BrowseController), ModuleScenes.OpravneCinnosti) as BrowseController;
			controller.Show(ModalMode.None);
		}

		private void bbiStotoznenieOsobVRIS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if (this.IsfOpenedForm(ModuleScenes.FOStotoznenieOsob))
				return;

			var controller = this.Shell.GetController(this, typeof(DetailController), ModuleScenes.FOStotoznenieOsob) as DetailController;
			controller.Show(ModalMode.None);
		}
    }
}
