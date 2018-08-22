using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesData.Data.Models
{
    public class tblCollaborator
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int NotesID { get; set; }
        public string SharID { get; set; }
        public string OwnerID { get; set; }
        public int Mode { get; set; }

    }
}
