using FoodEDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodEDAL.Interface
{
    public interface IUser
    {
        Task<Boolean> AddUser(string username, string fname, string lname, string password, string emailAddress, string cnum, DateTime dob, Int32 authlevelid);
        Task<List<User>> GetAllUsers();
        Task<User> SearchUser(string username, string password);
        Task<User> RetrieveUser(string username);
        Task<Room> CreateRoom(string roomname, string roomleader);
        Task<Boolean> UpdateProfile(string username, string password, string email, string contno);
        Task<Boolean> JoinRoom(string roomcode, string username);
        Task<List<Room>> RetrieveRoomData(string username);
        Task<Boolean> AddItem(string roomcode, string itemname, string itemtype, DateTime itemexp, Decimal itemquan);
        Task<List<RoomItem>> RetrieveItems(string roomcode);
        Task<Room> RedirectRoom(string roomcode);
        Task<Boolean> SubmitTicket(string name, string email, string subject, string message);
        Task<Boolean> DeleteItem(Int32 id);
        List<SupportTicket> RetrieveTickets();
        bool UpdateTickets(Int32 id);
        bool DeleteTickets(Int32 id);
        bool AddQuestion(string question, string ans1, string ans2, string ans3, string ans4);
        List<SurveyQuestion> GetQuestion();
        bool EditQuestion(Int32 id, string question, string ans1, string ans2, string ans3, string ans4);
        bool DeleteQuestion(Int32 id);
        List<SurveyQuestion> PrepareQuestion();
        bool SubmitSurvey(string username, Int16 score);
        ScoreRetriever UserScore(string username);
        bool AddTip(string tip, string tiptitle);
        List<Tip> GetTip();
        bool EditTip(Int32 id, string tip, string tiptitle);
        bool DeleteTip(Int32 id);
        bool UpdateCustomerProfile(string username, string fname, string lname, string password, string emailAddress, string cnum, DateTime dob);
        User RetrieveCustomerProfile(string username);
    }
}
