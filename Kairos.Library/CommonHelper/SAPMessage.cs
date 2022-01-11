using Kairos.Library.ControlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kairos.Library.CommonHelper
{
    public class SAPMessage
    {
        public const string ERROR = "E";
        public const string SUCCESS = "S";
        public const string WARNING = "W";
        public const string INFO = "I";

        public string Type { get; private set; }
        public string Message { get; set; }

        public SAPMessage()
        {
            Type = SUCCESS;
            Message = string.Empty;
        }
        public SAPMessage(string pType, string pMessage)
            : this()
        {
            Type = pType;
            Message = pMessage;
        }

        public static void AddMessage(List<SAPMessage> SAPMessageList, string Type, string Message)
        {
            SAPMessage SAPMsg = null;
            switch (Type.ToUpper())
            {
                case "S":
                case "W":
                case "E":
                case "I":
                    SAPMsg = new SAPMessage();
                    SAPMsg.Type = Type.ToUpper();
                    SAPMsg.Message = Message;
                    SAPMessageList.Add(SAPMsg);
                    break;
                case "":
                    if (!string.IsNullOrEmpty(Message))
                    {
                        SAPMsg = new SAPMessage();
                        SAPMsg.Type = Type.ToUpper();
                        SAPMsg.Message = Message;
                        SAPMessageList.Add(SAPMsg);
                    }
                    break;
                default:
                    throw new Exception("Unknown SAP Message Type");
            }
        }
        public static void AddMessage(List<SAPMessage> DestinationList, List<SAPMessage> SourceList)
        {
            DestinationList.AddRange(SourceList);
        }
        public static void AddSuccessMessage(List<SAPMessage> SAPMessageList, string Message)
        {
            AddMessage(SAPMessageList, SAPMessage.SUCCESS, Message);
        }
        public static void AddWarningMessage(List<SAPMessage> SAPMessageList, string Message)
        {
            AddMessage(SAPMessageList, SAPMessage.WARNING, Message);
        }
        public static void AddInfoMessage(List<SAPMessage> SAPMessageList, string Message)
        {
            AddMessage(SAPMessageList, SAPMessage.INFO, Message);
        }
        public static void AddErrorMessage(List<SAPMessage> SAPMessageList, string Message)
        {
            AddMessage(SAPMessageList, SAPMessage.ERROR, Message);
        }

        public static string DisplayToString(List<SAPMessage> SAPMessageList)
        {
            MessageBalloonNotification MsgBalloon = DisplayToWeb(SAPMessageList);
            StringBuilder sb = new StringBuilder();
            string TypeDesc = string.Empty;

            switch (MsgBalloon.MsgType.ToUpper())
            {
                case "A":
                    TypeDesc = "Abort";
                    break;
                case "I":
                    TypeDesc = "Info";
                    break;
                case "W":
                    TypeDesc = "Warning";
                    break;
                case "E":
                    TypeDesc = "Error";
                    break;
                case "S":
                    TypeDesc = "Success";
                    break;
                default:
                    TypeDesc = string.Format("Unknown Type ({0})", MsgBalloon.MsgType.ToUpper());
                    break;
            }
            sb.AppendLine(TypeDesc.ToUpper());
            foreach (string Msg in MsgBalloon.MsgList)
            {
                sb.AppendLine(Msg);
            }

            return MsgBalloon.ToString();
        }
        public static MessageBalloonNotification DisplayToWeb(List<SAPMessage> SAPMessageList)
        {
            MessageBalloonNotification MsgBalloon = new MessageBalloonNotification();
            MsgBalloon.MsgType = MessageBalloonNotification.SUCCESS;

            if (SAPMessageList == null || SAPMessageList.Count == 0)
            {
                MsgBalloon.MsgType = MessageBalloonNotification.ERROR;
                MsgBalloon.MsgList.Add("Empty Message");
            }
            else
            {
                string TypeDesc;
                string SumRespType = MessageBalloonNotification.SUCCESS;
                foreach (SAPMessage SingleSAPMessage in SAPMessageList)
                {
                    // Find Summarize Type
                    switch (SingleSAPMessage.Type.ToUpper())
                    {
                        case "A":
                        case "E":
                            SumRespType = MessageBalloonNotification.ERROR;
                            break;
                        case "W":
                            if (SumRespType == MessageBalloonNotification.SUCCESS)
                                SumRespType = MessageBalloonNotification.WARNING;
                            break;
                        case "S":
                        case "I":
                        case "":
                        default:
                            break;
                    }

                    // Find Type Desc
                    switch (SingleSAPMessage.Type.ToUpper())
                    {
                        case "A":
                            TypeDesc = "Abort";
                            break;
                        case "I":
                            TypeDesc = "Info";
                            break;
                        case "W":
                            TypeDesc = "Warning";
                            break;
                        case "E":
                            TypeDesc = "Error";
                            break;
                        case "S":
                            TypeDesc = "Success";
                            break;
                        default:
                            TypeDesc = string.Format("Unknown Type ({0})", SingleSAPMessage.Type.ToUpper());
                            break;
                    }
                    MsgBalloon.MsgList.Add(string.Format("{0} : {1}", TypeDesc, SingleSAPMessage.Message));
                }
                MsgBalloon.MsgType = SumRespType;
            }

            return MsgBalloon;
        }
        public static bool IsSuccess(List<SAPMessage> SAPMessageList)
        {
            bool Result = true;
            if (SAPMessageList == null || SAPMessageList.Count == 0)
            {
                Result = true;
            }
            else
            {
                foreach (SAPMessage SingleSAPMessage in SAPMessageList)
                {
                    // Find Summarize Type
                    if (Result)
                    {
                        switch (SingleSAPMessage.Type)
                        {
                            case "A":
                            case "E":
                                Result = false;
                                break;
                            case "W":
                            case "S":
                            case "I":
                            case "":
                                Result = true;
                                break;
                            default:
                                Result = false;
                                break;
                        }
                    }
                }
            }

            return Result; ;
        }
    }
}
