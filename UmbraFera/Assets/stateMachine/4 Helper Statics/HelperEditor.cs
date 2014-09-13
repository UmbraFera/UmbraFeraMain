using UnityEngine;
using System.Collections;
//using UnityEditor;

namespace FSM_NS
{
public static class HelperEditor : object {

	private static Texture2D _staticRectTexture;	
	private static GUIStyle _staticRectStyle;
	
	private static Color textColor=new Color(.7f,.8f,.5f);
	
	//---------------------------------------------------------------------------------
	public static void DrawColorBox( Rect position, Color color, string name_ ) 
		
	{	
		if( _staticRectTexture == null )
			
		{
			_staticRectTexture = new Texture2D( 1, 1 );
			
		}
		
		if( _staticRectStyle == null )
			
		{
			_staticRectStyle = new GUIStyle();
		}
		_staticRectTexture.SetPixel( 0, 0, color );
		_staticRectTexture.Apply();
		
		
		
		_staticRectStyle.normal.background = _staticRectTexture;
			_staticRectStyle.normal.textColor = textColor;
		_staticRectStyle.alignment=TextAnchor.UpperCenter;
		
		
		 GUI.Box( position, name_, _staticRectStyle );
		
	}
	
	public static void DrawColorBox( Rect position, Color color )
	{
		DrawColorBox(position,color,"");
	
	}
	public static void DrawColorBox( Rect position, Color color,string name_, string body )
	{
		DrawColorBox(position,color,name_+body);
		// GUI.TextArea(position,name_+body);
		
		
	}	


//---------------------------------------------------------------------------------


}
}