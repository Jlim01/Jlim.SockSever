using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RestaurantHost.Infrastructure.MessageConverter
{
    public class TransactionItem
    {
        public string RelatedBlockItem { get; set; }
        public string ItemName { get; set; }

        public TransactionItem(XmlElement transactionItemElement)
        {
            try
            {
                RelatedBlockItem = transactionItemElement.GetAttribute("Name");
                try
                {
                    ItemName = transactionItemElement.GetAttribute("ItemName");
                }
                catch (Exception e)
                {

                }
            }
            catch (XmlException e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
        }
    }

    public class Transaction
    {
        public string Name { get; set; }
        public string LogLevel { get; set; }
        public List<TransactionItem> TransactionItemList { get; private set; } = new List<TransactionItem>();

        public Transaction(XmlElement transactionElement)
        {
            try
            {
                Name = transactionElement.GetAttribute("Name");
                LogLevel = transactionElement.GetAttribute("LogLevel");

                if (string.IsNullOrEmpty(LogLevel))
                    LogLevel = "Info";

                foreach (XmlElement transactionItemElement in transactionElement)
                {
                    TransactionItem transactionItem = new TransactionItem(transactionItemElement);

                    if (transactionItem == null)
                        throw new Exception("Cannot Parse TransactionItem");

                    TransactionItemList.Add(transactionItem);
                }
            }
            catch (XmlException e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
        }
    }

    public class TransactionList
    {
        public int ModuleNo { get; private set; }
        public Dictionary<string, Transaction> TransactionMap { get; private set; } = new Dictionary<string, Transaction>();

        public TransactionList(XmlElement transactionListElement)
        {
            try
            {
                ModuleNo = int.Parse(transactionListElement.GetAttribute("ModuleNo"));

                foreach (XmlElement transactionElement in transactionListElement)
                {
                    Transaction transaction = new Transaction(transactionElement);

                    if (transaction == null)
                        throw new Exception("Cannot Parse Transaction");

                    TransactionMap.Add(transaction.Name, transaction);
                }
            }
            catch (XmlException e)
            {

                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
        }
    }

    public class TransactionMapping
    {
        public string MappingTransactionName { get; private set; }
        public string EventName { get; private set; }
        public string Desc { get; private set; }
        public string MappingEventIndex { get; private set; }
        public string MappingEventType { get; private set; }
        public string Ceid { get; private set; }

        public TransactionMapping(XmlElement transactionMappingElement)
        {
            try
            {
                MappingTransactionName = transactionMappingElement.GetAttribute("MappingTransactionName");
                EventName = transactionMappingElement.GetAttribute("EventName");
                MappingEventIndex = transactionMappingElement.GetAttribute("MappingEventIndex");
                MappingEventType = transactionMappingElement.GetAttribute("MappingEventType");
                Ceid = transactionMappingElement.GetAttribute("Ceid");
                Desc = transactionMappingElement.GetAttribute("Desc");
            }
            catch (XmlException e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
        }
    }

    public class TransactionMappingList
    {
        public int ModuleNo { get; private set; }
        public Dictionary<string, TransactionMapping> TransactionMappingMapByMappingTransactionName { get; private set; } = new Dictionary<string, TransactionMapping>();
        public Dictionary<string, TransactionMapping> TransactionMappingMapByEventName { get; private set; } = new Dictionary<string, TransactionMapping>();

        public TransactionMappingList(XmlElement transactionMappingListElement)
        {
            try
            {
                ModuleNo = int.Parse(transactionMappingListElement.GetAttribute("ModuleNo"));

                foreach (var obj in transactionMappingListElement)
                {
                    if (obj is XmlElement)
                    {
                        XmlElement transactionMappingElement = obj as XmlElement;

                        TransactionMapping transactionMapping = new TransactionMapping((XmlElement)transactionMappingElement);

                        if (transactionMapping == null)
                            throw new Exception("Cannot Parse TransactionMappingList");

                        if (TransactionMappingMapByMappingTransactionName.ContainsKey(transactionMapping.MappingTransactionName) == false)
                        {
                            TransactionMappingMapByMappingTransactionName.Add(transactionMapping.MappingTransactionName, transactionMapping);
                        }

                        if (TransactionMappingMapByEventName.ContainsKey(transactionMapping.EventName) == false)
                        {
                            TransactionMappingMapByEventName.Add(transactionMapping.EventName, transactionMapping);
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
        }

        public string GetEventNamegByMappingTransactionName(string mappingTransactionName)
        {
            return TransactionMappingMapByMappingTransactionName[mappingTransactionName].EventName;
        }

        public TransactionMapping GetTransactionMappingByMappingTransactionName(string mappingTransactionName)
        {
            return TransactionMappingMapByMappingTransactionName[mappingTransactionName];
        }

        public TransactionMapping GetTransactionMappingByEventNo(int eventNo)
        {
            if (TransactionMappingMapByEventName.ContainsKey(eventNo.ToString()))
            {
                return TransactionMappingMapByEventName[eventNo.ToString()];
            }
            else
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, "TransactionMappingMapByEventName.ContainsKey false");
                return null;
            }
        }
    }

    public class TransactionEventRule
    {
        public int ModuleNo { get; private set; }
        public int EventNameStartByteIndex { get; private set; }
        public int EventNameEndByteIndex { get; private set; }
        public string EventNameType { get; private set; }
        public int MessageSizeByteIndex { get; private set; }
        public string MessageSizeType { get; private set; }
        public string SocketType { get; private set; }

        public TransactionEventRule(XmlElement transactionEventRuleElement)
        {
            try
            {
                ModuleNo = int.Parse(transactionEventRuleElement.GetAttribute("ModuleNo"));
                EventNameStartByteIndex = int.Parse(transactionEventRuleElement.GetAttribute("EventNameStartByteIndex"));

                string strTmp = transactionEventRuleElement.GetAttribute("EventNameEndByteIndex");
                if (strTmp == "")
                {
                    EventNameEndByteIndex = 0;
                }
                else
                {
                    EventNameEndByteIndex = int.Parse(transactionEventRuleElement.GetAttribute("EventNameEndByteIndex"));
                }

                EventNameType = transactionEventRuleElement.GetAttribute("EventNameType");
                MessageSizeByteIndex = int.Parse(transactionEventRuleElement.GetAttribute("MessageSizeByteIndex"));
                MessageSizeType = transactionEventRuleElement.GetAttribute("MessageSizeType");
                SocketType = transactionEventRuleElement.GetAttribute("SocketType");
            }
            catch (XmlException e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
        }
    }


    public class SendSocketCharArrayRule
    {
        public int ModuleNo { get; private set; }
        public int CharArrayRule { get; private set; }

        public SendSocketCharArrayRule(XmlElement transactionEventRuleElement)
        {
            try
            {
                ModuleNo = int.Parse(transactionEventRuleElement.GetAttribute("ModuleNo"));
                CharArrayRule = int.Parse(transactionEventRuleElement.GetAttribute("CharArrayRule"));
            }
            catch (XmlException e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
        }
    }


    public class RemainByte
    {
        public int ReadPositionByte { get; set; }
        public int StructCount { get; set; }
        public int ByteSize { get; set; }
    }
}
