using System.Collections.Generic;
using UnityEngine;

public class Enemy : Movement
{
    [SerializeField]
    LayerMask blockingLayer;

    Rigidbody2D rigidBody;

    float horizontal, vertical;

    WeaponController wc;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        RandomDirection();
        wc = GetComponentInChildren<WeaponController>();
        Invoke(Constants.Event.OCCASIONAL_FIRE, Random.Range(0.1f, 3f));
    }

    public void RandomDirection()
    {
        CancelInvoke("RandomDirection");

        var lineCastChecking = new RandomDirectionGenerator(transform, blockingLayer);
        (horizontal, vertical) = lineCastChecking.GetRandomDirection();

        Invoke("RandomDirection", Random.Range(3, 6));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null && collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage();
        }
        RandomDirection();
    }

    private void FixedUpdate()
    {
        if (vertical != 0 && isMoving == false) StartCoroutine(MoveVertical(vertical, rigidBody));
        else if (horizontal != 0 && isMoving == false) StartCoroutine(MoveHorizontal(horizontal, rigidBody));
    }

    private void OccasionalFire()
    {
        wc.Fire();
        Invoke(Constants.Event.OCCASIONAL_FIRE, Random.Range(0.1f, 3f));
    }
}

public class RandomDirectionGenerator
{
    Transform _transform;
    LayerMask _blockingLayer;

    public enum Direction { Up, Down, Left, Right };

    public RandomDirectionGenerator(Transform transform, LayerMask blockingLayer)
    {
        _transform = transform;
        _blockingLayer = blockingLayer;
    }
    public bool CheckRight() => !Physics2D.Linecast(_transform.position, (Vector2)_transform.position + new Vector2(1, 0), _blockingLayer);
    public bool CheckLeft() => !Physics2D.Linecast(_transform.position, (Vector2)_transform.position + new Vector2(-1, 0), _blockingLayer);
    public bool CheckUp() => !Physics2D.Linecast(_transform.position, (Vector2)_transform.position + new Vector2(0, 1), _blockingLayer);
    public bool CheckDown() => !Physics2D.Linecast(_transform.position, (Vector2)_transform.position + new Vector2(0, -1), _blockingLayer);

    public List<Direction> GenerateRandomDirectionList()
    {
        var randomDirection = new List<Direction>();

        if (this.CheckRight())
        {
            randomDirection.Add(Direction.Right);
        }
        if (this.CheckLeft())
        {
            randomDirection.Add(Direction.Left);
        }
        if (this.CheckUp())
        {
            randomDirection.Add(Direction.Up);
        }
        if (this.CheckDown())
        {
            randomDirection.Add(Direction.Down);
        }
        return randomDirection;
    }

    public (float horizontal, float vertical) GetRandomDirection()
    {
        var randomDirections = GenerateRandomDirectionList();
        var selection = randomDirections[Random.Range(0, randomDirections.Count)];
        switch (selection)
        {
            case Direction.Up:
                return (0, 1);

            case Direction.Down:
                return (0, -1);

            case Direction.Left:
                return (-1, 0);

            case Direction.Right:
                return (1, 0);

            default:
                return (1, 0);
        }
    }
}