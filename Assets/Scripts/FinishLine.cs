using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private Image _victoryPlane;
    [SerializeField] private Animator _charectorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharectorMoving>())
        {
            CharectorMoving _charector = other.GetComponent<CharectorMoving>();
            _charector.enabled = false;
            _charector.transform.DORotate(new Vector3(-25, 180f, 0), 0.5f);
            _victoryPlane.gameObject.SetActive(true);
            _charectorAnimator.SetTrigger("Victory");
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}