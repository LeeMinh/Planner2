//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Planner2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CommentTask
    {
        public int ID { get; set; }
        public Nullable<int> TaskID { get; set; }
        public string NOIDUNG { get; set; }
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string TieuDe { get; set; }
        public string MucDich { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string Note { get; set; }
        public string FileUpload { get; set; }
        public Nullable<int> Type { get; set; }
        public int ParentID { get; set; }
        public string Email { get; set; }
    }
}
