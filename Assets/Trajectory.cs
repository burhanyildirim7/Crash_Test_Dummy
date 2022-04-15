using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
	public static Trajectory instance;
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
        CreatePhysicsScene();
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

}
