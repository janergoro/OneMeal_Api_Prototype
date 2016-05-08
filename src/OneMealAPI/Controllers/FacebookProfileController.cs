using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OneMealAPI.Models;
using Microsoft.AspNet.Authorization;
using OneMealAPI.Custom.Database;
using System.Data.Linq;

namespace OneMealAPI.Controllers
{
    [Route("api/[controller]")]
    public class FacebookProfileController : Controller
    {
        // GET: api/FacebookProfile
        [HttpGet]
        public List<Meals> Get()
        {
            /*
            DatabaseConnection db = DatabaseConnection.GetDBConnection();
            List<FaceBookProfile> profiles= db..Where(x => x.MealDate >= DateTime.UtcNow).ToList();
            return activeMeals;*/
            return null;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public List<FaceBookProfile> Get(int id)
        {

            /*DatabaseConnection db = DatabaseConnection.GetDBConnection();
            Meals currentMeal = db.Meals.Where(x => x.Id == id).FirstOrDefault();
            if (currentMeal == null) throw new Exception("Meal with current ID can not be found, ID= " + id);
            List<FaceBookProfile> profile = db.Profiles.Where(x => x.Id == currentMeal.UserID).ToList();
            if (profile== null || profile.FirstOrDefault() == null) throw new Exception("User with current ID can not be found, userFacebookId= " + currentMeal.UserID);
            return profile;*/
            return null;

        }

        // POST api/values
        [HttpPost]
        //[AllowAnonymous]
        public bool Post([FromBody]Data data)
        {
            if(data != null)
            {
                DatabaseConnection db = DatabaseConnection.GetDBConnection();
                if (data.userId == null) throw new Exception("userID is empty - UiD: " + data.userId);
                FaceBookProfile profile = db.Profiles.Where(i => i.UserId == data.userId).FirstOrDefault();
                if (profile == null)
                {
                    profile = CreateNewProfile(data, db, data.userId);
                    db.Profiles.InsertOnSubmit(profile);
                    db.SubmitChanges();
                }
                else
                {
                    profile.About = data.About;
                    profile.Keywords = data.Keywords;
                    profile.Profession = data.Profession;
                    db.SubmitChanges();
                }
                return true;
            }
            return false;

        }

        private FaceBookProfile CreateNewProfile(Data data, DatabaseConnection db, string userId)
        {
            DateTime bday;
            DateTime.TryParse(data.Birthday, out bday);
            FaceBookProfile newProfile = new FaceBookProfile()
            {
                Birthday =  bday,
                UserId = userId,
                Name = data.FirstName + " " + data.LastName,
                About = data.About,
                Keywords = data.Keywords,
                Profession = data.Profession
            };
            return newProfile;
        }

        public class Data
        {
            // the JSON to Model mapper match is case-insensitive
            public string userId { get; set; }
            public string token { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Profession { get; set; }
            public string Birthday { get; set; }
            public string About { get; set; }
            public string Keywords { get; set; }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

