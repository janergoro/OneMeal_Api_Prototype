using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace OneMealAPI.Models
{
    [Table(Name = "Meals")]
    public class Meals
    {
        [Column(Name="Id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "UserID")]
        public string UserID { get; set; }
        [Column(Name = "MealDate")]
        public DateTime MealDate { get; set; }
        [Column(Name = "PartnerID")]
        public string PartnerID { get; set; }
        [Column(Name = "Location")]
        public string Location { get; set; }
    }
}
