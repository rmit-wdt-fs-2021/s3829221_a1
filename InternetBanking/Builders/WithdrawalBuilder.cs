using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Builders
{
    class WithdrawalBuilder : TransactionBuilder
    {

        public override void BuildType()
        {
            Transaction.TransactionType = 'W';
        }
    }
}
