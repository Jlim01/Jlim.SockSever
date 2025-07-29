using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Service.Services.TableService
{
    class TableManagerService
    {
        //IEnumerable<TableStatusService> TableStatusServices = IEnumerable<TableStatusService> new();
        //List<TableStatusService> TableStatusServices { get; set; }
        public TableManagerService(IEnumerable<TableStatusService> tableStatusService ) // IEnumerable 사용하면 안될 듯함.
        {
            var TableStatusServices = tableStatusService;
            for(int idx = 0; idx < 5; ++idx)
                TableStatusServices.Append(new TableStatusService());
        }
    }
}
