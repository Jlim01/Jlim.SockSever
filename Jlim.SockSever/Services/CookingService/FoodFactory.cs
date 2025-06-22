using RestaurantHost.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Main.Services.CookingService
{
    abstract class FoodFactory
    {
        private static string foodName = "";
        public static FoodFactory CreateFood(FoodEnumType food)
        {
            foodName = food.ToString();
            switch (food)
            {
                case FoodEnumType.Jajangmyeon:
                    return new Jajangmyeon();
                default:
                    return new None();
            }
        }
        public void Make()
        {
            Console.WriteLine($"{foodName} 조리 시작");
            Cooking();
            Console.WriteLine($"{foodName} 조리 끝");
        }
        abstract protected void Cooking();
    }
}
