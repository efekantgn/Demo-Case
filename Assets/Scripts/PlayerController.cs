using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float GRAVITY_CONSTANT = 9.81f;
    public float moveSpeed;
    public float rotateSpeed;
    public float gravity;
    public GameObject Crosshair;
    [Space(10)]
    private Vector2 m_Rotation;
    private Vector2 m_Look;
    private Vector2 m_Move;
    private bool isInventoryActive = false;
    [SerializeField] private Inventory inventory;
    private CharacterController cController = null;
    private PlayerInput playerInput = null;
    [SerializeField] PlayerData playerData;
    private TextMeshProUGUI crosshairText;
    [SerializeField] GameObject Menu;
    Ray ray;


    private void Awake()
    {
        cController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        crosshairText = Crosshair.GetComponentInChildren<TextMeshProUGUI>();
        OpenCloseCursor(false, CursorLockMode.Locked);
    }

    public void OpenCloseCursor(bool isVisible, CursorLockMode cursorLockMode)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = cursorLockMode;
        Crosshair.SetActive(!isVisible);
    }

    public void OnMBLeft(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 2)) return;

        if (hit.transform.TryGetComponent(out NPC npc))
        {
            //OpenNPC Market
            npc.OpenNPCMarket();
        }
        else
        {
            inventory.UseSelectedHotbar();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        m_Look = context.ReadValue<Vector2>();
    }
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isInventoryActive = !isInventoryActive;
        inventory.gameObject.SetActive(isInventoryActive);
        EnableDisableInputActions(isInventoryActive, "Inventory");

    }
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Menu.SetActive(!Menu.activeSelf);
        EnableDisableInputActions(Menu.activeSelf, "Menu");
    }
    public void OnMenuButton()
    {
        Menu.SetActive(!Menu.activeSelf);
        EnableDisableInputActions(Menu.activeSelf, "Menu");
    }


    public void EnableDisableInputActions(bool isUIOpened, string EnabledUI)
    {
        if (isUIOpened)
        {
            playerInput.actions.Disable();
            playerInput.actions.FindAction(EnabledUI).Enable();
            OpenCloseCursor(true, CursorLockMode.None);
        }
        else
        {
            playerInput.actions.Enable();
            OpenCloseCursor(false, CursorLockMode.Locked);
        }
    }
    public void EnableDisableInputActions(bool isUIOpened)
    {
        if (isUIOpened)
        {
            playerInput.actions.Disable();
            OpenCloseCursor(true, CursorLockMode.None);
        }
        else
        {
            playerInput.actions.Enable();
            OpenCloseCursor(false, CursorLockMode.Locked);
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 2)) return;

        if (hit.transform.TryGetComponent(out IPickupable WorldItem))
        {
            if (inventory.SpawnInventoryItem(WorldItem.InventoryItemData))
                Destroy(hit.transform.gameObject);
        }


    }
    public void OnSelectionChange(InputAction.CallbackContext context)
    {
        float mouseScrollValue = context.ReadValue<float>();
        inventory.ChangeSelectedHotbar(mouseScrollValue);
    }
    public void Update()
    {
        Look(m_Look);

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2))
        {
            if (hit.transform.TryGetComponent(out IPickupable pickupable))
                crosshairText.text = "Interact with " + pickupable.t.name + ". " + "Press " + playerInput.actions.FindAction("Grab") + " button.";
            else if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                crosshairText.text = "Interact with " + interactable.t.name + ". " + "Press " + playerInput.actions.FindAction("Interact") + " button.";
            }
            else
                crosshairText.text = string.Empty;
        }
        else
            crosshairText.text = string.Empty;
    }
    private void FixedUpdate()
    {
        Move(m_Move);
        Gravity();
    }
    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        cController.Move(move * scaledMoveSpeed);
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += rotate.x * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation;
    }

    private void Gravity()
    {
        gravity -= GRAVITY_CONSTANT * Time.deltaTime;
        cController.Move(new Vector3(cController.center.x, gravity, cController.center.z));
        if (cController.isGrounded) gravity = 0;
    }
}
