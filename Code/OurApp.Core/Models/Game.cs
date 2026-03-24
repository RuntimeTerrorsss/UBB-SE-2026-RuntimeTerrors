using System;
using System.Collections.Generic;
using System.Linq;
using OurApp.Core.Models;

public class Game
{
    public Buddy Buddy { get; private set; }
    public IReadOnlyList<Scenario> Scenarios => scenarios;
    private readonly List<Scenario> scenarios;

    public string Conclusion { get; private set; }
    public bool IsPublished { get; private set; }

    public Game()
    {
        Buddy = new Buddy(0, "Default", "Default");
        scenarios = new List<Scenario>();
        scenarios.Add(new Scenario("ksjdck"));
        scenarios.Add(new Scenario("Default"));
        Conclusion = string.Empty;
    }

    public Game(Buddy buddy, IEnumerable<Scenario> scenarioList, string conclusion, bool isPublished = false)
    {
        Buddy = buddy ?? throw new ArgumentNullException(nameof(buddy));
        scenarios = scenarioList?.ToList() ?? throw new ArgumentNullException(nameof(scenarioList));
        Conclusion = conclusion ?? string.Empty;
        IsPublished = isPublished;
    }

    public void AddScenario(Scenario scenario)
    {
        scenarios.Add(scenario);
    }

    public void Publish()
    {
        IsPublished = true;
    }
}
