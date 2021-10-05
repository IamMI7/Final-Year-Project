using FoodEDAL.Entities;
using FoodEDAL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodEAPI.UserLogic
{
    public class UserLogic
    {
        private IUser _user = new FoodEDAL.Functions.UserFunctions();

        //Register New User
        public async Task<Boolean> CreateNewUser(string username, string fname, string lname, string password, string emailAddress, string cnum, DateTime dob, int authLevelId)
        {
            try
            {
                var result = await _user.AddUser(username, fname, lname, password, emailAddress, cnum, dob, authLevelId);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Get All Users
        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await _user.GetAllUsers();
            return users;
        }

        //Search User
        public async Task<Boolean> SearchUser(string username, string password)
        {
            try
            {
                var result = await _user.SearchUser(username, password);
                if (result.Username == username && result.Password == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }
        //Retrieve User Data
        public async Task<User> RetrieveUser(string username)
        {
            var result = await _user.RetrieveUser(username);
            return result;
        }

        //Create new Room
        public async Task<Boolean> CreateRoom(string roomname, string roomleader)
        {
            try
            {
                var result = await _user.CreateRoom(roomname, roomleader);
                if (result.RoomName != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Update User Profile
        public async Task<Boolean> UpdateProfile(string username, string password, string email, string contno)
        {
            try
            {
                Boolean result = await _user.UpdateProfile(username, password, email, contno);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Join Room
        public async Task<Boolean> JoinRoom(string roomcode, string username)
        {
            try
            {
                Boolean result = await _user.JoinRoom(roomcode, username);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Retrieve User Rooms
        public async Task<List<Room>> RetrieveRoomData(string username)
        {
            List<Room> result = await _user.RetrieveRoomData(username);
            return result;
        }

        //Add Item
        public async Task<Boolean> AddItem(string roomcode, string itemname, string itemtype, DateTime itemexp, Decimal itemquan)
        {
            try
            {
                Boolean result = await _user.AddItem(roomcode, itemname, itemtype, itemexp, itemquan);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Retrieve Items
        public async Task<List<RoomItem>> RetrieveItems(string roomcode)
        {
            List<RoomItem> result = await _user.RetrieveItems(roomcode);
            return result;
        }

        //Redirect Room
        public async Task<Room> RedirectRoom(string roomcode)
        {
            Room result = await _user.RedirectRoom(roomcode);
            return result;
        }

        //Submit Ticket
        public async Task<Boolean> SubmitTicket(string name, string email, string subject, string message)
        {
            try
            {
                Boolean result = await _user.SubmitTicket(name, email, subject, message);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        //Delete Item
        public async Task<Boolean> DeleteItem(Int32 id)
        {
            try
            {
                Boolean result = await _user.DeleteItem(id);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Retrieve Tickets
        public List<SupportTicket> RetrieveTickets()
        {
            List<SupportTicket> result = _user.RetrieveTickets();
            return result;
        }

        //Update Ticket
        public bool UpdateTickets(Int32 id)
        {
            bool result = _user.UpdateTickets(id);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete Ticket
        public bool DeleteTickets(Int32 id)
        {
            bool result = _user.DeleteTickets(id);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Add Survey Question
        public bool AddQuestion(string question, string ans1, string ans2, string ans3, string ans4)
        {
            try
            {
                Boolean result = _user.AddQuestion(question, ans1, ans2, ans3, ans4);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //Get Survey Question
        public List<SurveyQuestion> GetQuestion()
        {
            List<SurveyQuestion> result = _user.GetQuestion();
            return result;
        }
        //Edit Survey Question
        public bool EditQuestion(Int32 id, string question, string ans1, string ans2, string ans3, string ans4)
        {
            bool result = _user.EditQuestion(id, question, ans1, ans2, ans3, ans4);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete Survey Question
        public bool DeleteQuestion(Int32 id)
        {
            bool result = _user.DeleteQuestion(id);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Prepare Survey Question
        public List<SurveyQuestion> PrepareQuestion()
        {
            List<SurveyQuestion> result = _user.PrepareQuestion();
            return result;
        }

        //Submit Survey Question
        public bool SubmitSurvey(string username, Int16 score)
        {
            bool result = _user.SubmitSurvey(username, score);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Retrieve User Score
        public ScoreRetriever UserScore(string username)
        {
            ScoreRetriever model = new ScoreRetriever();
            model = _user.UserScore(username);
            return model;
        }

        //Add Tip
        public bool AddTip(string tip, string tiptitle)
        {
            try
            {
                Boolean result = _user.AddTip(tip, tiptitle);
                if (result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //Get Tips
        public List<Tip> GetTip()
        {
            List<Tip> result = _user.GetTip();
            return result;
        }
        //Edit Tip
        public bool EditTip(Int32 id, string tip, string tiptitle)
        {
            bool result = _user.EditTip(id, tip, tiptitle);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete Tip
        public bool DeleteTip(Int32 id)
        {
            bool result = _user.DeleteTip(id);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Update Customer Profile
        public bool UpdateCustomerProfile(string username, string fname, string lname, string password, string emailAddress, string cnum, DateTime dob)
        {
            bool result = _user.UpdateCustomerProfile(username, fname, lname, password, emailAddress, cnum, dob);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Retrieve Customer Profile
        public User RetrieveCustomerProfile(string username)
        {
            User result = new User();
            result = _user.RetrieveCustomerProfile(username);
            return result;
        }
    }
}
