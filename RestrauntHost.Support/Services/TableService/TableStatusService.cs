using RestaurantHost.Core.Enums;
using RestaurantHost.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Service.Services.TableService
{
    // Captin(관리자 == TableManager) 가 리스트 형태로 TableStatusService 즉 여러 테이블의 상태를 관리하기.
    public class TableStatusService
    {
        public List<FoodInfo> FoodInfos; // 복수로 음식 시킬 수 있기에.
        public int TotalPrice;
        public ProcessEnumType ProcessStatus;
        public int TableNo;

    }
}
