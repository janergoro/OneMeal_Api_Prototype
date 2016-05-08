using OneMealAPI.Custom.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMealAPI.Models
{
    public class OpenMeal : Meals
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string Profession { get; set; }
        public string Requester { get; set; }

        public OpenMeal()
        {

        }
        public OpenMeal(Meals meal, DatabaseConnection db, string secondPartyId)
        {
            
            var profile = db.Profiles.Where(x => x.UserId == secondPartyId).FirstOrDefault();
            if (profile == null) return;
            Name = profile.Name;
            Age = CalculateAge(profile.Birthday).ToString();
            Profession = profile.Profession;
            UserID = profile.UserId;
            Id = meal.Id;
            MealDate = meal.MealDate;
            return;
        }
        public OpenMeal(MealRequest req, DatabaseConnection db)
        {
            
            var profile = db.Profiles.Where(x => x.UserId == req.RequestUserId).FirstOrDefault();
            if (profile == null) return;
            Name = profile.Name;
            Age = CalculateAge(profile.Birthday).ToString();
            Profession = profile.Profession;
            UserID = profile.UserId;
            Id = req.MealId;
            MealDate = db.Meals.Where(x=> x.Id == req.MealId).FirstOrDefault().MealDate;
            return;
        }
        public static int CalculateAge(DateTime? birthday)
        {
            int age = birthday.HasValue ? DateTime.Now.Year - birthday.Value.Year : 0;
            if (age != 0 && birthday.Value.Month > DateTime.Now.Month)
            {
                age--;
            }
            else if (age != 0 && birthday.Value.Month == DateTime.Now.Month &&
               birthday.Value.Day > DateTime.Now.Day)
            {
                age--;
            }
            return age;
        }
    }
}
