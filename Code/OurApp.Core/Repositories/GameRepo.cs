using OurApp.Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace OurApp.Core.Repositories
{
    public class GameRepo : IGameRepo
    {
        // Expected schema (per your MapGame sample):
        // - Table: Game
        // - Columns: avatar_id, buddy_name, scen_1_text, scen2_text,
        //   scen1_answer1..3, scen1_reaction1..3, scen2_answer1..3, scen2_reaction1..3, final_quote
        private const string TableName = "Game";

        private readonly string _connectionString;

        public GameRepo(string? connectionString = null)
        {
            _connectionString = connectionString
                ?? Environment.GetEnvironmentVariable("UBB_DB_CONNECTION_STRING")
                ?? Environment.GetEnvironmentVariable("UBB_CONNECTION_STRING")
                ?? throw new InvalidOperationException(
                    "Missing DB connection string. Set environment variable 'UBB_DB_CONNECTION_STRING' (or 'UBB_CONNECTION_STRING').");
        }

        // Company-specific game stored inside the `companies` table.
        public Game GetGame(int companyId)
        {
            if (companyId <= 0)
                return new Game();

            const string query = @"
                SELECT
                    avatar_id,
                    buddy_name,
                    final_quote,
                    scen_1_text,
                    scen1_answer1, scen1_answer2, scen1_answer3,
                    scen1_reaction1, scen1_reaction2, scen1_reaction3,
                    scen2_text,
                    scen2_answer1, scen2_answer2, scen2_answer3,
                    scen2_reaction1, scen2_reaction2, scen2_reaction3
                FROM companies
                WHERE company_id = @CompanyId;";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);

            conn.Open();
            using var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!reader.Read())
                return new Game();

            return MapGame(reader);
        }

        public void SaveGame(Game game, int companyId)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId), "companyId must be > 0.");

            const string query = @"
                UPDATE companies
                SET
                    avatar_id = @AvatarId,
                    buddy_name = @BuddyName,
                    final_quote = @FinalQuote,

                    scen_1_text = @Scen1Text,
                    scen1_answer1 = @Scen1Answer1,
                    scen1_answer2 = @Scen1Answer2,
                    scen1_answer3 = @Scen1Answer3,
                    scen1_reaction1 = @Scen1Reaction1,
                    scen1_reaction2 = @Scen1Reaction2,
                    scen1_reaction3 = @Scen1Reaction3,

                    scen2_text = @Scen2Text,
                    scen2_answer1 = @Scen2Answer1,
                    scen2_answer2 = @Scen2Answer2,
                    scen2_answer3 = @Scen2Answer3,
                    scen2_reaction1 = @Scen2Reaction1,
                    scen2_reaction2 = @Scen2Reaction2,
                    scen2_reaction3 = @Scen2Reaction3
                WHERE company_id = @CompanyId;";

            var scenarios = game.Scenarios;

            string? GetAdvice(int scenarioIndex, int adviceIndex)
            {
                if (scenarioIndex < 0 || scenarioIndex >= scenarios.Count)
                    return null;

                var scenario = scenarios[scenarioIndex];
                if (adviceIndex < 0 || adviceIndex >= scenario.AdviceChoices.Count)
                    return null;

                return scenario.AdviceChoices[adviceIndex]?.Advice;
            }

            string? GetFeedback(int scenarioIndex, int adviceIndex)
            {
                if (scenarioIndex < 0 || scenarioIndex >= scenarios.Count)
                    return null;

                var scenario = scenarios[scenarioIndex];
                if (adviceIndex < 0 || adviceIndex >= scenario.AdviceChoices.Count)
                    return null;

                return scenario.AdviceChoices[adviceIndex]?.Feedback;
            }

            string? scen1Text = scenarios.Count > 0 ? scenarios[0].Description : null;
            string? scen2Text = scenarios.Count > 1 ? scenarios[1].Description : null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
            cmd.Parameters.AddWithValue("@AvatarId", game.Buddy.Id);
            cmd.Parameters.AddWithValue("@BuddyName", game.Buddy.Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FinalQuote", game.Conclusion ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Scen1Text", scen1Text ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen2Text", scen2Text ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Scen1Answer1", GetAdvice(0, 0) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen1Answer2", GetAdvice(0, 1) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen1Answer3", GetAdvice(0, 2) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen1Reaction1", GetFeedback(0, 0) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen1Reaction2", GetFeedback(0, 1) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen1Reaction3", GetFeedback(0, 2) ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@Scen2Answer1", GetAdvice(1, 0) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen2Answer2", GetAdvice(1, 1) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen2Answer3", GetAdvice(1, 2) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen2Reaction1", GetFeedback(1, 0) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen2Reaction2", GetFeedback(1, 1) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Scen2Reaction3", GetFeedback(1, 2) ?? (object)DBNull.Value);

            conn.Open();
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0)
                throw new InvalidOperationException($"No company found with id '{companyId}'.");
        }

        public Game GetGameById(int id)
        {
            // Backward compatible: old method name now acts as company-specific lookup.
            return GetGame(id);
        }

        public void SaveGame(Game game)
        {
            // Backward compatible: old method saves to a "default" company.
            // If you need per-company editing, call SaveGame(game, companyId) instead.
            SaveGame(game, 0);
        }

        public Game Get() => GetGame(0);

        public void Save(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            SaveGame(game, 0);
        }

        // Public so other repos (like CompanyRepo) can reuse the mapper.
        public Game MapGame(SqlDataReader reader)
        {
            var buddy = new Buddy(
                reader["avatar_id"] is DBNull ? 0 : Convert.ToInt32(reader["avatar_id"]),
                reader["buddy_name"]?.ToString() ?? "",
                // Your sample mapper hardcodes an empty introduction. If your DB has a column for it,
                // tell me its name and I’ll wire it up.
                ""
            );

            var scenarios = new List<Scenario>();

            if (!(reader["scen_1_text"] is DBNull))
            {
                var scen1 = new Scenario(reader["scen_1_text"].ToString() ?? "");

                scen1.AddChoice(new AdviceChoice(
                    reader["scen1_answer1"]?.ToString() ?? "",
                    reader["scen1_reaction1"]?.ToString() ?? ""
                ));

                scen1.AddChoice(new AdviceChoice(
                    reader["scen1_answer2"]?.ToString() ?? "",
                    reader["scen1_reaction2"]?.ToString() ?? ""
                ));

                scen1.AddChoice(new AdviceChoice(
                    reader["scen1_answer3"]?.ToString() ?? "",
                    reader["scen1_reaction3"]?.ToString() ?? ""
                ));

                scenarios.Add(scen1);
            }

            if (!(reader["scen2_text"] is DBNull))
            {
                var scen2 = new Scenario(reader["scen2_text"].ToString() ?? "");

                scen2.AddChoice(new AdviceChoice(
                    reader["scen2_answer1"]?.ToString() ?? "",
                    reader["scen2_reaction1"]?.ToString() ?? ""
                ));

                scen2.AddChoice(new AdviceChoice(
                    reader["scen2_answer2"]?.ToString() ?? "",
                    reader["scen2_reaction2"]?.ToString() ?? ""
                ));

                scen2.AddChoice(new AdviceChoice(
                    reader["scen2_answer3"]?.ToString() ?? "",
                    reader["scen2_reaction3"]?.ToString() ?? ""
                ));

                scenarios.Add(scen2);
            }

            return new Game(
                buddy,
                scenarios,
                reader["final_quote"]?.ToString() ?? "",
                true
            );
        }
    }
}
