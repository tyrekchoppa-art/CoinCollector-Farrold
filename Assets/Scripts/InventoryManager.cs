using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;                                              // ← DITAMBAHKAN

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private int slotCount = 16;
    private bool InventoryActive;
    public ItemSlot[] itemSlot;

    [Header("Item Detail Panel")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemQuantityText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Image itemDetailImage;

    void Start()
    {
        CreateSlots();
        InventoryCanvas.SetActive(false);
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        InventoryActive = false;
    }

    void Update()
    {
        if (inventoryAction.triggered && InventoryCanvas != null && InventoryActive == false)
        {
            InventoryActive = true;
            Time.timeScale = 0f;
            Debug.Log("Inventory is active");
            InventoryCanvas.SetActive(true);
        }
        else if (inventoryAction.triggered && InventoryCanvas != null && InventoryActive == true)
        {
            InventoryActive = false;
            Time.timeScale = 1f;
            Debug.Log("Inventory is deactivated");
            InventoryCanvas.SetActive(false);
        }
    }

    private void CreateSlots()
    {
        ItemSlot slotPrefab = Resources.Load<ItemSlot>("ItemSlot");
        if (slotPrefab == null)
        {
            Debug.LogError("ItemSlot prefab was not found in a Resources folder.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        Transform parent = InventoryCanvas.transform;
        GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
        if (grid != null)
        {
            parent = grid.transform;
        }

        itemSlot = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(slotPrefab, parent);
        }
    }

    public void AddItem(ItemDetails item)
    {
        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

        if (itemSlot == null || itemSlot.Length == 0)
        {
            Debug.LogWarning("Inventory has no slots, '" + item.ItemName + "' was lost.");
            return;
        }

        // Logic to check if item already exists in the inventory and can be stacked
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].CanStackWith(item.ItemName))
            {
                itemSlot[i].AddQuantity(item.Quantity);
                return;
            }
        }

        // Logic to add the item to an empty slot if it cannot be stacked
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(item);
                return;
            }
        }

        Debug.LogWarning("Inventory is full, '" + item.ItemName + "' was lost.");
    }

    public void DesselectAllSlot()
    {
        foreach (ItemSlot slot in itemSlot)
        {
            slot.isItemSelected = false;
            slot.selectedPanel.SetActive(false);
        }   
    }

   public void ShowItemDetail(string name, int quantity, Sprite icon, string description)
    {
        if (itemNameText != null) itemNameText.text = name;
        if (itemQuantityText != null) itemQuantityText.text = quantity.ToString();
        if (itemDescriptionText != null) itemDescriptionText.text = description;
        if (itemDetailImage != null) itemDetailImage.sprite = icon;
    }   
}