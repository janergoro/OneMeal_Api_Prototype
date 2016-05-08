using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace OneMealAPI.Models
{
    [Table(Name = "FacebookProfile")]
    public class FaceBookProfile
    {
        [Column(Name = "Id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "Name")]
        public string Name { get; set; }
        [Column(Name = "Birthday")]
        public DateTime? Birthday { get; set; }
        [Column(Name = "Profession")]
        public string Profession { get; set; }
        [Column(Name = "About")]
        public string About { get; set; }
        [Column(Name = "Keywords")]
        public string Keywords { get; set; }
        [Column(Name = "UserId")]
        public string UserId { get; set; }
        public int Age { get; set; }
    }

}
