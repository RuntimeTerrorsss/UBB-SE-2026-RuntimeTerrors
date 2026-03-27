using Microsoft.Data.SqlClient;
using OurApp.Core.Models;
using System;

namespace OurApp.Core.Repositories
{
    public class GameRepo : IGameRepo
    {
        public Game GetGameById(int id)
        {
            // DB: SELECT ...
            _ = id;
            return new Game();
        }

        public void SaveGame(Game game)
        {
            // DB: INSERT / UPDATE
            _ = game;
        }

        public Game Get() => GetGameById(0);

        public void Save(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            SaveGame(game);
        }

        public Game MapGame(SqlDataReader reader)
        {
            var buddy = new Buddy(
            reader["avatar_id"] is DBNull ? 0 : Convert.ToInt32(reader["avatar_id"]),
            reader["buddy_name"]?.ToString() ?? "",
            ""
            );

            var scenarios = new List<Scenario>();

            if (!(reader["scen_1_text"] is DBNull))
            {
                var scen1 = new Scenario(reader["scen_1_text"].ToString());

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
                var scen2 = new Scenario(reader["scen2_text"].ToString());

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
