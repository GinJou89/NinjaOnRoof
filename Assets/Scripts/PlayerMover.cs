using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Possition { middle, right, left }
public enum PlayerCondition { moving, jump, attack, run, dead}
public class PlayerMover : MonoBehaviour
{
    public float Distance = 1.4f;
    public float AnimationTime = 0.5f;
    public GameObject Suriken;
    public bool is_paused = false;
    [SerializeField]private Possition _playerPossition = Possition.middle;
    [SerializeField]private PlayerCondition _playerCondition = PlayerCondition.run;
    [SerializeField]private float _currentDirrection;
    private CharacterController _characterController;
    private Animator _animator;
    private float _dirrection;
    private float _currentDistance;

    private Vector3 _firstPossition;   //First touch position
    private Vector3 _lastPossition;   //Last touch position
    private float _dragDistance;  //minimum distance for a swipe to be registered

    public static PlayerMover instanse;

    private void Awake()
    {
        if (PlayerMover.instanse != null)
        {
            Destroy(gameObject);
            return;
        }
        PlayerMover.instanse = this;
    }
    void Start()
    {
        _dragDistance = Screen.height * 5 / 100; //dragDistance is 15% height of the screen
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        _dirrection = SetDirection();

        if (_playerCondition == PlayerCondition.run && _dirrection != 0)
        {
            _playerCondition = PlayerCondition.moving; 
            _currentDirrection = _dirrection;
            _currentDistance = Distance;

            if (_dirrection == -1)
            {
                _animator.SetTrigger("Left");
            }
            if (_dirrection == 1)
            {
                _animator.SetTrigger("Right");
            }
            if (_dirrection == 2 )
            {
                _currentDistance = 1.5f;
                _animator.SetTrigger("Flip");
                _playerCondition = PlayerCondition.jump;
            }

        }
        if (_playerCondition == PlayerCondition.moving)
        {
            Move();
        }
        if (_playerCondition == PlayerCondition.jump)
        {
            Jump();
        }
        if (_playerCondition == PlayerCondition.attack)
        {
            Attack();
        }

    }
    private float SetDirection() 
    {
        float dir = Swipe();//Input.GetAxisRaw("Horizontal"); 

        if (_playerPossition == Possition.left && dir == -1)
        {
            dir = 0;
        }
        else if (_playerPossition == Possition.right && dir == 1)
        {
            dir = 0;
        }

        return dir;
    }
    private void Move()
    {
        if (_currentDistance <= 0)
        {
            
            if (transform.position.x > 1f)
            {
                _playerPossition = Possition.right;
                _dirrection = _dirrection == 1 ? 0 : _dirrection;
            }
            else if (transform.position.x < -1f)
            {
                _playerPossition = Possition.left;
                _dirrection = _dirrection == -1 ? 0 : _dirrection;
            }
            else
            {
                _playerPossition = Possition.middle;
                transform.position = new Vector3(0, 0, transform.position.z);
            }
            _playerCondition = PlayerCondition.run;
            return;
        }
        float speed = Distance / AnimationTime;
        float tmpDist = Time.deltaTime * speed;
        _characterController.Move(Vector3.right * _currentDirrection * tmpDist);
        _currentDistance -= tmpDist;
    }
    public void Jump() 
    {
        float tmpDist = Time.deltaTime * (1.5f / 0.45f);
        if (_currentDistance <= 0)
        {
            _characterController.Move(Vector3.down * tmpDist);
            if (transform.position.y <= 0.1f)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                _playerCondition = PlayerCondition.run;
            }
        }
        else 
        {
            _characterController.Move(Vector3.up * tmpDist);
            _currentDistance -= tmpDist;
        }
    }
    public void Attack()
    {
        if (UIcontroller.instanse.GetPoint() > 0 )
        {
            if (_playerCondition != PlayerCondition.attack && _playerCondition == PlayerCondition.run)
            {
                _playerCondition = PlayerCondition.attack;
                _animator.SetTrigger("Attack");
            }
        }
        
    }
    public void Throw() 
    {
        Instantiate(Suriken, new Vector3(transform.position.x + 0.270f, 1.47f, transform.position.z + 2.479f), Suriken.transform.rotation);
        UIcontroller.instanse.SubtractBonus(1);
        _playerCondition = PlayerCondition.run;
    }
    private int Swipe()
    {
        if (Input.touchCount == 1 && !is_paused) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                _firstPossition = touch.position;
                _lastPossition = touch.position;
            }
            //else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            //{
            //    _lastPossition = touch.position;
            //}
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                _lastPossition = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(_lastPossition.x - _firstPossition.x) > _dragDistance || Mathf.Abs(_lastPossition.y - _firstPossition.y) > _dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(_lastPossition.x - _firstPossition.x) > Mathf.Abs(_lastPossition.y - _firstPossition.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((_lastPossition.x > _firstPossition.x))  //If the movement was to the right)
                        {   //Right swipe
                            return 1;
                        }
                        else
                        {   //Left swipe
                            return -1;
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (_lastPossition.y > _firstPossition.y)  //If the movement was up
                        {   //Up swipe
                            return 2;
                        }
                        else
                        {   //Down swipe
                            return 0;
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
        return 0;
    }
    private void Death() 
    {
        UIcontroller.instanse.ShowDeathMenu();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Barrier"))
        {
            WorldController.instanse.Speed = 0;
            _animator.SetTrigger("Death");
            _playerCondition = PlayerCondition.dead;
        }
    }

    
}
