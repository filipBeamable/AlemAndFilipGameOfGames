using Beamable;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI _textbox;

    private IBeamableAPI _api;
    async void Start()
    {
        _api = await API.Instance;
    }

    public void CallMicroService()
    {

        _textbox.text = "BLA";
    }
}
