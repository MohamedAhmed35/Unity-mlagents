using UnityEngine;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TurtleAgent : Agent
{
    [SerializeField] private Transform _goal;
    [SerializeField] private Renderer _groundRenderer;
    [SerializeField] private float _moveSpeed = 1.5f;
    [SerializeField] private float _rotationSpeed = 180f;

    private Renderer _renderer;

    [HideInInspector] public float CumulativeReward = 0f;
    [HideInInspector] public int CurrentEpisode = 0;

    private Color _defaultGroundColor;  // store the floor original color
    private Coroutine _flashGroundCoroutine;

    public override void Initialize()
    {
        Debug.Log("Initialize()");

        _renderer = GetComponent<Renderer>();
        CurrentEpisode = 0;
        CumulativeReward = 0f;

        if(_groundRenderer != null)
        {
            // Store default gray color of the ground plane
            _defaultGroundColor = _groundRenderer.material.color;
        }
    }


    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEposideBegin()");

        if(_groundRenderer != null && CumulativeReward != 0f)
        {
            Color flashColor = (CumulativeReward > 0f) ? Color.green : Color.red;

            // Stop any existing FlashGround coroutine before starting a new one
            if(_flashGroundCoroutine != null)
            {
                StopCoroutine(_flashGroundCoroutine);
            }

            // Let us smoothly fade the ground's color state without blocking the agent's execution
            // this allow the agent to continue training while the flash effect play out in the background
            _flashGroundCoroutine = StartCoroutine(FlashGround(flashColor, 3.0f));
        }

        CurrentEpisode++;
        CumulativeReward = 0f;
        _renderer.material.color = Color.blue;

        SpawnObjects();
    }

    // IEnumerator allow to define a Co-routine
    private IEnumerator FlashGround(Color targetColor, float duration)
    {
        float elapsedTime = 0f;

        _groundRenderer.material.color = targetColor;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            // gradually fade color back to ground's original color using "Lerp" function.
            _groundRenderer.material.color = Color.Lerp(targetColor, _defaultGroundColor, elapsedTime / duration);
            yield return null;
        }
    }

    private void SpawnObjects()
    {
        transform.localPosition = new Vector3(0f, 0.15f, 0f);
        transform.localRotation = Quaternion.identity;  // facing forward

        // Randomize the direction on the Y-axis (angle in degrees)
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;

        // Randomize the distance within the range [1, 2.5]
        float randomDistance = Random.Range(1f, 2.5f);

        // Calculate the goal's position
        Vector3 goalPosition = transform.localPosition + randomDirection * randomDistance;

        // Apply the calculated position to the goal
        _goal.localPosition = new Vector3(goalPosition.x, 0.25f, goalPosition.z);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("CollectObservations()");

        // The Goal's position
        // normalized by 5 because ground lenght is 10 so the normalized between -1 and 1
        float goalPosX_normalized = _goal.localPosition.x / 5f;
        float goalPosZ_normalized = _goal.localPosition.z / 5f;

        // The Turtle's position
        float turtlePosX_normalized = transform.localPosition.x / 5f;
        float turtlePosZ_normalized = transform.localPosition.z / 5f;

        // The Turtle's direction (on the Y axis)
        float turtleRotation_normalized = (transform.localRotation.eulerAngles.y / 360f) * 2f - 1f;

        sensor.AddObservation(goalPosX_normalized);
        sensor.AddObservation(goalPosZ_normalized);
        sensor.AddObservation(turtlePosX_normalized);
        sensor.AddObservation(turtlePosZ_normalized);
        sensor.AddObservation(turtleRotation_normalized);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        discreteActionsOut[0] = 0;  // don't move - do nothing!

        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[0] = 1;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActionsOut[0] = 2;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            discreteActionsOut[0] = 3;
        }    
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log("OnActionReceived()");

        // Move the agent using the action
        MoveAgent(actions.DiscreteActions);

        // Penalty given each step to encourage agent to finish task quickly
        AddReward(-2f / MaxStep);

        // Update the cumulative reward after adding the step penality
        CumulativeReward = GetCumulativeReward();
    }

    private void MoveAgent(ActionSegment<int> act)
    {
        var action = act[0];

        switch(action)
        {
            case 1: // Move forward
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;
                break;
            case 2: // Rotate left
                transform.Rotate(0f, -_rotationSpeed * Time.deltaTime, 0f);
                break;
            case 3: // Rotate right
                transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Goal"))
        {
            GoalReached();
        }
    }

    private void GoalReached()
    {
        AddReward(1.0f);    // Large reward for reaching the goal
        CumulativeReward = GetCumulativeReward();

        EndEpisode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Apply a small negative reward when the collision starts
            AddReward(-0.05f);

            // Change the color of the TurtleAgent to red
            if(_renderer != null)
            {
                _renderer.material.color = Color.red;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Continually penalize the agent while it is in contact with the wall
            AddReward(-0.01f * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            // Reset the color when the collision ends
            if (_renderer != null)
            {
                // Assuming blue is the default color
                _renderer.material.color = Color.blue;
            }
        }
    }
}

