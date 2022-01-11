using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.Script.Serialization;
using DevExpress.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.ASPxPopupControl;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxCallback;

namespace Kairos.Library.ControlHelper
{
    public class MessageBalloonNotification
    {
        //HOW TO USE
        //create an object of this class, and then call one of the method (ConvertListOfStringToJSON, ConvertListOfIntToJSON, ConvertListOfFloatToJSON)
        // to show the message if you have LIST type variable -> // to show the message -> msgNotif.SendListToMsgBox(this.Page, this.GetType(), test, MessageBalloonNotification.SUCCESS);
        // to show the message if you have STRING type variable -> // to show the message -> msgNotif.SendListToMsgBox(this.Page, this.GetType(), test, MessageBalloonNotification.SUCCESS);
        //The different is only the type of variable named 'test' where the first one is List type and the second one is a String
        //If you still don't know how to use it, you can see the sample of the implementation at "C:\IP2P\IP2P\WebEksternal\Sources\Administration\WebForm1.aspx.cs" or "C:\IP2P\IP2P\WebInternal\Sources\Administration\WebForm1.aspx.cs"
        //Or you could ask me
        public const string SUCCESS = "<script type='text/javascript'>showMessageBox('S','success',";
        public const string ERROR = "<script type='text/javascript'>showMessageBox('E','error',";
        public const string INFO = "<script type='text/javascript'>showMessageBox('I','info',";
        public const string WARNING = "<script type='text/javascript'>showMessageBox('W','warning',";
        public const string END_PARAMETER = ");</script>";
        public const string CLOSE_MESSAGE = "<script type='text/javascript'>closeMessageBox();</script>";

        public string MsgType { get; set; }
        public List<string> MsgList { get; private set; }

        public MessageBalloonNotification()
        {
            MsgList = new List<string>();
        }

