using RestaurantHost.Core.Models;
using RestaurantHost.Infrastructure.SockProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace RestaurantHost.Infrastructure.MessageConverter
{
    public class CustomerMessageConverter
    {
        public Dictionary<int, BlockList> ModuleBlockMap { get; private set; } = new Dictionary<int, BlockList>(); // ModuleNo, BlockName 
        public Dictionary<int, TransactionList> ModuleTransactionMap { get; private set; } = new Dictionary<int, TransactionList>();// ModuleNo, TransactionName
        public Dictionary<int, TransactionMappingList> ModuleTransactionMappingMap { get; private set; } = new Dictionary<int, TransactionMappingList>();// ModuleNo, TransactionName
        public SockMessage ConvertFromByteArrayToMessage(int moduleNo, byte[] byteArr, int byteSize)
        {
            //try
            {
                if (byteArr == null || byteArr.Length == 0 || byteSize == 0)
                    throw new Exception("ByteArr Is Null");

                SockMessage recvMessage = new SockMessage(moduleNo);

                MemoryStream memoryStream = new MemoryStream(byteArr);
                BinaryReader binaryReader = new BinaryReader(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Current);


                string xmlMessage = new string(binaryReader.ReadChars(byteSize + 2 + 10)); // body + etx
                xmlMessage = xmlMessage.Substring(0, byteSize - 1);//ETX제거

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlMessage);
                XmlElement rootElement = doc.DocumentElement;
                foreach (XmlElement SubElement in rootElement)
                {
                    if (SubElement.Name == "HEADER" ||
                         SubElement.Name == "DATA" ||
                         SubElement.Name == "RESULT")
                    {
                        DeserializeXmlElementToMap(SubElement, recvMessage.MessageMap);
                    }
                    else
                    {

                        //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.MES, "[Recv <<] : " + xmlMessage);
                        throw new Exception("Unkown Data Item");
                    }
                }

                if (recvMessage.MessageMap.ContainsKey("MSG_ID") == true)
                {
                    string msgID = (string)recvMessage.MessageMap["MSG_ID"];

                    TransactionMappingList transactionMappingList = this.ModuleTransactionMappingMap[0];
                    if (transactionMappingList != null)
                    {
                        TransactionMapping transactionMapping = transactionMappingList.TransactionMappingMapByEventName[msgID];
                        if (transactionMapping != null)
                        {
                            //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.MES, string.Format("[Recv << - {0}({1})]: {2}", msgID, transactionMapping.Desc, xmlMessage));
                        }
                        else
                        {
                            //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.MES, string.Format("[Recv << - {0}]: {1}", msgID, xmlMessage));
                        }
                    }
                    else
                    {
                        //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.MES, string.Format("[Recv << - {0}]: {1}", msgID, xmlMessage));
                    }

                    recvMessage.SetCommand(msgID);
                    if (recvMessage.MessageMap.ContainsKey("CEID") == true)
                    {
                        string ceID = (string)recvMessage.MessageMap["CEID"];
                        if (string.IsNullOrEmpty(ceID) == false)
                        {
                            recvMessage.SetCommand(string.Format("{0}C{1}", msgID, ceID));
                        }
                        else
                        {
                            recvMessage.SetCommand(msgID);
                        }
                    }
                }
                else
                {

                    //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.MES, "[Recv <<] : " + xmlMessage);
                    throw new Exception("Don't find MSG ID");
                }
                return recvMessage;
            }
        }
        public bool InsertXmlNode(XmlNode root, string key, BlockItem block, XmlDocument doc, Dictionary<string, object> MessageMap)
        {
            if (MessageMap.ContainsKey(key) == true)
            {   //단수
                XmlNode newNode = doc.CreateElement(key);
                if (block.BlockMap.Count > 0)
                {   //List
                    foreach (KeyValuePair<string, Block> blockItem in block.BlockMap)
                    {
                        Dictionary<string, object> ChildMessageMap = new Dictionary<string, object>();
                        if (MessageMap[key].GetType() == typeof(Dictionary<string, object>))
                        {
                            ChildMessageMap = (Dictionary<string, object>)MessageMap[key];
                        }
                        InsertXmlNode(newNode, blockItem.Key, block, doc, ChildMessageMap);
                    }
                    root.AppendChild(newNode);
                    return true;
                }
                else
                {   //string
                    if (MessageMap[key].GetType() == typeof(string))
                    {
                        newNode.InnerText = (string)MessageMap[key];
                        root.AppendChild(newNode);
                        return true;
                    }
                    else if (MessageMap[key].GetType() == typeof(Int32))
                    {
                        newNode.InnerText = ((int)MessageMap[key]).ToString();
                        root.AppendChild(newNode);
                        return true;
                    }
                }
            }
            else
            {   //복수 ex) #0, #1
                int idx = 0;
                string ListKey = string.Format("{0}#{1}", key, idx);
                if (MessageMap.ContainsKey(ListKey) == false)
                {
                    //복수도 아님 그냥 매칭되는게 없음
                    XmlNode newNode = doc.CreateElement(key);
                    root.AppendChild(newNode);
                    return true;
                }
                else
                {   //복수 맞았음
                    while (true)
                    {
                        ListKey = string.Format("{0}#{1}", key, idx);
                        if (MessageMap.ContainsKey(ListKey) == true)
                        {
                            XmlNode childNode = doc.CreateElement(key);
                            if (block.BlockMap[key].BlockItemMap.Count > 0)
                            {   //List
                                foreach (KeyValuePair<string, BlockItem> blockItem in block.BlockMap[key].BlockItemMap)
                                {
                                    Dictionary<string, object> ChildMessageMap = new Dictionary<string, object>();
                                    if (MessageMap[ListKey].GetType() == typeof(Dictionary<string, object>))
                                    {
                                        ChildMessageMap = (Dictionary<string, object>)MessageMap[ListKey];
                                    }
                                    InsertXmlNode(childNode, blockItem.Key, blockItem.Value, doc, ChildMessageMap);
                                }
                                root.AppendChild(childNode);
                            }
                            else
                            {   //string
                                if (MessageMap[ListKey].GetType() == typeof(string))
                                {
                                    childNode.InnerText = (string)MessageMap[ListKey];
                                    root.AppendChild(childNode);
                                }
                            }
                            idx++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return true;
                }

            }
            return false;
        }

        public byte[] ConvertMessageToByteArray(int moduleNo, string MessangeName, SockMessage message)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            { //ConvertMessageToXmlElement

                Dictionary<string, object> messageMap = message.MessageMap;
                if (messageMap == null)
                    throw new Exception("MessageMap Is Null");

                TransactionMappingList transactionMappingList = this.ModuleTransactionMappingMap[0];
                if (transactionMappingList == null)
                    throw new Exception(string.Format("Cannot Find TransactionMappingList ModuleNo {0}", 0));

                TransactionMapping transactionMapping = transactionMappingList.GetTransactionMappingByMappingTransactionName(MessangeName);
                if (transactionMapping == null)
                    throw new Exception(string.Format("Cannot Find TransactionMapping ModuleNo {0}, ID {1}", moduleNo, MessangeName));


                TransactionList transactionList = ModuleTransactionMap[0];
                if (transactionList == null)
                    throw new Exception(string.Format("Cannot Find TransactionList"));

                Transaction transaction = transactionList.TransactionMap[transactionMapping.MappingTransactionName];
                if (transaction == null)
                    throw new Exception(string.Format("Cannot Find Transaction By TransactionName {0}", MessangeName));

                BlockList blockList = ModuleBlockMap[0];
                if (blockList == null)
                    throw new Exception(string.Format("Cannot Find BlockList}"));

                XmlNode root = xmlDocument.CreateElement("MESSAGE");
                foreach (TransactionItem transactionItem in transaction.TransactionItemList)
                {
                    if (blockList.BlockMap.ContainsKey(transactionItem.RelatedBlockItem) == true)
                    {
                        XmlNode SubRoot = xmlDocument.CreateElement(transactionItem.ItemName);
                        foreach (KeyValuePair<string, BlockItem> blocks in blockList.BlockMap[transactionItem.RelatedBlockItem].BlockItemMap)
                        {
                            if (InsertXmlNode(SubRoot, blocks.Key, blocks.Value, xmlDocument, message.MessageMap) == false)
                            {
                                throw new Exception(string.Format("Cannot InsertNode Block : {0} }", blocks.Key));
                            }
                        }
                        root.AppendChild(SubRoot);
                    }
                    else
                    {
                        throw new Exception(string.Format("Cannot Find Block : {0} }", transactionItem.RelatedBlockItem));
                    }
                    //transactionItem.Key

                }
                xmlDocument.AppendChild(root);

                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.MES, string.Format("[Send >> - {0}({1})]: {2}", MessangeName, transactionMapping.Desc, xmlDocument.InnerXml));
            }
            catch (Exception e)
            {

            }
            //ConvertXmlElementToByteArray
            return ConvertXmlElementToByteArray(xmlDocument);
        }
        public byte[] ConvertXmlElementToByteArray(XmlDocument doc)
        {
            try
            {
                Encoding encoding = Encoding.ASCII;
                byte[] docAsBytes = encoding.GetBytes(doc.OuterXml);
                byte[] sendData = new byte[docAsBytes.Length + 12];
                sendData[0] = 0x02;  //STX
                sendData[docAsBytes.Length + 11] = 0x03; //ETX
                Array.Copy(docAsBytes, 0, sendData, 11, docAsBytes.Length);
                for (int i = 0; i < 10; i++)
                {
                    //Length Init
                    sendData[1 + i] = 0x20;  //Space
                }
                string strLen = docAsBytes.Length.ToString();
                byte[] StrLenByte = Encoding.ASCII.GetBytes(strLen);
                Array.Copy(StrLenByte, 0, sendData, 1, StrLenByte.Length);
                return sendData;
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString().Replace("\r\n", ""));
            }

            return null;
        }
        public void DeserializeXmlElementToMap(XmlElement element, Dictionary<string, object> MessageMap)
        {
            foreach (XmlElement childElement in element)
            {
                //복수값 체크
                int checkOverride = 0;
                int overrideIdx = 0;
                foreach (XmlElement checkElement in element)
                {
                    if (childElement.Name == checkElement.Name)
                    {
                        if (childElement == checkElement)
                        {
                            overrideIdx = checkOverride;
                        }
                        checkOverride++;
                    }
                }

                string MessageKey;
                if (checkOverride <= 1)
                {
                    MessageKey = childElement.Name;
                }
                else
                {
                    MessageKey = string.Format("{0}#{1}", childElement.Name, overrideIdx);
                }

                if (childElement.HasChildNodes == true)
                {
                    if (childElement.FirstChild.Name == "#text")
                    {
                        MessageMap.Add(MessageKey, childElement.InnerText);
                    }
                    else
                    {
                        Dictionary<string, object> newMsgMap = new Dictionary<string, object>();
                        MessageMap.Add(MessageKey, newMsgMap);
                        DeserializeXmlElementToMap(childElement, newMsgMap);
                    }
                }
                else
                {
                    MessageMap.Add(childElement.Name, "");
                }
            }
        }
    }
}
