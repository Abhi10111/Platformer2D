using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] bool isOnGround=false;
    [SerializeField] private bool isJumping;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance=0.1f;
    [SerializeField] float groundSpeed=8.0f;
    [SerializeField] float jumpSpeed=10.0f;
    private PlayerInput input;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    private bool canJump;
    private int direction = 1;
    
    void Start()
    {
        boxCollider=GetComponent<BoxCollider2D>();
        rigidBody=GetComponent<Rigidbody2D>();
        input=GetComponent<PlayerInput>();
    }

    void FixedUpdate(){
        CheckEnvironment();
        GroundMovement();
        MidAirMovement();
    }
    void Update()
    {
    }
    private void GroundMovement(){
        float xVelocity = groundSpeed * input.horizontal;
        rigidBody.linearVelocity=new Vector2(xVelocity,rigidBody.linearVelocity.y);

        if(xVelocity*direction<0f){
            FlipCharacterDirection();
        }
    }
    private void MidAirMovement(){
        if(input.jumpPressed && canJump){
            rigidBody.linearVelocity=new Vector2(rigidBody.linearVelocity.x,jumpSpeed);
            canJump=!isJumping;
            isJumping=true;
        }
    }

    private void CheckEnvironment(){
        isOnGround=BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,Vector2.down,groundDistance,groundLayer)?true:false;
        if(isOnGround){
            canJump=true;
            isJumping=false;
        }
    }

    private void FlipCharacterDirection()
	{
		direction *= -1;
        transform.localScale=new Vector3(-1*transform.localScale.x,transform.localScale.y,transform.localScale.z);

	}
    private RaycastHit2D BoxCast(Vector2 pos, Vector2 size, Vector2 direction, float distance, LayerMask mask){
        RaycastHit2D hit=Physics2D.BoxCast(pos,size,0f,direction,distance,mask);
        return hit;
    }
}
