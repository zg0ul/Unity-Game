using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {

        // There is no KitchenObject here
        if (player.HasKitchenObject())
        {
            // Player is carrying something
            player.GetKitchenObject().DestroySelf();
        }
        else
        {
            // Player not carrying anything.. do nothing
        }

    }
}
