using Beamable.ConsoleCommands;
using UnityEngine;

[BeamableConsoleCommandProvider]
public class CustomConsoleCommandProvider
{
    [BeamableConsoleCommand("add health", "adds health", "Add <int>")]
    public string Add(string[] args)
    {
        var a = int.Parse(args[0]);
        return "";
    }
}

public class AdminFlow : MonoBehaviour
{


    /// <summary>
    /// Demonstrates <see cref="AdminFlow"/>.
    /// </summary>
    public class AdminFlowCustomCommandExample : MonoBehaviour
    {
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            Debug.Log($"Start() Instructions...\n" +
                      " * Run The Scene\n" +
                      " * Type '~' in Unity Game Window to open Admin Console\n" +
                      " * Type 'Add 5 10'\n" +
                      " * See 'Result: 15' in Unity Console Window\n");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.CurrentActivePlayer.Health += 20;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
