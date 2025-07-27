using System;
using System.Collections.Generic;
using System.Text;

//todo : 다른 코드 참고하기.
namespace RestaurantHost.Proxy.SockProxy
{
    public class SockMessage
    {
        // HEADER
        public int ClientId { get; }
        public string LineTable { get; set; } = "";
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string Command { get; set; } = "";

        // DATA
        public Dictionary<string, string> DataFields { get; } = new(); // 단일 키-값
        public Dictionary<string, List<Dictionary<string, string>>> DataLists { get; } = new(); // 반복 구조

        public SockMessage(int clientId)
        {
            ClientId = clientId;
        }

        public SockMessage(int clientId, string command)
        {
            ClientId = clientId;
            Command = command;
        }

        public void AddData(string key, string value)
        {
            DataFields[key] = value;
        }

        public void AddDataList(string listName, Dictionary<string, string> item)
        {
            if (!DataLists.ContainsKey(listName))
                DataLists[listName] = new List<Dictionary<string, string>>();

            DataLists[listName].Add(item);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<Message>");
            sb.AppendLine("  <HEADER>");
            sb.AppendLine($"    <LINE_TABLE>{LineTable}</LINE_TABLE>");
            sb.AppendLine($"    <FROM>{From}</FROM>");
            sb.AppendLine($"    <TO>{To}</TO>");
            sb.AppendLine($"    <COMMAND>{Command}</COMMAND>");
            sb.AppendLine("  </HEADER>");
            sb.AppendLine("  <DATA>");

            foreach (var field in DataFields)
            {
                sb.AppendLine($"    <{field.Key}>{field.Value}</{field.Key}>");
            }

            foreach (var list in DataLists)
            {
                sb.AppendLine($"    <{list.Key}>");
                foreach (var item in list.Value)
                {
                    sb.AppendLine("      <ITEM>");
                    foreach (var field in item)
                    {
                        sb.AppendLine($"        <{field.Key}>{field.Value}</{field.Key}>");
                    }
                    sb.AppendLine("      </ITEM>");
                }
                sb.AppendLine($"    </{list.Key}>");
            }

            sb.AppendLine("  </DATA>");
            sb.AppendLine("</Message>");
            return sb.ToString();
        }
    }
}
