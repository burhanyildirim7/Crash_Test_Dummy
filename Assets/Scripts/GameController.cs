using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance; // singleton yapisi icin gerekli ornek ayrintilar icin BeniOku 22. satirdan itibaren bak.
    [HideInInspector]public int score, elmas; // ayrintilar icin benioku 9. satirdan itibaren bak
    [HideInInspector] public bool isContinue;  // ayrintilar icin beni oku 19. satirdan itibaren bak
    public float power, height;
    public GameObject heightPlatform;
    public Transform carTarget;

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	void Start()
    {
        power = 5;
        height = 5;
        isContinue = false;
        SetHeightPlatform();
    }


    /// <summary>
    /// Bu fonksiyon geçerli leveldeki scoreu belirtilen miktarda artirir veya azaltir. Artirma icin +5 gibi pozitif eksiltme
    /// icin -5 gibi negatif deger girin.
    /// </summary>
    /// <param name="eklenecekScore">Her collectible da ne kadar score eklenip cikarilacaksa parametre olarak o sayi verilmeli</param>
    public void SetScore(int eklenecekScore)
	{
        if(PlayerController.instance.collectibleVarMi) score += eklenecekScore;
        // Eðer oyunda collectible yok ise developer kendi score sistemini yazmalý...

    }

    /// <summary>
    /// Bu fonksiyon geçerli leveldeki elmasi belirtilen miktarda artirir veya azaltir. Artirma icin +5 gibi pozitif eksiltme
    /// icin -5 gibi negatif deger girin.
    /// </summary>
    /// <param name="eklenecekElmas">Her collectible da ne kadar elmas eklenip cikarilacaksa parametre olarak o sayi verilmeli</param>
    public void SetElmas(int eklenecekElmas)
    {
        elmas += eklenecekElmas;
        // buradaki elmas artýnca totalScore da otomatik olarak artacak.. bu sebeple asagidaki kodlar eklendi.
        PlayerPrefs.SetInt("totalElmas", PlayerPrefs.GetInt("totalElmas" + eklenecekElmas));
       // UIController.instance.SetTotalElmasText(); // totalElmaslarýn yazili oldugu texti
    }
    /// <summary>
    /// Oyun sonu x ler hesaplanip kac ile carpilacaksa parametre olacak o sayi gonderilmeli.
    /// </summary>
    /// <param name="katsayi"></param>
    public void ScoreCarp(int katsayi)
	{
        if (PlayerController.instance.xVarMi) score *= katsayi;
        else score = 1 * score;
        PlayerPrefs.SetInt("totalScore", PlayerPrefs.GetInt("totalScore") + score);
    }


    public void IncreasePower()
	{
        power++;
	}

    public void IncreaseHeight()
	{
        height++;
        SetHeightPlatform();
	}

    public void SetHeightPlatform()
	{
        heightPlatform.transform.position = new Vector3(0, height, 0);
        AracControl.instance.transform.position = carTarget.position;
        AracControl.instance.transform.rotation = carTarget.rotation;
        StartCoroutine(AracControl.instance.DelayAndActivateCar());
    }


}
