using UnityEngine;
using TMPro;

public class ElectricLock : MonoBehaviour
{
    public TextMeshPro passwordDisplay;
    public GameObject lockPanel;
    public Animator doorAnim;       // Сюда перетащи дверь в инспекторе
    public Camera lockCamera;       // Сюда перетащи камеру для замка
    public int correctPassword = 1234;

    private string currentInput = "";
    private bool isUnlocked = false;
    private Controller playerController;

    public void ChangeViewToCodeLock(Controller controller)
    {
        playerController = controller;
        playerController.canMove = false;

        // Переключаем камеры
        if (lockCamera != null) lockCamera.enabled = true;
        if (playerController.playerCam != null) playerController.playerCam.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        lockPanel.SetActive(true);
        currentInput = "";
        UpdateDisplay();
    }

    public void AddDigit(int digit)
    {
        if (isUnlocked || currentInput.Length >= 4) return;
        currentInput += digit.ToString();
        UpdateDisplay();
        if (currentInput.Length == 4) CheckCode();
    }

    private void UpdateDisplay()
    {
        if (passwordDisplay != null) passwordDisplay.text = currentInput.PadRight(4, '_');
    }

    private void CheckCode()
    {
        if (int.TryParse(currentInput, out int entered) && entered == correctPassword)
        {
            isUnlocked = true;
            if (passwordDisplay != null) passwordDisplay.color = Color.green;

            // Открываем дверь
            if (doorAnim != null) doorAnim.SetBool("isOpen", true);

            Invoke("BackToPlayer", 1f);
        }
        else
        {
            currentInput = "";
            UpdateDisplay();
        }
    }

    public void BackToPlayer()
    {
        // Возвращаем камеры
        if (lockCamera != null) lockCamera.enabled = false;
        if (playerController != null)
        {
            if (playerController.playerCam != null) playerController.playerCam.enabled = true;
            playerController.GiveBackControl();
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        lockPanel.SetActive(false);
        playerController = null;
    }
}