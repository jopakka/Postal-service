using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _speedTextField;
    [SerializeField] private TMP_Text _interactionTextField;
    [SerializeField] private Image _progress;

    public void SetSpeed(float speed)
    {
        _speedTextField.text = $"Speed: {speed}";
    }

    public void SetInteraction(string key, string text)
    {
        _interactionTextField.text = $"{key}: {text}";
        _interactionTextField.gameObject.SetActive(true);
    }

    public void ClearInteraction()
    {
        _interactionTextField.gameObject.SetActive(false);
    }

    public void SetProgress(float progress)
    {
        _progress.fillAmount = progress;
        _progress.gameObject.SetActive(true);
    }

    public void ClearProgress()
    {
        _progress.gameObject.SetActive(false);
    }
}
