using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OneMealAPI.Models;
using Microsoft.AspNet.Authorization;
using OneMealAPI.Custom.Database;
using System.Data.Linq;
using OneMealAPI.Custom.Extensions;
using OneMealAPI.Custom.BusinessLogic;

namespace OneMealAPI.Controllers
{
    /*
    #TODO
    Refacto one Get
    Add Get(x,y, distance)
    Alter Post(datetime,user) to Post(datetime, user, location)
    */
    [Route("api/[controller]")]
    public class ActiveProfilesController : Controller
    {
        DatabaseConnection db;
        const double latitudeUT = 58.379058;
        const double longitudeUT = 26.722544;
        // GET: api/activeprofiles
        [HttpGet ("{latitude}/{longitude}/{distance}")]
        public List<OpenMeal> Get(double latitude, double longitude, double distance)
        {
            double distFromUT = GeoLocationCalc.CalcDistance(latitude, longitude, latitudeUT, longitudeUT, GeoLocationCalc.GeoCodeCalcMeasurement.Kilometers);
            if (distFromUT > distance)
                return null;
            db = DatabaseConnection.GetDBConnection();
            List<Meals> activeMeals = db.Meals.Where(x => x.MealDate >= DateTime.Now && x.PartnerID == null).ToList();
            List<OpenMeal> distinctmealInfo = FillProfileInfo(db, activeMeals);
            List<OpenMeal> mealInfo = new List<OpenMeal>();
            foreach (Meals meal in activeMeals)
            {
                var distinctMeal = distinctmealInfo.Where(i => i.UserID == meal.UserID).FirstOrDefault();
                if (distinctMeal == null) continue;
                OpenMeal newMeal = new OpenMeal();
                newMeal.Age = distinctMeal.Age;
                newMeal.Name = distinctMeal.Name;
                newMeal.Profession = distinctMeal.Profession;
                newMeal.UserID = meal.UserID;
                newMeal.MealDate = meal.MealDate;
                newMeal.Id = meal.Id;
                newMeal.Location = meal.Location;
                mealInfo.Add(newMeal);

            }
            db.Dispose();
            return mealInfo;
        }

        private List<OpenMeal> FillProfileInfo(DatabaseConnection db, List<Meals> activeMeals)
        {
            List<OpenMeal> meals = new List<OpenMeal>();
            var distinctMeals = activeMeals;
            distinctMeals.DistinctBy(x => x.UserID);
            foreach (Meals meal in distinctMeals)
            {
                var user = db.Profiles.Where(x => x.UserId == meal.UserID).FirstOrDefault();
                if (user != null)
                {
                    meals.Add(new OpenMeal()
                    {
                        Name = user.Name,
                        Profession = user.Profession,
                        Age = OpenMeal.CalculateAge(user.Birthday).ToString(),
                        UserID = user.UserId,
                        
                    });
                }

            }
            return meals;
        }




        // GET api/values/5
        [HttpGet("{id}/{profileid}")]
        public List<FaceBookProfile> Get(int id, int profileid)
        {
            List<FaceBookProfile> profile;
            using (DatabaseConnection db = DatabaseConnection.GetDBConnection())
            {
                profile = db.Profiles.Where(x => x.UserId == profileid.ToString()).ToList();
                if (profile == null || profile.FirstOrDefault() == null)
                    throw new Exception("User with current ID can not be found, userFacebookId= " + profileid);
                profile.ForEach(x => x.Age = OpenMeal.CalculateAge(x.Birthday));            
            }
            return profile;
        }
        [HttpGet("{id}")]
        public List<FaceBookProfile> Get(string id)
        {
            List<FaceBookProfile> profile;
            using (DatabaseConnection db = DatabaseConnection.GetDBConnection())
            {
                profile = db.Profiles.Where(x => x.UserId == id.ToString()).ToList();
                if (profile == null || profile.FirstOrDefault() == null)
                    throw new Exception("User with current ID can not be found, userFacebookId= " + id.ToString());
                profile.ForEach(x => x.Age = OpenMeal.CalculateAge(x.Birthday));
            }
            return profile;
        }
        
        // POST api/values
        [HttpPost]
        //[AllowAnonymous]
        public void Post([FromBody]Data data)
        {
            if (data == null || string.IsNullOrEmpty(data.chosenDate) || string.IsNullOrEmpty(data.userID)) return;
            DateTime result;
            if(DateTime.TryParse(data.chosenDate, out result))
            {
                result = result.ToLocalTime();
            }
            string resp = WriteRequestToTable(result, data.userID, data.location);

        }

        private string WriteRequestToTable(DateTime result, string userID, string location)
        {
            DatabaseConnection conn = DatabaseConnection.GetDBConnection();

            conn.Meals.InsertOnSubmit(
                new Meals
                {
                    MealDate = result,
                    UserID = userID,
                    Location = location
                }
                );
            conn.SubmitChanges();
            return "ADDED";
        }

        public class Data
        {
            // the JSON to Model mapper match is case-insensitive
            public string chosenDate { get; set; }
            public string userID { get; set; }
            public string location { get; set; }
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
