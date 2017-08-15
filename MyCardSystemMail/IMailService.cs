using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyCardSystemMail
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IMailService"。
    [ServiceContract]
    public interface IMailService : Ibase
    {
        //[OperationContract(Name = "Mail")]
        //ReturnResult Mail(string serverHost, string addressFrom, string addressNameFrom, string addressTo, string addressNameTo, string subject, string body);
        [OperationContract]
        ReturnResult Mail(InputMail Value);
         
    }
    [DataContract]
    public class InputMail
    { 
        [DataMember]
        public string addressFrom { get; set; }
        [DataMember]
        public string addressNameFrom { get; set; }
        [DataMember]
        public string addressTo { get; set; }
        [DataMember]
        public string addressNameTo { get; set; }
        [DataMember]
        public string subject { get; set; }
        [DataMember]
        public string body { get; set; }
        [DataMember]
        public string filebyte { get; set; }
        [DataMember]
        public string fileName { get; set; } 
    }
     
} 