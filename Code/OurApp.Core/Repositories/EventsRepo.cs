using Microsoft.Data.SqlClient;
using OurApp.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public class EventsRepo : IEventsRepo
    {
        private string connectionString {  get; set; }

        /// <summary>
        /// Event repository constructor
        /// </summary>
        /// <param name="connectionString"> database conection string </param>
        public EventsRepo(string connectionString) 
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Function that inserts an event into the database repository
        /// </summary>
        /// <param name="eventToBeAdded"> event to be inserted into the database </param>
        public void AddEventToRepo(Event eventToBeAdded)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string queryToBeRun = @"
                    INSERT INTO events 
                    (event_id, photo, title, description, start_date, end_date, location, host_company_id, posted_at)
                    VALUES (@Id, @Photo, @Title, @Description, @StartDate, @EndDate, @Location, @Host, @CurrentDateTime)";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@Id", eventToBeAdded.Id);
                sqlCommand.Parameters.AddWithValue("@Photo", eventToBeAdded.Photo ?? (object)DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@Title", eventToBeAdded.Title);
                sqlCommand.Parameters.AddWithValue("@Description", eventToBeAdded.Description ?? (object)DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@StartDate", eventToBeAdded.StartDate);
                sqlCommand.Parameters.AddWithValue("@EndDate", eventToBeAdded.EndDate);
                sqlCommand.Parameters.AddWithValue("@Location", eventToBeAdded.Location);
                sqlCommand.Parameters.AddWithValue("@Host", eventToBeAdded.HostID);
                sqlCommand.Parameters.AddWithValue("@CurrentDateTime", DateTime.Now);

                sqlCommand.ExecuteNonQuery();
            }
        }

        public void RemoveEventFromRepo(Event eventToBeRemoved)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM events WHERE event_id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", eventToBeRemoved.Id);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Function that returns a collection of all the current events, 
        /// whose ending date has not exceeded the current date
        /// </summary>
        /// <returns> ObservableCollection of current events </returns>
        public ObservableCollection<Event> getCurrentEventsFromRepo(int loggedInUser)
        {
            var currentEvents = new ObservableCollection<Event>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string queryToBeRun = "SELECT * FROM events WHERE host_company_id = @HostID and end_date >= @TodaysDate";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@HostID", loggedInUser);
                sqlCommand.Parameters.AddWithValue("@TodaysDate", DateTime.Now.Date);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    currentEvents.Add(new Event(
                        reader["photo"].ToString(),
                        reader["title"].ToString(),
                        reader["description"].ToString(),
                        (DateTime)reader["start_date"],
                        (DateTime)reader["end_date"],
                        reader["location"].ToString(),
                        1,
                        new List<Company>()
                    )
                    {
                        Id = (int)reader["event_id"]
                    });
                }
            }

            return currentEvents;
        }

        /// <summary>
        /// Function that returns a collection of all the past events, 
        /// whose ending date has exceeded the current date
        /// </summary>
        /// <returns> ObservableCollection of past events </returns>
        public ObservableCollection<Event> getPastEventsFromRepo(int loggedInUser)
        {
            var pastEvents = new ObservableCollection<Event>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string queryToBeRun = "SELECT * FROM events WHERE host_company_id = @HostID and end_date < @TodaysDate";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);


                sqlCommand.Parameters.AddWithValue("@HostID", loggedInUser);
                sqlCommand.Parameters.AddWithValue("@TodaysDate", DateTime.Now.Date);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    pastEvents.Add(new Event(
                        reader["photo"].ToString(),
                        reader["title"].ToString(),
                        reader["description"].ToString(),
                        (DateTime)reader["start_date"],
                        (DateTime)reader["end_date"],
                        reader["location"].ToString(),
                        1, 
                        new List<Company>()
                    )
                    {
                        Id = (int)reader["event_id"]
                    });
                }
            }

            return pastEvents;
        
        }

        /// <summary>
        /// Function that updates the contents of an event.
        /// </summary>
        /// <param name="eventIdToBeUpdated"> id of the event that is updated </param>
        /// <param name="newEventPhoto"> the updated photo url </param>
        /// <param name="newEventTitle"> the updated title of the event </param>
        /// <param name="newEventDescription"> the updated description of the event </param>
        /// <param name="newEventStartDate"> the updated starting date of the event </param>
        /// <param name="newEventEndDate"> the updated ending date of the event </param>
        /// <param name="newEventLocation"> the updated location of the event </param>
        public void UpdateEventToRepo(int eventIdToBeUpdated, string newEventPhoto, string newEventTitle, string newEventDescription, DateTime newEventStartDate, DateTime newEventEndDate, string newEventLocation)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string queryToBeRun = @"UPDATE events SET 
                                photo=@Photo,
                                title=@Title,
                                description=@Description,
                                start_date=@StartDate,
                                end_date=@EndDate,
                                location=@Location,
                                posted_at=@PostedAt
                                WHERE event_id=@Id";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@Photo", newEventPhoto ?? (object)DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@Title", newEventTitle);
                sqlCommand.Parameters.AddWithValue("@Description", newEventDescription ?? (object)DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@StartDate", newEventStartDate);
                sqlCommand.Parameters.AddWithValue("@EndDate", newEventEndDate);
                sqlCommand.Parameters.AddWithValue("@Location", newEventLocation);
                sqlCommand.Parameters.AddWithValue("@PostedAt", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@Id", eventIdToBeUpdated);

                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
