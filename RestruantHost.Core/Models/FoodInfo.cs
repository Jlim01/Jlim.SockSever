using RestaurantHost.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Core.Models
{
    public class FoodInfo
    { // 각 메뉴 요소 정보.
        public FoodEnumType FoodName;
        public int Price;
        public int Count;
    }
}
