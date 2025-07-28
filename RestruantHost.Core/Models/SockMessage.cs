using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantHost.Core.Models
{
    //SockMessage는 Proxy와 Support가 공통으로 사용하는 메시지 모델이므로,
    //Core에 정의하는 것이 가장 적절하고 안정적인 구조다.
    public class SockMessage
    {
        // HEADER
        public int ModuleNo { get; }
        public string LineTable { get; set; } = "";
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string Command { get; set; } = "";

        //// DATA
        //public Dictionary<string, string> DataFields { get; } = new(); // 단일 키-값
        //public Dictionary<string, List<Dictionary<string, string>>> DataLists { get; } = new(); // 반복 구조

        public SockMessage(int moduleNo)
        {
            ModuleNo = moduleNo;
        }

        public SockMessage(int moduleNo, string command)
        {
            ModuleNo = moduleNo;
            Command = command;
        }
        public void SetCommand(string command)
        {
            Command = command;
        }
        public Dictionary<string, object> MessageMap { get; private set; } = new Dictionary<string, object>(); // For SImple Structure

        public bool AddValue(string key, object value)
        {
            try
            {
                if (value != null)
                {
                    if (value.GetType() == typeof(Dictionary<string, object>))
                    {
                        if (MessageMap.ContainsKey(key))
                        {
                            MessageMap[key] = value;
                        }
                        else
                        {
                            MessageMap.Add(key, value);
                        }
                    }
                    else
                    {
                        if (MessageMap.ContainsKey(key))
                        {
                            MessageMap[key] = value.ToString();
                        }
                        else
                        {
                            MessageMap.Add(key, value.ToString());
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
        public string GetString(string key)
        {
            if (MessageMap.ContainsKey(key))
            {
                return (string)MessageMap[key];
            }
            else
            {
                return null;
            }

        }

        public object GetObject(string key)
        {
            if (MessageMap.ContainsKey(key))
            {
                return MessageMap[key];
            }
            else
            {
                return null;
            }
        }
    }
}