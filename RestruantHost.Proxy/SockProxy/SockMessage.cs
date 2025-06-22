using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Proxy.SockProxy
{
    public class SockMessage
    {
        public int TableNo { get; private set; }
        public string TransactionName { get; private set; } = "";
        public string TransactionKey { get; set; } = "";
        public SockMessage(int tableNo)
        {
            TableNo = tableNo;
        }
        public SockMessage(int moduleNo, string transactionName)
        {
            TableNo = moduleNo;
            TransactionName = transactionName;
        }
    }
}
