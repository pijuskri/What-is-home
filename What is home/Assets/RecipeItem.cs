using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public RecipeDef recipe;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(player.EnoughItems(recipe.input, recipe.inputAmount));
            if (player.EnoughItems(recipe.input, recipe.inputAmount))
            {
                player.AddItemToInventory(new Item(player.FindItemByName(recipe.output), recipe.outputAmount));
                player.AddItemToInventory(new Item(player.FindItemByName(recipe.input), -recipe.inputAmount));
            }
        }
    }
}
