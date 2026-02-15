using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.SysFra.WinUI.Shared;
using Ditec.WinUI.Shell.Infrastructure.Mvc;
using LinFu.IoC.Configuration;
using Ditec.WinUI.Shell.Infrastructure;
using Ditec.RIS.CC.Dol;

namespace Ditec.RIS.RFO.Inf
{
    [ScenePart(Scene = ModuleScenes.FOBrowse)]
	[Implements(typeof(BrowseModel), ServiceName = ModuleScenes.FOBrowse)]
    public class FOBrowseModel : BaseBrowseModel
    {
        private FyzickaOsobaFilterCriteria filterCriteria;
   
        /// <summary>
        /// SelectedRecordID 
        /// </summary>
        public Guid? SelectedRecordID { get; set; }
        
        /// <summary>
        /// SelectedRecordName 
        /// </summary>
        public string SelectedRecordName { get; set; }

        
        /// <summary>
        /// SelectedRecord 
        /// </summary>
        public FyzickaOsoba SelectedRecord { get; set; }


        /// <summary>
		/// FyzickaOsobaViewOpen 
        /// </summary>
        public BrowseViewOpenType OpenType { get; set; }


        /// <summary>
        /// Vyhladavacie kriteria
        /// </summary>
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

		private Int32? _SelectedRecordEduId;
		public Int32? SelectedRecordEduId
		{
			get
			{
				return this._SelectedRecordEduId;
			}
			set
			{
				if (value != this._SelectedRecordEduId)
				{
					this._SelectedRecordEduId = value;
					this.OnPropertyChanged("SelectedRecordEduId");
				}
			}
		}
    }
}
