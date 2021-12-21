using Beamable;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    public Canvas canvas;
    public TextMeshProUGUI _textbox;

    [Space]
    public GameObject healthUIPrefab;
    public Transform healthUIParent;

    private IBeamableAPI _api;

    private void Awake()
    {
        Instance = this;
    }

    async void Start()
    {
        _api = await API.Instance;
    }

    public void CallMicroService()
    {

        _textbox.text = "BLA";
    }

    public HealthUI InstantiateHealthUI()
    {
        return Instantiate(healthUIPrefab, healthUIParent).GetComponent<HealthUI>();
    }
}
