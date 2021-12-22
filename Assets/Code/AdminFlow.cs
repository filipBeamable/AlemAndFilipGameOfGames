using Beamable.ConsoleCommands;
using UnityEngine;

[BeamableConsoleCommandProvider]
public class CustomConsoleCommandProvider
{
    [BeamableConsoleCommand("health", "adds health", "health <number>")]
    public string Add(string[] args)
    {
        var a = int.Parse(args[0]);
        PlayerController currentActivePlayer = PlayerManager.Instance.CurrentActivePlayer;
        currentActivePlayer.SetHealthSynced(currentActivePlayer.Health + a);
        return a.ToString();
    }
}

public class AdminFlow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Start() Instructions...\n" +
               " * Run The Scene\n" +
               " * Type '~' in Unity Game Window to open Admin Console\n" +
               " * Type 'Add 5 10'\n" +
               " * See 'Result: 15' in Unity Console Window\n");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
