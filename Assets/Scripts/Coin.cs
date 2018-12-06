using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        myAnimator.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.GetCoin();
            myAnimator.SetTrigger("Collected");
        }
    }
}
