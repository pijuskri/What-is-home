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
            if (player.EnoughItems(recipe.input))
            {
                foreach (var item in recipe.input)
                {
                    player.AddItemToInventory(new Item(player.FindItemByName(item.name), -item.amount));
                }
                foreach (var item in recipe.output)
                {
                    player.AddItemToInventory(new Item(player.FindItemByName(item.name), item.amount));
                }
            }
        }
    }
}
