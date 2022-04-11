using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance; // singleton yapisi icin gerekli ornek ayrintilar icin BeniOku 22. satirdan itibaren bak.
    [HideInInspector] public bool isContinue;  // ayrintilar icin beni oku 19. satirdan itibaren bak
    [Header("Arac gucu - Power")]
    public int power;
    [Header("Rampa Y�ksekligi - Height")]
    public int height;
    [Header("Ilk Firlatma Azaltma Katsay�s�")]
    public float firlatmaForce = 1;
    [Header("Ikinci Firlatma Azaltma Katsay�s�")]
    public float firlatmaAzaltma1 = 2;
    [Header("Ucuncu Firlatma Azaltma Katsay�s�")]
    public float firlatmaAzaltma2 = 4;
    [Header("Diger Degiskenler")]
    public GameObject heightPlatform;
    public Transform carTarget;
    [HideInInspector] public int para;
    public List<GameObject> vehicles = new();

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	void Start()
    {
       // PlayerPrefs.DeleteAll();
        para = 250000;
        PlayerPrefs.SetInt("para", para);
        PlayerPrefs.SetInt("power", power);
        PlayerPrefs.SetInt("height", height);

        //power = PlayerPrefs.GetInt("power");
        //height = PlayerPrefs.GetInt("height");
        //para = PlayerPrefs.GetInt("para");
        //power = 15;
        //height = 15;
        isContinue = false;
        SetHeightPlatform();
        UIController.instance.SetPowerAndLevelText();
    }

    public void IncreasePower()
	{
        if(para >= 20 * power)
		{
            
            para -= 20 * power;
            power++;
            PlayerPrefs.SetInt("para", para);
            PlayerPrefs.SetInt("power", power);
        }
        UIController.instance.SetPowerAndLevelText();
	}

    public void IncreaseHeight()
	{
        if (para >= 20 * height)
        {
           
            para -= 20 * height;
            height++;
            SetHeightPlatform();
            PlayerPrefs.SetInt("para", para);
            PlayerPrefs.SetInt("height", height);
        }
        UIController.instance.SetPowerAndLevelText();
    }

    public void SetHeightPlatform()
	{
        heightPlatform.transform.position = new Vector3(0, height, 0);
        //AracControl.instance.transform.position = carTarget.position;
        //AracControl.instance.transform.rotation = carTarget.rotation;
        
    }

    public void SetVehicleType()
	{
        int type = (int) power / 5;
        foreach(GameObject vehicle in vehicles)
		{
            vehicle.SetActive(false);
		}
        vehicles[type].SetActive(true);
	}


}
