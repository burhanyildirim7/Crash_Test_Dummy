using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance; // singleton yapisi icin gerekli ornek ayrintilar icin BeniOku 22. satirdan itibaren bak.
    [HideInInspector] public bool isContinue;  // ayrintilar icin beni oku 19. satirdan itibaren bak
    [Header("Arac gucu - Power")]
    public int power;
    [Header("Rampa Yüksekligi - Height")]
    public int height;
    [Header("Ilk Firlatma Azaltma Katsayýsý")]
    public float firlatmaForce = 1;
    [Header("Ikinci Firlatma Azaltma Katsayýsý")]
    public float firlatmaAzaltma1 = 2;
    [Header("Ucuncu Firlatma Azaltma Katsayýsý")]
    public float firlatmaAzaltma2 = 4;
    [Header("Arabanýn doðrusal ve dairesel hýzý")]
    public float aracSpeed;
    [Header("Guc carpani.. addforce degerini etkiler")]
    public float gucCarpani;
    [Header("Para")]
    public int para;
    public float aracrRotSpeed;
    [Header("Diger Degiskenler")]
    public GameObject heightPlatform;
    public Transform carTarget;
    [HideInInspector] public int levelPara;
    public List<GameObject> vehicles = new();
    public Animator DummyAnim;
    public int type;
    [HideInInspector]public bool firstCrash;
    public GameObject zeminTarget;
    public GameObject coinPrefab, birdPrefab;
    int fiyatPower,fiyatHeight;


    private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	void Start()
    {
        PlayerPrefs.DeleteAll();
        //para = 250000;
        PlayerPrefs.SetInt("para", para);
        PlayerPrefs.SetInt("power", power);
        PlayerPrefs.SetInt("height", height);
        fiyatPower = PlayerPrefs.GetInt("fiyatp");
        fiyatHeight = PlayerPrefs.GetInt("fiyath");
        if(fiyatPower == 0)
		{
            fiyatPower = 10;
            PlayerPrefs.SetInt("fiyatp",10);
		}
        if (fiyatHeight == 0)
        {
            fiyatHeight = 10;
            PlayerPrefs.SetInt("fiyath", 10);
        }

        
        //power = PlayerPrefs.GetInt("power");
        //height = PlayerPrefs.GetInt("height");
        //para = PlayerPrefs.GetInt("para");
        //power = 15;
        //height = 15;
        isContinue = false;
        SetHeightPlatform();
        UIController.instance.SetPowerAndLevelText();
        SetVehicleType();
    }

    public void IncreasePower()
	{
        if(para >= fiyatPower)
		{
            Debug.Log("ilk " + para);
            Debug.Log("ilk " + fiyatPower);
            Debug.Log("ilk " + power);
            para -= fiyatPower;
            power++;
            fiyatPower += fiyatPower / 2;
            PlayerPrefs.SetInt("para", para);
            PlayerPrefs.SetInt("fiyatp", fiyatPower);
            PlayerPrefs.SetInt("power", power);
            Debug.Log("son " + para);
            Debug.Log("son " + fiyatPower);
            Debug.Log("son " + power);
        }
        UIController.instance.SetPowerAndLevelText();
        UIController.instance.ControlButtonsActivate();
	}

    public void IncreaseHeight()
	{
        if (para >= fiyatHeight)
        {
           
            para -= fiyatHeight;
            height++;
            SetHeightPlatform();
            fiyatHeight += fiyatHeight / 2; 
            PlayerPrefs.SetInt("fiyath", fiyatHeight);
            PlayerPrefs.SetInt("para", para);
            PlayerPrefs.SetInt("height", height);
        }
        UIController.instance.SetPowerAndLevelText();
        UIController.instance.ControlButtonsActivate();
    }

    public void SetHeightPlatform()
	{
        heightPlatform.transform.position = new Vector3(0, 2 + (float)height/4, 0);
        zeminTarget.transform.position = new Vector3(0,.95f,5.5f+((float)height /10));
        AracControl.instance.transform.position = carTarget.position;
        AracControl.instance.transform.rotation = carTarget.rotation;
    }


    public void SetVehicleType()
	{
        type = (int) power / 4;
        foreach(GameObject vehicle in vehicles)
		{
            vehicle.SetActive(false);
		}
        if (power > 92) type = 23; 
        vehicles[type].SetActive(true);

        if (type > 3 && type < 8) 
        { 
            DummyAnim.SetTrigger("keko");
        }
        else if (type >= 8) DummyAnim.SetTrigger("koltuk");

        if (type < 4) PlayerController.instance.transform.localPosition = new Vector3(0, 1.17f, .49f);
        else if(type == 4) PlayerController.instance.transform.localPosition = new Vector3(0, .5f, .49f);
        else if (type > 4 && type <= 7) PlayerController.instance.transform.localPosition = new Vector3(0, -.5f, 0);
        else if(type == 8) PlayerController.instance.transform.localPosition = new Vector3(-0.27f, 0, -0.66f);
        else if(type > 8 && type <= 11) PlayerController.instance.transform.localPosition = new Vector3(-0.56f, 1, -0.66f);
        else if(type == 12 ) PlayerController.instance.transform.localPosition = new Vector3(-0.37f, -0.14f, -0.28f);
        else if(type > 12 && type <= 15 ) PlayerController.instance.transform.localPosition = new Vector3(-0.5f, 0.43f, -0.46f);
        else if(type == 16 ) PlayerController.instance.transform.localPosition = new Vector3(-0.7f, -0.37f, -0.67f);
        else if(type > 16 && type <= 19) PlayerController.instance.transform.localPosition = new Vector3(-0.85f, 0.59f, -0.58f);
        else if(type == 20) PlayerController.instance.transform.localPosition = new Vector3(-0.5f, -0.62f, -0.37f);
        else if(type > 20) PlayerController.instance.transform.localPosition = new Vector3(-0.6f, -0.17f, -0.1f);
    }


    public void SetAracSpeedAndRotate()
	{
        int level = power + height;
        aracSpeed = aracrRotSpeed = 12 + level * .2f;
	}

    public void PreStartingEvents()
	{
        firstCrash = true;
        isContinue = true;
        levelPara = 0;
        SetHeightPlatform();
        SetAracSpeedAndRotate();
    }

}
