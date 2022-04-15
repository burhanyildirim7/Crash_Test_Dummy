using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
	public static Trajectory instance;
    LineRenderer lineRenderer;
    int lineSegmentCount = 100;
    List<Vector3> linePoints = new();

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}

    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _obstaclesParent;

    public Scene _simulationScene;
    public PhysicsScene _physicsScene;
    public Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

  

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
       // CreatePhysicsScene();
    }

    public void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();
    }


    public void AddGhostToScene(Transform obj2)
    {
        var ghostObj2 = Instantiate(obj2.gameObject, obj2.position, obj2.rotation);
        ghostObj2.GetComponentInChildren<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj2, _simulationScene);
    }

    public void ClearForNewScene()
    {
        foreach (GameObject obj in _simulationScene.GetRootGameObjects())
        {
            Destroy(obj.gameObject);
        }
        _spawnedObjects.Clear();
    }


    public void SimulateTrajectory(GameObject playerHips, Vector3 pos, Vector3 velocity)
    {
        var ghostObj = Instantiate(playerHips, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

        ghostObj.GetComponent<Rigidbody>().AddForce(velocity);

        _line.positionCount = (int)velocity.z;

        for (var i = 0; i < 300; i++)
        {
            int rnd = Random.Range(1,300);
            if(rnd < 10)
			{
                GameObject obj = Instantiate(GameController.instance.coinPrefab,ghostObj.transform.position,Quaternion.identity);
                obj.transform.tag = "para";
                obj.transform.parent = PlayerController.instance.paralarParenti.transform;
			}
            else if(rnd == 11)
			{
                GameObject obj = Instantiate(GameController.instance.birdPrefab, ghostObj.transform.position, Quaternion.identity);
                obj.transform.tag = "kus";
                obj.transform.parent = PlayerController.instance.paralarParenti.transform;
            }
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostObj.transform.position);
        }
        Destroy(ghostObj.gameObject);
    }

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidBody, Vector3 startingPoint)
	{
        //lineSegmentCount = 70 + (GameController.instance.power + GameController.instance.height)*2;
        lineSegmentCount = 10 + ((int)forceVector.z /500);
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;

        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;

        float stepTime = FlightDuration / lineSegmentCount;

        linePoints.Clear();

		for (int i = 0; i < lineSegmentCount; i++)
		{


            float stepTimePassed = stepTime * i;

            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z*stepTimePassed
                );

            linePoints.Add(-MovementVector + startingPoint);

            //int rnd = Random.Range(1, 200);
            //if (rnd < 10)
            //{
            //    GameObject obj = Instantiate(GameController.instance.coinPrefab, -MovementVector + startingPoint, Quaternion.identity);
            //    obj.transform.tag = "para";
            //    obj.transform.parent = PlayerController.instance.paralarParenti.transform;
            //}
        }

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());

		for (int i = 5; i < linePoints.Count; i+=5)
		{
            Debug.Log(i);
            int rnd = Random.Range(1,3);
            if (rnd == 1)
            {
                GameObject obj = Instantiate(GameController.instance.coinPrefab, linePoints[i], Quaternion.identity);
                obj.transform.tag = "para";
                obj.transform.parent = PlayerController.instance.paralarParenti.transform;
            }
        }
	}

}