        public string ConvertListOfStringToJSON(List<string> args_list)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string myJSON = jss.Serialize(args_list);
            myJSON = myJSON.Replace("\"", "'");
            return myJSON;
        }
        public string ConvertListOfStringToString(List<string> args_list)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string myJSON = jss.Serialize(args_list);
            myJSON = myJSON.Replace("\"", "");
            myJSON = myJSON.Replace("[", "");
            myJSON = myJSON.Replace("]", "");
            myJSON = myJSON.Replace(",", "~");
            return myJSON;
        }
        public string ConvertListOfIntToJSON(List<int> args_list)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string myJSON = jss.Serialize(args_list);
            myJSON = myJSON.Replace("\"", "'");
            return myJSON;
        }
        public string ConvertListOfFloatToJSON(List<int> args_list)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string myJSON = jss.Serialize(args_list);
            myJSON = myJSON.Replace("\"", "'");
            return myJSON;
        }
        //seng only a string
        public void SendStringToMsgBox(Control page, object theObject, string arg_theMessage,string arg_msgType)
        {
            List<string> oneString = new List<string>();
            oneString.Add(arg_theMessage);
            string theMessage = ConvertListOfStringToJSON(oneString);
            string startScript = DefineMsgType(arg_msgType);
            ScriptManager.RegisterStartupScript(page.Page, theObject.GetType(), "tmp", startScript + theMessage + MessageBalloonNotification.END_PARAMETER, false);
        }
        //seng a list of string
        public void SendListToMsgBox(Control page, object theObject)
        {
            SendListToMsgBox(page, theObject, this.MsgList, this.MsgType);
        }
        public void SendListToMsgBox(Control page, object theObject, List<string> arg_listOfString, string arg_msgType)
        {
            string startScript = DefineMsgType(arg_msgType);
            string theMessage = ConvertListOfStringToJSON(arg_listOfString);
            ScriptManager.RegisterStartupScript(page.Page, theObject.GetType(), "tmp", startScript + theMessage + MessageBalloonNotification.END_PARAMETER, false);
        }
        public void SendJSONToMsgBox(Control page, object theObject, string arg_theMessage, string arg_msgType)
        {
            string startScript = DefineMsgType(arg_msgType);
            ScriptManager.RegisterStartupScript(page.Page, theObject.GetType(), "tmp", startScript + arg_theMessage + MessageBalloonNotification.END_PARAMETER, false);
        }

        public void DVXsendStringToMsgBox(object arg_sender, string arg_theMessage, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            ((ASPxGridView)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxGridView)arg_sender).JSProperties["cpTheMsg"] = arg_theMessage;
        }

        public void DVXsendStringToMsgBox_UploadControl(object arg_sender, string arg_theMessage, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            ((ASPxUploadControl)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxUploadControl)arg_sender).JSProperties["cpTheMsg"] = arg_theMessage;
        }
        public void DVXsendListToMsgBox(object arg_sender, List<string> arg_listOfString, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            string theMessage = ConvertListOfStringToString(arg_listOfString);
            ((ASPxGridView)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxGridView)arg_sender).JSProperties["cpTheMsg"] = theMessage;
        }

        public void DVXsendStringToMsgBox_PopUpControl(object arg_sender, string arg_theMessage, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            ((ASPxPopupControl)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxPopupControl)arg_sender).JSProperties["cpTheMsg"] = arg_theMessage;
        }

        public void DVXsendListToMsgBox_PopUpControl(object arg_sender, List<string> arg_listOfString, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            string theMessage = ConvertListOfStringToString(arg_listOfString);
            ((ASPxPopupControl)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxPopupControl)arg_sender).JSProperties["cpTheMsg"] = theMessage;
        }
        

        public void DVXsendStringToMsgBox_PageControl(object arg_sender, string arg_theMessage, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            ((ASPxPageControl)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxPageControl)arg_sender).JSProperties["cpTheMsg"] = arg_theMessage;
        }

        public void DVXsendListToMsgBox_PageControl(object arg_sender, List<string> arg_listOfString, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            string theMessage = ConvertListOfStringToString(arg_listOfString);
            ((ASPxPageControl)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxPageControl)arg_sender).JSProperties["cpTheMsg"] = theMessage;
        }


        /// <summary>
        /// Send string to alert on callback panel
        /// example:
        /// "test don't quote~please, use Comma"
        /// ~ as new line
        /// </summary>
        /// <param name="sender as object"></param>
        /// <param name="theMessage as string"></param>
        /// <param name="msgType as message balloon type"></param>
        ///
        public void DVXsendStringToMsgBox_CallbackPanel(object arg_sender, string arg_theMessage, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            ((ASPxCallbackPanel)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxCallbackPanel)arg_sender).JSProperties["cpTheMsg"] = arg_theMessage;
        }


        /// <summary>
        /// send list to alert on callback panel
        /// example:
        ///     List<string> = new { "test don't quote", "please, use Comma" }
        /// </summary>
        /// <param name="sender as object"></param>
        /// <param name="theMessage as list"></param>
        /// <param name="msgType as message balloon type"></param>
        ///
        public void SendListToMessage_CallbackPanel(object arg_sender, List<string> arg_theMessage, string arg_msgType)
        {
            string message = string.Empty;
            foreach (string item in arg_theMessage)
            {
                if (string.IsNullOrEmpty(message))
                    message = item;
                else
                    message = message + "~" + item;
            }

            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            ((ASPxCallbackPanel)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxCallbackPanel)arg_sender).JSProperties["cpTheMsg"] = message;
        }

        
        public void DVXsendListToMsgBox_CallbackPanel(object arg_sender, List<string> arg_listOfString, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            string theMessage = ConvertListOfStringToString(arg_listOfString);
            ((ASPxCallbackPanel)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxCallbackPanel)arg_sender).JSProperties["cpTheMsg"] = theMessage;
        }

        public void DVXsendStringToMsgBox_Callback(object arg_sender, string arg_theMessage, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            ((ASPxCallback)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxCallback)arg_sender).JSProperties["cpTheMsg"] = arg_theMessage;
        }

        public void DVXsendListToMsgBox_Callback(object arg_sender, List<string> arg_listOfString, string arg_msgType)
        {
            string typeOfMessage = DVXdefineMsgType(arg_msgType);
            string theMessage = ConvertListOfStringToString(arg_listOfString);
            ((ASPxCallback)arg_sender).JSProperties["cpTypeMsg"] = typeOfMessage;
            ((ASPxCallback)arg_sender).JSProperties["cpTheMsg"] = theMessage;
        }

        public void DVXsendListToMsgBox(object arg_sender)
        {
            DVXsendListToMsgBox(arg_sender, this.MsgList, this.MsgType);
        }
        public void DVXsendListToMsgBox_Callback(object arg_sender)
        {
            DVXsendListToMsgBox_Callback(arg_sender, this.MsgList, this.MsgType);
        }
        public void DVXsendListToMsgBox_CallbackPanel(object arg_sender)
        {
            DVXsendListToMsgBox_CallbackPanel(arg_sender, this.MsgList, this.MsgType);
        }
        public void SendListToMessage_CallbackPanel(object arg_sender)
        {
            SendListToMessage_CallbackPanel(arg_sender, this.MsgList, this.MsgType);
        }
        public void CloseMessageBox(Control page, object theObject)
        {
            ScriptManager.RegisterStartupScript(page.Page, theObject.GetType(), "tmp", MessageBalloonNotification.CLOSE_MESSAGE, false);
        }

        private string DefineMsgType(string arg_typeOfMsg)
        {
            string typeOfMsg=string.Empty;

            if (arg_typeOfMsg.Equals(MessageBalloonNotification.SUCCESS))
            {
                typeOfMsg = MessageBalloonNotification.SUCCESS;
            }
            else if (arg_typeOfMsg.Equals(MessageBalloonNotification.ERROR))
            {
                typeOfMsg = MessageBalloonNotification.ERROR;
            }
            else if (arg_typeOfMsg.Equals(MessageBalloonNotification.INFO))
            {
                typeOfMsg = MessageBalloonNotification.INFO;
            }
            else if (arg_typeOfMsg.Equals(MessageBalloonNotification.WARNING))
            {
                typeOfMsg = MessageBalloonNotification.WARNING;
            }

            return typeOfMsg;
        }

        private string DVXdefineMsgType(string arg_typeOfMsg)
        {
            string typeOfMsg=string.Empty;

            if (arg_typeOfMsg.Equals(MessageBalloonNotification.SUCCESS))
            {
                typeOfMsg = "S";
            }
            else if (arg_typeOfMsg.Equals(MessageBalloonNotification.ERROR))
            {
                typeOfMsg = "E";
            }
            else if (arg_typeOfMsg.Equals(MessageBalloonNotification.INFO))
            {
                typeOfMsg = "I";
            }
            else if (arg_typeOfMsg.Equals(MessageBalloonNotification.WARNING))
            {
                typeOfMsg = "W";
            }

            return typeOfMsg;
        }
        
    }
}
