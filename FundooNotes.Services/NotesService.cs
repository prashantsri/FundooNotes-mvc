using FundooNotesData.Data.Infrastructure;
using FundooNotesData.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FundooNotes.Services
{
    public class NotesService
    {


        public List<tblLabel> label1 { get; set; }
        public List<tblLabelNotes> label2 { get; set; }
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        public async Task<int> AddNotes(tblNotes objtbl)
        {
            int result = 0;
            int status = 0;
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

                // Add collaborator
                if (objtbl.Mode == 5)
                {

                    AccountService accountService = new AccountService();
                    string ShareWth = string.Empty;

                    // Get the Email to whome want to share from dialog text enter.
                    string[] email = objtbl.share.Split();
                    ShareWth = objtbl.share.Split().Last();

                    // Get the data of whome want to share.
                    ApplicationUser ShareWithdata = await accountService.GetUser(ShareWth, 1);

                    // if Email is not valid
                    if (ShareWithdata == null)
                    {
                        // return result = "Email not register";
                    }


                    if (objtbl.owner == null)
                    {
                        ApplicationUser ownerData = await accountService.GetUser(objtbl.UserID, 2);

                        objtbl.owner = ownerData.Email;
                    }

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
                    obj.Label = objtbl.Label;
                    obj.owner = objtbl.owner;
                    obj.share = objtbl.share;

                    //  applicationDbContext.tblNote.Add(obj);
                    status = await applicationDbContext.SaveChangesAsync();

                    // Add note to Share with User.
                    if (status != 0)
                    {

                        obj.UserID = ShareWithdata.Id;
                        obj.share = ShareWth;

                        applicationDbContext.tblNotes.Add(obj);
                        status = await applicationDbContext.SaveChangesAsync();

                        //   return "Shared Successfully";

                    }

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
            foreach (var row in applicationDbContext.tblNotes)
            {
                list.Add(row);
            }
            return list;
        }

        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }


        public List<tblNotes> GetNotes(string Userid)
        {
            var list = new List<tblNotes>();
            var sharelist = new List<tblCollaborator>();
            tblNotes tblNote = new tblNotes();

            var Notesdata = from t in applicationDbContext.tblNotes where t.UserID == Userid select t;
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var newdata = from t in applicationDbContext.tblCollaborators
                          where t.SharID == user.Email
                          select t;

            foreach (tblCollaborator data in newdata)
            {
                sharelist.Add(data);
            }
            for (int i = 0; i < sharelist.Count(); i++)
            {
                int id = sharelist[i].NotesID;
                var sharedata = from t in applicationDbContext.tblNotes
                                where t.ID == id
                                select t;
                foreach (tblNotes data in sharedata)
                {
                    list.Add(data);
                }

            }

            // Select All rows from tblnotes.
            foreach (tblNotes data in Notesdata)
            {
                list.Add(data);
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

        public async Task<int> AddCollaborator(tblCollaborator tblColl)
        {
            int result = 0;

            string ShareWth = string.Empty;
            ShareWth = tblColl.SharID;

            AccountService accountService = new AccountService();
            ApplicationUser ShareWithdata = await accountService.GetUser(ShareWth, 1);

            // if Email is not valid
            if (ShareWithdata == null)
            {
                //return result = "Email not register";
            }
            else
            {
                try
                {
                    if (tblColl.Mode == 1)
                    {
                        applicationDbContext.tblCollaborators.Add(tblColl);
                        result = await applicationDbContext.SaveChangesAsync();
                        return result;
                    }
                    if (tblColl.Mode == 2)
                    {
                        //tblLabelNotes obj = applicationDbContext.tblLabelNotes.Where(i => i.LabelID == abc && i.NotesID == objtbl.ID).FirstOrDefault();
                        tblCollaborator obj = applicationDbContext.tblCollaborators.Where<tblCollaborator>(t => t.NotesID == tblColl.NotesID && t.OwnerID == tblColl.OwnerID && t.SharID == tblColl.SharID).First();
                        applicationDbContext.tblCollaborators.Remove(obj);
                        result = await applicationDbContext.SaveChangesAsync();
                        return result;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public List<tblLabel> GetLabel(string userid)
        {
            var list = new List<tblLabel>();

            var validlist = from a in applicationDbContext.tblLabels
                            where a.UserID == userid
                            select a;
            foreach (var row in validlist)
            {
                list.Add(row);
            }
            return list;
        }



        public async Task<int> AddLabel(tblLabel tblLabel)
        {
            int result = 0;
            try
            {
                // Add new label.
                if (tblLabel.Mode == 1)
                {
                    applicationDbContext.tblLabels.Add(tblLabel);
                    result = await applicationDbContext.SaveChangesAsync();
                    return result;
                }

                if (tblLabel.Mode == 2)
                {
                    tblLabel obj = applicationDbContext.tblLabels.Where<tblLabel>(t => t.ID == tblLabel.ID).First();

                    obj.Label = tblLabel.Label;
                    obj.ID = tblLabel.ID;
                    result = await applicationDbContext.SaveChangesAsync();
                    return result;

                }
                if (tblLabel.Mode == 3)
                {
                    tblLabel obj = applicationDbContext.tblLabels.Where<tblLabel>(t => t.ID == tblLabel.ID).First();

                    applicationDbContext.tblLabels.Remove(obj);
                    result = await applicationDbContext.SaveChangesAsync();
                    return result;
                }

            }
            catch (Exception ex)
            {

                ex.ToString();
            }
            return result;
        }



        public async Task<int> addnotelabel(tblNotes objtbl)
        {
            int result = 0;
            // Add new label.
            var listlabels = new List<int>();
            var labelnames = new List<string>();
            if (objtbl.Mode == 1)
            {
                try
                {
                    var labels = objtbl.Label.Split(',');

                    foreach (string data in labels)
                    {
                        labelnames.Add(data);
                    }

                    foreach (string item in labelnames)
                    {
                        var iddata = from a in applicationDbContext.tblLabels
                                     where a.Label == item
                                     select a;
                        foreach (tblLabel itemdata in iddata)
                        {
                            listlabels.Add(itemdata.ID);

                        }
                    }

                    for (int i = 0; i < listlabels.Count(); i++)
                    {
                        string cn = listlabels[i].ToString();
                        var check = from a in applicationDbContext.tblLabelNotes
                                    where a.NotesID == objtbl.ID && a.LabelID == cn
                                    select a;

                        if (check.Count() == 0)
                        {
                            tblLabelNotes label = new tblLabelNotes();

                            label.LabelID = cn;
                            label.NotesID = objtbl.ID;
                            label.UserID = objtbl.UserID;
                            applicationDbContext.tblLabelNotes.Add(label);
                            result = await applicationDbContext.SaveChangesAsync();

                        }
                    }
                }
                catch (Exception ex)
                {

                    ex.ToString();
                }

                return result;
            }

            if (objtbl.Mode == 2)
            {
                var iddata = from a in applicationDbContext.tblLabels
                             where a.Label == objtbl.Label
                             select a;
                foreach (tblLabel itemdata in iddata)
                {
                    listlabels.Add(itemdata.ID);
                }

                var abc = listlabels[0].ToString();
                tblLabelNotes obj = applicationDbContext.tblLabelNotes.Where(i => i.LabelID == abc && i.NotesID == objtbl.ID).FirstOrDefault();
                applicationDbContext.tblLabelNotes.Remove(obj);

                result = await applicationDbContext.SaveChangesAsync();
            }

            if (objtbl.Mode == 3)
            {
                try
                {
                    var latestid = from a in applicationDbContext.tblNotes
                                   orderby a.ID descending
                                   select a.ID;
                    var id = latestid.First();

                    var newrecord = from a in applicationDbContext.tblLabelNotes
                                    where a.UserID == objtbl.UserID && a.NotesID == objtbl.ID
                                    select a;
                    var list = new List<string>();
                    foreach(tblLabelNotes tbl in newrecord)
                    {
                        list.Add(tbl.LabelID);
                    }

                    for (int i = 0; i < list.Count(); i++)
                    {
                        tblLabelNotes label = new tblLabelNotes();

                        label.LabelID = list[i] ;
                        label.NotesID = id;
                        label.UserID = objtbl.UserID;
                        applicationDbContext.tblLabelNotes.Add(label);
                        result = await applicationDbContext.SaveChangesAsync();
                    }



                }
                catch (Exception ex)
                {

                    ex.ToString();
                }

                return result;
            }
            return result;

        }

        public List<tblNotes> Displaylabel(object label, object userId)
        {
            var listlabels = new List<int>();
            var list = new List<tblNotes>();

            tblNotes tblNote = new tblNotes();
            string lablestr = "";
            var iddata = from a in applicationDbContext.tblLabels
                         where a.Label == label.ToString()
                         select a;
            foreach (tblLabel itemdata in iddata)
            {
                lablestr = itemdata.ID.ToString();
            }

            var Notesdata = (from t in applicationDbContext.tblNotes
                             join tb in applicationDbContext.tblLabelNotes on t.ID equals tb.NotesID

                             where t.UserID == userId && tb.LabelID == lablestr
                             select t).ToList();

            // Select All rows from tblnotes.
            foreach (tblNotes data in Notesdata)
            {
                list.Add(data);
            }
            return list;
        }

        public Tuple<List<tblLabel>, List<tblLabelNotes>, List<tblCollaborator>> labelshow(tblLabelNotes objtbl, string userid)
        {
            int result = 0;
            //List<tblLabel> response =new List<tblLabel>();


            var lbl = new List<tblLabel>();
            var lblname = new List<tblLabelNotes>();
            var Collaborator = new List<tblCollaborator>();

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //current user check in share colum tblCollaborators tbl 
            var share = from t in applicationDbContext.tblCollaborators
                        where t.SharID == user.Email
                        select t;

            var sharelist = new List<tblCollaborator>();
            foreach (tblCollaborator data1 in share)
            {
                sharelist.Add(data1);
            }
            var temp = new List<tblNotes>();

            for (int i = 0; i < sharelist.Count(); i++)
            {
                int id = sharelist[i].NotesID;
                var sharedata = from t in applicationDbContext.tblNotes
                                where t.ID == id
                                select t;
                foreach (tblNotes data12 in sharedata)
                {
                    temp.Add(data12);

                }

            }

            for (int i = 0; i < temp.Count(); i++)
            {
                string userid1 = temp[i].UserID;
                int nodeid = temp[i].ID;
                var data1 = from a in applicationDbContext.tblLabelNotes
                            where a.UserID == userid1 && a.NotesID == nodeid
                            select a;

                foreach (tblLabelNotes itemdata in data1)
                {
                    lblname.Add(itemdata);
                }

                var newdata12 = from a in applicationDbContext.tblCollaborators
                                where a.UserID == userid1 && a.NotesID == nodeid && a.SharID == user.Email
                                select a;
                foreach (tblCollaborator itm in newdata12)
                {
                    //change email for share
                    var own = itm.OwnerID;
                    var sh = itm.SharID;

                    itm.SharID = own;
                    itm.OwnerID = sh;
                    Collaborator.Add(itm);
                }

            }




            var data = from a in applicationDbContext.tblLabelNotes
                       where a.UserID == userid
                       select a;
            foreach (tblLabelNotes itemdata in data)
            {
                lblname.Add(itemdata);
            }


            for (int i = 0; i < lblname.Count; i++)
            {
                var id = int.Parse(lblname[i].LabelID.ToString());
                var newdata = from a in applicationDbContext.tblLabels
                              where a.ID == id
                              select a;

                foreach (tblLabel itemdata in newdata)
                {
                    lbl.Add(itemdata);

                }

            }





            var newdata1 = from a in applicationDbContext.tblCollaborators
                           where a.UserID == userid
                           select a;
            foreach (tblCollaborator itm in newdata1)
            {
                Collaborator.Add(itm);
            }

            return Tuple.Create(lbl, lblname, Collaborator);




        }



        public List<tblCollaborator> Coll(tblCollaborator objtbl)
        {
            var Collaborator = new List<tblCollaborator>();

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var data = from a in applicationDbContext.tblCollaborators
                       where a.SharID == user.Email && a.NotesID == objtbl.NotesID
                       select a;
            foreach (tblCollaborator col in data)
            {
                Collaborator.Add(col);
            }
            //if (Collaborator.Count() == 0)
            //{
            var data1 = from a in applicationDbContext.tblCollaborators
                        where a.OwnerID == user.Email && a.NotesID == objtbl.NotesID
                        select a;
            foreach (tblCollaborator col in data1)
            {
                Collaborator.Add(col);
            }
            //}
            return Collaborator;
        }

    }
}
