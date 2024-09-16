using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameStartCountDownUI : MonoBehaviour
{
	private const string NUMBER_POPUP = "NumberPopup";

   [SerializeField] private TextMeshProUGUI _countdownText;
   
   private Animator _animator;
	private int _previousCountdownNumber;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
   {
	   KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
	   Hide();
   }
   private void Update()
   {
		int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());

     _countdownText.text = countdownNumber.ToString();  

		if(_previousCountdownNumber != countdownNumber)
		{
			_previousCountdownNumber = countdownNumber;
			_animator.SetTrigger(NUMBER_POPUP);
			SoundManager.Instance.PlayCountdownSound();
		}
   }
   
   private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
	  if(KitchenGameManager.Instance.IsCountdownToStartActive())
	  {
		  Show();
	  }
	  else
	  {
		  Hide();
	  }
    }
	
	private void Show()
	{
		gameObject.SetActive(true);
	}
	private void Hide()
	{
		gameObject.SetActive(false);
	}
}
