using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_TurtleAgent : MonoBehaviour
{
    [SerializeField] private TurtleAgent _turtleAgent;  // Reference to TurtleAgent class

    private GUIStyle _defautStyle = new GUIStyle();
    private GUIStyle _positionStyle = new GUIStyle();
    private GUIStyle _negativeStyle = new GUIStyle();

    // Start is called before the first frame update
    void Start()
    {
        // Define GUI Styles
        _defautStyle.fontSize = 20;
        _defautStyle.normal.textColor = Color.yellow;

        _positionStyle.fontSize = 20;
        _positionStyle.normal.textColor = Color.green;

        _negativeStyle.fontSize = 20;
        _negativeStyle.normal.textColor = Color.red;
    }

    // Runs every frame and allows us to draw GUI elements directly onto the screen
    private void OnGUI()
    {
        string debugEpisode = "Episode: " + _turtleAgent.CurrentEpisode + " - Step: " + _turtleAgent.StepCount;  // StepCount (Agent's attribute)
        string debugReward = "Reward: " + _turtleAgent.CumulativeReward.ToString();

        // Select Style based on reward value
        GUIStyle rewardStyle = _turtleAgent.CumulativeReward < 0? _negativeStyle : _positionStyle;

        // Display the debug text
        GUI.Label(new Rect(20, 20, 500, 30), debugEpisode, _defautStyle);
        GUI.Label(new Rect(20, 60, 500, 30), debugReward, rewardStyle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
