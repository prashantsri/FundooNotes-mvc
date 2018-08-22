using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesData.Data.Models
{
   public class tblNotes
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ColorCode { get; set; }
        public string Reminder { get; set; }
        public int DisplayOrde { get; set; }
        public int IsPin { get; set; }
        public int IsArchive { get; set; }
        public int IsActive { get; set; }
        public int IsDelete { get; set; }
        public int IsTrash { get; set; }
        public string ImageUrl { get; set; }
        public int Mode { get; set; }      
        public string Label { get; set; }
        public string owner { get; set; }
        public string share { get; set; }
    }
}
