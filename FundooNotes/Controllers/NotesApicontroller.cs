using FundooNotes.Services;
using FundooNotesData.Data.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace FundooNotes.Controllers
{
    public class NotesApicontroller : ApiController
    {
        //NoteRepository _noteRepository = new NoteRepository();

        NotesService noteService = new NotesService();

        public async Task<int> AddNotes(tblNotes model)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = await noteService.AddNotes(model);
            }
            return i;
        }

        //public async Task<List<tblNotes>> GetNotes() // getvalue
        //{
        //    var list = new List<tblNotes>();
        //    list = _noteRepository.GetNotes();
        //    return list;
        //}
        public List<tblNotes> GetNotes(string userid)
        {

            var list = new List<tblNotes>();
            list = noteService.GetNotes(userid);
            return list;
        }

        public async Task<int> UpdatePin(tblNotes model)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = await noteService.UpdatePin(model);
            }
            return i;
        }

        public async Task<int> UpdateColor(tblNotes model)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = await noteService.UpdateColor(model);
            }
            return i;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/UploadImage")]
  
        public async Task<string> UploadImage()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Images");

            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            List<string> files = new List<string>();

            await Request.Content.ReadAsMultipartAsync(provider);

            var data = provider.FileData.Count;

            var Title = provider.FormData["Title"];
            var Content = provider.FormData["Content"];
            var ColorCode = provider.FormData["ColorCode"];
            var ID = provider.FormData["ID"];
            var IsPin = provider.FormData["IsPin"];
            string Reminder = provider.FormData["Reminder"];
            var IsArchive = provider.FormData["IsArchive"];
            var IsTrash = provider.FormData["IsTrash"];
            var owner = provider.FormData["owner"];
            var Label = provider.FormData["Label"];
            var share = provider.FormData["share"];
            var UserID = provider.FormData["UserID"];
            tblNotes model = new tblNotes();
            var url = RequestContext.Url.Request.RequestUri.Authority;

            foreach (MultipartFileData file in provider.FileData)
            {
                files.Add(Path.GetFileName(file.LocalFileName));

                var fileLocation = file.LocalFileName;
                var split = fileLocation.Split('\\');
                var length = split.Length;
                var filename = split[length - 1];

                model.Title = Title;
                model.Content = Content;
                model.ColorCode = ColorCode;
                model.ImageUrl = "https://" + url + "/Images/" + filename;
                model.ID = Convert.ToInt16(ID);
                model.Mode = 3;
                model.Reminder = Reminder;
                model.IsPin = Convert.ToInt16(IsPin);
                model.IsTrash = Convert.ToInt16(IsTrash);
                model.Label = Label;
                model.owner = owner;
                model.share = share;
                model.UserID = UserID;


            }

            NotesController notesController = new NotesController();
            await noteService.AddNotes(model);
            return model.ImageUrl;
        }

        public List<tblNotes> GetNotesexternal(string UserId)
        {

            var list = new List<tblNotes>();
            list = noteService.GetNotes(UserId);
            return list;
        }

        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            }
        }

        public List<tblLabel> GetLabel(string userid)
        {
            var list = new List<tblLabel>();
            list = noteService.GetLabel(userid);
            return list;
        }


        public async Task<int> AddLabel(tblLabel objtbllabel)
        {
            int result = 0;
            result = await noteService.AddLabel(objtbllabel);
            return result;
        }

        public async Task<int> addnotelabel(tblNotes objtbl)
        {
            int result = 0;
            result = await noteService.addnotelabel(objtbl);
            return result;
        }

        //public Tuple <List<tblLabel>,List<tblLabelNotes>>labelshow(tblLabelNotes objtbl,string userid)
        //{
        //    Tuple<List<tblLabel>,List<tblLabelNotes>> result = null;
        //    result = noteService.labelshow(objtbl, userid);
        //    return result;
        //}

        public async Task<string> labelshow(tblLabelNotes objtbl)
        {
            Tuple<List<tblLabel>, List<tblLabelNotes>,List<tblCollaborator>> result = null;
            result = noteService.labelshow(objtbl, objtbl.UserID);
            string JSONString;
            JSONString = JsonConvert.SerializeObject(result);

            return JSONString;
        }

        public async Task<string> Coll(tblCollaborator objtbl)
        {
            //Tuple<List<tblLabel>, List<tblLabelNotes>, List<tblCollaborator>> result = null;
            var list = new List<tblCollaborator>();
            list = noteService.Coll(objtbl);
            string JSONString;
            JSONString = JsonConvert.SerializeObject(list);

            return JSONString;
            //return "";
        }

        public List<tblNotes> Displaylabel(string label, string UserId)
        {
            var list = new List<tblNotes>();
            list = noteService.Displaylabel(label, UserId);
            return list;
        }

        public async Task<int> AddCollaboratorAsync(tblCollaborator objtbllabel)
        {
            int result = 0;
            result = await noteService.AddCollaborator(objtbllabel);
            return result;
        }

    }
}