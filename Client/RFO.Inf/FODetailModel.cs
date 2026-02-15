using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.SysFra.WinUI.Shared;
using Ditec.WinUI.Shell.Infrastructure.Mvc;
using LinFu.IoC.Configuration;
using System.Windows.Forms;
using Ditec.RIS.RFO.Inf;
using Ditec.RIS.CC.Dol;

namespace Ditec.RIS.RFO.Inf
{
	[ScenePart(Scene = ModuleScenes.FODetail)]
	[Implements(typeof(DetailModel), ServiceName = ModuleScenes.FODetail)]
	public class FODetailModel : DetailModel
    {
		public FyzickaOsoba FyzickaOsoba { get; set; }

		public String FyzickaOsobaTitulPred { get; set; }

		public String FyzickaOsobaTitulZa { get; set; }

		public String FyzickaOsobaMiestoNarodenia { get; set; }

		private FyzickaOsobaFilterCriteria filterCriteria;

		public FyzickaOsobaFilterCriteria FilterCriteria
		{
			get
			{
				if (this.filterCriteria == null)
					this.filterCriteria = new FyzickaOsobaFilterCriteria();

				return this.filterCriteria;
			}
			set
			{
				this.filterCriteria = value;
				this.OnPropertyChanged("FilterCriteria");
			}
		}

		private Int32? _EDUIDPravejOsoby;

		public Int32? EDUIDPravejOsoby
		{
			get
			{
				return this._EDUIDPravejOsoby;
			}
			set
			{
				this._EDUIDPravejOsoby = value;
				this.OnPropertyChanged("EDUIDPravejOsoby");
			}
		}
    }
}
