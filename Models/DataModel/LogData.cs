//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class LogData
    {
        public int ID { get; set; }
        public string MachineName { get; set; }
        public System.DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Properties { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
