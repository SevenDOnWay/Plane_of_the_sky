using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BlockUi : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        eventData.Use();
    }
}
