using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Text distanceTextZemin, bestDistancneTextZemin;
    public Text distanceUiText;
    [SerializeField] float bestDistance;
    [HideInInspector] public bool distanceTextTime;
    public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject engel;
    [HideInInspector] public bool canTap;
    [HideInInspector] public bool isDistanceTime;
    public Animator playerAnimator;
    public List<Rigidbody> ragDollsRb = new();
    public GameObject cameraLookAtTarget, hips;
    [HideInInspector] public bool zeminde, havada;
    public GameObject paralarParenti, birdPrefab, onBoarding;
    public GameObject arac;
    int atisSirasi; // 0 ise ilk extra f?rlatma  1 ise ikinci extra f?rlatma m?mk?nd?r
    public GameObject groundTextCanvas, cizgi, distancePanel, bestDistancePanel;
    public GameObject kuslar, coinCalculator;
    private Vector3 kuslarPozisyon;
    public GameObject tuyEfecti;
    bool coinTime, isOnBoardingTime;
    float tempDistance;



    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        DOTween.Init();
        //PlayerPrefs.DeleteAll();
        // PlayerPrefs.SetInt("distance",100);
        StartingEvents();
        DistanceTextGroundForStart();
        zeminde = true;
        kuslarPozisyon = kuslar.transform.localPosition;
    }

    private void Update()
    {

        if (zeminde && Mathf.Abs(hips.GetComponent<Rigidbody>().velocity.z) <= .1f && isDistanceTime)
        {
            CalculateDistance();
            isDistanceTime = false;
            Time.timeScale = 1;
            onBoarding.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (havada)
            {
                if (atisSirasi == 0 && canTap && !zeminde)
                {
                    transform.DOKill();
                    Jump2();
                    isOnBoardingTime = false;
                    canTap = false;
                    atisSirasi++;
                    onBoarding.SetActive(false);
                    Time.timeScale = 1;
                    kuslar.SetActive(true);
                    kuslar.transform.position = hips.transform.position + new Vector3(0, 1, -4);
                    StartCoroutine(KuslarCarpti());

                }
                else if (atisSirasi == 1 && canTap && !zeminde)
                {
                    transform.DOKill();
                    Jump3();
                    isOnBoardingTime = false;
                    kuslar.transform.DOKill();
                    kuslar.SetActive(false);
                    kuslar.SetActive(true);
                    kuslar.transform.position = hips.transform.position + new Vector3(0, 1, -4);
                    StartCoroutine(KuslarCarpti());
                    canTap = false;
                    atisSirasi++;
                    onBoarding.SetActive(false);
                    Time.timeScale = 1;

                }
            }
        }

        #region Control By Physics
        //if (isForceTime)
        //      {
        //          //StartCoroutine(TimeSlow());
        //          CloseGravities();
        //          lastForce = 2000;
        //          isForceTime = false;
        //          Debug.Log("forse 1");
        //          foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
        //          float power = (float)(GameController.instance.power + GameController.instance.height) / 2;
        //          lastForce = lastForce * 8 + lastForce * power * GameController.instance.firlatmaForce; ;
        //          Debug.Log("force " + lastForce);
        //          tempLastForce = lastForce;
        //          Trajectory.instance.UpdateTrajectory(new Vector3(0, .3f, 1) * lastForce, hips.GetComponent<Rigidbody>(), hips.transform.position,true);
        //          hips.GetComponent<Rigidbody>().AddForce(new Vector3(0, .3f, 1) * lastForce);     
        //      }
        //      else if (isForceTime2)
        //      {
        //          isForceTime2 = false;
        //          foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
        //          float power = (float)(GameController.instance.power + GameController.instance.height) / 5;
        //          lastForce = tempLastForce / 1.5f;
        //          tempLastForce = lastForce;
        //          Trajectory.instance.UpdateTrajectory(new Vector3(0, .3f, 1) * lastForce, hips.GetComponent<Rigidbody>(), hips.transform.position,false);
        //          hips.GetComponent<Rigidbody>().AddForce(new Vector3(0, .3f, 1) * lastForce);

        //      }
        //      else if (isForceTime3)
        //      {
        //          isForceTime3 = false;
        //          foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
        //          float power = (float)(GameController.instance.power + GameController.instance.height) / 5;
        //          lastForce = tempLastForce / 1.5f;
        //          tempLastForce = lastForce;
        //          Trajectory.instance.UpdateTrajectory(new Vector3(0, .3f, 1) * lastForce, hips.GetComponent<Rigidbody>(), hips.transform.position,false);
        //          hips.GetComponent<Rigidbody>().AddForce(new Vector3(0, .3f, 1) * lastForce);

        //      }
        #endregion

        if (isOnBoardingTime && !zeminde)
        {
            if (atisSirasi < 2)
            {
                if (GameController.instance.power + GameController.instance.height < 4)
                {
                    onBoarding.SetActive(true);
                    Time.timeScale = .4f;
                }
                canTap = true;
            }
        }

        if (GameController.instance.isContinue)
        {
            DistanceTextUi();
        }
    }

    private void LateUpdate()
    {
        if (!AracControl.instance.isAracActive)
            cameraLookAtTarget.transform.position = new(0, hips.transform.position.y, hips.transform.position.z + 2);
    }

    public IEnumerator KuslarCarpti()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject tuy = Instantiate(tuyEfecti, hips.transform.position, Quaternion.identity);
            tuy.transform.parent = transform;
        }
        kuslar.transform.DOMove(new Vector3(
            10,
            6,
            hips.transform.position.z + 300 + (GameController.instance.power + GameController.instance.height) * 5), 10).OnComplete(() =>
        {
            kuslar.SetActive(false);
            kuslar.transform.position = kuslarPozisyon;
        });
        yield return new WaitForSeconds(.1f);

    }

    public IEnumerator TimeSlow()
    {
        yield return new WaitForSeconds(.2f);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {
        coinTime = true;
        atisSirasi = 0;
        canTap = false;
        UIController.instance.bestDistanceText.text = "";
        distanceTextTime = false;
        playerAnimator.enabled = true;
        transform.parent = arac.transform;
        GameController.instance.SetAracSpeedAndRotate();
        GameController.instance.SetVehicleType();
        engel.GetComponent<Collider>().enabled = true;
        AracControl.instance.current = 0;
        CloseColliders(); // titrememesi i?in bu silme bunu
        CloseRagDolsRb();
        onBoarding.SetActive(false);
        isDistanceTime = false;
        groundTextCanvas.transform.position = new Vector3(0, 0, -30);
        ClearParalarParenti();
    }

    public void CalculateDistance()
    {

        UIController.instance.ActivateWinScreen();
        groundTextCanvas.transform.position = new Vector3(1.2f, 10, hips.transform.position.z);
        groundTextCanvas.transform.DOMove(new Vector3(1.2f, 3.2f, hips.transform.position.z), 1f).SetEase(Ease.OutBounce);
        bestDistancePanel.SetActive(false);
        distancePanel.SetActive(true);
        int distance = (int)hips.transform.position.z - (int)engel.transform.position.z;
        distanceTextZemin.text = distance.ToString() + "m";
        //distanceUiText.text = distance.ToString() + "m";
        bestDistance = PlayerPrefs.GetInt("distance");
        if (distance > bestDistance)
        {
            bestDistance = distance;
            PlayerPrefs.SetInt("distance", distance);
            cizgi.transform.position = new Vector3(0, 2F, hips.transform.position.z);
            bestDistancneTextZemin.text = distance.ToString() + "m";
            bestDistancePanel.SetActive(true);
            distancePanel.SetActive(false);
            UIController.instance.bestDistanceText.text = "Best Distance : " + distance.ToString();
        }

    }

    public void DistanceTextUi()
    {
        int distance = (int)hips.transform.position.z - (int)engel.transform.position.z;

        if (distance > 0)
        {
            distanceUiText.text = distance.ToString() + " m";
        }
        else
        {
            distanceUiText.text = "0 m";
        }


        /*
        if (distance > bestDistance)
        {
            //Debug.Log(distance);
            bestDistance = distance;
            PlayerPrefs.SetInt("distance", distance);
            groundTextCanvas.transform.position = new Vector3(.5f, 5f, hips.transform.position.z);
        }
        */
    }

    public void DistanceTextGroundForStart()
    {
        bestDistance = PlayerPrefs.GetInt("distance");
        //distanceUiText.text = bestDistance.ToString() + "m";
        cizgi.transform.position = new Vector3(0, 2F, engel.transform.position.z + bestDistance);
    }


    public void Jump1()
    {
        OpenColliders();
        // OpenRagDolsRb();
        onBoarding.SetActive(false);
        playerAnimator.enabled = false;
        float power = (float)GameController.instance.power + (float)GameController.instance.height;
        float distance = Mathf.Lerp(0, 2500, power / 200);
        if (power > 75) distance = Mathf.Lerp(1000, 4000, power / 200);
        tempDistance = distance;
        Vector3 endPositon = new(0, 1.4f, transform.position.z + distance);
        float jumpPower = 10f + power / 2f;
        float time = 2f + power / 4;
        if (power < 10) time = 3f + power / 4;
        else if (power < 20) time = 4f + power / 4;
        transform.DOJump(endPositon, jumpPower, 1, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            ApplyLastForce();
        });
        StartCoroutine(ActivateIsOnBoarding(time));
        int rnd = Random.Range(0, 175);
        hips.transform.DORotate(Vector3.one * rnd, .05f);
    }

    public void Jump2()
    {
        onBoarding.SetActive(false);
        float power = (float)GameController.instance.power + (float)GameController.instance.height;
        power /= 1.2f;
        Vector3 endPositon = new(0, 1.4f, transform.position.z + tempDistance / 1.4f);
        float jumpPower = 10f + power;
        float time = 1.5f + power / 5;
        if (power < 10) time = 2f + power / 5;
        else if (power < 20) time = 3f + power / 5;
        transform.DOJump(endPositon, jumpPower, 1, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            ApplyLastForce();
        }); ;
        StartCoroutine(ActivateIsOnBoarding(time));
    }

    public void Jump3()
    {
        onBoarding.SetActive(false);
        float power = (float)GameController.instance.power + (float)GameController.instance.height;
        power /= 1.4f;
        Vector3 endPositon = new(0, 1.4f, transform.position.z + tempDistance / 2f);
        float jumpPower = 10f + power;
        float time = 1f + power / 6;
        if (power < 10) time = 1.5f + power / 6;
        else if (power < 20) time = 2f + power / 6;
        transform.DOJump(endPositon, jumpPower, 1, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            ApplyLastForce();
        }); ;
        StartCoroutine(ActivateIsOnBoarding(time));
    }

    void ApplyLastForce()
    {
        OpenRagDolsRb();
        onBoarding.SetActive(false);
        Time.timeScale = 1;
        isOnBoardingTime = false;
        canTap = false;
        float force = GameController.instance.power + GameController.instance.height;
        float time = force / 200;
        force = Mathf.Lerp(15000, 45000, time);
        hips.GetComponent<Rigidbody>().AddForce(Vector3.forward * force);
    }

    public IEnumerator ActivateIsOnBoarding(float time)
    {
        yield return new WaitForSeconds(time / 2);
        isOnBoardingTime = true;
    }

    public void CalculateCoins1()
    {

        // zaman? ve yeri lerp ile hesaplamay? d???n
        float power = (float)GameController.instance.power + (float)GameController.instance.height;
        float distance = Mathf.Lerp(0, 2500, power / 200);
        if (power > 75) distance = Mathf.Lerp(1000, 4000, power / 200);
        Vector3 endPosition = new(0, 1.4f, engel.transform.position.z + distance);
        float jumpPower = 10f + power / 2f;
        float time = 1f + power / 4;
        coinCalculator.transform.position = engel.transform.position + new Vector3(0, 3f, 0);
        coinCalculator.transform.DOJump(endPosition, jumpPower, 1, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            coinTime = false;
        });
        StartCoroutine(CreateCoins());

    }

    IEnumerator CreateCoins()
    {
        while (coinTime)
        {
            int rnd = Random.Range(0, 18);
            if (GameController.instance.power + GameController.instance.height < 10) rnd = Random.Range(0, 12);
            if (rnd == 1)
            {
                GameObject coin = Instantiate(coinPrefab, coinCalculator.transform.position, Quaternion.identity);
                coin.transform.tag = "para";
                coin.transform.parent = paralarParenti.transform;
            }

            yield return new WaitForEndOfFrame();
        }

    }

    public void ClearParalarParenti()
    {
        int childs = paralarParenti.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(paralarParenti.transform.GetChild(i).gameObject);
        }
    }


    public void OpenKinematics()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.isKinematic = true;
        }
    }

    public void CloseKinematics()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.isKinematic = false;
        }
    }

    public void OpenGravities()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.useGravity = true;
        }
    }

    public void CloseGravities()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.useGravity = false;
        }
        hips.GetComponent<Rigidbody>().useGravity = true;
    }



    public void OpenRagDolsRb()
    {
        onBoarding.SetActive(false);
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.useGravity = true;
        }
    }

    public void CloseRagDolsRb()
    {
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.useGravity = false;
        }
    }

    public void CloseColliders()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.gameObject.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
        }
    }

    public void OpenColliders()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.gameObject.GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
        }
    }

    public IEnumerator CloseConstraints()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;

        yield return new WaitForSeconds(.2f);
        OpenConstraints();

    }

    public void OpenConstraints()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
    }


}
