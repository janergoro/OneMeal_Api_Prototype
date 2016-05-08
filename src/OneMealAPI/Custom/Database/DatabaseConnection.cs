using OneMealAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace OneMealAPI.Custom.Database
{
    public class DatabaseConnection : DataContext
    {
        public Table<FaceBookProfile> Profiles;
        public Table<Meals> Meals;
        public Table<MealRequest> MealRequests;
        private DatabaseConnection(string connectionString) :
            base(connectionString) {}
        public static DatabaseConnection GetDBConnection()
        {
            /*tring connectionstring = @"Data Source = UPTJANERG-LT\SQLEXPRESS; " +
                                        "Initial Catalog= OneMeal;" +
                                        "Integrated Security=SSPI;";*/
            string connectionstring = ConfigurationManager.AppSettings["ConnectionString"];
            //ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            //configFileMap.ExeConfigFilename();
            return new DatabaseConnection(connectionstring);
        }

    }
}
