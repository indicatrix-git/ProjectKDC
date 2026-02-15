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
	[ScenePart(Scene = ModuleScenes.OpravneCinnosti)]
	[Implements(typeof(BrowseModel), ServiceName = ModuleScenes.OpravneCinnosti)]
	public class OpravneCinnostiModel : BaseBrowseModel
    {
		private Guid? _OsVRisID;

		public Guid? OsVRisID
		{
			get
			{
				return this._OsVRisID;
			}
			set
			{
				this._OsVRisID = value;
				this.OnPropertyChanged("OsVRisID");
			}
		}

		private String _OsVRisEduid;

		public String OsVRisEduid
		{
			get
			{
				return this._OsVRisEduid;
			}
			set
			{
				this._OsVRisEduid = value;
				this.OnPropertyChanged("OsVRisEduid");
			}
		}

		private Osoba _OsVRis;

		public Osoba OsVRis
		{
			get
			{
				return this._OsVRis;
			}
			set
			{
				this._OsVRis = value;
				this.OnPropertyChanged("OsVRis");
			}
		}

		private String _VyhlOsVRfoMeno;

		public String VyhlOsVRfoMeno
		{
			get
			{
				return this._VyhlOsVRfoMeno;
			}
			set
			{
				this._VyhlOsVRfoMeno = value;
				this.OnPropertyChanged("VyhlOsVRfoMeno");
			}
		}

		private String _VyhlOsVRfoPriezvisko;

		public String VyhlOsVRfoPriezvisko
		{
			get
			{
				return this._VyhlOsVRfoPriezvisko;
			}
			set
			{
				this._VyhlOsVRfoPriezvisko = value;
				this.OnPropertyChanged("VyhlOsVRfoPriezvisko");
			}
		}

		private String _VyhlOsVRfoRodnePriezvisko;

		public String VyhlOsVRfoRodnePriezvisko
		{
			get
			{
				return this._VyhlOsVRfoRodnePriezvisko;
			}
			set
			{
				this._VyhlOsVRfoRodnePriezvisko = value;
				this.OnPropertyChanged("VyhlOsVRfoRodnePriezvisko");
			}
		}

		private String _VyhlOsVRfoRodneCislo;

		public String VyhlOsVRfoRodneCislo
		{
			get
			{
				return this._VyhlOsVRfoRodneCislo;
			}
			set
			{
				this._VyhlOsVRfoRodneCislo = value;
				this.OnPropertyChanged("VyhlOsVRfoRodneCislo");
			}
		}

		private DateTime? _VyhlOsVRfoDatumNarodenia;

		public DateTime? VyhlOsVRfoDatumNarodenia
		{
			get
			{
				return this._VyhlOsVRfoDatumNarodenia;
			}
			set
			{
				this._VyhlOsVRfoDatumNarodenia = value;
				this.OnPropertyChanged("VyhlOsVRfoDatumNarodenia");
			}
		}

		private Guid? _OsVRfoID;

		public Guid? OsVRfoID
		{
			get
			{
				return this._OsVRfoID;
			}
			set
			{
				this._OsVRfoID = value;
				this.OnPropertyChanged("OsVRfoID");
			}
		}

		private List<Osoba> _ZoznamOsobVRfo;

		public List<Osoba> ZoznamOsobVRfo
		{
			get
			{
				return this._ZoznamOsobVRfo;
			}
			set
			{
				this._ZoznamOsobVRfo = value;
				this.OnPropertyChanged("ZoznamOsobVRfo");
			}
		}

		private FyzickaOsoba _OsVRfo;

		public FyzickaOsoba OsVRfo
		{
			get
			{
				return this._OsVRfo;
			}
			set
			{
				this._OsVRfo = value;
				this.OnPropertyChanged("OsVRfo");
			}
		}

		private NajdenaOsoba _NajdenaOsoba;

		public NajdenaOsoba NajdenaOsoba
		{
			get
			{
				return this._NajdenaOsoba;
			}
			set
			{
				this._NajdenaOsoba = value;
				this.OnPropertyChanged("NajdenaOsoba");
			}
		}
    }
}
