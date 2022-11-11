using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanvas : MonoBehaviour
{
    [SerializeField] string itemType;
    public void AddItemPressed(){
        VarManager.AddItem(itemType);
    }
    public void SubItemPresed(){
        VarManager.RemoveItem(itemType);
    }
    public void CheckoutPresed(){
        VarManager.Checkout();
    }
}