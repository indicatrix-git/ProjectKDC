using Ditec.RIS.RFO.Dol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ditec.RIS.RFO.Inf
{
	public interface IOpravneCinnosti
	{
		void SelectOsVRis();
		void LoadOsVRis(MethodInvoker continuator);
		void LoadZoznamOsVRfo(MethodInvoker continuator);
		void OpenDetailFyzickejOsoby(Guid? IDFyzickaOsoba);
		void OpenDetailOsobyZRFO(Osoba osoba);
		void NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(System.Windows.Forms.MethodInvoker invoker);
		void SparovatOsobuZRisSOsobouSRfo(System.Windows.Forms.MethodInvoker invoker);
		void StotoznenieOsobRIS(MethodInvoker invoker);
		void ZrusenieOznaceniaZaujmovejOsoby(MethodInvoker invoker);
	}
}
