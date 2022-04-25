using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public static GameController instance; // singleton yapisi icin gerekli ornek ayrintilar icin BeniOku 22. satirdan itibaren bak.
    [HideInInspector] public bool isContinue;  // ayrintilar icin beni oku 19. satirdan itibaren bak
    [Header("Arac gucu - Power")]
    public int power;
    [Header("Rampa Y?ksekligi - Height")]
    public int height;
    [Header("Araban?n do?rusal ve dairesel h?z?n? etkileyen carpan")]
    public float aracSpeed;
    public float aracrRotSpeed;
    [Header("Para")]
    public int para;

    [Header("Diger Degiskenler")]
    public GameObject heightPlatform;
    public Transform carTarget;
    [HideInInspector] public int levelPara;
    public List<GameObject> vehicles = new();
    public Animator DummyAnim;
    public int type;
    [HideInInspector] public bool firstCrash;
    public GameObject zeminTarget;
    public GameObject coinPrefab, birdPrefab,parlamaPrefab;
    int fiyatPower, fiyatHeight;
    public float yakit;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
         PlayerPrefs.DeleteAll();
        //para = 250000;
  //      PlayerPrefs.SetInt("para", para);
		//PlayerPrefs.SetInt("power", power);
		//PlayerPrefs.SetInt("height", height);
		fiyatPower = PlayerPrefs.GetInt("fiyatp");
        fiyatHeight = PlayerPrefs.GetInt("fiyath");
        if (fiyatPower == 0)
        {
            fiyatPower = 50;
            PlayerPrefs.SetInt("fiyatp", 50);
        }
        if (fiyatHeight == 0)
        {
            fiyatHeight = 50;
            PlayerPrefs.SetInt("fiyath", 50);
        }


		power = PlayerPrefs.GetInt("power");
		height = PlayerPrefs.GetInt("height");
		yakit = PlayerPrefs.GetFloat("yakit");

        if (power == 0)
        {
            power = 1;
            PlayerPrefs.SetInt("power", 1);
        }
        if (height == 0)
        {
            height = 1;
            PlayerPrefs.SetInt("height", 1);
        }
        if(yakit == 0)
		{
            Debug.Log("yakit" + yakit);
            yakit = 10;
            PlayerPrefs.SetFloat("yakit", 10);
            Debug.Log("yakit" + yakit);
        }
        para = PlayerPrefs.GetInt("para");
        //power = 15;
        //height = 15;
        isContinue = false;
        SetHeightPlatform();
        UIController.instance.SetPowerAndLevelText();
        SetVehicleType();

        Debug.Log(para);
    }

    public void IncreasePower()
    {
        if (para >= fiyatPower)
        {
            yakit += .5f;
            AracControl.instance.anlikYakit = yakit;
            PlayerPrefs.SetFloat("yakit", yakit);

            para -= fiyatPower;
            power++;
            if(power %2 == 0 && power < 44)
			{
                Instantiate(parlamaPrefab,AracControl.instance.transform.position + new Vector3(0,3,0),Quaternion.identity);
			}
            SetVehicleType();
            fiyatPower = 100 + (power * power * 4);
            PlayerPrefs.SetInt("para", para);
            PlayerPrefs.SetInt("fiyatp", fiyatPower);
            PlayerPrefs.SetInt("power", power);
        }
        UIController.instance.SetPowerAndLevelText();
        UIController.instance.ControlButtonsActivate();
        UIController.instance.SetSpeedSlider();
    }

    public void IncreaseHeight()
    {
        if (para >= fiyatHeight)
        {
            
            para -= fiyatHeight;
            height++;
            SetVehicleType();
            SetHeightPlatform();
            fiyatHeight = 100 + (height * height * 4);
            PlayerPrefs.SetInt("fiyath", fiyatHeight);
            PlayerPrefs.SetInt("para", para);
            PlayerPrefs.SetInt("height", height);

        }
        UIController.instance.SetPowerAndLevelText();
        UIController.instance.ControlButtonsActivate();
    }

    public void SetHeightPlatform()
    {
        float y = (16f / 100f) * height;
        float z = (-11f / 100f) * height;

        heightPlatform.transform.position = new Vector3(0, 5.2f, -2.2f) + new Vector3(0, y, z);
        AracControl.instance.UpdateRoad();
        AracControl.instance.transform.position = carTarget.position;
        AracControl.instance.transform.rotation = carTarget.rotation;
    }


    public void SetVehicleType()
    {
        type = (int)power / 2;


        foreach (GameObject vehicle in vehicles)
        {
            vehicle.SetActive(false);
        }


        if (power > 44) type = 23;
        vehicles[type].SetActive(true);

        if (type > 3 && type < 8)
        {
            DummyAnim.SetTrigger("keko");
        }
        else if (type >= 8) DummyAnim.SetTrigger("koltuk");


        if (type == 0) PlayerController.instance.transform.localPosition = new Vector3(0, .5f, .49f);
        else if (type < 4) PlayerController.instance.transform.localPosition = new Vector3(0, .66f, .49f);
        else if (type == 4) PlayerController.instance.transform.localPosition = new Vector3(0, 0f, .49f);
        else if (type > 4 && type <= 7) PlayerController.instance.transform.localPosition = new Vector3(0, -1f, 0);
        else if (type == 8) PlayerController.instance.transform.localPosition = new Vector3(-0.27f, 0, -0.66f);
        else if (type > 8 && type <= 11)
        {
            PlayerController.instance.transform.localPosition = new Vector3(-0.56f, 1, -0.66f);
        }
        else if (type == 12) PlayerController.instance.transform.localPosition = new Vector3(-0.37f, -0.3f, -0.28f);
        else if (type > 12 && type <= 15)
        {
            PlayerController.instance.transform.localPosition = new Vector3(-0.5f, 0.43f, -0.46f);
        }
        else if (type == 16) PlayerController.instance.transform.localPosition = new Vector3(-0.7f, -0.55f, -0.67f);
        else if (type > 16 && type <= 19)
        {
            PlayerController.instance.transform.localPosition = new Vector3(-0.85f, 0.26f, -0.58f);
        }
        else if (type == 20) PlayerController.instance.transform.localPosition = new Vector3(-0.5f, -0.92f, -0.37f);
        else if (type > 20)
        {
            PlayerController.instance.transform.localPosition = new Vector3(-0.6f, 0.33f, -0.1f);
        }



    }


    public void SetAracSpeedAndRotate()
    {
        int level = power + height;
        aracSpeed = aracrRotSpeed = 12 + level * .5f;
    }

    public void PreStartingEvents()
    {
        AracControl.instance.cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 2;
        AracControl.instance.cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 2;
        AracControl.instance.cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 2;
        AracControl.instance.cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_YawDamping = 2;
        AracControl.instance.anlikYakit = yakit;
        firstCrash = true;
        isContinue = true;
        levelPara = 0;
        SetHeightPlatform();
        SetAracSpeedAndRotate();
    }

}
