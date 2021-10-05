using FoodEDAL.DataContext;
using FoodEDAL.Entities;
using FoodEDAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace FoodEDAL.Functions
{
    public class UserFunctions : IUser
    {
        //Register User
        public async Task<Boolean> AddUser(string username, string fname, string lname, string password, string emailAddress, string cnum, DateTime dob, int authLevelId)
        {
            User newUser = new User
            {
                Username = username,
                FirstName = fname,
                LastName = lname,
                Password = password,
                Email = emailAddress,
                ContactNo = cnum,
                DOB = dob,
                AuthLevelRefId = authLevelId
            };
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                var srUser = (from user in context.Users where user.Username == username select user).SingleOrDefault();

                if (srUser == null)
                {
                    await context.Users.AddAsync(newUser);
                    await context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        //Get All Users
        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                users = await context.Users.ToListAsync();
            }
            return users;
        }

        //Search User
        public async Task<User> SearchUser(string username, string password)
        {
            User userExist = new User();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                //var srUser = context.Users.Find(username);
                var srUser = (from user in context.Users where user.Username == username && user.Password == password select user).SingleOrDefault();
                if (srUser == null)
                {
                    userExist.Email = null;
                    userExist.Password = null;
                    userExist.Username = null;
                    userExist.AuthLevelRefId = 3;
                }
                else
                {
                    userExist.Password = password;
                    userExist.Username = username;
                }

            }
            return userExist;
        }

        //Retrieve User Data
        public async Task<User> RetrieveUser(string username)
        {
            User retUser = new User();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                var srUser = context.Users.Find(username);
                retUser.Username = srUser.Username;
                retUser.Password = srUser.Password;
                retUser.FirstName = srUser.FirstName;
                retUser.LastName = srUser.LastName;
                retUser.Email = srUser.Email;
                retUser.ContactNo = srUser.ContactNo;
                retUser.DOB = srUser.DOB;
                retUser.AuthLevelRefId = srUser.AuthLevelRefId;
            }
            return retUser;
        }
        //Create new Room
        public async Task<Room> CreateRoom(string roomname, string roomleader)
        {
            string id = genCode();

            Room newRoom = new Room
            {
                RoomCode = id,
                RoomName = roomname,
                RoomLeader = roomleader
            };
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                while (context.Rooms.Any(x => x.RoomCode == id))
                {
                    id = genCode();
                }
                newRoom.RoomCode = id;

                await context.Rooms.AddAsync(newRoom);
                await context.SaveChangesAsync();
            }
            return newRoom;
        }

        //Random Number Generator
        public static string genCode()
        {
            Random generator = new Random();
            string r = generator.Next(100000, 1000000).ToString();

            return r;
        }

        //Update User Data
        public async Task<Boolean> UpdateProfile(string username, string password, string email, string contno)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var updprof = await context.Users.FindAsync(username);
                    updprof.Password = password;
                    updprof.Email = email;
                    updprof.ContactNo = contno;

                    await context.SaveChangesAsync();

                    return true;
                }
                catch 
                {
                    return false;
                }

            }
        }

        //Join Room
        public async Task<Boolean> JoinRoom(string roomcode, string username)
        {
            RoomParticipant model = new RoomParticipant();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                var srRoom = (from room in context.Rooms where room.RoomCode == roomcode select room).SingleOrDefault();
                var srRoomPart = (from roompart in context.RoomParticipants where roompart.RoomCode == roomcode && roompart.Participant == username select roompart)
                    .SingleOrDefault();
                if (srRoom != null && srRoom.RoomCode == roomcode)
                {
                    if (username == null || username == "")
                    {
                        return true;
                    }
                    else
                    {
                        if (srRoomPart != null)
                        {
                            return true;
                        }
                        else if (srRoom.RoomLeader != username)
                        {
                            model.Participant = username;
                            model.RoomCode = roomcode;

                            await context.RoomParticipants.AddAsync(model);
                            await context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        //Retrieve Users Rooms
        public async Task<List<Room>> RetrieveRoomData(string username)
        {
            List<Room> roomdata = new List<Room>();
            List<RoomParticipant> roomprdata = new List<RoomParticipant>();

            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                var retdata = (from room in context.Rooms
                               where room.RoomLeader == username
                               select new Room
                               {
                                   RoomCode = room.RoomCode,
                                   RoomId = room.RoomId,
                                   RoomLeader = room.RoomLeader,
                                   RoomName = room.RoomName
                               }).ToList();

                roomdata.AddRange(retdata);

                //roomprdata = await (from roompr in context.RoomParticipants where roompr.Participant == username select roompr).ToListAsync();
                roomprdata = (from roompr in context.RoomParticipants where roompr.Participant == username select roompr).ToList();

                roomprdata.ForEach(x =>
                {
                    if (x.Participant == username)
                    {
                        roomdata.AddRange((from room in context.Rooms where x.RoomCode == room.RoomCode select room));
                    }
                });

                return roomdata;
            }
        }

        //Add Items
        public async Task<Boolean> AddItem(string roomcode, string itemname, string itemtype, DateTime itemexp, Decimal itemquan)
        {
            RoomItem model = new RoomItem()
            {
                RoomCode = roomcode,
                ItemName = itemname,
                ItemType = itemtype,
                ItemExpiry = itemexp,
                ItemQuantity = itemquan
            };

            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    _ = context.RoomItems.AddAsync(model);
                    _ = context.SaveChangesAsync();
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
        }

        //Retrieve Items
        public async Task<List<RoomItem>> RetrieveItems(string roomcode)
        {
            List<RoomItem> model = new List<RoomItem>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                model = (from item in context.RoomItems where item.RoomCode == roomcode select item).ToList();
                return model;
            }
        }

        //Redirect Room
        public async Task<Room> RedirectRoom(string roomcode)
        {
            Room model = new Room();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                //var srRoom = context.Rooms.Find(roomcode);
                var srRoom = (from room in context.Rooms where room.RoomCode == roomcode select room).SingleOrDefault();
                model.RoomCode = srRoom.RoomCode;
                model.RoomLeader = srRoom.RoomLeader;
                model.RoomName = srRoom.RoomName;
                return model;
            }
        }

        //Submit Ticket
        public async Task<Boolean> SubmitTicket(string name, string email, string subject, string message)
        {
            SupportTicket model = new SupportTicket()
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message,
                Status = "Available"
            };
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    await context.SupportTickets.AddAsync(model);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch 
                {
                    return false;
                }

            }
        }

        //Delete Item
        public async Task<Boolean> DeleteItem(Int32 id)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var result = context.RoomItems.Find(id);
                    if(result == null)
                    {
                        return false;
                    }
                    else
                    {
                        context.RoomItems.Remove(context.RoomItems.Find(id));
                        context.SaveChanges();
                        return true;
                    }
                    
                }
                catch 
                {
                    return false;
                }

            }
        }

        //Retrieve Tickets
        public List<SupportTicket> RetrieveTickets()
        {
            List<SupportTicket> model = new List<SupportTicket>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                model = context.SupportTickets.ToList();
            }
            return model;
        }

        //Update Ticket Status
        public bool UpdateTickets(Int32 id)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var model = context.SupportTickets.Find(id);
                    if(model == null)
                    {
                        return false;
                    }
                    else
                    {
                        model.Status = "Taken";

                        context.SaveChanges();

                        return true;
                    }
                    
                }
                catch 
                {
                    return false;
                }

            }
        }

        //Delete Ticket
        public bool DeleteTickets(Int32 id)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var result = context.SupportTickets.Find(id);
                    if(result == null)
                    {
                        return false;
                    }
                    else
                    {
                        context.SupportTickets.Remove(context.SupportTickets.Find(id));
                        context.SaveChanges();
                        return true;
                    }
                    
                }
                catch
                {
                    return false;
                }

            }
        }

        //Add Survey Question
        public bool AddQuestion(string question, string ans1, string ans2, string ans3, string ans4)
        {
            SurveyQuestion model = new SurveyQuestion()
            {
                Question = question,
                Answer1 = ans1,
                Answer2 = ans2,
                Answer3 = ans3,
                Answer4 = ans4
            };
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    context.SurveyQuestions.Add(model);
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
                
            }
        }
        //Get Survey Question
        public List<SurveyQuestion> GetQuestion()
        {
            List<SurveyQuestion> model = new List<SurveyQuestion>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                model = context.SurveyQuestions.ToList();
                return model;
            }
        }
        //Edit Survey Question
        public bool EditQuestion(Int32 id, string question, string ans1, string ans2, string ans3, string ans4)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var model = context.SurveyQuestions.Find(id);
                    model.Question = question;
                    model.Answer1 = ans1;
                    model.Answer2 = ans2;
                    model.Answer3 = ans3;
                    model.Answer4 = ans4;

                    context.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        //Delete Survey Question
        public bool DeleteQuestion(Int32 id)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var result = context.SurveyQuestions.Find(id);
                    if(result == null)
                    {
                        return false;
                    }
                    else
                    {
                        context.SurveyQuestions.Remove(context.SurveyQuestions.Find(id));
                        context.SaveChanges();
                        return true;
                    }
                    
                }
                catch
                {
                    return false;
                }

            }
        }

        //Prepare Survey Question
        public List<SurveyQuestion> PrepareQuestion()
        {
            List<SurveyQuestion> count = new List<SurveyQuestion>();
            List<SurveyQuestion> model = new List<SurveyQuestion>();
            SurveyQuestion temp = new SurveyQuestion();

            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                count = context.SurveyQuestions.ToList();
                for(int i = 0; i < 10; i++)
                {
                    Int32 r = new Random().Next(count.Count);
                    temp = context.SurveyQuestions.Skip(r).FirstOrDefault();
                    if(model.Any(item => item.Id == temp.Id) == true)
                    {
                        i--;
                    } else
                    {
                        model.Add(temp);
                    }
                    
                }
            }

            return model;
        }

        //Submit Survey Question
        public bool SubmitSurvey(string username, Int16 score)
        {
            SurveyScore model = new SurveyScore()
            {
                Username = username,
                Score = score
            };
            SurveyScore check = new SurveyScore();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    check = (from x in context.SurveyScores where x.Username == username select x).SingleOrDefault();

                    if(check == null)
                    {
                        context.SurveyScores.Add(model);
                        context.SaveChanges();
                    } else
                    {
                        var updscore = context.SurveyScores.Find(username);
                        updscore.Score = score;
                        context.SaveChanges();
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        //Retrieve Average Score
        public Int16 AverageScore()
        {
            Int16 score = 0;
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                List<SurveyScore> model = new List<SurveyScore>();
                model = context.SurveyScores.ToList();

                foreach(var x in model)
                {
                    score = (short)(score + x.Score);
                }
                score = (short)(score / model.Count);
                return score;
            }
        }

        //Retrieve User Score
        public ScoreRetriever UserScore(string username)
        {
            ScoreRetriever model = new ScoreRetriever();
            SurveyScore result = new SurveyScore();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                result = context.SurveyScores.Find(username);
                if(result == null)
                {
                    return model;
                }
                Int16 aScore = AverageScore();
                model.UserScore = result.Score;
                model.AverageScore = aScore;
                return model;
            }
        }

        //Add Tip
        public bool AddTip(string tip, string tiptitle)
        {
            Tip model = new Tip()
            {
                Tips = tip,
                TipTitle = tiptitle
            };
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    context.Tips.Add(model);
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        //Get Tip
        public List<Tip> GetTip()
        {
            List<Tip> model = new List<Tip>();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                model = context.Tips.ToList();
                return model;
            }
        }
        //Edit Tip
        public bool EditTip(Int32 id, string tip, string tiptitle)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var model = context.Tips.Find(id);
                    model.TipTitle = tiptitle;
                    model.Tips = tip;

                    context.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        //Delete Tip
        public bool DeleteTip(Int32 id)
        {
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                try
                {
                    var result = context.Tips.Find(id);

                    if(result == null)
                    {
                        return false;
                    }
                    else
                    {
                        context.Tips.Remove(context.Tips.Find(id));
                        context.SaveChanges();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }

            }
        }

        //Update Customer Profile
        public bool UpdateCustomerProfile(string username, string fname, string lname, string password, string emailAddress, string cnum, DateTime dob)
        {
            User result = new User();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {

                try
                {
                    result = context.Users.Find(username);
                    if(result == null)
                    {
                        return false;
                    }
                    else
                    {
                        result.FirstName = fname;
                        result.LastName = lname;
                        result.Email = emailAddress;
                        result.DOB = dob;
                        result.ContactNo = cnum;
                        result.Password = password;

                        context.SaveChanges();

                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        //Retrieve Customer Profile
        public User RetrieveCustomerProfile(string username)
        {
            User result = new User();
            using (var context = new DatabaseContext(DatabaseContext.ops.dbOptions))
            {
                result = context.Users.Find(username);
                return result;
            }
        }
    }
}

