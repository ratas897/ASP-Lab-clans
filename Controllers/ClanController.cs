using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP_Lab_clans.Models;

namespace ASP_Lab_clans.Controllers
{
    public class ClanController : Controller
    {
        [Authorize]
        public ActionResult Chat()
        {
            string username = HttpContext.User.Identity.Name;
            var isInClan = IsInClan(username);
            if (isInClan)
            {
                using (Database1Entities dc = new Database1Entities())
                {
                    var Player_data = dc.Users.FirstOrDefault(a => a.Username == username);
                    var currentclan = dc.Clan_member.Include("Clan").FirstOrDefault(a => a.User_ID == Player_data.UserID);
                    return View(currentclan);
                }
            }
            else
            {
                return RedirectToAction("JoinClan", "Clan");
            }
        }
        //Clan info
        [Authorize]
        public ActionResult Clan()
        {

            string username = User.Identity.Name;
            if (username == null)
            {
                return RedirectToAction("Login", "User");

            }
            var isInClan = IsInClan(username);
            if (isInClan)
            {
                using (Database1Entities dc = new Database1Entities())
                {
                    var Player_data = dc.Users.FirstOrDefault(a => a.Username == username);
                    var b = dc.Clan_member.FirstOrDefault(a => a.User_ID == Player_data.UserID);
                    List<Clan_member> list_clanmembers = dc.Clan_member.Include("Clan").Include("User").Include("Clan_roles").Where(a => a.Clan_ID == b.Clan_ID).ToList();
                    return View(list_clanmembers);
                }
            }
            else
            {
                return RedirectToAction("JoinClan", "Clan");
            }
            
        }

        //Join clan
        [Authorize]
        public ActionResult JoinClan()
        {
            string username = HttpContext.User.Identity.Name;
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            var isInClan = IsInClan(username);
                if (isInClan)
                {
                    return RedirectToAction("Clan", "Clan");
                }
                else
                {
                    using (Database1Entities dc = new Database1Entities())
                    {
                        List<Clan> list_clans = dc.Clans.ToList();
                        return View(list_clans);
                    }
                }
            
            
        }
        //Join clan POST
        [Authorize]
        public ActionResult PostJoinClan(int ClanID)
        {
            string username = HttpContext.User.Identity.Name;
            var isInClan = IsInClan(username);
            if (isInClan)
            {
                return RedirectToAction("Clan", "Clan");
            }

            using (Database1Entities dc = new Database1Entities())
            {
                var clanmember = new Clan_member();
                clanmember.Clan_ID = ClanID;
                var Player_data = dc.Users.FirstOrDefault(a => a.Username == username);
                clanmember.User_ID = Player_data.UserID;
                clanmember.Role_ID = 0;
                dc.Clan_member.Add(clanmember);
                var members = dc.Clans.SingleOrDefault(a => a.Id == ClanID);
                int? add = members.Clan_total_members+1;
                members.Clan_total_members = add;
                dc.SaveChanges();
                return RedirectToAction("Clan","Clan");
            }

        }


        //Create a clan
        [Authorize]
        [HttpGet]
        public ActionResult CreateClan()
        {
            return View();
        }
        //Clan POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClan(ClanClass clan)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                #region Clan exists
                var isExist = DoesClanExist(clan.Clan_name);
                if (isExist)
                {
                    ModelState.AddModelError("ClanExist", "Clan with that name already exists");
                    return View(clan);
                }
                #endregion
                #region Tag already exists
                var isExistUser = DoesTagExist(clan.Clan_tag);
                if (isExistUser)
                {
                    ModelState.AddModelError("TagExist", "A clan with that tag already exists");
                    return View(clan);
                }
                #endregion

                #region Saving to database
                using (Database1Entities database = new Database1Entities())
                {
                    Clan klanas = new Clan();
                    klanas.Clan_leader = HttpContext.User.Identity.Name;
                    klanas.Clan_message = clan.Clan_message;
                    klanas.Clan_name = clan.Clan_name;
                    klanas.Clan_tag = clan.Clan_tag;
                    klanas.Clan_total_members = 1;
                    database.Clans.Add(klanas);
                    database.SaveChanges();

                    string username = HttpContext.User.Identity.Name;
                    var clanmember = new Clan_member();
                    var clan_id = database.Clans.FirstOrDefault(a => a.Clan_name == klanas.Clan_name).Id;
                    var Player_data = database.Users.FirstOrDefault(a => a.Username == username);
                    clanmember.Clan_ID = clan_id;
                    clanmember.User_ID = Player_data.UserID;
                    clanmember.Role_ID = 2;
                    database.Clan_member.Add(clanmember);
                    database.SaveChanges();
                }

                #endregion

                Status = true;
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return RedirectToAction("Clan", "Clan");
        }

        //Leave clan
        [Authorize]
        public ActionResult Leaveclan()
        {
            using (Database1Entities dc = new Database1Entities())
            {
                
                string username = HttpContext.User.Identity.Name;
                var isexist = IsInClan(username);
                if (isexist)
                {
                    var c = dc.Clan_member.FirstOrDefault(a => a.User.Username == username);
                    var clan_id = c.Clan_ID;
                    var bla = dc.Clans.FirstOrDefault(a => a.Id == clan_id);
                    int? value = bla.Clan_total_members - 1;
                    bla.Clan_total_members = value;
                    if (value == 0)
                    {
                        var PlayerData = dc.Users.FirstOrDefault(a => a.Username == username);
                        var CurrrentClanID = dc.Clan_member.FirstOrDefault(a => a.User_ID == PlayerData.UserID);
                        List<Message> Messages = dc.Messages.Where(a => a.Clan_ID == CurrrentClanID.Clan_ID).ToList();
                        dc.Messages.RemoveRange(Messages);
                        dc.SaveChanges();
                        var remove = dc.Clans.FirstOrDefault(a => a.Id == clan_id);
                        dc.Clans.Remove(remove);
                    }
                    var h = dc.Clan_member.Remove(c);
                    dc.SaveChanges();
                }
            }
            return RedirectToAction("Clan","Clan");
        }
        public bool IsInClan(string user)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var Player_data = dc.Users.FirstOrDefault(a => a.Username == user);
                if (Player_data != null)
                {
                    var v = dc.Clan_member.Where(a => a.User_ID == Player_data.UserID).FirstOrDefault();
                    return v != null;
                }
                return false;
            }
        }
        public bool DoesClanExist(string clan)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Clans.FirstOrDefault(a => a.Clan_name == clan);
                return v != null;
            }
        }
        public bool DoesTagExist(string clan)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.Clans.FirstOrDefault(a => a.Clan_tag == clan);
                return v != null;
            }
        }

    }

    

}