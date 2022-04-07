using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;
    [SerializeField]private GameObject coinPrefab;
    [SerializeField]private GameObject engel;
    int status = 0;
    bool canTap, isStatus1,isStatus2;
    float tempY;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        StartingEvents();
        StartCoroutine(CalculateCoins());
    }

	private void Update()
	{


		if (Input.GetMouseButtonDown(0) && canTap)
		{
			if (status == 0)
			{
				isStatus1 = true;
				status = 1;
				canTap = false;
				StartCoroutine(Tap1());
			}
			else if (status == 1)
			{
				isStatus2 = true;
				status = 2;
				canTap = false;
				StartCoroutine(Tap2());
			}
		}
	}

	/// <summary>
	/// Playerin collider olaylari.. collectible, engel veya finish noktasi icin. Burasi artirilabilir.
	/// elmas icin veya baska herhangi etkilesimler icin tag ekleyerek kontrol dongusune eklenir.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("collectible"))
        {
            // COLLECTIBLE CARPINCA YAPILACAKLAR...
            GameController.instance.SetScore(collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku

        }
        else if (other.CompareTag("engel"))
        {
            // ENGELELRE CARPINCA YAPILACAKLAR....
            GameController.instance.SetScore(-collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
            if (GameController.instance.score < 0) // SKOR SIFIRIN ALTINA DUSTUYSE
			{
                // FAİL EVENTLERİ BURAYA YAZILACAK..
                GameController.instance.isContinue = false; // çarptığı anda oyuncunun yerinde durması ilerlememesi için
                UIController.instance.ActivateLooseScreen(); // Bu fonksiyon direk çağrılada bilir veya herhangi bir effect veya animasyon bitiminde de çağrılabilir..
                // oyuncu fail durumunda bu fonksiyon çağrılacak.. 
			}


        }
        else if (other.CompareTag("finish")) 
        {
            // finishe collider eklenecek levellerde...
            // FINISH NOKTASINA GELINCE YAPILACAKLAR... Totalscore artırma, x işlemleri, efektler v.s. v.s.
            GameController.instance.isContinue = false;
            GameController.instance.ScoreCarp(7);  // Bu fonksiyon normalde x ler hesaplandıktan sonra çağrılacak. Parametre olarak x i alıyor. 
            // x değerine göre oyuncunun total scoreunu hesaplıyor.. x li olmayan oyunlarda parametre olarak 1 gönderilecek.
            UIController.instance.ActivateWinScreen(); // finish noktasına gelebildiyse her türlü win screen aktif edilecek.. ama burada değil..
            // normal de bu kodu x ler hesaplandıktan sonra çağıracağız. Ve bu kod çağrıldığında da kazanılan puanlar animasyonlu şekilde artacak..

            
        }

    }


    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {

        //transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        //transform.parent.transform.position = Vector3.zero;
        //GameController.instance.isContinue = false;
        //GameController.instance.score = 0;
        //transform.position = new Vector3(0, transform.position.y, 0);
        //GetComponent<Collider>().enabled = true;

    }



    public IEnumerator ThrowPlayer()
	{
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power/2;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 && !isStatus1)
		{
            if (tempY > transform.position.y) canTap = true;
			tempY = transform.position.y;
            time += 1 / (distance*5);
            if(aci < 180) aci = 180 *time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci)/(100/power);
            pos.z += .5f;
            transform.position = pos;
            yield return new WaitForSeconds(.015f);
        }
    }

    public IEnumerator Tap1()
    {
        StopCoroutine(ThrowPlayer());
        StartCoroutine(CalculateCoins2());
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power/3;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 && !isStatus2)
        {
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            transform.position = pos;
            yield return new WaitForSeconds(.015f);
        }
    }

    public IEnumerator Tap2()
    {
        StopCoroutine(Tap1());
        StartCoroutine(CalculateCoins3());
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power/4;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 )
        {
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            transform.position = pos;
            yield return new WaitForSeconds(.015f);
        }
    }

    public IEnumerator CalculateCoins()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power / 2;
        float time = 0;
        float aci = 0;
        Vector3 pos = engel.transform.position;
        while (time < 1 )
        {
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 15);
            if (rnd == 0)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "collectible";
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CalculateCoins2()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power / 3;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1)
        {
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 15);
            if (rnd == 0)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "collectible";
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CalculateCoins3()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power / 4;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1)
        {
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 15);
            if (rnd == 0)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "collectible";
            }
            yield return new WaitForEndOfFrame();
        }
    }



}
