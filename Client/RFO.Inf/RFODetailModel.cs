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
	[ScenePart(Scene = ModuleScenes.RFODetail)]
	[Implements(typeof(DetailModel), ServiceName = ModuleScenes.RFODetail)]
	public class RFODetailModel : DetailModel
    {
		public Osoba OsobaZRfo { get; set; }

		public FyzickaOsoba FyzickaOsoba { get; set; }

		public String FyzickaOsobaTitulPred { get; set; }

		public String FyzickaOsobaTitulZa { get; set; }

		public String FyzickaOsobaMiestoNarodenia { get; set; }

		public FyzickaOsobaFilterCriteria filterCriteria;

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
    }
}
