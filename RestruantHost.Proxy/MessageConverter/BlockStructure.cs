using RestaurantHost.Core.Models;
using RestaurantHost.Infrastructure.SockProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RestaurantHost.Infrastructure.MessageConverter
{
    public class BlockItem
    {
        public string Name { get; private set; }
        public string ValueType { get; private set; }
        public int ByteSize { get; private set; }
        public string DefaultValue { get; private set; }
        public string BlockName { get; private set; }
        public string BlockCount { get; private set; }

        public string Value { get; private set; }

        public List<Block> BlockListArray { get; private set; } = new List<Block>();
        public Dictionary<string, Block> BlockMap { get; private set; } = new Dictionary<string, Block>();

        public BlockItem(XmlElement blockItemElement)
        {
            try
            {
                Name = blockItemElement.GetAttribute("Name");
                ValueType = blockItemElement.GetAttribute("Type");
                if (string.IsNullOrEmpty(blockItemElement.GetAttribute("ByteSize")) == false)
                {
                    ByteSize = int.Parse(blockItemElement.GetAttribute("ByteSize"));
                }
                BlockName = blockItemElement.GetAttribute("BlockName");
                DefaultValue = blockItemElement.GetAttribute("DefaultValue");
                BlockCount = blockItemElement.GetAttribute("BlockCount");
            }
            catch (XmlException e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }
            foreach (XmlElement blockElement in blockItemElement)
            {
                Block block = new Block(blockElement);

                if (block == null)
                    throw new Exception("Cannot Parse Block");

                BlockMap.Add(block.Name, block);
            }
        }

        public bool IsBlockList()
        {
            if (string.IsNullOrEmpty(BlockName))
                return false;
            return true;
        }

        public int GetBlockListCount(SockMessage message, string upperBlockName)
        {
            try
            {
                if (int.TryParse(BlockCount, out int val))
                {
                    // 고정 크기
                    return val;
                }
                else if (message.MessageMap.ContainsKey(upperBlockName + BlockCount))
                {
                    // 가변 크기
                    return int.Parse((string)message.MessageMap[upperBlockName + BlockCount]);
                }
            }
            catch (Exception e)
            {
                //PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
            }

            return 0;
        }

        public void SetValue(string value)
        {
            Value = value;
        }
    }

    public class Block
    {
        public string Name { get; private set; }
        public int ByteSize { get; set; }
        public Dictionary<string, BlockItem> BlockItemMap { get; private set; } = new Dictionary<string, BlockItem>();  // For Contains Check
        public List<BlockItem> BlockItemList { get; private set; } = new List<BlockItem>();              // For Converting To ByteArry

        public Block(XmlElement blockElement)
        {
            try
            {
                string blockName = blockElement.GetAttribute("Name");
                int blockByteSize = 0;
                try
                {
                    blockByteSize = int.Parse(blockElement.GetAttribute("ByteSize"));
                }
                catch (Exception e)
                {
                    ////PNELog.PNELogService.Instance.WriteLog(PNELog.PNELogService.LogLevelEnum.SBCSOCKET_ERROR, e.ToString());
                }

                foreach (var obj in blockElement)
                {
                    if (obj is XmlComment) continue;

                    if (obj is XmlElement)
                    {
                        XmlElement blockItemElement = obj as XmlElement;

                        BlockItem blockItem = new BlockItem(blockItemElement);

                        if (blockItem == null)
                            throw new Exception("Cannot Parse BlockItem");

                        BlockItemMap.Add(blockItem.Name, blockItem);
                        BlockItemList.Add(blockItem);
                    }
                }

                Name = blockName;
                ByteSize = blockByteSize;
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

    public class BlockList
    {
        public int ModuleNo { get; private set; }
        public Dictionary<string, Block> BlockMap { get; private set; } = new Dictionary<string, Block>();
        public int installChCount { get; private set; } = 0; // "ChannelDataReport", "StepEndResultNotify"는 sbc installCount에 따라 구조체 사이즈가 변경됨


        public BlockList(XmlElement blockListElement)
        {
            try
            {
                ModuleNo = int.Parse(blockListElement.GetAttribute("ModuleNo"));

                foreach (XmlElement blockElement in blockListElement)
                {
                    Block block = new Block(blockElement);

                    if (block == null)
                        throw new Exception("Cannot Parse Block");

                    BlockMap.Add(block.Name, block);
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

        public void SetInstallChCount(int installChCount)
        {
            this.installChCount = installChCount;
        }
        
    }
}
