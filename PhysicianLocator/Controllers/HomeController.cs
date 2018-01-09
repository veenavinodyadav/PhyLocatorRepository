using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicianLocator.Controllers
{
    public class HomeController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(HomeController));  //Declaring Log4Net
        LocatorContext context = new LocatorContext();
        public ActionResult Index()
        {
            //try
            //{
            //    int x, y, z;
            //    x = 5; y = 0;
            //    z = x / y;
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex.ToString());
            //}


            List<QuestionAnswerViewModel> questionList = new List<QuestionAnswerViewModel>();
            var query_question = from question in context.DBContext_question select question;
            CommonFunction commonfunction = new CommonFunction();
            if (query_question != null)
            {
                //int userid = Convert.ToInt32(Session["UserID"]);
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
                                        join document in context.DBContext_document
                                        on register.UserId equals document.CreatedBy
                                        where a.Any(name => name.Contains(question.Question) || question.Question.Contains(name))/* && register.UserId == userid*/
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
                                   where (tempSearch == null)/* && register.UserId == userid*/
                                   select new QuestionAnswerViewModel
                                   {
                                       QuestionAnswerId = question.QuestionId,
                                       QuestionAnswerQuestion = question.Question,
                                       QuestionAnswerUserName = register.UserName,
                                       QuestionAnswerCreatedDate = question.CreatedOn,
                                       QuestionAnswerUserID = register.UserId
                                   }).ToList();
                       
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
                    // return View(questionList.ToList());
                }
                else
                {
                   
                    var query = (from question in context.DBContext_question
                                 join register in context.DBContext_register
                                 on question.CreatedBy equals register.UserId

                                 orderby question.QuestionId descending
                                 select new QuestionAnswerViewModel
                                 {
                                     QuestionAnswerId = question.QuestionId,
                                     QuestionAnswerQuestion = question.Question,
                                     QuestionAnswerUserName = register.UserName,
                                     QuestionAnswerCreatedDate = register.CreatedOn,
                                     QuestionAnswerUserID = register.UserId
                                 }).ToList().Take(5);
                   
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
                            ViewBag.Message = "";
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
        public ActionResult QuestionDescription(int id)
        {
            List<QuestionAnswerViewModel> questionList = new List<QuestionAnswerViewModel>();
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

        public ActionResult About()
        {
          
            ViewBag.Message = "Your application description page.";
            return View();
        }
       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult map()
        {
            return View();
        }
        public ActionResult FirstPage()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}