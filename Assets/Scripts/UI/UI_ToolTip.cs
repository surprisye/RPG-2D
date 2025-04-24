using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = Screen.width / 2;
    [SerializeField] private float yLimit = Screen.height / 2;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;
    
    public virtual void AdjustPosition()
    {
        Vector2 mousePos = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePos.x > xLimit)
            newXOffset = -xOffset;
        
        else
            newXOffset = xOffset;
        

        if (mousePos.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;
        
        transform.position = new Vector2(mousePos.x + newXOffset, mousePos.y + newYOffset);
    }

    public void AdjustFontSize(TextMeshProUGUI _textMeshPro)
    {
        if (_textMeshPro.text.Length > 12)
        {
            _textMeshPro.fontSize *= 0.8f;
        }
    }
}
