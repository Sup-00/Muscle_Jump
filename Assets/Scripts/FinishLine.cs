using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private Image _victoryPlae;
    [SerializeField] private Animator _charectorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharectorMoving>())
        {
            other.GetComponent<CharectorMoving>().enabled = false;
            _victoryPlae.gameObject.SetActive(true);
            _charectorAnimator.SetTrigger("Victory");
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}