using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerTest : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _UIPanel;
    private bool _isOpen = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        _isOpen = !_isOpen;
        _UIPanel.SetActive(_isOpen);
        Debug.Log("PointerTest.OnPointerClick");
    }
    

    // Update is called once per frame
    private void Update()
    {
        
    }
}


