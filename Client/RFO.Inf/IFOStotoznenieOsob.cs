using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ditec.RIS.RFO.Inf
{
	public interface IFOStotoznenieOsob
	{
		void SelectOsZosVRis();
		void LoadOsZosVRis(MethodInvoker continuator);
		void SelectOkss();
		void LoadOkss(MethodInvoker continuator);
		void OpenDetailFyzickejOsoby(Guid? IDFyzickaOsoba);
		void StotoznenieOsobRIS(MethodInvoker invoker);
		void ZrusenieOznaceniaZaujmovejOsoby(MethodInvoker invoker);
	}
}
