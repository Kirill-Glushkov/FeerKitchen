using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _interactAlternateButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _gamepadInteractButton;
    [SerializeField] private Button _gamepadInteractAlternateButton;
    [SerializeField] private Button _gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI _soundEffectsText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private TextMeshProUGUI _moveDownText;
    [SerializeField] private TextMeshProUGUI _moveRightText;
    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private TextMeshProUGUI _interactAlternateText;
    [SerializeField] private TextMeshProUGUI _pauseText;
    [SerializeField] private TextMeshProUGUI _gamepadInteractText;
    [SerializeField] private TextMeshProUGUI _gamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI _gamepadPauseText;

    [SerializeField] private Transform _pressToRebindKeyTransform;

    private Action _onCloseButtonAction;

    private void Awake()
    {
        Instance = this;

        _soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        _closeButton.onClick.AddListener(() =>
        {
            /*KitchenGameManager.Instance.OnGameUnPaused += KitchenGameManager_OnGameUnPaused;

            MusicManager.Instance.ChangeVolume();*/
            Hide();
            _onCloseButtonAction();
        });

        _moveUpButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.Move_Up);});
        _moveDownButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.Move_Down);});
        _moveLeftButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.Move_Left);});
        _moveRightButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.Move_Right);});
        _interactButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.Interact);});
        _interactAlternateButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.InteractAlternate);});
        _pauseButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.Pause);});
        _gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
        _gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_InteractAlternate); });
        _gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });
    }

    private void KitchenGameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Start()
    {
        Hide();
        HidePressToRebindKey();
        UpdateVisual();
    }

    private void UpdateVisual()
    {

        _moveUpText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Move_Up);
        _moveDownText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Move_Down);
        _moveLeftText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Move_Left);
        _moveRightText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Move_Right);
        _interactText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Interact);
        _interactAlternateText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.InteractAlternate);
        _pauseText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Pause);
        _gamepadInteractText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Gamepad_Interact);
        _gamepadInteractAlternateText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Gamepad_InteractAlternate);
        _gamepadPauseText.text = GameInput.Instance.GetBindingTxt(GameInput.Binding.Gamepad_Pause);

        _soundEffectsText.text = "SoundEffects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        _musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }

    public void Show(Action onCloseButtonAction)
    {
        _soundEffectsButton.Select();
        this._onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        _pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        _pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
        
    }
}
