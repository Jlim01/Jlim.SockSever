using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Service.Services.CookingService
{
    internal class None : FoodFactory
    {
        protected override void Cooking()
        {
            //실시간 타이머 로그 찍기
            return;
        }
    }
}
