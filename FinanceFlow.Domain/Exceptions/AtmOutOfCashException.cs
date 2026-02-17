using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.Exceptions
{
    public class AtmOutOfCashException:Exception
    {
        public AtmOutOfCashException():base("Out of cash .....") { }
    }
}
