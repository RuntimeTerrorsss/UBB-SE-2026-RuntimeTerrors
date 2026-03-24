using OurApp.Core.Models;

public class Game
{
    public Buddy Buddy { get; private set; }
    public IReadOnlyList<Scenario> Scenarios => _scenarios;
    private readonly List<Scenario> _scenarios;

    public string Conclusion { get; private set; }
    public bool IsPublished { get; private set; }

    public Game()
    {
        Buddy = new Buddy(0, "Default", "Default");
        _scenarios = new List<Scenario>();
        Conclusion = string.Empty;
    }

    public Game(Buddy buddy, IEnumerable<Scenario> scenarios, string conclusion, bool isPublished = false)
    {
        Buddy = buddy ?? throw new ArgumentNullException(nameof(buddy));
        _scenarios = scenarios?.ToList() ?? throw new ArgumentNullException(nameof(scenarios));
        Conclusion = conclusion ?? string.Empty;
        IsPublished = isPublished;
    }

    public void AddScenario(Scenario scenario)
    {
        _scenarios.Add(scenario);
    }

    public void SetConclusion(string conclusion)
    {
        Conclusion = conclusion ?? string.Empty;
    }

    public void Publish()
    {
        IsPublished = true;
    }
}