using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanvas : VarManager
{
    [SerializeField] string itemType;
    public void AddItemPressed(){
        AddItem(itemType);
    }
    public void SubItemPresed(){
        RemoveItem(itemType);
    }
    public void CheckoutPresed(){
        Checkout();
    }
}