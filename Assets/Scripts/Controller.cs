using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    public float walkSpeed = 7.5f;
    public float runSpeed = 11.5f;
    public float gravity = 20f;
    public Camera playerCam;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 45f;
    public bool canMove = true;
    public float interactDist = 3f;
    public LayerMask interactMask;

    private CharacterController cc;
    private float rotX = 0f;
    private ElectricLock currentLock;

    // Настройки для плавной ходьбы
    private Vector3 startPos;
    private float timer = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startPos = playerCam.transform.localPosition;
    }

    void Update()
    {
        if (canMove)
        {
            // 1. Движение
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 move = (transform.right * moveX + transform.forward * moveZ);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            cc.Move(move * currentSpeed * Time.deltaTime);
            cc.Move(new Vector3(0, -gravity, 0) * Time.deltaTime);

            // 2. Взгляд
            rotX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotX = Mathf.Clamp(rotX, -verticalLookLimit, verticalLookLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
            transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);

            // 3. Плавная ходьба (покачивание)
            if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
            {
                timer += Time.deltaTime * 10f;
                float bob = Mathf.Sin(timer) * 0.03f;
                playerCam.transform.localPosition = new Vector3(startPos.x, startPos.y + bob, startPos.z);
            }
            else
            {
                timer = 0;
                playerCam.transform.localPosition = Vector3.Lerp(playerCam.transform.localPosition, startPos, Time.deltaTime * 5f);
            }
        }

        // 4. Взаимодействие
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDist, interactMask))
            {
                ElectricLock el = hit.collider.GetComponent<ElectricLock>();
                if (el != null)
                {
                    currentLock = el;
                    el.ChangeViewToCodeLock(this);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && currentLock != null)
        {
            currentLock.BackToPlayer();
            currentLock = null;
        }
    }

    public void GiveBackControl()
    {
        canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentLock = null;
    }
}