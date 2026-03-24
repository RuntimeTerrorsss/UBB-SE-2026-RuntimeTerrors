using OurApp.Core.Models;
using OurApp.Core.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;

namespace OurApp.Core.Repositories
{
    public class CompanyRepo : ICompanyRepo
    {
        ObservableCollection<Company> companies;
        private readonly string _connectionString;

        public CompanyRepo()
        {
            companies = new ObservableCollection<Company>();
            _connectionString =
                Environment.GetEnvironmentVariable("ISS_PROJECT_DB_CONNECTION_STRING") ??
                "Data Source=TEA\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        }

        private void ValidateRequiredFields(Company c)
        {
            if (c is null) throw new ArgumentNullException(nameof(c));
            if (string.IsNullOrWhiteSpace(c.Name)) throw new ArgumentException("Company name is required.", nameof(c));
            if (string.IsNullOrWhiteSpace(c.CompanyLogoPath)) throw new ArgumentException("Company logo url/path is required.", nameof(c));
        }

        private static object DbValue(string? value) => value is null ? DBNull.Value : value;
        private static object DbValue(int? value) => value.HasValue ? value.Value : DBNull.Value;

        private static Company MapCompany(SqlDataReader reader)
        {
            return new Company(
                name: reader["company_name"]?.ToString() ?? "",
                aboutus: reader["about_us"] is DBNull ? "" : reader["about_us"]?.ToString() ?? "",
                pfpUrl: reader["profile_picture_url"] is DBNull ? "" : reader["profile_picture_url"]?.ToString() ?? "",
                logoUrl: reader["logo_picture_url"]?.ToString() ?? "",
                location: reader["location"] is DBNull ? "" : reader["location"]?.ToString() ?? "",
                email: reader["email"] is DBNull ? "" : reader["email"]?.ToString() ?? "",
                companyId: Convert.ToInt32(reader["company_id"]),
                buddyName: reader["buddy_name"] is DBNull ? "" : reader["buddy_name"]?.ToString() ?? "",
                avatarId: reader["avatar_id"] is DBNull ? null : Convert.ToInt32(reader["avatar_id"]),
                finalQuote: reader["final_quote"] is DBNull ? "" : reader["final_quote"]?.ToString() ?? "",
                scenario1Text: reader["scen_1_text"] is DBNull ? "" : reader["scen_1_text"]?.ToString() ?? "",
                scenario1Answer1: reader["scen1_answer1"] is DBNull ? "" : reader["scen1_answer1"]?.ToString() ?? "",
                scenario1Answer2: reader["scen1_answer2"] is DBNull ? "" : reader["scen1_answer2"]?.ToString() ?? "",
                scenario1Answer3: reader["scen1_answer3"] is DBNull ? "" : reader["scen1_answer3"]?.ToString() ?? "",
                scenario1Reaction1: reader["scen1_reaction1"] is DBNull ? "" : reader["scen1_reaction1"]?.ToString() ?? "",
                scenario1Reaction2: reader["scen1_reaction2"] is DBNull ? "" : reader["scen1_reaction2"]?.ToString() ?? "",
                scenario1Reaction3: reader["scen1_reaction3"] is DBNull ? "" : reader["scen1_reaction3"]?.ToString() ?? "",
                scenario2Text: reader["scen2_text"] is DBNull ? "" : reader["scen2_text"]?.ToString() ?? "",
                scenario2Answer1: reader["scen2_answer1"] is DBNull ? "" : reader["scen2_answer1"]?.ToString() ?? "",
                scenario2Answer2: reader["scen2_answer2"] is DBNull ? "" : reader["scen2_answer2"]?.ToString() ?? "",
                scenario2Answer3: reader["scen2_answer3"] is DBNull ? "" : reader["scen2_answer3"]?.ToString() ?? "",
                scenario2Reaction1: reader["scen2_reaction1"] is DBNull ? "" : reader["scen2_reaction1"]?.ToString() ?? "",
                scenario2Reaction2: reader["scen2_reaction2"] is DBNull ? "" : reader["scen2_reaction2"]?.ToString() ?? "",
                scenario2Reaction3: reader["scen2_reaction3"] is DBNull ? "" : reader["scen2_reaction3"]?.ToString() ?? "",
                postedJobsCount: reader["posted_jobs_count"] is DBNull ? 0 : Convert.ToInt32(reader["posted_jobs_count"]),
                collaboratorsCount: reader["collaborators_count"] is DBNull ? 0 : Convert.ToInt32(reader["collaborators_count"])
            );
        }

        private void RefreshCompaniesFromDatabase()
        {
            var list = new ObservableCollection<Company>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                @"SELECT c.company_id, c.company_name, c.about_us, c.profile_picture_url, c.logo_picture_url, c.location, c.email,
                         c.buddy_name, c.avatar_id, c.final_quote, c.scen_1_text, c.scen1_answer1, c.scen1_answer2, c.scen1_answer3,
                         c.scen1_reaction1, c.scen1_reaction2, c.scen1_reaction3, c.scen2_text, c.scen2_answer1, c.scen2_answer2,
                         c.scen2_answer3, c.scen2_reaction1, c.scen2_reaction2, c.scen2_reaction3,
                         (SELECT COUNT(*) FROM jobs j WHERE j.company_id = c.company_id) AS posted_jobs_count,
                         (SELECT COUNT(*) FROM collaborators col WHERE col.company_id = c.company_id) AS collaborators_count
                  FROM companies;",
                conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapCompany(reader));
            }

