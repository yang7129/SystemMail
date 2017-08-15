using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyCardSystemMail
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "Ibase"。
    [ServiceContract]
    public interface Ibase
    { 
    }
    [DataContract]
    public class ReturnResult
    {
        [DataMember]
        public int ReturnMsgNo { get; set; }
        [DataMember]
        public string ReturnMsg { get; set; }
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public int LogSn { get; set; }
    }       
}
