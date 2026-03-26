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

        public Game GetGameById(int id)
        {
            // This app currently stores a single active game (no multi-game selection in IGameRepo).
            // If you later add multi-game support, update this method to query by a real primary key.
            if (id != 0)
                throw new NotSupportedException("GameRepo currently supports a single active game row only (id must be 0).");

            var sql =
                $@"SELECT TOP 1
                        avatar_id,
                        buddy_name,
                        scen_1_text,
                        scen2_text,
                        scen1_answer1, scen1_reaction1,
                        scen1_answer2, scen1_reaction2,
                        scen1_answer3, scen1_reaction3,
                        scen2_answer1, scen2_reaction1,
                        scen2_answer2, scen2_reaction2,
                        scen2_answer3, scen2_reaction3,
                        final_quote
                   FROM {TableName};";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!reader.Read())
                return new Game();

            return MapGame(reader);
        }

        public void SaveGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));

            // Upsert a single active row. If your DB uses a different key strategy, adjust accordingly.
            var sql =
                $@"IF EXISTS (SELECT 1 FROM {TableName})
                    BEGIN
                        UPDATE {TableName}
                        SET
                            avatar_id = @avatar_id,
                            buddy_name = @buddy_name,
                            scen_1_text = @scen_1_text,
                            scen2_text = @scen2_text,
                            scen1_answer1 = @scen1_answer1,
                            scen1_reaction1 = @scen1_reaction1,
                            scen1_answer2 = @scen1_answer2,
                            scen1_reaction2 = @scen1_reaction2,
                            scen1_answer3 = @scen1_answer3,
                            scen1_reaction3 = @scen1_reaction3,
                            scen2_answer1 = @scen2_answer1,
                            scen2_reaction1 = @scen2_reaction1,
                            scen2_answer2 = @scen2_answer2,
                            scen2_reaction2 = @scen2_reaction2,
                            scen2_answer3 = @scen2_answer3,
                            scen2_reaction3 = @scen2_reaction3,
                            final_quote = @final_quote;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO {TableName} (
                            avatar_id,
                            buddy_name,
                            scen_1_text,
                            scen2_text,
                            scen1_answer1, scen1_reaction1,
                            scen1_answer2, scen1_reaction2,
                            scen1_answer3, scen1_reaction3,
                            scen2_answer1, scen2_reaction1,
                            scen2_answer2, scen2_reaction2,
                            scen2_answer3, scen2_reaction3,
                            final_quote
                        )
                        VALUES (
                            @avatar_id,
                            @buddy_name,
                            @scen_1_text,
                            @scen2_text,
                            @scen1_answer1, @scen1_reaction1,
                            @scen1_answer2, @scen1_reaction2,
                            @scen1_answer3, @scen1_reaction3,
                            @scen2_answer1, @scen2_reaction1,
                            @scen2_answer2, @scen2_reaction2,
                            @scen2_answer3, @scen2_reaction3,
                            @final_quote
                        );
                    END";

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

            var scenario1Text = scenarios.Count > 0 ? scenarios[0].Description : null;
            var scenario2Text = scenarios.Count > 1 ? scenarios[1].Description : null;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@avatar_id", SqlDbType.Int).Value = game.Buddy.Id;
            cmd.Parameters.Add("@buddy_name", SqlDbType.NVarChar, 200).Value =
                (object?)(game.Buddy.Name ?? string.Empty);

            cmd.Parameters.Add("@scen_1_text", SqlDbType.NVarChar).Value =
                (object?)scenario1Text ?? DBNull.Value;
            cmd.Parameters.Add("@scen2_text", SqlDbType.NVarChar).Value =
                (object?)scenario2Text ?? DBNull.Value;

            // Scenario 1 (index 0)
            cmd.Parameters.Add("@scen1_answer1", SqlDbType.NVarChar).Value = (object?)GetAdvice(0, 0) ?? DBNull.Value;
            cmd.Parameters.Add("@scen1_reaction1", SqlDbType.NVarChar).Value = (object?)GetFeedback(0, 0) ?? DBNull.Value;
            cmd.Parameters.Add("@scen1_answer2", SqlDbType.NVarChar).Value = (object?)GetAdvice(0, 1) ?? DBNull.Value;
            cmd.Parameters.Add("@scen1_reaction2", SqlDbType.NVarChar).Value = (object?)GetFeedback(0, 1) ?? DBNull.Value;
            cmd.Parameters.Add("@scen1_answer3", SqlDbType.NVarChar).Value = (object?)GetAdvice(0, 2) ?? DBNull.Value;
            cmd.Parameters.Add("@scen1_reaction3", SqlDbType.NVarChar).Value = (object?)GetFeedback(0, 2) ?? DBNull.Value;

            // Scenario 2 (index 1)
            cmd.Parameters.Add("@scen2_answer1", SqlDbType.NVarChar).Value = (object?)GetAdvice(1, 0) ?? DBNull.Value;
            cmd.Parameters.Add("@scen2_reaction1", SqlDbType.NVarChar).Value = (object?)GetFeedback(1, 0) ?? DBNull.Value;
            cmd.Parameters.Add("@scen2_answer2", SqlDbType.NVarChar).Value = (object?)GetAdvice(1, 1) ?? DBNull.Value;
            cmd.Parameters.Add("@scen2_reaction2", SqlDbType.NVarChar).Value = (object?)GetFeedback(1, 1) ?? DBNull.Value;
            cmd.Parameters.Add("@scen2_answer3", SqlDbType.NVarChar).Value = (object?)GetAdvice(1, 2) ?? DBNull.Value;
            cmd.Parameters.Add("@scen2_reaction3", SqlDbType.NVarChar).Value = (object?)GetFeedback(1, 2) ?? DBNull.Value;

            cmd.Parameters.Add("@final_quote", SqlDbType.NVarChar).Value = (object?)game.Conclusion ?? DBNull.Value;

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public Game Get() => GetGameById(0);

        public void Save(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            SaveGame(game);
        }

        private static Game MapGame(SqlDataReader reader)
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
