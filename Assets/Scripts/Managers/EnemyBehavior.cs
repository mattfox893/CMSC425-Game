using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    bool isMoving;
    
    [SerializeField] float moveSpeed = 4f;
    public static EnemyBehavior Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Act(Unit unit)
    {
        if (!AttackEnemyWithinRange(unit))
        {
            while (unit.GetMovement() > 0)
            {
                StartCoroutine(Move(unit, DetermineClosest(unit)));
            }
        }

        unit.state = UnitState.Acted;
    }

    // find the closest unit, return a string of their relative direction
    string DetermineClosest(Unit unit)
    {
        foreach (Unit target in UnitManager.Instance.units)
        {

        }

        return null;
    }

    // try to attack an enemy within range of unit, return true if successful and false otherwise
    bool AttackEnemyWithinRange(Unit unit)
    {
        foreach (Unit target in UnitManager.Instance.units)
        {
            // if there is an "Ally" within range to attack,
            if (target.team == Team.Ally && unit.inRange(target))
            {
                // attack the target, dont forget to eventually check if the unit's weapon is magical
                unit.Attack(target, false, unit.NumAttacks(target));
                return true;
            }
        }
        return false;
    }

    // if moving in dir would clash with an immovable tile,
    bool CheckMoveValid(Unit unit, string dir)
    {
        Vector3 direction = ReturnStringAsDirection(dir);
        Vector3 target = unit.transform.position + direction;
        Tile targetTile = GridManager.GetTile(new Vector2(target.x, target.z), null);

        return (targetTile.GetAttribute() != Attribute.Impassable);
    }

    // convert a given string into a vector3 direction
    Vector3 ReturnStringAsDirection(string dir)
    {
        Vector3 direction;

        switch (dir)
        {
            case "left":
                direction = new Vector3(-1, 0, 0);
                break;
            case "down":
                direction = new Vector3(0, 0, -1);
                break;
            case "right":
                direction = new Vector3(1, 0, 0);
                break;
            case "up":
                direction = new Vector3(0, 0, 1);
                break;
            default:
                Debug.Log($"ERROR! Incorrect Movement Call: {dir}");
                direction = new Vector3(0, 0, 0);
                break;
        }

        return direction;
    }

    IEnumerator Move(Unit unit, string dir)
    {            
        isMoving = true;
        Vector3 direction = ReturnStringAsDirection(dir);
        int moveCost = 0;
        Vector3 target = unit.transform.position + direction;
        Vector3 start = unit.transform.position;
        Tile startTile = GridManager.GetTile(new Vector2(start.x, start.z), null);
        Tile targetTile = GridManager.GetTile(new Vector2(target.x, target.z), null);

        if (targetTile.GetAttribute() != Attribute.Impassable)
        {
            if (startTile.GetAttribute() == Attribute.Slow)
            {
                moveCost = 2;
            }
            else
            {
                moveCost = 1;
            }

            unit.MoveUnit(moveCost);

            while (Vector3.Distance(start, unit.transform.position) <= 1f)
            {
                unit.transform.position += (target - start) * moveSpeed * Time.deltaTime;
                yield return null;
            }
        }

        unit.transform.position = new Vector3(Mathf.Round(unit.transform.position.x), unit.transform.position.y, Mathf.Round(unit.transform.position.z));
        isMoving = false;
    }
}