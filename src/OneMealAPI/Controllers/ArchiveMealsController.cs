using Microsoft.AspNet.Mvc;
using OneMealAPI.Custom.Database;
using OneMealAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMealAPI.Controllers
{
    [Route("api/[Controller]")]
    public class ArchiveMealsController : Controller
    {


        [HttpGet("{UserId}")]
        public List<OpenMeal> Get(string UserId)
        {
            List<OpenMeal> meals = new List<OpenMeal>();
            using (DatabaseConnection db = DatabaseConnection.GetDBConnection())
            {
                List<Meals> reqMeals = db.Meals.
                    Where(i => (i.PartnerID == UserId || i.UserID == UserId) && i.PartnerID != "" ).
                    OrderByDescending(x => x.MealDate).
                    ToList();
                foreach (Meals meal in reqMeals)
                {
                    var secondPartyId = meal.UserID == UserId ? meal.PartnerID : meal.UserID;
                    OpenMeal curMeal = new OpenMeal(meal, db, secondPartyId);
                    meals.Add(curMeal);
                }
            }
            return meals;
        }

        
    }
}
