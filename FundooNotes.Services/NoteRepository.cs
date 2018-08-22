using FundooNotesData.Data.Infrastructure;
using FundooNotesData.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Services
{
    public class NoteRepository : INoteRepository
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        public async Task<int> AddNotes(tblNotes objtbl)
        {
            int result = 0;
            try
            {
                if (objtbl.Mode == 1)
                {
                    applicationDbContext.tblNotes.Add(objtbl);
                    result = await applicationDbContext.SaveChangesAsync();
                    return result;
                }
                //Delete Note Forever.
                if (objtbl.Mode == 2)
                {
                    tblNotes obj = applicationDbContext.tblNotes.Where<tblNotes>(t => t.ID == objtbl.ID).First();
                    applicationDbContext.tblNotes.Remove(obj);
                    result = await applicationDbContext.SaveChangesAsync();
                    return result;
                }

                // Update Note:Pinned note,delete note.
                if (objtbl.Mode == 3)
                {

                    tblNotes obj = applicationDbContext.tblNotes.Where<tblNotes>(t => t.ID == objtbl.ID).First();
                    obj.Title = objtbl.Title;
                    obj.ID = objtbl.ID;
                    obj.UserID = objtbl.UserID;
                    obj.Content = objtbl.Content;
                    obj.ColorCode = objtbl.ColorCode;
                    obj.IsPin = objtbl.IsPin;
                    obj.IsTrash = objtbl.IsTrash;
                    obj.IsArchive = objtbl.IsArchive;
                    obj.ID = objtbl.ID;
                    obj.Reminder = objtbl.Reminder;
                    obj.ImageUrl = objtbl.ImageUrl;

                    result = await applicationDbContext.SaveChangesAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return result;
        }

        public List<tblNotes> GetNotes()
        {
            var list = new List<tblNotes>();
            foreach(var row in applicationDbContext.tblNotes)
            {
                list.Add(row);
            }
            return list;
        }

        public async Task<int> UpdatePin(tblNotes model)
        {
            int i = 0;
            try
            {
                tblNotes tbl = applicationDbContext.tblNotes.Where<tblNotes>(a => a.ID == model.ID).First();
                if (model.IsPin == 1)
                {
                    tbl.IsPin = 0;
                }
                else
                {
                    tbl.IsPin = 1;
                }
                i = i = await applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return i;
        }


        public async Task<int> UpdateColor(tblNotes model)
        {
            int i = 0;
            try
            {
                tblNotes tbl = applicationDbContext.tblNotes.Where<tblNotes>(a => a.ID == model.ID).First();
                tbl.ColorCode = model.ColorCode;
                i = await applicationDbContext.SaveChangesAsync();
                return i;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return i;
        }

    }
}
