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

namespace OneMealAPI.Controllers
{
    [Route("api/[controller]")]
    public class OneMealController : Controller
    {
        DatabaseConnection db;
        // GET: api/OneMeal
        [HttpGet]
        public void Get()
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public List<FaceBookProfile> Get(int id)
        {

            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        //[AllowAnonymous]
        public bool Post([FromBody]Data data)
        {
            if (data == null) return false;
            if (db == null) db = DatabaseConnection.GetDBConnection();
            if (data.MealId.HasValue)
            {
                Meals meal = db.Meals.Where(m => m.Id == data.MealId.Value).FirstOrDefault();
                if (meal != null)
                {
                    if (data.RequestExists)
                    {
                        var requests = db.MealRequests.Where(m => m.MealId == meal.Id);
                        meal.PartnerID = data.PartnerID;  
                        db.MealRequests.DeleteAllOnSubmit(requests);           
                    }
                    else {
                        MealRequest req = new MealRequest();
                        req.MealId = meal.Id;
                        req.SourceUserId = meal.UserID;
                        req.RequestUserId = data.PartnerID;
                        req.Accepted = false;
                        db.MealRequests.InsertOnSubmit(req);
                        
                    }
                    db.SubmitChanges();
                    return true;
                }
                else
                    throw new NotImplementedException("#TODO Meal has been deleted");
                
            }
            return false;
        }

        public class Data
        {
            // the JSON to Model mapper match is case-insensitive
            public int? MealId { get; set; }
            public string PartnerID { get; set; }
            public bool RequestExists { get; set; }
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
