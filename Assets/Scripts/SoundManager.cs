using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_EFFECTS_VOLUME = "SoundEffectsVolume";
	public static SoundManager Instance {get; private set;}
	
	[SerializeField] private AudioClipRefsSO _audioClipRefsSO; 

    private float _volume = 1f;
	
	private void Awake()
	{
		Instance = this;

       _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_EFFECTS_VOLUME, 1f);
	}
	
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
		CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
		Player.Instance.OnPickedSomething += Player_OnPickedSomething;
		BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
		TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }
	
	private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
		TrashCounter trashCounter = sender as TrashCounter;
      PlaySound(_audioClipRefsSO.ObjectDrop, trashCounter.transform.position);
    }

	 private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
		BaseCounter baseCounter = sender as BaseCounter;
      PlaySound(_audioClipRefsSO.ObjectDrop, baseCounter.transform.position);
    }
	
	 private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
      PlaySound(_audioClipRefsSO.ObjectPickup, Player.Instance.transform.position);
    }
	
	 private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
	  CuttingCounter cuttingCounter = sender as CuttingCounter;
      PlaySound(_audioClipRefsSO.Chop, cuttingCounter.transform.position);
    }
	
    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
		DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
      PlaySound(_audioClipRefsSO.DeliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
		DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(_audioClipRefsSO.DeliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * _volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
	
	public void PlayFootStepsSound(Vector3 position, float volume)
	{
		PlaySound(_audioClipRefsSO.Footstep, position, volume);
	}

    public void PlayCountdownSound()
    {
        PlaySound(_audioClipRefsSO.Warning, Vector3.zero);
    }

    public void ChangeVolume()
    {
        _volume += 0.1f;
        if(_volume > 1f)
        {
            _volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_EFFECTS_VOLUME, _volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return _volume;
    }
}
