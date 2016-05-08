using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace OneMealAPI.Models
{
    [Table(Name = "MealRequestHistory")]
    public class MealRequest
    {
        [Column(Name = "Id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "SourceUserId")]
        public string SourceUserId { get; set; }
        [Column(Name = "RequestUserId")]
        public string RequestUserId { get; set; }
        [Column(Name = "MealId")]
        public int MealId { get; set; }
        [Column(Name = "Accepted")]
        public bool Accepted { get; set; }

    }

}
