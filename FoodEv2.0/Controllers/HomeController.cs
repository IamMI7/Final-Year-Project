using FoodEAPI.UserLogic;
using FoodEv2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace FoodEv2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("uAuth") == null)
            {
                HttpContext.Session.SetInt32("uAuth", 3);
            }
            return View();
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (HttpContext.Session.GetInt32("uAuth") == 3 || HttpContext.Session.GetInt32("uAuth") == null)
            {
                model.AuthLevel = 2;
            } else
            {

            }
            if (ModelState.IsValid)
            {
                Boolean result = AddUser(model.Username,model.FirstName,model.LastName, model.Password, model.EmailAddress, model.ContactNo, model.DOB, model.AuthLevel).Result;

                if(result == true)
                {
                    ViewData["Register"] = "true";
                    return View();
                }
                else
                {
                    ViewData["Register"] = "false";
                    return View();
                }
            }
            else
            {
                return View();
            }
            
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(RegisterViewModel model)
        {
            try
            {
                bool logStat = Login(model.Username, model.Password).Result;
                if(logStat == true)
                {
                    _ = RetrieveUser(model.Username);
                    return View("Index");
                } else
                {
                    ViewData["Login"] = "false";
                    return View();
                }
            } catch(Exception e)
            {
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return View("Index");
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult RoomLobby()
        {
            if(HttpContext.Session.GetInt32("uAuth") == 3 || HttpContext.Session.GetInt32("uAuth") == null)
            {
                return View();
            }
            else
            {
                String uname = HttpContext.Session.GetString("uName");
                _ = RetrieveRoomData(uname);

                return View();
            }
        }
        [HttpPost]
        public IActionResult RoomLobby(RoomViewModel model)
        {
            String uname = HttpContext.Session.GetString("uName");
            if (HttpContext.Session.GetString("uName") == null)
            {
                ViewData["AddRoom"] = "noLogin";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    bool result = NewRoom(model.RoomName, HttpContext.Session.GetString("uName")).Result;
                    if(result == true)
                    {
                        _ = RetrieveRoomData(uname);
                        ViewData["AddRoom"] = "true";
                        return View();
                    }
                    else
                    {
                        _ = RetrieveRoomData(uname);
                        ViewData["AddRoom"] = "error";
                    }
                }
                return View();
            }
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult ProfileManagement()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProfileManagement(RegisterViewModel model)
        {
            model.DOB = DateTime.Now;
            if (ModelState.IsValid)
            {
                bool result = UpdateProfile(model).Result;
                if(result == true)
                {
                    HttpContext.Session.SetString("uPass", model.Password);
                    HttpContext.Session.SetString("uMail", model.EmailAddress);
                    HttpContext.Session.SetString("uCont", model.ContactNo);
                    ViewData["UpdateProf"] = "true";
                }
                else
                {
                    ViewData["UpdateProf"] = "false";
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult Room()
        {
            var vm = new RoomItemViewModel();
            vm.ItemTypeList.ToList();

            _ = RetrieveItems(HttpContext.Session.GetString("rCode"));

            return View(vm);
        }

        [HttpPost]
        public IActionResult Room(RoomViewModel model)
        {
            HttpContext.Session.SetString("rName", model.RoomName);
            HttpContext.Session.SetString("rCode", model.RoomCode);
            HttpContext.Session.SetString("rLead", model.RoomLeader);

            var vm = new RoomItemViewModel();
            vm.ItemTypeList.ToList();

            _ = RetrieveItems(HttpContext.Session.GetString("rCode"));

            return View(vm);
        }

        //*******************************************************************************************************************************************************
        public IActionResult RoomJoin(ParticipantViewModel model)
        {
            String uname = HttpContext.Session.GetString("uName");
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("uName") == null || HttpContext.Session.GetString("uName") == ""
            || HttpContext.Session.GetInt32("uAuth") == 3 || HttpContext.Session.GetInt32("uAuth") == null)
                {
                    Boolean result = JoinRoom(model).Result;
                    if(result == true)
                    {
                        _ = RedirectRoom(model.RoomCode);

                        var vm = new RoomItemViewModel();
                        vm.ItemTypeList.ToList();

                        _ = RetrieveItems(HttpContext.Session.GetString("rCode"));

                        return View("Room", vm);
                    }
                    else
                    {
                        ViewData["JoinStat"] = "false";
                        return View("RoomLobby");
                    }
                }
                else
                {
                    model.Participant = HttpContext.Session.GetString("uName");
                    Boolean result = JoinRoom(model).Result;
                    if (result == true)
                    {
                        _ = RedirectRoom(model.RoomCode);

                        var vm = new RoomItemViewModel();
                        vm.ItemTypeList.ToList();

                        _ = RetrieveItems(HttpContext.Session.GetString("rCode"));

                        return View("Room", vm);
                    }
                    else
                    {
                        _ = RetrieveRoomData(uname);
                        ViewData["JoinStat"] = "false";
                        return View("RoomLobby");
                    }
                }
            }
            else
            {
                return View("RoomLobby");
            }
        }

        //*******************************************************************************************************************************************************
        public IActionResult AddItem(RoomItemViewModel model)
        {
            model.RoomCode = HttpContext.Session.GetString("rCode");
            var vm = new RoomItemViewModel();
            vm.ItemTypeList.ToList();
            if(ModelState.IsValid)
            {
                _ = AddItemCaller(model);
                _ = RetrieveItems(model.RoomCode);
                ModelState.Clear();
                return View("Room", vm);

            }
            else
            {
                _ = RetrieveItems(model.RoomCode);
                return View("Room", vm);
            }
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ContactUs(SupportTicketViewModel model)
        {
            Task<Boolean> result = SubmitTicket(model);
            if(result.Result == true)
            {
                ViewData["stStat"] = "true";
                return View();
            }
            else
            {
                ViewData["stStat"] = "false";
                return View();
            }
            
        }

        //*******************************************************************************************************************************************************
        public IActionResult RemoveItem(RoomItemViewModel model)
        {
            var vm = new RoomItemViewModel();
            vm.ItemTypeList.ToList();
            _ = DeleteItem(model);

            _ = RetrieveItems(model.RoomCode);
            return View("Room", vm);
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult CheckTicket()
        {
            _ = RetrieveTickets();
            return View();
        }

        [HttpPost]
        public IActionResult CheckTicket(SupportTicketViewModel model)
        {
            _ = UpdateTicket(model.Id);
            _ = RetrieveTickets();
            return View();
        }

        //*******************************************************************************************************************************************************
        public IActionResult RemoveTicket(SupportTicketViewModel model)
        {
            _ = DeleteTicket(model.Id);
            _ = RetrieveTickets();
            return View("CheckTicket");
        }


        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult Survey()
        {
            _ = PrepareQuestion();
            return View();
        }
        [HttpPost]
        public IActionResult Survey(SurveyAnswersViewModel model)
        {
            Int16 score;
            if(ModelState.IsValid)
            {
                score = (short)(model.Answer1 + model.Answer2 + model.Answer3 + model.Answer4 + model.Answer5 + model.Answer6 + model.Answer7 + model.Answer8 + model.Answer9 + model.Answer10);
                score = (short)(score * 2.5);

                if (HttpContext.Session.GetString("uName") == null || HttpContext.Session.GetString("uName") == ""
                    || HttpContext.Session.GetInt32("uAuth") == 3 || HttpContext.Session.GetInt32("uAuth") == null)
                {
                    _ = PrepareQuestion();
                    ViewData["sQuestionsErr"] = "error";
                    return View();
                } else
                {
                    bool result = SubmitSurvey(HttpContext.Session.GetString("uName"), score);
                    if(result == true)
                    {
                        ScoreRetrieverViewModel uScore = new ScoreRetrieverViewModel();
                        uScore = UserScore();
                        ViewData["uScore"] = uScore.UserScore;
                        ViewData["aScore"] = uScore.AverageScore;
                        return View("SurveyResult");
                    }
                    else
                    {
                        ViewData["sSubFail"] = "fail";
                        return View("SurveyRedirector");
                    }
                    
                }
            } 
            else
            {
                _ = PrepareQuestion();
                ViewData["sQuestionsErr"] = "error";
                return View();
            }
            
        }

        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult SurveyAdmin()
        {
            _ = GetQuestion();

            return View();
        }
        [HttpPost]
        public IActionResult SurveyAdmin(SurveyQuestionViewModel model)
        {
            if(model.Id == 0)
            {
                bool result = AddQuestion(model);
                if (result == true)
                {
                    ViewData["sAdmin"] = "Added";
                }
                else
                {
                    ViewData["sAdmin"] = "NotAdded";
                }
            }
            else
            {
                bool result = EditQuestion(model);
                if (result == true)
                {
                    ViewData["sAdmin"] = "Edited";
                }
                else
                {
                    ViewData["sAdmin"] = "NotEdited";
                }
            }
            
            _ = GetQuestion();
            return View();
        }
        //*******************************************************************************************************************************************************
        public IActionResult DeleteSurveyAdmin(SurveyQuestionViewModel model)
        {
            bool result = DeleteQuestion(model.Id);
            if (result == true)
            {
                ViewData["sAdmin"] = "Deleted";
            }
            else
            {
                ViewData["sAdmin"] = "NotDeleted";
            }
            _ = GetQuestion();
            return View("SurveyAdmin");
        }
        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult SurveyRedirector()
        {
            return View();
        }
        //*******************************************************************************************************************************************************
        public IActionResult SurveyResult()
        {
            ScoreRetrieverViewModel model = new ScoreRetrieverViewModel();
            if (HttpContext.Session.GetString("uName") == null || HttpContext.Session.GetString("uName") == ""
                    || HttpContext.Session.GetInt32("uAuth") == 3 || HttpContext.Session.GetInt32("uAuth") == null)
            {
                return View();
            }
            else
            {
                model = UserScore();
                if(model.AverageScore == 0 || model.UserScore == 0)
                {
                    ViewData["uScore"] = null;
                    ViewData["aScore"] = null;
                    return View();
                }
                else
                {
                    ViewData["uScore"] = model.UserScore;
                    ViewData["aScore"] = model.AverageScore;
                    return View();
                }

                
            }
        }
        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult TipsAdmin()
        {
            _ = GetTip();

            return View();
        }
        [HttpPost]
        public IActionResult TipsAdmin(TipsViewModel model)
        {
            if (model.Id == 0)
            {
                bool result = AddTip(model.Tip, model.TipTitle);
                if (result == true)
                {
                    ViewData["aTip"] = "Added";
                }
                else
                {
                    ViewData["aTip"] = "NotAdded";
                }
            }
            else
            {
                bool result = EditTip(model);
                if (result == true)
                {
                    ViewData["aTip"] = "Edited";
                }
                else
                {
                    ViewData["aTip"] = "NotEdited";
                }
            }

            _ = GetTip();
            return View();
        }
        //*******************************************************************************************************************************************************
        public IActionResult DeleteTipsAdmin(TipsViewModel model)
        {
            bool result = DeleteTip(model.Id);
            if (result == true)
            {
                ViewData["aTip"] = "Deleted";
            }
            else
            {
                ViewData["aTip"] = "NotDeleted";
            }
            _ = GetTip();
            return View("TipsAdmin");
        }
        //*******************************************************************************************************************************************************
        public IActionResult Tips()
        {
            _ = GetTip();
            return View();
        }
        //*******************************************************************************************************************************************************
        [HttpGet]
        public IActionResult CustomerProfileManagement()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerProfileManagement(RegisterViewModel model)
        {
            bool result = UpdateCustomerProfile(model);
            if(result == true)
            {
                ViewData["uCustProf"] = "Updated";
                return View();
            }
            else
            {
                ViewData["uCustProf"] = "NotUpdated";
                return View();
            }
        }
        //*******************************************************************************************************************************************************
        public IActionResult SearchCustomerProfile(String username)
        {
            RegisterViewModel result = new RegisterViewModel();
            result = RetrieveCustomerProfile(username);
            if(result.Username == null)
            {
                ViewData["uCustProf"] = "NotSearched";
            }
            else
            {
                ViewData["SearchResult"] = result;
            }
            return View("CustomerProfileManagement");
        }
        //*******************************************************************************************************************************************************

        //Methods
        private UserLogic userLogic = new UserLogic();


        //Call AddUser API
        public async Task<Boolean> AddUser(string username, string fname, string lname, string password, string emailAddress, string cnum, DateTime dob, Int32 authLevelId)
        {
            bool result = await userLogic.CreateNewUser(username, fname, lname, password, emailAddress, cnum, dob, authLevelId);
            return result;
        }
        //Call SearchUser API
        public async Task<Boolean> Login(string username, string password)
        {
            bool loginStatus = await userLogic.SearchUser(username, password);

            if (loginStatus == true)
            {
                HttpContext.Session.SetString("uName", username);
            }
            else
            {
                HttpContext.Session.SetString("uName", username);
            }

            return loginStatus;
        }
        //Call Retrieve User Data API
        public async Task<Boolean> RetrieveUser(string username)
        {
            try
            {
                var retdata = await userLogic.RetrieveUser(username);
                HttpContext.Session.SetString("uName", retdata.Username);
                HttpContext.Session.SetString("uFame", retdata.FirstName);
                HttpContext.Session.SetString("uLame", retdata.LastName);
                HttpContext.Session.SetString("uPass", retdata.Password);
                HttpContext.Session.SetString("uMail", retdata.Email);
                HttpContext.Session.SetString("uCont", retdata.ContactNo);
                HttpContext.Session.SetInt32("uAuth", retdata.AuthLevelRefId);
                return true;
            } catch(Exception e)
            {
                return false;
            }
        }

        //Call CreateRoom API
        public async Task<Boolean> NewRoom(string roomname, string roomleader)
        {
            bool result = await userLogic.CreateRoom(roomname, roomleader);
            return result;
        }

        //Call Update Profile API
        public async Task<Boolean> UpdateProfile(RegisterViewModel model)
        {
            bool result = await userLogic.UpdateProfile(model.Username, model.Password, model.EmailAddress, model.ContactNo);
            return result;
        }

        //Call Join Room API
        public async Task<Boolean> JoinRoom(ParticipantViewModel model)
        {
            bool result = await userLogic.JoinRoom(model.RoomCode, model.Participant);
            return result;
        }

        //Call Retrieve User Room Data API
        public async Task<Boolean> RetrieveRoomData(string username)
        {
            try {
                List<RoomViewModel> model = new List<RoomViewModel>();
                var result = await userLogic.RetrieveRoomData(username);
                if(result.Count == 0)
                {
                    ViewData["MyRooms"] = null;
                    return false;
                }
                else
                {
                    foreach (var x in result)
                    {
                        var y = new RoomViewModel()
                        {
                            RoomCode = x.RoomCode,
                            RoomLeader = x.RoomLeader,
                            RoomName = x.RoomName
                        };
                        model.Add(y);
                    }
                    ViewData["MyRooms"] = model;
                }
                return true;
            } catch(Exception e)
            {
                return false;
            }
            
        }

        //Call Add Item API
        public async Task<Boolean> AddItemCaller(RoomItemViewModel model)
        {
            Boolean result = await userLogic.AddItem(model.RoomCode, model.ItemName, model.ItemType, model.ItemExpiry, model.ItemQuantity);
            return result;
        }

        //Call Retrieve Items API
        public async Task<Boolean> RetrieveItems(string roomcode)
        {
            roomcode = HttpContext.Session.GetString("rCode");
            List<RoomItemViewModel> model = new List<RoomItemViewModel>();
            var result = await userLogic.RetrieveItems(roomcode);
            if(result.Count == 0)
            {
                ViewData["rItems"] = null;
                return false;
            }
            else
            {
                foreach(var x in result)
                {
                    var y = new RoomItemViewModel()
                    {
                        Id = x.Id,
                        ItemType = x.ItemType,
                        ItemName = x.ItemName,
                        ItemExpiry = x.ItemExpiry,
                        ItemQuantity = x.ItemQuantity,
                    };
                    model.Add(y);
                }
                ViewData["rItems"] = model;

                return true;
            }
        }

        //Call Room Redirect API
        public async Task<Boolean> RedirectRoom(string roomcode)
        {
            var result = await userLogic.RedirectRoom(roomcode);
            HttpContext.Session.SetString("rName", result.RoomName);
            HttpContext.Session.SetString("rCode", result.RoomCode);
            HttpContext.Session.SetString("rLead", result.RoomLeader);
            return true;
        }

        //Call submit ticket API
        public async Task<Boolean> SubmitTicket(SupportTicketViewModel model)
        {
            Boolean result = await userLogic.SubmitTicket(model.Name, model.Email, model.Subject, model.Message);
            if(result == true)
            {
                
                return true;
            }
            else
            {
                ViewData["stStat"] = "false";
                return false;
            }
        }

        //Call Delete Item API
        public async Task<Boolean> DeleteItem(RoomItemViewModel model)
        {
            _ = await userLogic.DeleteItem(model.Id);
            return true;
        }

        //Call Retrieve Tickets API
        public async Task<Boolean> RetrieveTickets()
        {
            List<SupportTicketViewModel> model = new List<SupportTicketViewModel>();
            var result = userLogic.RetrieveTickets();

            if (result.Count == 0)
            {
                ViewData["Tickets"] = null;
                return false;
            }
            else
            {
                foreach (var x in result)
                {
                    var y = new SupportTicketViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        Subject = x.Subject,
                        Message = x.Message,
                        Status = x.Status
                    };
                    model.Add(y);
                }
                ViewData["Tickets"] = model;

                return true;
            }
        }

        //Call Update Ticket API
        public bool UpdateTicket(Int32 id)
        {
            bool result = userLogic.UpdateTickets(id);
            if(result == true)
            {
                return true;
            } else
            {
                return false;
            }
        }

        //Call Delete Ticket API
        public bool DeleteTicket(Int32 id)
        {
            bool result = userLogic.DeleteTickets(id);
            if(result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Call Add Survey Question API
        public bool AddQuestion(SurveyQuestionViewModel model)
        {
            try
            {
                bool result = userLogic.AddQuestion(model.Question, model.Answer1, model.Answer2, model.Answer3, model.Answer4);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Call Get Survey Question API
        public bool GetQuestion()
        {
            List<SurveyQuestionViewModel> model = new List<SurveyQuestionViewModel>();
            var result = userLogic.GetQuestion();

            if (result.Count == 0)
            {
                ViewData["sQuestions"] = null;
                return false;
            }
            else
            {
                foreach (var x in result)
                {
                    var y = new SurveyQuestionViewModel()
                    {
                        Question = x.Question,
                        Answer1 = x.Answer1,
                        Answer2 = x.Answer2,
                        Answer3 = x.Answer3,
                        Answer4 = x.Answer4,
                        Id = x.Id
                    };
                    model.Add(y);
                }
                ViewData["sQuestions"] = model;

                return true;
            }
        }

        //Call Edit Survey Question API
        public bool EditQuestion(SurveyQuestionViewModel model)
        {
            bool result = userLogic.EditQuestion(model.Id, model.Question, model.Answer1, model.Answer2, model.Answer3, model.Answer4);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Call Delete Survey Question API
        public bool DeleteQuestion(Int32 id)
        {
            bool result = userLogic.DeleteQuestion(id);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Call Prepare Survey Question API
        public bool PrepareQuestion()
        {
            List<SurveyQuestionViewModel> model = new List<SurveyQuestionViewModel>();
            var result = userLogic.PrepareQuestion();
            if(result.Count == 0)
            {
                ViewData["sQuestions"] = null;
                return false;
            }
            else
            {
                foreach(var x in result)
                {
                    var y = new SurveyQuestionViewModel()
                    {
                        Question = x.Question,
                        Answer1 = x.Answer1,
                        Answer2 = x.Answer2,
                        Answer3 = x.Answer3,
                        Answer4 = x.Answer4
                    };
                    model.Add(y);
                }
                ViewData["sQuestions"] = model;
                return true;
            }
        }

        //Call Submit Survey API
        public bool SubmitSurvey(string username, Int16 score)
        {
            bool result = userLogic.SubmitSurvey(username, score);
            return result;
        }

        //Call Retrieve User Score API
        public ScoreRetrieverViewModel UserScore()
        {
            var result = userLogic.UserScore(HttpContext.Session.GetString("uName"));
            ScoreRetrieverViewModel model = new ScoreRetrieverViewModel()
            {
                UserScore = result.UserScore,
                AverageScore = result.AverageScore
            };
            return model;
        }

        //Call Add Tip API
        public bool AddTip(string tip, string tiptitle)
        {
            try
            {
                bool result = userLogic.AddTip(tip, tiptitle);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Call Get Survey Question API
        public bool GetTip()
        {
            List<TipsViewModel> model = new List<TipsViewModel>();
            var result = userLogic.GetTip();

            if (result.Count == 0)
            {
                ViewData["Tip"] = null;
                return false;
            }
            else
            {
                foreach (var x in result)
                {
                    var y = new TipsViewModel()
                    {
                        Tip = x.Tips,
                        TipTitle = x.TipTitle,
                        Id = x.Id
                    };
                    model.Add(y);
                }
                ViewData["Tip"] = model;

                return true;
            }
        }

        //Call Edit Survey Question API
        public bool EditTip(TipsViewModel model)
        {
            bool result = userLogic.EditTip(model.Id, model.Tip, model.TipTitle);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Call Delete Survey Question API
        public bool DeleteTip(Int32 id)
        {
            bool result = userLogic.DeleteTip(id);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Call Update Customer Profile API
        public bool UpdateCustomerProfile(RegisterViewModel model)
        {
            bool result = userLogic.UpdateCustomerProfile(model.Username, model.FirstName, model.LastName, model.Password, model.EmailAddress, model.ContactNo, model.DOB);
            if(result == true)
            { 
                return true;
            }
            else 
            { 
                return false;
            }
        }

        //Call Retrieve Customer Profile API
        public RegisterViewModel RetrieveCustomerProfile(string username)
        {
            RegisterViewModel model = new RegisterViewModel();
            var result = userLogic.RetrieveCustomerProfile(username);
            if(result == null)
            {

            }
            else
            {
                model.Username = result.Username;
                model.FirstName = result.FirstName;
                model.LastName = result.LastName;
                model.EmailAddress = result.Email;
                model.ContactNo = result.ContactNo;
                model.DOB = result.DOB;
                model.Password = result.Password;
            }
            

            return model;
        }

        //Auto Gen
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
