using UnityEngine;
using System.Collections;

namespace FSM_NS
{
public static class HelperGraphics : object {

	// Use this for initialization
public static float angle(Vector2 start, Vector2 end)
	{		
		float angle = Vector2.Angle(Vector2.right,
		                            new Vector2(  (end.x-start.x), (end.y-start.y)));
		
		if(end.y>start.y) angle = -1*angle;
		
		return angle;
	}
	
	
public static Vector2 midPoint(Vector2 a, Vector2 b)
   {
		return new Vector2(a.x+(b.x-a.x)/2,a.y+(b.y-a.y)/2);	
   }	
	
public static Vector2 quarterPoint(Vector2 a, Vector2 b)
	{
		return new Vector2(a.x+(b.x-a.x)/4,a.y+(b.y-a.y)/4);
	}	
}
}