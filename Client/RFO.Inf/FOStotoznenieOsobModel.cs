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
	[ScenePart(Scene = ModuleScenes.FOStotoznenieOsob)]
	[Implements(typeof(DetailModel), ServiceName = ModuleScenes.FOStotoznenieOsob)]
	public class FOStotoznenieOsobModel : DetailModel
    {
		private Guid? _OsZosVRisID;

		public Guid? OsZosVRisID
		{
			get
			{
				return this._OsZosVRisID;
			}
			set
			{
				this._OsZosVRisID = value;
				this.OnPropertyChanged("OsZosVRisID");
			}
		}

		private String _OsZosVRisEduid;

		public String OsZosVRisEduid
		{
			get
			{
				return this._OsZosVRisEduid;
			}
			set
			{
				this._OsZosVRisEduid = value;
				this.OnPropertyChanged("OsZosVRisEduid");
			}
		}

		private Osoba _OsZosVRis;

		public Osoba OsZosVRis
		{
			get
			{
				return this._OsZosVRis;
			}
			set
			{
				this._OsZosVRis = value;
				this.OnPropertyChanged("OsZosVRis");
			}
		}

		public String OsZosVRisTitulPred { get; set; }

		public String OsZosVRisTitulZa { get; set; }

		private Guid? _OkssID;

		public Guid? OkssID
		{
			get
			{
				return this._OkssID;
			}
			set
			{
				this._OkssID = value;
				this.OnPropertyChanged("OkssID");
			}
		}

		private String _OkssEduid;

		public String OkssEduid
		{
			get
			{
				return this._OkssEduid;
			}
			set
			{
				this._OkssEduid = value;
				this.OnPropertyChanged("OkssEduid");
			}
		}

		private Osoba _Okss;

		public Osoba Okss
		{
			get
			{
				return this._Okss;
			}
			set
			{
				this._Okss = value;
				this.OnPropertyChanged("Okss");
			}
		}

		public String OkssTitulPred { get; set; }

		public String OkssTitulZa { get; set; }
    }
}
