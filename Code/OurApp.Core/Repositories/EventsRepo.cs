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
        ObservableCollection<Event> eventsCollection;
        private string connectionString {  get; set; }

        public EventsRepo(string connectionString) 
        {
            eventsCollection = new ObservableCollection<Event>();
            this.connectionString = connectionString;
        }

        public void printAll()
        {
            for (int i = 0; i < eventsCollection.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{eventsCollection[i]} ");
            }
        }

        //public ObservableCollection<Event> GetCollectionFromRepo()
        //{
        //    return eventsCollection;
        //}

        public void AddEventToRepo(Event eventToBeAdded)
        {
            //eventsCollection.Add(eventToBeAdded);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO events 
            (event_id, photo, title, description, start_date, end_date, location, host_company_id, posted_at)
            VALUES (@Id, @Photo, @Title, @Description, @StartDate, @EndDate, @Location, @Host, @CurrentDateTime)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", eventToBeAdded.Id);
                cmd.Parameters.AddWithValue("@Photo", eventToBeAdded.Photo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Title", eventToBeAdded.Title);
                cmd.Parameters.AddWithValue("@Description", eventToBeAdded.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", eventToBeAdded.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", eventToBeAdded.EndDate);
                cmd.Parameters.AddWithValue("@Location", eventToBeAdded.Location);
                cmd.Parameters.AddWithValue("@Host", 1);
                cmd.Parameters.AddWithValue("@CurrentDateTime", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveEventFromRepo(Event eventToBeRemoved)
        {
            //eventsCollection.Remove(eventToBeRemoved);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM events WHERE event_d = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", eventToBeRemoved.Id);

                cmd.ExecuteNonQuery();
            }
        }

        public ObservableCollection<Event> getCurrentEventsFromRepo()
        {
            //ObservableCollection<Event> currentEvents = new ObservableCollection<Event>();

            //foreach (Event @event in eventsCollection)
            //{
            //    DateTime eventEndDate = @event.EndDate;
            //    DateTime todaysDate = DateTime.Now;

            //    if (eventEndDate.Date >= todaysDate.Date)
            //    {
            //        currentEvents.Add(@event);
            //    }
            //}

            //return currentEvents;

            var events = new ObservableCollection<Event>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM events WHERE end_date >= @TodaysDate";

                SqlCommand cmd = new SqlCommand(query, conn);


                cmd.Parameters.AddWithValue("@TodaysDate", DateTime.Now.Date);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    events.Add(new Event(
                        reader["photo"].ToString(),
                        reader["title"].ToString(),
                        reader["description"].ToString(),
                        (DateTime)reader["start_date"],
                        (DateTime)reader["end_date"],
                        reader["location"].ToString(),
                        1,
                        new List<Company>() // adjust if needed
                    )
                    {
                        Id = (int)reader["event_id"]
                    });
                }
            }

            return events;
        }

        public ObservableCollection<Event> getPastEventsFromRepo()
        {
            //ObservableCollection<Event> pastEvents = new ObservableCollection<Event>();

            //foreach (Event @event in eventsCollection)
            //{
            //    DateTime eventEndDate = @event.EndDate;
            //    DateTime todaysDate = DateTime.Now;

            //    if (eventEndDate.Date < todaysDate.Date)
            //    {
            //        pastEvents.Add(@event);
            //    }
            //}

            //return pastEvents;

            var events = new ObservableCollection<Event>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM events WHERE end_date < @TodaysDate";

                SqlCommand cmd = new SqlCommand(query, conn);


                cmd.Parameters.AddWithValue("@TodaysDate", DateTime.Now.Date);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    events.Add(new Event(
                        reader["photo"].ToString(),
                        reader["title"].ToString(),
                        reader["description"].ToString(),
                        (DateTime)reader["start_date"],
                        (DateTime)reader["end_date"],
                        reader["location"].ToString(),
                        1, 
                        new List<Company>() // adjust if needed
                    )
                    {
                        Id = (int)reader["event_id"]
                    });
                }
            }

            return events;
        
        }


        public void UpdateEventToRepo(int eventIdToBeUpdated, string newEventPhoto, string newEventTitle, string newEventDescription, DateTime newEventStartDate, DateTime newEventEndDate, string newEventLocation)
        {
            //foreach (Event @event in eventsCollection)
            //{
            //    if (@event.Id == eventIdToBeUpdated)
            //    {
            //        @event.Photo = newEventPhoto;
            //        @event.Title = newEventTitle;
            //        @event.Description = newEventDescription;
            //        @event.StartDate = newEventStartDate;
            //        @event.EndDate = newEventEndDate;
            //        @event.Location = newEventLocation;
            //        return;
            //    }
            //}

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"UPDATE events SET 
            photo=@Photo,
            title=@Title,
            description=@Description,
            start_date=@StartDate,
            end_date=@EndDate,
            location=@Location,
            posted_at=@PostedAt
            WHERE event_id=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Photo", newEventPhoto ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Title", newEventTitle);
                cmd.Parameters.AddWithValue("@Description", newEventDescription ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", newEventStartDate);
                cmd.Parameters.AddWithValue("@EndDate", newEventEndDate);
                cmd.Parameters.AddWithValue("@Location", newEventLocation);
                cmd.Parameters.AddWithValue("@PostedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id", eventIdToBeUpdated);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