            companies = list;
        }

        public void PrintAll()
        {
            RefreshCompaniesFromDatabase();
            foreach (var c in companies)
                System.Diagnostics.Debug.WriteLine($"{c} ");
        }

        ObservableCollection<Company> ICompanyRepo.GetAll()
        {
            RefreshCompaniesFromDatabase();
            return companies;
        }

        Company? ICompanyRepo.GetById(int companyId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                @"SELECT c.company_id, c.company_name, c.about_us, c.profile_picture_url, c.logo_picture_url, c.location, c.email,
                         c.buddy_name, c.avatar_id, c.final_quote, c.scen_1_text, c.scen1_answer1, c.scen1_answer2, c.scen1_answer3,
                         c.scen1_reaction1, c.scen1_reaction2, c.scen1_reaction3, c.scen2_text, c.scen2_answer1, c.scen2_answer2,
                         c.scen2_answer3, c.scen2_reaction1, c.scen2_reaction2, c.scen2_reaction3,
                         (SELECT COUNT(*) FROM jobs j WHERE j.company_id = c.company_id) AS posted_jobs_count,
                         (SELECT COUNT(*) FROM collaborators col WHERE col.company_id = c.company_id) AS collaborators_count
                  FROM companies c
                  WHERE c.company_id = @CompanyId;",
                conn);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return MapCompany(reader);
        }

        void ICompanyRepo.Add(Company c)
        {
            ValidateRequiredFields(c);

            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            var nextIdCmd = new SqlCommand(
                @"SELECT COALESCE(MAX(company_id), 0) + 1
                  FROM companies WITH (UPDLOCK, HOLDLOCK);",
                conn,
                tx);
            int nextId = (int)nextIdCmd.ExecuteScalar();

            var insertCmd = new SqlCommand(
                @"INSERT INTO companies
                  (company_id, company_name, about_us, profile_picture_url, logo_picture_url, location, email,
                   buddy_name, avatar_id, final_quote, scen_1_text, scen1_answer1, scen1_answer2, scen1_answer3,
                   scen1_reaction1, scen1_reaction2, scen1_reaction3, scen2_text, scen2_answer1, scen2_answer2,
                   scen2_answer3, scen2_reaction1, scen2_reaction2, scen2_reaction3)
                  VALUES
                  (@CompanyId, @Name, @AboutUs, @ProfilePictureUrl, @LogoPictureUrl, @Location, @Email,
                   @BuddyName, @AvatarId, @FinalQuote, @Scenario1Text, @Scenario1Answer1, @Scenario1Answer2, @Scenario1Answer3,
                   @Scenario1Reaction1, @Scenario1Reaction2, @Scenario1Reaction3, @Scenario2Text, @Scenario2Answer1, @Scenario2Answer2,
                   @Scenario2Answer3, @Scenario2Reaction1, @Scenario2Reaction2, @Scenario2Reaction3);",
                conn,
                tx);

            insertCmd.Parameters.AddWithValue("@CompanyId", nextId);
            insertCmd.Parameters.AddWithValue("@Name", c.Name);
            insertCmd.Parameters.AddWithValue("@AboutUs", DbValue(c.AboutUs));
            insertCmd.Parameters.AddWithValue("@ProfilePictureUrl", DbValue(c.ProfilePicturePath));
            insertCmd.Parameters.AddWithValue("@LogoPictureUrl", c.CompanyLogoPath);
            insertCmd.Parameters.AddWithValue("@Location", DbValue(c.Location));
            insertCmd.Parameters.AddWithValue("@Email", DbValue(c.Email));
            insertCmd.Parameters.AddWithValue("@BuddyName", DbValue(c.BuddyName));
            insertCmd.Parameters.AddWithValue("@AvatarId", DbValue(c.AvatarId));
            insertCmd.Parameters.AddWithValue("@FinalQuote", DbValue(c.FinalQuote));
            insertCmd.Parameters.AddWithValue("@Scenario1Text", DbValue(c.Scenario1Text));
            insertCmd.Parameters.AddWithValue("@Scenario1Answer1", DbValue(c.Scenario1Answer1));
            insertCmd.Parameters.AddWithValue("@Scenario1Answer2", DbValue(c.Scenario1Answer2));
            insertCmd.Parameters.AddWithValue("@Scenario1Answer3", DbValue(c.Scenario1Answer3));
            insertCmd.Parameters.AddWithValue("@Scenario1Reaction1", DbValue(c.Scenario1Reaction1));
            insertCmd.Parameters.AddWithValue("@Scenario1Reaction2", DbValue(c.Scenario1Reaction2));
            insertCmd.Parameters.AddWithValue("@Scenario1Reaction3", DbValue(c.Scenario1Reaction3));
            insertCmd.Parameters.AddWithValue("@Scenario2Text", DbValue(c.Scenario2Text));
            insertCmd.Parameters.AddWithValue("@Scenario2Answer1", DbValue(c.Scenario2Answer1));
            insertCmd.Parameters.AddWithValue("@Scenario2Answer2", DbValue(c.Scenario2Answer2));
            insertCmd.Parameters.AddWithValue("@Scenario2Answer3", DbValue(c.Scenario2Answer3));
            insertCmd.Parameters.AddWithValue("@Scenario2Reaction1", DbValue(c.Scenario2Reaction1));
            insertCmd.Parameters.AddWithValue("@Scenario2Reaction2", DbValue(c.Scenario2Reaction2));
            insertCmd.Parameters.AddWithValue("@Scenario2Reaction3", DbValue(c.Scenario2Reaction3));

            insertCmd.ExecuteNonQuery();
            c.CompanyId = nextId;

            tx.Commit();
            RefreshCompaniesFromDatabase();
        }

        void ICompanyRepo.Remove(int companyId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var deleteCmd = new SqlCommand(
                @"DELETE FROM companies
                  WHERE company_id = @CompanyId;",
                conn);
            deleteCmd.Parameters.AddWithValue("@CompanyId", companyId);
            deleteCmd.ExecuteNonQuery();

            RefreshCompaniesFromDatabase();
        }

        void ICompanyRepo.Update(Company c)
        {
            ValidateRequiredFields(c);

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                @"UPDATE companies
                  SET company_name = @Name,
                      about_us = @AboutUs,
                      profile_picture_url = @ProfilePictureUrl,
                      logo_picture_url = @LogoPictureUrl,
                      location = @Location,
                      email = @Email,
                      buddy_name = @BuddyName,
                      avatar_id = @AvatarId,
                      final_quote = @FinalQuote,
                      scen_1_text = @Scenario1Text,
                      scen1_answer1 = @Scenario1Answer1,
                      scen1_answer2 = @Scenario1Answer2,
                      scen1_answer3 = @Scenario1Answer3,
                      scen1_reaction1 = @Scenario1Reaction1,
                      scen1_reaction2 = @Scenario1Reaction2,
                      scen1_reaction3 = @Scenario1Reaction3,
                      scen2_text = @Scenario2Text,
                      scen2_answer1 = @Scenario2Answer1,
                      scen2_answer2 = @Scenario2Answer2,
                      scen2_answer3 = @Scenario2Answer3,
                      scen2_reaction1 = @Scenario2Reaction1,
                      scen2_reaction2 = @Scenario2Reaction2,
                      scen2_reaction3 = @Scenario2Reaction3
                  WHERE company_id = @CompanyId;",
                conn);

            cmd.Parameters.AddWithValue("@CompanyId", c.CompanyId);
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@AboutUs", DbValue(c.AboutUs));
            cmd.Parameters.AddWithValue("@ProfilePictureUrl", DbValue(c.ProfilePicturePath));
            cmd.Parameters.AddWithValue("@LogoPictureUrl", c.CompanyLogoPath);
            cmd.Parameters.AddWithValue("@Location", DbValue(c.Location));
            cmd.Parameters.AddWithValue("@Email", DbValue(c.Email));
            cmd.Parameters.AddWithValue("@BuddyName", DbValue(c.BuddyName));
            cmd.Parameters.AddWithValue("@AvatarId", DbValue(c.AvatarId));
            cmd.Parameters.AddWithValue("@FinalQuote", DbValue(c.FinalQuote));
            cmd.Parameters.AddWithValue("@Scenario1Text", DbValue(c.Scenario1Text));
            cmd.Parameters.AddWithValue("@Scenario1Answer1", DbValue(c.Scenario1Answer1));
            cmd.Parameters.AddWithValue("@Scenario1Answer2", DbValue(c.Scenario1Answer2));
            cmd.Parameters.AddWithValue("@Scenario1Answer3", DbValue(c.Scenario1Answer3));
            cmd.Parameters.AddWithValue("@Scenario1Reaction1", DbValue(c.Scenario1Reaction1));
            cmd.Parameters.AddWithValue("@Scenario1Reaction2", DbValue(c.Scenario1Reaction2));
            cmd.Parameters.AddWithValue("@Scenario1Reaction3", DbValue(c.Scenario1Reaction3));
            cmd.Parameters.AddWithValue("@Scenario2Text", DbValue(c.Scenario2Text));
            cmd.Parameters.AddWithValue("@Scenario2Answer1", DbValue(c.Scenario2Answer1));
            cmd.Parameters.AddWithValue("@Scenario2Answer2", DbValue(c.Scenario2Answer2));
            cmd.Parameters.AddWithValue("@Scenario2Answer3", DbValue(c.Scenario2Answer3));
            cmd.Parameters.AddWithValue("@Scenario2Reaction1", DbValue(c.Scenario2Reaction1));
            cmd.Parameters.AddWithValue("@Scenario2Reaction2", DbValue(c.Scenario2Reaction2));
            cmd.Parameters.AddWithValue("@Scenario2Reaction3", DbValue(c.Scenario2Reaction3));

            int affected = cmd.ExecuteNonQuery();
            if (affected == 0)
                throw new InvalidOperationException($"No company found with id '{c.CompanyId}' to update.");

            RefreshCompaniesFromDatabase();
        }
    }
}
