
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;



namespace FSM_NS
{
public class EndPoint
{
	public Rect location;
	public EndPoint(Rect startLocation)
	{
		location = startLocation;
	}
}



//---------------------------------------------------------------------------------
public class statePanel
{
    public List<EndPoint> panelEndPoints;
	public List<int> eventsList=new List<int>();
	
	public Rect location;
	public string stateName="newName";
	public string stateDiscription = "";
	public int id=0;
	public bool selected=false;
	//bool   changed=false;
	public bool markedForDeath=false;
	
	public EndPoint activeEndPoint;
	     //200 X 150
         //name,id,events,locationX,locationY
		//state,   0,  ,212,283
	public statePanel(string line)
		{
		   string[] s=line.Split(',');
		   stateName=s[0];
		   id = int.Parse(s[1]);
		   location = new Rect(float.Parse(s[3]),
			                   float.Parse(s[4]),
			                   HelperConstants.StateWidth,
			                   HelperConstants.StateHeight);
		    stateDiscription = s[5];
		    
		    string[] eventsLine = s[2].Split(':');
		    
		    for(int i=0;i<eventsLine.Length;i++)
			{   if(eventsLine[i].Length>0)
			      {
			      
					//Debug.Log("EVENT["+eventsLine[i]+"]");
				   eventsList.Add(int.Parse(eventsLine[i]));
				  }
		    }
		   
		}
	
	//---------------------------------------------------------------------------------
	public statePanel(Rect currentLocation, string newName)
	{
	    panelEndPoints = new List<EndPoint>();
		location = currentLocation;
		stateName = newName;
		
	}
	//---------------------------------------------------------------------------------
	
	public bool handleHolds(Vector2 point)
	{
	   if(activeEndPoint==null) return false;
	   return activeEndPoint.location.Contains(point);
	}
	
	public bool stateHolds(Vector2 point)
	{
		
		return location.Contains(point);
	}
	
	
	
	
	public void showHighlight()
	{
		float boarder=2;
		Rect back = new Rect(location.position.x-boarder,location.position.y-boarder,location.width+boarder*2,location.height+boarder*2);
		HelperEditor.DrawColorBox(back,new Color(.65f,.65f,.15f),"");
	}
	
	//---------------------------------------------------------------------------------
	public void show()
	{
		
		
		Rect back    = location;
		
		float boarder=8;
		
		back.width+=boarder;
		back.height+=boarder;
		back.x-=boarder/2.0f;
		back.y-=boarder/2.0f;
		

		Rect pt = new Rect(location.position.x,location.position.y+location.height+2,10,10);
		EndPoint ep=new EndPoint(pt);
		Rect resized = new Rect(ep.location.x-1,ep.location.y-1,ep.location.width+2,ep.location.height+2);
		
		HelperEditor.DrawColorBox(resized,new Color(.35f,.35f,.35f),"");
		HelperEditor.DrawColorBox(pt,new Color(.15f,.15f,.15f),"");

		activeEndPoint=ep;

	}
	//---------------------------------------------------------------------------------
	
}
}