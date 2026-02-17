using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Domain.Exceptions
{
    public class InvalidDenominationException:Exception
    {
        public InvalidDenominationException():base("Unsupported denomination ...")
        {
        }
    }
}
