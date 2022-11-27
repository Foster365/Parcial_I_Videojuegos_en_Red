using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using System.Runtime.CompilerServices;

public class HealthBar : MonoBehaviourPun
{
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private float updateSpeed = 0.5f;
    float uiFillAmount;

    private void Awake()
    {
        healthImage.enabled = true;
        gameObject.SetActive(true);
        GetComponentInParent<HealthManager>().OnHealthPctChanged += HandleHealthChanged;
    }

    [PunRPC]
    void SetUIHealthBar()
    {
    }

    private void Update()
    {
        //if(uiFillAmount <= 0)
        //{
        //    PhotonNetwork.Destroy(this.gameObject);
        //}
    }

    private void HandleHealthChanged(float pct)
    {
        if (healthImage.enabled)
        {
            Debug.Log("Health image is enabled: " + healthImage.enabled);
            StartCoroutine(ChangeToPct(pct));
        }
    }

    private IEnumerator ChangeToPct(float pct)
    {
        if(healthImage.IsActive())
        {
            Debug.Log("ESTÁ ACTIVO!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Debug.Log("Active? " + gameObject.activeInHierarchy);
            float preChangePct = healthImage.fillAmount;
            float elapsed = 0f;

            while (elapsed < updateSpeed)
            {
                elapsed += Time.deltaTime;
                uiFillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeed);
                healthImage.fillAmount = uiFillAmount;
                StartCoroutine(WaitToCheckUIFill(uiFillAmount));
                yield return null;
            }

            healthImage.fillAmount = pct;
        }
    }



    IEnumerator WaitToCheckUIFill(float _fillAmount)
    {
        yield return new WaitForSeconds(2f);

        photonView.RPC("CheckUIFill", RpcTarget.All, _fillAmount);
    }

    [PunRPC]
    void CheckUIFill(float _fillAmount)
    {
        healthImage.fillAmount = _fillAmount;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
