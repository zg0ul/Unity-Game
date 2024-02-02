using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    // an interface is like a contract, we define the functions we want then we can have different implementations in different places that extends the interface
    public Transform GetKitchenObjectFollowTransform();


    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();


    public void ClearKitchenObject();


    public bool HasKitchenObject();
    
}
