using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PhysicianLocator.Controllers
{
    public class QuestionAnswerController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(HomeController));  //Declaring Log4Net

        LocatorContext context = new LocatorContext();
        // GET: QuestionAnswer
        [CheckSessionOut]
        public ActionResult Index()
        {

            List<QuestionAnswerViewModel> questionList = new List<QuestionAnswerViewModel>();
            var query_question = from question in context.DBContext_question select question;
            CommonFunction commonfunction = new CommonFunction();
            if (query_question != null)
            {
                int userid = Convert.ToInt32(Session["UserID"]);
                string str = Request.Params["submitbutton1"];
                if (str == "Search")
                {

                    //var query = (from question in context.DBContext_question
                    //            join register in context.DBContext_register
                    //            on question.QuestionCreatedBy equals register.ID
                    //             select new
                    //             {
                    //                 questionRegistrationQuestion = question.Question,
                    //                 questionRegistrationUserName = register.UserName
                    //             }).ToList();

                    //foreach (var item in query) //retrieve each item and assign to model
                    //{
                    //    questionList.Add(new QuestionRegistrationViewModel()
                    //    {
                    //        QuestionRegistrationQuestion = item.questionRegistrationQuestion,
                    //        QuestionRegistrationUserName = item.questionRegistrationUserName
                    //    });
                    //}
                    //var tempSearch = Request["required"];
                    var tempSearch = Request["Textbox"];
                    string s = Convert.ToString(tempSearch);
                    //if (tempSearch != null)
                    //{
                    //    if (tempSearch != string.Empty)
                    //    {
                    //        tempSearch= tempSearch+1;
                    //    }

                    //}
                    if (tempSearch != null && tempSearch != string.Empty)
                    {
                        string[] a = tempSearch.Split(',');
                        for (int i = 0; i <= a.Length - 1; i++)
                        {
                            //questionList = context.DBContext_question.Where(p => p.Question.Contains(l) || tempSearch == null).ToList();
                            var meds = (from question in context.DBContext_question
                                        join register in context.DBContext_register
                                        on question.CreatedBy equals register.UserId
                                        where (a.Any(name => name.Contains(question.Question) || question.Question.Contains(name)) && question.IsActive==true )
                                        select new QuestionAnswerViewModel
                                        {
                                            QuestionAnswerId = question.QuestionId,
                                            QuestionAnswerQuestion = question.Question,
                                            QuestionAnswerUserName = register.UserName,
                                            QuestionAnswerCreatedDate = question.CreatedOn,
                                            QuestionAnswerUserID = register.UserId
                                        }).ToList();
                            if (meds != null)
                            {
                                ViewBag.Result = meds.ToList();
                            }

                            //questionList = meds.ToList();
                            foreach (var item in meds) //retrieve each item and assign to model
                            {


                                DocumentViewModel query_document = (from document in context.DBContext_document where document.CreatedBy == item.QuestionAnswerUserID && document.PrimaryObjectId == "tblUsers" select document).FirstOrDefault();
                                if (query_document != null)
                                {
                                    //ViewBag.contentpath = query_document.DocumentPath;
                                    questionList.Add(new QuestionAnswerViewModel()
                                    {
                                        QuestionAnswerId = item.QuestionAnswerId,
                                        QuestionAnswerQuestion = item.QuestionAnswerQuestion,
                                        QuestionAnswerUserName = item.QuestionAnswerUserName,
                                        date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                                        QuestionAnswerUserID = item.QuestionAnswerUserID,
                                        QuestionAnswerDocumentPath = query_document.DocumentPath
                                    });
                                }
                                else
                                {
                                    //ViewBag.contentpath = "~/Images/2/vee.png";

                                    questionList.Add(new QuestionAnswerViewModel()
                                    {
                                        QuestionAnswerId = item.QuestionAnswerId,
                                        QuestionAnswerQuestion = item.QuestionAnswerQuestion,
                                        QuestionAnswerUserName = item.QuestionAnswerUserName,
                                        date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                                        QuestionAnswerUserID = item.QuestionAnswerUserID,
                                        QuestionAnswerDocumentPath = "~/Images/2/vee.png"
                                    });
                                }

                            }
                        }
                        return View(questionList);

                    }
                    else
                    {
                        var ddd = (from question in context.DBContext_question
                                   join register in context.DBContext_register
                                   on question.CreatedBy equals register.UserId
                                   where (tempSearch == null && question.IsActive == true)/* && register.UserId == userid*/
                                   select new QuestionAnswerViewModel
                                   {
                                       QuestionAnswerId = question.QuestionId,
                                       QuestionAnswerQuestion = question.Question,
                                       QuestionAnswerUserName = register.UserName,
                                       QuestionAnswerCreatedDate = question.CreatedOn,
                                       QuestionAnswerUserID = register.UserId
                                   }).ToList();
                        ViewBag.Result = "";
                        foreach (var item in ddd) //retrieve each item and assign to model
                        {
                            DocumentViewModel query_document = (from document in context.DBContext_document where document.CreatedBy == item.QuestionAnswerUserID && document.PrimaryObjectId == "tblUsers" select document).FirstOrDefault();
                            if (query_document != null)
                            {
                                //ViewBag.contentpath = query_document.DocumentPath;
                                questionList.Add(new QuestionAnswerViewModel()
                                {
                                    QuestionAnswerId = item.QuestionAnswerId,
                                    QuestionAnswerQuestion = item.QuestionAnswerQuestion,
                                    QuestionAnswerUserName = item.QuestionAnswerUserName,
                                    date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                                    QuestionAnswerUserID = item.QuestionAnswerUserID,
                                    QuestionAnswerDocumentPath = query_document.DocumentPath
                                });
                            }
                            else
                            {
                                //ViewBag.contentpath = "~/Images/2/vee.png";

                                questionList.Add(new QuestionAnswerViewModel()
                                {
                                    QuestionAnswerId = item.QuestionAnswerId,
                                    QuestionAnswerQuestion = item.QuestionAnswerQuestion,
                                    QuestionAnswerUserName = item.QuestionAnswerUserName,
                                    date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                                    QuestionAnswerUserID = item.QuestionAnswerUserID,
                                    QuestionAnswerDocumentPath = "~/Images/2/vee.png"
                                });
                            }

                        }
                        questionList = ddd.ToList();
                        return View(questionList);

                    }
                    //return View(questionList.ToList());
                }
                else
                {
                    var query = (from question in context.DBContext_question
                                 join register in context.DBContext_register
                                 on question.CreatedBy equals register.UserId
                                 where question.IsActive==true
                                 orderby question.QuestionId descending
                                 select new QuestionAnswerViewModel
                                 {
                                     QuestionAnswerId = question.QuestionId,
                                     QuestionAnswerQuestion = question.Question,
                                     QuestionAnswerUserName = register.UserName,
                                     QuestionAnswerCreatedDate = question.CreatedOn,
                                     QuestionAnswerUserID = register.UserId
                                 }).ToList();
                   

                    foreach (var item in query) //retrieve each item and assign to model
                    {

                        DocumentViewModel query_document = (from document in context.DBContext_document where document.CreatedBy == item.QuestionAnswerUserID && document.PrimaryObjectId == "tblUsers" select document).FirstOrDefault();
                        if (query_document != null)
                        {
                            //ViewBag.contentpath = query_document.DocumentPath;
                            questionList.Add(new QuestionAnswerViewModel()
                            {
                                QuestionAnswerId = item.QuestionAnswerId,
                                QuestionAnswerQuestion = item.QuestionAnswerQuestion,
                                QuestionAnswerUserName = item.QuestionAnswerUserName,
                                date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                                QuestionAnswerUserID = item.QuestionAnswerUserID,
                                QuestionAnswerDocumentPath = query_document.DocumentPath
                            });
                        }
                        else
                        {
                            //ViewBag.contentpath = "~/Images/2/vee.png";

                            questionList.Add(new QuestionAnswerViewModel()
                            {
                                QuestionAnswerId = item.QuestionAnswerId,
                                QuestionAnswerQuestion = item.QuestionAnswerQuestion,
                                QuestionAnswerUserName = item.QuestionAnswerUserName,
                                date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                                QuestionAnswerUserID = item.QuestionAnswerUserID,
                                QuestionAnswerDocumentPath = "~/Images/2/vee.png"
                            });
                        }
                    }
                }
            }
            return View(questionList);

        }
        [CheckSessionOut]
        public ActionResult QuestionAndAnswer(int id)
        {
            CommonFunction commonfunction = new CommonFunction();
            List<QuestionAnswerViewModel> questionList = new List<QuestionAnswerViewModel>();
            int userid = Convert.ToInt32(Session["UserID"]);

            var query_question = (from question in context.DBContext_question
                                  join register in context.DBContext_register
                                  on question.CreatedBy equals register.UserId
                                  where (question.QuestionId == id && question.IsActive == true)
                                  orderby question.QuestionId descending
                                  select new QuestionAnswerViewModel
                                  {
                                      QuestionAnswerId = question.QuestionId,
                                      QuestionAnswerQuestion = question.Question,
                                      QuestionAnswerUserName = register.UserName,
                                      QuestionAnswerCreatedDate = register.CreatedOn,
                                      QuestionAnswerDescription = question.QuestionDescription
                                  }).ToList().Take(5);

            foreach (var item in query_question) //retrieve each item and assign to model
            {
               
                    //ViewBag.contentpath = query_document.DocumentPath;
                    questionList.Add(new QuestionAnswerViewModel()
                    {
                        QuestionAnswerId = item.QuestionAnswerId,
                        QuestionAnswerQuestion = item.QuestionAnswerQuestion,
                        QuestionAnswerUserName = item.QuestionAnswerUserName,
                        date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                        QuestionAnswerUserID = item.QuestionAnswerUserID,                      
                    });
                
            }
                
            ViewBag.Question = questionList;
            List<QuestionAnswerViewModel> answerList = new List<QuestionAnswerViewModel>();
            var query_answer = (from question in context.DBContext_question
                                join answer in context.DBContext_answer
                                on question.QuestionId equals answer.QuestionId
                                join register in context.DBContext_register
                                on answer.CreatedBy equals register.UserId
                                where (question.QuestionId == id && answer.IsActive == true)

                                select new QuestionAnswerViewModel
                                {
                                    QuestionAnswerId = question.QuestionId,
                                    QuestionAnswerAnswerId = answer.AnswerId,
                                    QuestionAnswerAnswer = answer.Answer,
                                    QuestionAnswerUserName = register.UserName,
                                    QuestionAnswerCreatedDate = answer.CreatedOn,

                                }).ToList();

            List<commentview> commentList = new List<commentview>();


            foreach (var item in query_answer) //retrieve each item and assign to model
            {
                answerList.Add(new QuestionAnswerViewModel()
                {
                    QuestionAnswerId = item.QuestionAnswerId,
                    QuestionAnswerAnswerId = item.QuestionAnswerAnswerId,
                    QuestionAnswerAnswer = item.QuestionAnswerAnswer,
                    QuestionAnswerUserName = item.QuestionAnswerUserName,
                    //QuestionAnswerCreatedDate = item.QuestionAnswerCreatedDate,
                    date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                });



                var query_comment = (from question in context.DBContext_question
                                     join answer in context.DBContext_answer
                                     on question.QuestionId equals answer.QuestionId
                                     join comment in context.DBContext_comment
                                     on answer.AnswerId equals comment.AnswerId
                                     join register in context.DBContext_register
                                     on answer.CreatedBy equals register.UserId
                                     where (comment.AnswerId == item.QuestionAnswerAnswerId && comment.IsActive == true)
                                     select new commentview
                                     {
                                         answerid = comment.AnswerId,
                                         comm = comment.Comment,
                                         ParentCommentId = comment.ParentCommentId,
                                         CreatedBy = register.UserName,
                                         CreatedOn = comment.CreatedOn,
                                         CommentId = comment.CommentId
                                     }).ToList();
                foreach (var item1 in query_comment) //retrieve each item and assign to model
                {
                    if (item1.ParentCommentId == 0 && item1.answerid == item.QuestionAnswerAnswerId)
                    {
                        commentList.Add(new commentview()
                        {
                            comm = item1.comm,
                            answerid = item1.answerid,
                            ParentCommentId = item1.ParentCommentId,
                            CreatedBy = item1.CreatedBy,
                            date = commonfunction.getTimeAgo(item.QuestionAnswerCreatedDate),
                            CreatedOn = item1.CreatedOn,
                            CommentId = item1.CommentId
                        });
                        ViewBag.Comment = commentList;

                        commentList = recursive_comment(item1.CommentId, item1.answerid, commentList);
                    }

                }
                ViewBag.Answer = answerList;




            }
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuestionAndAnswer(AnswerViewModel model, int id)
        {
            int lastanswerid;
            int lastuserid;
            var query_user = (from user in context.DBContext_register select user).Any();
            if (!query_user)
            {
                lastuserid = 1;
            }
            else
            {
                lastuserid = context.DBContext_question.Max(item => item.QuestionId + 1);
            }
            var query_answer = (from answer in context.DBContext_answer select answer).Any();

            if (!query_answer)
            {
                lastanswerid = 1;
            }
            else
            {
                lastanswerid = context.DBContext_answer.Max(item => item.AnswerId + 1);
            }

            int userid = Convert.ToInt32(Session["UserID"]);
            var tempCreatedDate = DateTime.Now;
            DateTime createdDate = Convert.ToDateTime(tempCreatedDate);
            model.CreatedOn = createdDate;
            var tempModifiedDate = DateTime.Now;
            DateTime modifiedDate = Convert.ToDateTime(tempModifiedDate);
            model.LastModifiedOn = modifiedDate;
            var tempEditor = Request["cat"];
            int length = tempEditor.Length;
            model.Answer = tempEditor;
            int tempcreatedby = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = tempcreatedby;
            QuestionViewModel question_query = (from question in context.DBContext_question where question.QuestionId == id select question).SingleOrDefault();

            model.QuestionId = question_query.QuestionId;
            if (length >= 1)
            {


                if (ModelState.IsValid)
                {
                    using (LocatorContext context = new LocatorContext())
                    {
                        //var user = context.DBContext_question.Where(u => u.Question == model.Question).FirstOrDefault();
                        //if (user == null)
                        //{
                        var record = new AnswerViewModel()
                        {
                            Answer = model.Answer,
                            ParentAnswerId = 1,
                            AcceptingComments = "abc",
                            QuestionId = model.QuestionId,
                            SearchTags = "abc",
                            IsThisTopAnswer = true,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,
                        };
                        context.DBContext_answer.Add(model);
                        context.SaveChanges();
                        ViewBag.message = "successfully";
                        TempData["Data"] = "Successful";


                        DocumentViewModel document_model = new DocumentViewModel();

                        string dirarr = Server.MapPath("~/Temp/" + tempcreatedby + "/Answers/" + lastanswerid);
                        if (Directory.Exists(dirarr))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Images/" + tempcreatedby));

                            Directory.CreateDirectory(Server.MapPath("~/Images/" + tempcreatedby + "/Answers"));
                            var source = Server.MapPath("~/Temp/" + tempcreatedby + "/Answers/" + lastanswerid);
                            var destination = Server.MapPath("~/Images/" + tempcreatedby + "/Answers/" + lastanswerid);

                            System.IO.Directory.Move(source, destination);
                            string[] filesarr = Directory.GetFiles(destination);
                            for (int i = 0; i < filesarr.Length; i++)
                            {
                                document_model.DocumentName = filesarr[i];
                                var physicalPath = Path.Combine(Server.MapPath("~/Temp/" + tempcreatedby + "/Answers/" + lastanswerid), filesarr[i]);

                                document_model.DocumentPath = physicalPath;
                                var table = "Answers";
                                document_model.PrimaryObjectId = table;
                                document_model.PrimaryKeyId = lastanswerid;
                                var document_record = new DocumentViewModel()
                                {
                                    DocumentName = document_model.DocumentName,
                                    DocumentPath = document_model.DocumentPath,
                                    PrimaryKeyId = document_model.PrimaryKeyId,
                                    IsActive = model.IsActive,
                                    IsDeleted = false,
                                    CreatedBy = 0,
                                    CreatedOn = DateTime.Now,
                                    LastModifiedBy = 0,
                                    LastModifiedOn = DateTime.Now,
                                };
                                context.DBContext_document.Add(document_model);
                                context.SaveChanges();
                            }
                        }
                        //return View("../Shared/Error");
                        return RedirectToAction("QuestionAndAnswer", "QuestionAnswer");
                    }
                    //    else
                    //    {
                    //        ModelState.AddModelError("", "Already asked");
                    //        TempData["Data"] = "Already asked";
                    //        return View("../Shared/Error");
                    //    }
                    //}

                }
                ModelState.Clear();
            }
            return RedirectToAction("QuestionAndAnswer", "QuestionAnswer");

        }
        [CheckSessionOut]
        public ActionResult CommentInsert(int id)
        {

            List<QuestionAnswerViewModel> questionList = new List<QuestionAnswerViewModel>();
            int userid = Convert.ToInt32(Session["UserID"]);
            var query_question = (from question in context.DBContext_question
                                  join answer in context.DBContext_answer
                                  on question.QuestionId equals answer.QuestionId
                                  where answer.AnswerId == id

                                  select new QuestionAnswerViewModel
                                  {
                                      QuestionAnswerId = question.QuestionId,
                                      QuestionAnswerAnswerId = answer.AnswerId,
                                      QuestionAnswerAnswer = answer.Answer,
                                      //QuestionAnswerUserName = answer.UserName,
                                      QuestionAnswerCreatedDate = answer.CreatedOn,
                                      //QuestionAnswerDescription = question.QuestionDescription
                                  })/*.ToList().Take(5)*/;

            questionList = query_question.ToList();
            ViewBag.Answer = questionList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentInsert(CommentViewModel model, int id)
        {
            int lastanswerid;
            int lastuserid;
            var query_user = (from user in context.DBContext_register select user).Any();
            if (!query_user)
            {
                lastuserid = 1;
            }
            else
            {
                lastuserid = context.DBContext_question.Max(item => item.QuestionId + 1);
            }
            var query_answer = (from answer in context.DBContext_answer select answer).Any();

            if (!query_answer)
            {
                lastanswerid = 1;
            }
            else
            {
                lastanswerid = context.DBContext_answer.Max(item => item.AnswerId + 1);
            }

            int userid = Convert.ToInt32(Session["UserID"]);
            var tempCreatedDate = DateTime.Now;
            DateTime createdDate = Convert.ToDateTime(tempCreatedDate);
            model.CreatedOn = createdDate;
            var tempModifiedDate = DateTime.Now;
            DateTime modifiedDate = Convert.ToDateTime(tempModifiedDate);
            model.LastModifiedOn = modifiedDate;
            var tempEditor = Request["editor"];

            model.Comment = tempEditor;
            int tempcreatedby = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = tempcreatedby;
            //QuestionViewModel question_query = (from question in context.DBContext_question where question.QuestionId == userid select question).SingleOrDefault();

            AnswerViewModel answer_query = (from answer in context.DBContext_answer where answer.AnswerId == id select answer).SingleOrDefault();
            int question_id = answer_query.QuestionId;
            model.AnswerId = answer_query.AnswerId;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var record = new CommentViewModel()
                    {
                        Comment = model.Comment,
                        ParentCommentId = 1,
                        AnswerId = model.AnswerId,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = model.CreatedBy,
                        CreatedOn = model.CreatedOn,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_comment.Add(model);
                    context.SaveChanges();
                    ViewBag.message = "successfully";
                    TempData["Data"] = "Successful";


                }
                return RedirectToAction("QuestionAndAnswer", "QuestionAnswer", new { id = @question_id });
            }
            ModelState.Clear();
            return RedirectToAction("QuestionAndAnswer", "QuestionAnswer", new { id = @question_id });

        }
        [CheckSessionOut]
        public ActionResult CommentCommentInsert(int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            List<commentview> commentList = new List<commentview>();
            var query_comment = (from question in context.DBContext_question
                                 join answer in context.DBContext_answer
                                 on question.QuestionId equals answer.QuestionId
                                 join comment in context.DBContext_comment
                                     on answer.AnswerId equals comment.AnswerId
                                 join register in context.DBContext_register
                                 on answer.CreatedBy equals register.UserId
                                 where comment.CommentId == id
                                 select new commentview
                                 {
                                     answerid = comment.AnswerId,
                                     comm = comment.Comment,
                                     ParentCommentId = comment.ParentCommentId,
                                     CreatedBy = register.UserName,
                                     CreatedOn = comment.CreatedOn,
                                     CommentId = comment.CommentId

                                 }).ToList();
            commentList = query_comment.ToList();
            ViewBag.Comment = commentList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentCommentInsert(CommentViewModel model, int id)
        {


            int userid = Convert.ToInt32(Session["UserID"]);
            var tempCreatedDate = DateTime.Now;
            DateTime createdDate = Convert.ToDateTime(tempCreatedDate);
            model.CreatedOn = createdDate;
            var tempModifiedDate = DateTime.Now;
            DateTime modifiedDate = Convert.ToDateTime(tempModifiedDate);
            model.LastModifiedOn = modifiedDate;
            var tempEditor = Request["editor"];

            model.Comment = tempEditor;
            int tempcreatedby = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = tempcreatedby;
            //QuestionViewModel question_query = (from question in context.DBContext_question where question.QuestionId == userid select question).SingleOrDefault();

            CommentViewModel comment_query = (from comment in context.DBContext_comment where comment.CommentId == id select comment).SingleOrDefault();
            AnswerViewModel answer_query = (from answer in context.DBContext_answer where answer.AnswerId == comment_query.AnswerId select answer).SingleOrDefault();
            List<commentview> commentList = new List<commentview>();
            QuestionViewModel query_comment_question = (from question in context.DBContext_question
                                                        join answer in context.DBContext_answer
                                                        on question.QuestionId equals answer.QuestionId
                                                        join comment in context.DBContext_comment
                                                            on answer.AnswerId equals comment.AnswerId
                                                        join register in context.DBContext_register
                                                        on answer.CreatedBy equals register.UserId
                                                        where comment.CommentId == id
                                                        select question).SingleOrDefault();
            AnswerViewModel query_comment_answer = (from question in context.DBContext_question
                                                    join answer in context.DBContext_answer
                                                    on question.QuestionId equals answer.QuestionId
                                                    join comment in context.DBContext_comment
                                                        on answer.AnswerId equals comment.AnswerId
                                                    join register in context.DBContext_register
                                                    on answer.CreatedBy equals register.UserId
                                                    where comment.CommentId == id
                                                    select answer).SingleOrDefault();
            //{
            //    answerid = comment.AnswerId,
            //    comm = comment.Comment,
            //    ParentCommentId = comment.ParentCommentId,
            //    CreatedBy = register.UserName,
            //    CreatedOn = comment.CreatedOn,
            //    CommentId = comment.CommentId

            //}).ToList();

            //foreach (var item1 in query_comment) //retrieve each item and assign to model
            //{            //commentList = query_comment.ToList();

            model.AnswerId = query_comment_answer.AnswerId;
            int question_id = query_comment_question.QuestionId;
            model.ParentCommentId = id;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var record = new CommentViewModel()
                    {
                        Comment = model.Comment,
                        ParentCommentId = model.CommentId,
                        AnswerId = model.AnswerId,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = model.CreatedBy,
                        CreatedOn = model.CreatedOn,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_comment.Add(model);
                    context.SaveChanges();
                    ViewBag.message = "successfully";
                    TempData["Data"] = "Successful";


                }
                return RedirectToAction("QuestionAndAnswer", "QuestionAnswer", new { id = @question_id });

            }

            ModelState.Clear();
            return RedirectToAction("QuestionAndAnswer", "QuestionAnswer", new { id = @question_id });
            //return View();
        }
        [CheckSessionOut]
        public ActionResult Answer()
        {
            int userid = Convert.ToInt32(Session["UserID"]);

            List<AnswerOnQuestionViewModel> questionList = new List<AnswerOnQuestionViewModel>();
            var meds = from question in context.DBContext_question
                       join register in context.DBContext_register
                       on question.CreatedBy equals register.UserId
                       where question.UserId == userid
                       select new AnswerOnQuestionViewModel
                       {
                           AnswerOnQuestionQuestion = question.Question,
                           AnswerOnQuestionQuestionDescription = question.QuestionDescription
                           //AnswerOnQuestionUserName = register.UserName
                           //QuestionRegistrationCreatedDate = question.CreatedOn
                       };
            questionList = meds.ToList();

            //var med = from question in context.DBContext_question
            //           join answer in context.DBContext_answer
            //           on question.QuestionId equals answer.QuestionId
            //           where question.QuestionId == userid
            //           select new AnswerOnQuestionViewModel
            //           {
            //                AnswerOnAnswer= answer.Answer,
            //               //AnswerOnQuestionQuestionDescription = question.QuestionDescription
            //               //AnswerOnQuestionUserName = register.UserName
            //               //QuestionRegistrationCreatedDate = question.CreatedOn
            //           };

            //questionList = med.ToList();
            return View(questionList);

        }
        [CheckSessionOut]
        public ActionResult EditQuestion(int id)
        {

            List<QuestionAnswerViewModel> questionList = new List<QuestionAnswerViewModel>();
            int userid = Convert.ToInt32(Session["UserID"]);
            var query_question = (from question in context.DBContext_question
                                  join register in context.DBContext_register
                                  on question.CreatedBy equals register.UserId
                                  where question.QuestionId == id
                                  orderby question.QuestionId descending
                                  select new QuestionAnswerViewModel
                                  {
                                      QuestionAnswerId = question.QuestionId,
                                      QuestionAnswerQuestion = question.Question,
                                      QuestionAnswerUserName = register.UserName,
                                      QuestionAnswerCreatedDate = register.CreatedOn,
                                      QuestionAnswerDescription = question.QuestionDescription
                                  }).ToList().Take(5);

            questionList = query_question.ToList();
            ViewBag.Question = questionList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(int id, QuestionViewModel model)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            var tempquestion = Request["Textbox"];
            QuestionViewModel query_question = (from question in context.DBContext_question where question.QuestionId == id select question).SingleOrDefault();
            model.Question = tempquestion;
            var tempquestiondescription = Request["editor"];
            model.QuestionDescription = tempquestiondescription;
            if (query_question != null)
            {

                if (tempquestion.Length > 1)
                {
                    query_question.Question = model.Question;
                    query_question.QuestionDescription = model.QuestionDescription;
                    query_question.CreatedOn = DateTime.Now;
                    query_question.CreatedBy = userid;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        return View("Error");
                    }
                    return RedirectToAction("Index", "QuestionAnswer");
                }
                return RedirectToAction("Index", "QuestionAnswer");


            }
            return View(model);
        }
        [CheckSessionOut]
        public ActionResult DeleteQuestion(AnswerViewModel model, int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            int tempcreatedby = Convert.ToInt32(Session["UserID"]);
            var deleteOrderDetails = context.DBContext_question.Where(a => a.QuestionId == id);
            QuestionViewModel query_question = (from question in context.DBContext_question
                                           
                                           
                                            join register in context.DBContext_register
                                            on question.CreatedBy equals register.UserId
                                            where question.QuestionId == id
                                            select question).SingleOrDefault();

          
            if (tempcreatedby == query_question.CreatedBy )
            {
                var isactive = false;
                query_question.IsActive = isactive;
                query_question.CreatedBy = userid;
                context.SaveChanges();
            }

            return RedirectToAction("Index", "QuestionAnswer", new { @Id = userid});
        }
        [CheckSessionOut]
        public ActionResult EditAnswer(int id)
        {

            List<QuestionAnswerViewModel> questionList = new List<QuestionAnswerViewModel>();
            int userid = Convert.ToInt32(Session["UserID"]);
            var query_question = (from question in context.DBContext_question
                                  join answer in context.DBContext_answer
                                  on question.QuestionId equals answer.QuestionId
                                  where answer.AnswerId == id

                                  select new QuestionAnswerViewModel
                                  {
                                      QuestionAnswerId = question.QuestionId,
                                      QuestionAnswerAnswerId = answer.AnswerId,
                                      QuestionAnswerAnswer = answer.Answer,
                                      //QuestionAnswerUserName = answer.UserName,
                                      QuestionAnswerCreatedDate = answer.CreatedOn,
                                      //QuestionAnswerDescription = question.QuestionDescription
                                  })/*.ToList().Take(5)*/;

            questionList = query_question.ToList();
            ViewBag.Answer = questionList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAnswer(int id, AnswerViewModel model)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            AnswerViewModel query_answer = (from answer in context.DBContext_answer where answer.AnswerId == id select answer).SingleOrDefault();

            var tempanswerdescription = Request["editor"];
            model.Answer = tempanswerdescription;
            if (query_answer != null)
            {
                if (tempanswerdescription.Length > 1)
                {


                    query_answer.Answer = model.Answer;
                    query_answer.CreatedOn = DateTime.Now;
                    query_answer.CreatedBy = userid;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                    return RedirectToAction("Index", "QuestionAnswer");
                }
                return RedirectToAction("Index", "QuestionAnswer");
            }
            return View();
        }
        [CheckSessionOut]
        public ActionResult EditCommit(int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            List<commentview> commentList = new List<commentview>();
            var query_comment = (from question in context.DBContext_question
                                 join answer in context.DBContext_answer
                                 on question.QuestionId equals answer.QuestionId
                                 join comment in context.DBContext_comment
                                     on answer.AnswerId equals comment.AnswerId
                                 join register in context.DBContext_register
                                 on answer.CreatedBy equals register.UserId
                                 where comment.CommentId == id
                                 select new commentview
                                 {
                                     answerid = comment.AnswerId,
                                     comm = comment.Comment,
                                     ParentCommentId = comment.ParentCommentId,
                                     CreatedBy = register.UserName,
                                     CreatedOn = comment.CreatedOn,
                                     CommentId = comment.CommentId

                                 }).ToList();
            commentList = query_comment.ToList();
            ViewBag.Comment = commentList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCommit(int id, CommentViewModel model)
        {
            CommentViewModel query_comment = (from comment in context.DBContext_comment where comment.CommentId == id select comment).SingleOrDefault();
            var tempanswerdescription = Request["editor"];
            model.Comment = tempanswerdescription;
            if (query_comment != null)
            {
                query_comment.Comment = model.Comment;
                query_comment.CreatedOn = DateTime.Now;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
                return RedirectToAction("Index", "QuestionAnswer");
            }
            return View();
        }
        [CheckSessionOut]
        public ActionResult DeleteAnswer(AnswerViewModel model, int id)
        {
            int tempcreatedby = Convert.ToInt32(Session["UserID"]);
            var deleteOrderDetails = context.DBContext_answer.Where(a => a.AnswerId == id);
            AnswerViewModel query_answer = (from question in context.DBContext_question
                                            join answer in context.DBContext_answer
                                            on question.QuestionId equals answer.QuestionId
                                            join register in context.DBContext_register
                                            on answer.CreatedBy equals register.UserId
                                            where answer.AnswerId == id
                                            select answer).SingleOrDefault();

            QuestionViewModel query_question = (from question in context.DBContext_question
                                                join answer in context.DBContext_answer
                                                on question.QuestionId equals answer.QuestionId
                                                join register in context.DBContext_register
                                                on answer.CreatedBy equals register.UserId
                                                where answer.AnswerId == id
                                                select question).SingleOrDefault();

            if (tempcreatedby == query_answer.CreatedBy || tempcreatedby == query_question.CreatedBy)
            {
                var isactive = false;
                query_answer.IsActive = isactive;
                //context.DBContext_answer.RemoveRange(deleteOrderDetails);
                context.SaveChanges();
            }

            return RedirectToAction("QuestionAndAnswer", "QuestionAnswer", new { @Id = query_question.QuestionId });
        }
        [CheckSessionOut]
        public ActionResult DeleteComment(CommentViewModel model, int id)
        {
            int tempcreatedby = Convert.ToInt32(Session["UserID"]);
            var deleteOrderDetails = context.DBContext_comment.Where(a => a.CommentId == id);
            CommentViewModel query_comment = (from question in context.DBContext_question
                                              join answer in context.DBContext_answer
                                              on question.QuestionId equals answer.QuestionId
                                              join comment in context.DBContext_comment
                                           on answer.AnswerId equals comment.AnswerId
                                              join register in context.DBContext_register
                                                  on answer.CreatedBy equals register.UserId

                                              where comment.CommentId == id
                                              select comment).SingleOrDefault();

            QuestionViewModel query_question = (from question in context.DBContext_question
                                                join answer in context.DBContext_answer
                                                on question.QuestionId equals answer.QuestionId
                                                join comment in context.DBContext_comment
                                             on answer.AnswerId equals comment.AnswerId
                                                join register in context.DBContext_register
                                                    on answer.CreatedBy equals register.UserId

                                                where comment.CommentId == id
                                                select question).SingleOrDefault();


            if (tempcreatedby == query_comment.CreatedBy || tempcreatedby == query_question.CreatedBy)
            {
                var isactive = false;
                query_comment.IsActive = isactive;
                //context.DBContext_answer.RemoveRange(deleteOrderDetails);
                context.SaveChanges();
            }

            return RedirectToAction("QuestionAndAnswer", "QuestionAnswer", new { @Id = query_question.QuestionId });
        }
        [CheckSessionOut]
        public ActionResult AskQuestion(QuestionViewModel model)
        {
            IList<UploadInitialFile> initialFiles =
                SessionUploadInitialFilesRepository.GetAllInitialFiles();

            return View(initialFiles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AskQuestion()
        {
            QuestionViewModel model = new QuestionViewModel();
            int lastquestionid;
            int lastuserid;
            var query_user = (from user in context.DBContext_register select user).Any();
            if (!query_user)
            {
                lastuserid = 1;
            }
            else
            {
                lastuserid = context.DBContext_register.Max(item => item.UserId + 1);
            }
            var query_question = (from question in context.DBContext_question select question).Any();

            if (!query_question)
            {
                lastquestionid = 1;
            }
            else
            {
                lastquestionid = context.DBContext_question.Max(item => item.QuestionId + 1);
            }




            var tempCreatedDate = DateTime.Now;
            DateTime createdDate = Convert.ToDateTime(tempCreatedDate);
            model.CreatedOn = createdDate;
            var tempModifiedDate = DateTime.Now;
            DateTime modifiedDate = Convert.ToDateTime(tempModifiedDate);
            model.LastModifiedOn = modifiedDate;
            var tempEditor = Request["questioneditor"];
            model.QuestionDescription = tempEditor;
            model.QuestionCategoryId = 1;
            var tempQuestion = Request["questions"];
            model.Question = tempQuestion;
            int tempcreatedby = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = tempcreatedby;
            model.UserId = tempcreatedby;

            //double[] myDoubleArray = new double[] { 1.0, 1.2, 1.3, 1.4 };

            string[] image = (string[])Session["DoubleList"];
            //double[] sessionDoubles = (double[])Session["DoubleList"];
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var user = context.DBContext_question.Where(u => u.Question == model.Question).FirstOrDefault();
                    if (user == null)
                    {
                        var record = new QuestionViewModel()
                        {
                            Question = model.Question,
                            QuestionCategoryId = 1,
                            QuestionDescription = model.QuestionDescription,
                            SearchTags = "health",
                            UserId = model.UserId,
                            AcceptingAnswers = true,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedBy = 1,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,

                        };
                        context.DBContext_question.Add(model);
                        context.SaveChanges();
                        ViewBag.message = "successfully";
                        TempData["Data"] = "Successful";

                        DocumentViewModel document_model = new DocumentViewModel();

                        string dirarr = Server.MapPath("~/Temp/" + tempcreatedby + "/Questions/" + lastquestionid);
                        if (Directory.Exists(dirarr))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Images/" + tempcreatedby));

                            Directory.CreateDirectory(Server.MapPath("~/Images/" + tempcreatedby + "/Questions"));
                            var source = Server.MapPath("~/Temp/" + tempcreatedby + "/Questions/" + lastquestionid);
                            var destination = Server.MapPath("~/Images/" + tempcreatedby + "/Questions/" + lastquestionid);

                            System.IO.Directory.Move(source, destination);
                            string[] filesarr = Directory.GetFiles(destination);
                            for (int i = 0; i < filesarr.Length; i++)
                            {
                                document_model.DocumentName = filesarr[i];
                                var physicalPath = Path.Combine(Server.MapPath("~/Temp/" + tempcreatedby + "/Questions/" + lastquestionid), filesarr[i]);

                                document_model.DocumentPath = physicalPath;
                                var table = "Questions";
                                document_model.PrimaryObjectId = table;
                                document_model.PrimaryKeyId = lastquestionid;
                                var document_record = new DocumentViewModel()
                                {
                                    DocumentName = document_model.DocumentName,
                                    DocumentPath = document_model.DocumentPath,
                                    PrimaryKeyId = document_model.PrimaryKeyId,
                                    IsActive = model.IsActive,
                                    IsDeleted = false,
                                    CreatedBy = 0,
                                    CreatedOn = DateTime.Now,
                                    LastModifiedBy = 0,
                                    LastModifiedOn = DateTime.Now,
                                };
                                context.DBContext_document.Add(document_model);
                                context.SaveChanges();
                            }
                        }

                        return RedirectToAction("Index", "QuestionAnswer");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Already asked");
                        TempData["Data"] = "Already asked";
                        return View("../Shared/Error");
                    }
                }

            }
            ModelState.Clear();
            return View(model);
        }
        [CheckSessionOut]
        public ActionResult SaveAndPersist(IEnumerable<HttpPostedFileBase> files)
        {

            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);

                    //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
                    Directory.CreateDirectory(Server.MapPath("~/Temp/hh"));
                    var tempQuestion = Request["hidden"];
                    var fileExtension = Path.GetExtension(file.FileName);

                    SessionUploadInitialFilesRepository.Add(new UploadInitialFile(fileName, file.ContentLength, fileExtension));

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                    DocumentViewModel model = new DocumentViewModel();
                    var user = context.DBContext_document.Where(u => u.DocumentName == model.DocumentName && u.DocumentPath == model.DocumentPath).FirstOrDefault();
                    model.DocumentName = fileName;
                    model.DocumentPath = physicalPath;
                    if (user == null)
                    {
                        var record = new DocumentViewModel()
                        {
                            DocumentName = model.DocumentName,
                            DocumentPath = model.DocumentPath,
                            PrimaryKeyId = model.PrimaryKeyId,
                            IsActive = model.IsActive,
                            IsDeleted = false,
                            CreatedBy = 0,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,
                        };
                        context.DBContext_document.Add(model);
                        context.SaveChanges();
                    }
                }
            }
            // Return an empty string to signify success
            return Content("");
        }
        [CheckSessionOut]
        public ActionResult QuestionSaveAndPersist(IEnumerable<HttpPostedFileBase> files)
        {
            int lastuserid = Convert.ToInt32(Session["UserID"]);
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    ArrayList i = new ArrayList();
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    int lastquestionid;
                    //int lastuserid;
                    //var query_user = (from user in context.DBContext_register select user).Any();
                    //if (!query_user)
                    //{
                    //    lastuserid= 1;
                    //}
                    //else
                    //{
                    //    lastuserid = context.DBContext_question.Max(item => item.QuestionId + 1);
                    //}
                    var query_question = (from question in context.DBContext_question select question).Any();

                    if (!query_question)
                    {
                        lastquestionid = 1;
                    }
                    else
                    {
                        lastquestionid = context.DBContext_question.Max(item => item.QuestionId + 1);
                    }

                    Directory.CreateDirectory(Server.MapPath("~/Temp/" + lastuserid));
                    Directory.CreateDirectory(Server.MapPath("~/Temp/" + lastuserid + "/Questions/" + lastquestionid));
                    var physicalPath = Path.Combine(Server.MapPath("~/Temp/" + lastuserid + "/Questions/" + lastquestionid), fileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    SessionUploadInitialFilesRepository.Add(new UploadInitialFile(fileName, file.ContentLength, fileExtension));
                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }
        [CheckSessionOut]
        public ActionResult AnswerSaveAndPersist(IEnumerable<HttpPostedFileBase> files)
        {
            int lastuserid = Convert.ToInt32(Session["UserID"]);
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    ArrayList i = new ArrayList();
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    int lastanswerid;
                    //int lastuserid;
                    //var query_user = (from user in context.DBContext_register select user).Any();
                    //if (!query_user)
                    //{
                    //    lastuserid= 1;
                    //}
                    //else
                    //{
                    //    lastuserid = context.DBContext_question.Max(item => item.QuestionId + 1);
                    //}
                    var query_answer = (from answer in context.DBContext_answer select answer).Any();

                    if (!query_answer)
                    {
                        lastanswerid = 1;
                    }
                    else
                    {
                        lastanswerid = context.DBContext_answer.Max(item => item.AnswerId + 1);
                    }

                    Directory.CreateDirectory(Server.MapPath("~/Temp/" + lastuserid));
                    Directory.CreateDirectory(Server.MapPath("~/Temp/" + lastuserid + "/Answers/" + lastanswerid));
                    var physicalPath = Path.Combine(Server.MapPath("~/Temp/" + lastuserid + "/Answers/" + lastanswerid), fileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    SessionUploadInitialFilesRepository.Add(new UploadInitialFile(fileName, file.ContentLength, fileExtension));
                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }
        [CheckSessionOut]
        public ActionResult RemoveAndPersist(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);

                    SessionUploadInitialFilesRepository.Remove(fileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                    DocumentViewModel model = new DocumentViewModel();
                    var deleteOrderDetails = context.DBContext_document.Where(u => u.DocumentName == model.DocumentName && u.DocumentPath == model.DocumentPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public List<commentview> recursive_comment(int id, int a_id, List<commentview> commentList)
        {
            var query_comment = (from question in context.DBContext_question
                                 join answer in context.DBContext_answer
                                 on question.QuestionId equals answer.QuestionId
                                 join comment in context.DBContext_comment
                                 on answer.AnswerId equals comment.AnswerId
                                 join register in context.DBContext_register
                                 on answer.CreatedBy equals register.UserId
                                 where (comment.ParentCommentId == id && comment.AnswerId == a_id && comment.IsActive == true)
                                 select new commentview
                                 {
                                     answerid = comment.AnswerId,
                                     comm = comment.Comment,
                                     ParentCommentId = comment.ParentCommentId,
                                     CreatedBy = register.UserName,
                                     CreatedOn = comment.CreatedOn,
                                     CommentId = comment.CommentId
                                 }).ToList();
            if (query_comment.Count != 0)
            {


                foreach (var item1 in query_comment) //retrieve each item and assign to model
                {

                    {
                        commentList.Add(new commentview()
                        {
                            comm = item1.comm,
                            answerid = item1.answerid,
                            ParentCommentId = item1.ParentCommentId,
                            CreatedBy = item1.CreatedBy,
                            CreatedOn = item1.CreatedOn,
                            CommentId = item1.CommentId
                        });

                        recursive_comment(item1.CommentId, item1.answerid, commentList);

                    }

                }
            }
            //var query_comment_answer = (from comment in context.DBContext_comment  where comment.CommentId == id && comment.AnswerId==a_id select comment).SingleOrDefault();


            return commentList;
        }
    }
}