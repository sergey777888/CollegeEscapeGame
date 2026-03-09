using UnityEngine;

public class DoorLol : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Camera cam;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        bool isLookingAtDoor = Physics.Raycast(ray, distance, layer);

        if (isLookingAtDoor)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetBool("isOpen", true);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                anim.SetBool("isOpen", false);
            }
        }
    }
}