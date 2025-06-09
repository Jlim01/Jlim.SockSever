using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Core.Enums
{
    enum ProcessEnumType
    {
        Empty, Come_IN,Client_IN, Client_OUT, Menu_Ack, Order_Ack, Pay_Ack, Waiting_Time, Wait_Req
    }
    //Empty 손님없음, Come_IN 자리빔에 따른 출입 요청, Client_IN 손님 입장, Client_OUT 손님 나감,
    //Menu_Ack 메뉴판 요청에 대한 응답, Order_Ack 주문 요청에 대한 응답, Pay_Ack 결제 요청에 대한 응답
    // Waiting_Time 곧 나와요~, 만석에 따른 대기요청
}
