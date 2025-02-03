using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask _slingShotAreaMask;

    public bool IsWithinSlingShot()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Physics2D.OverlapPoint(worldPos, _slingShotAreaMask))
        {

            return true;

        }
        else
        {
            return false;
        }



    }




}
