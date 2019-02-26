using Microsoft.AspNet.Identity;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Quiz.Controllers
{
    public class HomeController : Controller
    {
        AppModelContext db = new AppModelContext();

        public ActionResult Index(string message)
        {
            if (!string.IsNullOrEmpty(message))
                TempData["Message"] = message;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        
        [Authorize]
        [HttpGet]
        //currentUser wants to create his own question
        public ActionResult CreateQuestion()
        {
            //transfer info for DropDownList
            int selectedIndex = 1;
            SelectList universities = new SelectList(db.Universities, "Id", "Name", selectedIndex);
            ViewBag.Universities = universities;

            SelectList groups = new SelectList(db.Groups.Where(c => c.UniversityId == selectedIndex), "Id", "Name");
            ViewBag.Groups = groups;
            return View();
        }

        [Authorize]
        [HttpPost]
        //currentUser created his own question
        public RedirectToRouteResult CreateQuestion(CreateQuestionViewModel model)
        {
            //write the question in db
            Question question = new Question
            {
                UserId = User.Identity.GetUserId(),
                Name = model.Name,
                Text = model.Text,
                СheckPattern = model.СheckPattern,
                CreateTime = DateTime.Now
            };
            
            db.Questions.Add(question);
            db.SaveChanges();

            //take all who need to send the question
            List<Group> groups = new List<Group>();
            foreach (int id in model.Groups)
            {
                groups.Add(db.Groups.Include(t => t.UserProfiles).FirstOrDefault(t => t.Id == id));
            }


            //send them the question
            List<QuestionAnswer> questionAnswers = new List<QuestionAnswer>();
            foreach (var group in groups)
            {
                foreach (UserProfile item in group.UserProfiles)
                {
                    if (item.Id == question.UserId) //skip the author
                        continue;
                    QuestionAnswer u = new QuestionAnswer
                    {
                        QuestionId = question.Id,
                        UserId = item.Id,
                        StatusOfQuestionAnswerId = 1 //sended
                    };
                    questionAnswers.Add(u);
                }
            }
            
            db.QuestionAnswers.AddRange(questionAnswers);
            db.SaveChanges();
            
            return RedirectToAction("Index", new { message = "Congratulations! Your question has been sent to users." });
        }

        //for the second DropDownList from CreateQuestion
        public ActionResult GetGroups(int? id)
        {
            return PartialView(db.Groups.Where(g => g.UniversityId == id).ToList());
        }

        [Authorize]
        [HttpGet]
        //currentUser looks at the list of his questions and answers to his questions
        public ActionResult MyQuestions()
        {
            //take all the information about questions created by the user
            var userId = User.Identity.GetUserId();
            List<Question> myQuestions = new List<Question>();

            myQuestions.AddRange(db.Questions
                .Include("QuestionAnswers.User.Group.University")
                .Include("QuestionAnswers.StatusOfQuestionAnswer")
                .Include(q => q.QuestionAnswers.Select(qa => qa.Сhecks)) 
                .Where(t => t.UserId == userId).ToList());

            //transfer to TablesViewModel
            List<TableViewModel> model = new List<TableViewModel>();
            List<string> headLine = new List<string>()
                    { "#","Surname", "Name", "Group", "University", "Status", "Mark", "Answer" };

            if (myQuestions.Count == 0)
            {
                model.Add(new TableViewModel());
                model.Last().Name = "Unfortunately, you didn't create any question yet.";
                model.Last().Headline = new List<string>();
                model.Last().DataRows.Add(new DataRow());
            }
            else
            {
                foreach (var m in myQuestions)
                {
                    model.Add(new TableViewModel(m.Name,m.Id,headLine));
                    int counter = 1;

                    foreach (var qa in m.QuestionAnswers)
                    {
                        //calculate the mark
                        string mark = "";
                        if (qa.Сhecks.Count > 0)
                        {
                            double tempMark = 0;
                            
                            foreach (var сheck in qa.Сhecks)
                                tempMark += сheck.Mark;
                            tempMark = tempMark / qa.Сhecks.Count;

                            foreach (var сheck in qa.Сhecks)
                                if (tempMark + 1 < сheck.Mark || tempMark - 1 > сheck.Mark)
                                    mark = " !!!";
                            
                            tempMark = Math.Round(tempMark);
                            mark = mark.Insert(0, tempMark.ToString());
                        }
                        else
                            mark = "-";

                        string linkName;
                        if (qa.StatusOfQuestionAnswerId == 1)
                            linkName = "Missing";
                        else
                            linkName = "Show answer";
                        
                        model.Last().DataRows.Add(new DataRow());
                        model.Last().DataRows.Last().Id = qa.Id;
                        model.Last().DataRows.Last().Row = new List<string>
                        {
                            counter++.ToString(), qa.User.Surname, qa.User.Name, qa.User.Group.Name,
                            qa.User.Group.University.Name, qa.StatusOfQuestionAnswer.Name, mark, linkName
                        };
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        //currentUser looks at the list of questions asked to him
        public ActionResult QuestionsForMe()
        {
            //take all the questions to the user without an answer
            var userId = User.Identity.GetUserId();
            List<QuestionAnswer> questionAnswersForMy = new List<QuestionAnswer>();

            questionAnswersForMy.AddRange(db.QuestionAnswers
                .Include("Question.User")
                .Where(qa => qa.UserId == userId)
                .Where(qa => qa.StatusOfQuestionAnswerId == 1).ToList());

            //transfer to TablesViewModel
            TableViewModel model = new TableViewModel();

            if (questionAnswersForMy.Count == 0)
            {
                model.Name = "Unfortunately, there are no questions for you.";
                model.Headline = new List<string>();
                model.DataRows.Add(new DataRow());
            }
            else
            {
                int counter = 1;
                model.Name = "List of questions for me";
                model.Headline = new List<string>()
                    { "#","Headline","Who","When","Link"};

                foreach (QuestionAnswer qa in questionAnswersForMy)
                {
                    model.DataRows.Add(new DataRow());
                    model.DataRows.Last().Id = qa.Id;
                    model.DataRows.Last().Row = new List<string>
                    {
                        counter++.ToString(), qa.Question.Name, qa.Question.User.Surname + " " + qa.Question.User.Name,
                        qa.Question.CreateTime.ToString(), "to answer"
                    };
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        //currentUser wants to answer the question asked to him
        public ActionResult AnswerQuestionForMe(int? questionAnswerId)
        {
            return View(db.QuestionAnswers.Include(x => x.Question).FirstOrDefault(x => x.Id == questionAnswerId));
        }

        [Authorize]
        [HttpPost]
        //currentUser answered the question and save his answer
        public RedirectToRouteResult AnswerQuestionForMe(QuestionAnswer model)
        {
            model.CreateTime = DateTime.Now;

            UserProfile currentUserProfile  = db.UserProfiles.Find(User.Identity.GetUserId());

            string message = null;
            //changing the status of the new answer depending on the rating
            if (currentUserProfile.Rating >= RatingValueStatement.CostOfAnswer)
            {
                currentUserProfile.Rating -= RatingValueStatement.CostOfAnswer;
                model.StatusOfQuestionAnswerId = 3; //Awaiting review
                message = "Your answer is submitted for review";
            }
            else
            {
                if ((ListOfMyChecks(currentUserProfile.Id).Count + currentUserProfile.Rating) >= 3)
                {
                    model.StatusOfQuestionAnswerId = 2;
                    message = "Сheck someone's answer, your answer has been saved but not submitted for review";
                }
                else
                {
                    model.StatusOfQuestionAnswerId = 3;
                    message = "Your answer is submitted for review";
                }
            }
            
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { message });
        }

        [Authorize]
        [HttpGet]
        //currentUser wants to check the answer to his question
        public ActionResult CheckAnswerByAuthor(int? questionAnswerId)
        {
            CheckAnswerByAuthorViewModel model = new CheckAnswerByAuthorViewModel();
            model.QuestionAnswer = db.QuestionAnswers
                .Include(x => x.Question)
                .Include(x => x.User)
                .Include(x => x.Сhecks.Select(ch => ch.User))
                .FirstOrDefault(x => x.Id == questionAnswerId);

            //transfer to TablesViewModel
            model.Table = new TableViewModel();

            if (model.QuestionAnswer.Сhecks.Count == 0)
            {
                model.Table.Name = "Unfortunately, there are no checks in this question.";
                model.Table.Headline = new List<string>();
                model.Table.DataRows.Add(new DataRow());
            }
            else
            {
                if (model.QuestionAnswer.Сhecks.FirstOrDefault().UserId == User.Identity.GetUserId())
                {
                    model.Table.Name = "";
                    model.Table.Headline = new List<string>();
                    model.Table.DataRows.Add(new DataRow());
                }
                else
                {
                    int counter = 1;
                    model.Table.Name = "List of checks";
                    model.Table.Headline = new List<string>()
                    { "#","Who","Mark","Report"};

                    foreach (Check ch in model.QuestionAnswer.Сhecks)
                    {
                        model.Table.DataRows.Add(new DataRow());
                        model.Table.DataRows.Last().Id = ch.Id;
                        model.Table.DataRows.Last().Row = new List<string>
                        {
                        counter++.ToString(),ch.User.Surname + " " + ch.User.Name, ch.Mark.ToString(), "!!!"
                        };
                    }
                }

                
            }
            
            return View(model);
        }

        [Authorize]
        [HttpPost]
        //currentUser checked the answer to his question
        public RedirectToRouteResult CheckAnswerByAuthor(CheckAnswerByAuthorViewModel model)
        {
            //write the answer to the db
            Check check = new Check
            {
                UserId = User.Identity.GetUserId(),
                CreateTime = DateTime.Now,
                Mark = model.Mark,
                QuestionAnswerId = model.QuestionAnswer.Id
            };

            //status of the answer - Author's review
            db.QuestionAnswers.Find(model.QuestionAnswer.Id).StatusOfQuestionAnswerId = 9;

            //remove all checks except the author's
            db.Checks.RemoveRange(db.Checks.Where(ch => ch.QuestionAnswerId == model.QuestionAnswer.Id));
            
            db.Checks.Add(check);
            db.SaveChanges();

            return RedirectToAction("MyQuestions", "Home");
        }

        [Authorize]
        [HttpGet]
        //currentUser looks at the list of answers that he can check
        public ActionResult MyChecks()
        {
            List<QuestionAnswer> questionAnswerForCheck = ListOfMyChecks(User.Identity.GetUserId());

            //transfer to TablesViewModel
            TableViewModel model = new TableViewModel();

            if (questionAnswerForCheck.Count == 0)
            {
                model.Name = "Unfortunately, there are no answers to check for you.";
                model.Headline = new List<string>();
                model.DataRows.Add(new DataRow());
            }
            else
            {
                int counter = 1;
                model.Name = "List of answers to check for me";
                model.Headline = new List<string>()
                    { "#","Question","Who is author","When asked","Answer","Link"};

                foreach (QuestionAnswer qa in questionAnswerForCheck)
                {
                    model.DataRows.Add(new DataRow());
                    model.DataRows.Last().Id = qa.Id;
                    model.DataRows.Last().Row = new List<string>
                    {
                        counter++.ToString(),
                        qa.Question.Name,
                        qa.Question.User.Surname + " " + qa.Question.User.Name,
                        qa.Question.CreateTime.ToString(),
                        qa.Answer.Length < 30?qa.Answer.Substring(0,qa.Answer.Length) : qa.Answer.Substring(0,30) + "...",
                        "check it"
                    };
                }
            }
            return View(model);
        }
        
        [Authorize]
        [HttpGet]
        //currentUser wants to check one of the other people's answers
        public ActionResult CheckAnswer(int questionAnswerId)
        {
            QuestionAnswer questionAnswer = db.QuestionAnswers
                .Include(x => x.Question)
                .FirstOrDefault(x => x.Id == questionAnswerId);
            CheckAnswerViewModel model = new CheckAnswerViewModel
            {
                QuestionAnswerId = questionAnswer.Id,
                QuestionName = questionAnswer.Question.Name,
                QuestionText = questionAnswer.Question.Text,
                QuestionСheckPattern = questionAnswer.Question.СheckPattern,
                Answer = questionAnswer.Answer
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        //currentUser checked one of the other people's answer
        public RedirectToRouteResult CheckAnswer(Check model)
        {
            //write the answer to the db
            Check check = new Check
            {
                UserId = User.Identity.GetUserId(),
                CreateTime = DateTime.Now,
                Mark = model.Mark,
                QuestionAnswerId = model.QuestionAnswerId
            };

            //add the status of the answer
            db.QuestionAnswers.Find(model.QuestionAnswerId).StatusOfQuestionAnswerId += 1;

            //add rating of the checker
            db.UserProfiles.Find(User.Identity.GetUserId()).Rating += RatingValueStatement.CostOfCheck;

            string message = null;
            if (ListOfMyAnswerWithoutCheck(User.Identity.GetUserId()).Count > 0)
            {
                ListOfMyAnswerWithoutCheck(User.Identity.GetUserId()).FirstOrDefault().StatusOfQuestionAnswerId = 3;
                message = "Congratulations! One of your answers has been submitted for review.";
            }

            db.Checks.Add(check);
            db.SaveChanges();

            return RedirectToAction("Index", new { message });
        }
        
        public ActionResult AddUniversity()
        {
            return PartialView("AddUniversity");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUniversity(University model)
        {
            db.Universities.Add(model);
            db.SaveChanges();
            return RedirectToAction("Register","Account");
        }

        public ActionResult AddGroup()
        {
            AddGroupViewModel model = new AddGroupViewModel();
            model.UniversityId = 1;
            model.ListOfUniversities = new SelectList(db.Universities, "Id", "Name", model.UniversityId);
            
            return PartialView("AddGroup", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGroup(AddGroupViewModel model)
        {
            Group group = new Group();
            group.Name = model.GroupName;
            group.UniversityId = model.UniversityId;

            db.Groups.Add(group);
            db.SaveChanges();
            return RedirectToAction("Register", "Account");
        }

        public ActionResult DelMyQuestion(int id)
        {
            Question q = db.Questions.Find(id);
            if (q != null)
            {
                return PartialView("DelMyQuestion", q);
            }
            return View("MyQuestions", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DelMyQuestion")]
        public ActionResult DeleteMyQuestion(int id)
        {
            Question q = db.Questions.Find(id);
            if (q != null)
            {
                db.Questions.Remove(q);
                db.SaveChanges();
            }
            return RedirectToAction("MyQuestions", "Home");
        }
        
        //author of the question gives a penalty for poor quality check in CheckAnswerByAuthor
        public ActionResult PenaltyForChecking(int checkId, int qaId)
        {
            Check check = db.Checks
                    .Include(ch => ch.User)
                    .FirstOrDefault(ch => ch.Id == checkId);

            UserProfile user = check.User;
            if (user.Rating >= 2)
                user.Rating -= 2;
            else
                user.Rating = 0;

            QuestionAnswer qa = db.QuestionAnswers.Find(qaId);
            qa.StatusOfQuestionAnswerId -= 1;

            db.Entry(qa).State = EntityState.Modified;
            db.Entry(user).State = EntityState.Modified;
            db.Checks.Remove(check);
            db.SaveChanges();

            return RedirectToAction("CheckAnswerByAuthor", "Home", new { questionAnswerId = qaId }); 
        }

        //list of answers FOR verifying
        private List<QuestionAnswer> ListOfMyChecks(string сurrentUserId)
        {
            //take all questionsAnswers that are suitable for us
            UserProfile сurrentUser = db.UserProfiles.FirstOrDefault(x => x.Id == сurrentUserId);

            List<QuestionAnswer> questionAnswerForCheck = new List<QuestionAnswer>();

            questionAnswerForCheck.AddRange(db.QuestionAnswers
                .Include("Question.User")
                .Include(qa => qa.User)
                .Include(qa => qa.Сhecks)                                               //take only those where (comment what isn't needed)
                .Where(qa => qa.StatusOfQuestionAnswerId >= 3)                          //status 3 - Awaiting review
                .Where(qa => qa.StatusOfQuestionAnswerId <= 5)                          //status 5 - Verified by 2 users (3 checks at all)
                .Where(qa => qa.UserId != сurrentUser.Id)                               //сurrentUser isn't the author of the answer 
                .Where(qa => qa.Question.UserId != сurrentUser.Id)                      //сurrentUser isn't the author of the question
                //.Where(qa => qa.User.GroupId != сurrentUser.GroupId)                 //author of the answer is not from the сurrentUser's group
                //.Where(qa => qa.Question.User.GroupId != сurrentUser.GroupId)        //author of the question is not from the сurrentUser's group
                .ToList());

            //remove the answers that currentUser has already checked
            foreach (QuestionAnswer item in questionAnswerForCheck.ToArray())
            {
                if (item.Сhecks.Count != 0 && item.Сhecks.Any(ch => ch.UserId == сurrentUser.Id))
                {
                    questionAnswerForCheck.Remove(item);
                }
            }

            return questionAnswerForCheck;
        }

        //list of answers WITHOUT verification (status - Completed by the respondent)
        private List<QuestionAnswer> ListOfMyAnswerWithoutCheck(string сurrentUserId)
        {
            return db.QuestionAnswers
                        .Where(qa => qa.UserId == сurrentUserId)
                        .Where(qa => qa.StatusOfQuestionAnswerId == 2)
                        .ToList(); ;
        }

        static class RatingValueStatement
        {
            public const int CostOfQuestion = 0;
            public const int CostOfAnswer = 3;
            public const int CostOfCheck = 1;
        }
    }
}