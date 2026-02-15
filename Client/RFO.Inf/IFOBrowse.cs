using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ditec.RIS.RFO.Inf
{
	public interface IFOBrowse
	{
		void LoadData(MethodInvoker continuator);
		void OpenDetail();
	}
}
