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
    public class PendingMealsController : Controller
    {
        [HttpGet("{UserId}")]
        public List<OpenMeal> Get(string UserId)
        {
            List<OpenMeal> meals = new List<OpenMeal>();
            using (DatabaseConnection db = DatabaseConnection.GetDBConnection())
            {
                List<MealRequest> reqMeals = db.MealRequests.Where(i => i.SourceUserId == UserId && i.Accepted == false).ToList();
                foreach (MealRequest mealReq in reqMeals)
                {
                    //Meals curMeal = db.Meals.Where(x => x.Id == mealReq.MealId).FirstOrDefault();
                    //if (curMeal == null) continue;
                    OpenMeal curMeal = new OpenMeal(mealReq, db);
                    meals.Add(curMeal);

                }
            }
            return meals;
        }

    }
}
