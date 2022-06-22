using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using CAIR_Schedule.Models;
using System.Globalization;

namespace CAIR_Schedule
{
    internal class Database
    {
        private static Database _database;
        private NpgsqlConnection connection;
        public Database(){
            string ConnectionString = "Server=54.73.56.235; Port=5432; User Id=gqowskvg; Password=FJevZc2BGCDPLbnh5G5FOAVFibEUgHul; Database=gqowskvg";
            try
            {
                connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                Console.WriteLine("Success!!");
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
            }
            
        }
        public static Database getDatabase()
        {
            if (_database == null)
            {
                _database = new Database();
            }
            return _database;
        }

        public bool Login(string login, string pass) 
        {
            connection.Open();
            try
            {
            
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id FROM users WHERE login = '" + login + "' AND password = '" + pass+"'";
            
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int id=Int32.Parse(reader[0].ToString());
                
                if (id != 0)
                {
                    Console.WriteLine("User id: " + id);
                    App.Current.Properties["logged"] = true;
                    App.Current.Properties["user_id"] = id;
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            connection.Close();
            return false;
        }

        public List<Item> getData()
        {
            DateTime date = DateTime.Now;
            int day = (int)date.DayOfWeek;
            DateTime Monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime Sunday = date.AddDays((1) * (day == 0 ? day : 15 - day));
            Console.WriteLine(Monday.ToString("dd/MM/yyyy"));
            Console.WriteLine(CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(Sunday.DayOfWeek));

            connection.Open();
            List<Item> data = new List<Item>();
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT group_name, comment, full_name, to_char(date_time, 'DD/MM/YYYY HH24:MI:SS') FROM user_groups 
            inner join groups on user_groups.group_id = groups.id
            inner join lessons on user_groups.group_id = lessons.group_id
            where user_groups.user_id = " + App.Current.Properties["user_id"] + " and date_time between '"
            + Monday.ToString("yyyy/MM/dd") + "' and '" + Sunday.ToString("yyyy/MM/dd") + "' ORDER BY date_time";
            Console.WriteLine(command.CommandText);
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString()+" "+reader[1].ToString()+" "+reader[2].ToString()+" "+reader[3].ToString());
                    data.Add(new Item { Id = Guid.NewGuid().ToString(), Group_name = reader[0].ToString(), Comment = reader[1].ToString(), Name = reader[2].ToString(), Date_time= reader[3].ToString() });
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            connection.Close();
            return data;  
        }
    }
}
