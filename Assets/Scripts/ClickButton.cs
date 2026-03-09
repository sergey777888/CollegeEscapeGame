using UnityEngine;

public class Button : MonoBehaviour
{
    public ElectricLock lockMain; // Сюда перетащи объект замка из сцены
    public int number;            // Укажи цифру для этой кнопки (0-9)

    // Unity вызывает это при клике мышкой
    private void OnMouseDown()
    {
        if (lockMain != null)
        {
            lockMain.AddDigit(number);
        }
    }
}