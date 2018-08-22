using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundooNotesData.Data.Models;

namespace FundooNotes.Services
{
    public interface INoteRepository
    {
        Task<int> AddNotes(tblNotes objtbl);       
    }
}
