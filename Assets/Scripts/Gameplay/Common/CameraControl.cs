using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{

    public enum ControlType
    {
        ConstantWidth,		
		ConstantHeight,		
		OriginCameraSize	
    }

	
    public ControlType controlType;
	
	public SpriteRenderer focusObjectRenderer;
	
	public float offsetX = 0f;
	
	public float offsetY = 0f;
	
    public float dragSpeed = 2f;

	
	private float maxX, minX, maxY, minY;
	
    private float moveX, moveY;

	private Camera cam;
	
	private float originAspect;



	void Start()
	{
		cam = GetComponent<Camera>();
		Debug.Assert(focusObjectRenderer && cam, "Wrong initial settings");
		originAspect = cam.aspect;
		
		maxX = focusObjectRenderer.bounds.max.x;
		minX = focusObjectRenderer.bounds.min.x;
		maxY = focusObjectRenderer.bounds.max.y;
		minY = focusObjectRenderer.bounds.min.y;
		UpdateCameraSize();
	}


    void LateUpdate()
    {
		
		if (originAspect != cam.aspect)
		{
			UpdateCameraSize();
			originAspect = cam.aspect;
		}

        if (moveX != 0f)
        {
			bool permit = false;
			
			if (moveX > 0f)
			{
				
				if (cam.transform.position.x + (cam.orthographicSize * cam.aspect) < maxX - offsetX)
				{
					permit = true;
				}
			}
			
			else
			{
				
				if (cam.transform.position.x - (cam.orthographicSize * cam.aspect) > minX + offsetX)
				{
					permit = true;
				}
			}
			if (permit == true)
			{
				
				transform.Translate(Vector3.right * moveX * dragSpeed, Space.World);
			}
            moveX = 0f;
        }
		
        if (moveY != 0f)
        {
			bool permit = false;
			
			if (moveY > 0f)
			{
				
				if (cam.transform.position.y + cam.orthographicSize < maxY - offsetY)
				{
					permit = true;
				}
			}
		
			else
			{
				
				if (cam.transform.position.y - cam.orthographicSize > minY + offsetY)
				{
					permit = true;
				}
			}
			if (permit == true)
			{
				
				transform.Translate (Vector3.up * moveY * dragSpeed, Space.World);
			}
            moveY = 0f;
        }
    }

	
    public void MoveX(float distance)
    {
        moveX = distance;
    }


    public void MoveY(float distance)
    {
        moveY = distance;
    }

	
	private void UpdateCameraSize()
	{
		switch (controlType)
		{
		case ControlType.ConstantWidth:
			cam.orthographicSize = (maxX - minX - 2 * offsetX) / (2f * cam.aspect);
			break;
		case ControlType.ConstantHeight:
			cam.orthographicSize = (maxY - minY - 2 * offsetY) / 2f;
			break;
		}
	}
}
