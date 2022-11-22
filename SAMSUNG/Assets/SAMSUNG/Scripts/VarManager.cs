using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void MarketTriggers();
public delegate void MarketTrigger(string caseType);
public struct Images{
    public Image Coke;
    public Image IceCream;
    public Image Pocky;
}
public class VarManager : MonoBehaviour
{
    private static VarManager _singleton;
    public static VarManager Singleton
    {
            get => _singleton;
            private set
        {
            if (_singleton == null)
            _singleton = value;
            else if (_singleton != value)
            {
            Debug.Log($"{nameof(VarManager)} instance already exists, destroying object!");
            Destroy(value);
            }
        }
    }
    #region Canvas
    public Canvas StartCanvas = null;
    public Canvas GUICanvas = null;
    public Canvas SumCanvas = null;
    [SerializeField]
    Canvas TestCanvas = null;
    #endregion
    public static event MarketTrigger animCall;
    public static event MarketTriggers itemUpdate;
    [SerializeField] string[] ItemList;
    [SerializeField] double[] PriceList;
    private static Dictionary<string, int> itemList = new();
    private static Dictionary<string, double> priceList = new();
    public Animator possessedDemon = null;
    public GameObject Robot = null;
    private void Awake() {
        Singleton = this;
    }
    private void Start() {
        for(int i = 0; i < ItemList.Length; i++){
            itemList.Add(ItemList[i], 0);
            priceList.Add(ItemList[i], PriceList[i]);
            Debug.Log(ItemList[i]);
        }
    }
    #region Commands
    public static void AddItem(string type){
        itemList[type]++;
        Debug.Log("Added 1 " + type + " , new total : " + itemList[type]);
        itemUpdate.Invoke();
    }
    public static void AddItem(string type, int Amount){
        for(int i = 0; i < Amount; i++){
            itemList[type]++;
        }
        Debug.Log("Added " + Amount + " " + type + "s, new total: " + itemList[type]);
        itemUpdate.Invoke();
    }

    public static void RemoveItem(string type){
        if(itemList[type] > 0){
            itemList[type]--;
            Debug.Log("Removed 1 " + type + " , new total : " + itemList[type]);
        }
        itemUpdate.Invoke();
    }
    public static void Checkout(){
        double TotalAmount = 0;
        Canvas TestCanvas = VarManager.Singleton.TestCanvas;
        if(TestCanvas != null)
        TestCanvas.enabled = false;
        foreach(string type in itemList.Keys){
            TotalAmount += itemList[type] * priceList[type];
        }
        Debug.Log("The total price of your items is $" + TotalAmount);
    }
    #endregion
    public void AnimCall(string Type){
        animCall.Invoke(Type);
    }
    public int totalItems(){
        int tempInt = 0;
        foreach(string type in itemList.Keys){
            tempInt += itemList[type];
        }
        return tempInt;
    }
}
