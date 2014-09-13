using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


//---------------------------------------------------------------------------------



//---------------------------------------------------------------------------------



//---------------------------------------------------------------------------------
namespace FSM_NS
{
public class editorFSM : EditorWindow {
	//Rect windowRect =new Rect (100,100,200,200);
	List<statePanel> states =new List<statePanel>();
	List<EventConnection> events =new List<EventConnection>();
	List<AttributePair> attributes =new List<AttributePair>();
	private static bool needUpdate=false;
	private bool needToAddControllerScript=false;
	
static editorFSM()
{
			justRecompiled = true;
			//Debug.Log("test");
			
}
    public  Object source;
    public  Texture arrow;
    private int runCounter=0;

    public  statePanel startStateSelected;
	public  statePanel endStateSelected;
	private string currentState="";
	public  stateController currentStateController;

	bool    dragging_ = false;
	bool    dirty=false;
	Vector2 startPos;
	Vector2 currentPos;
	Vector2 deltaPos;
	Vector2 size = new Vector2(150,100);

	Rect windowRect = new Rect (200, 100, 250, 200);
	//Rect windowRect2 = new Rect (600, 100, 300, 300);
	statePanel selectedState;
	EventConnection selectedEvent;

    private Vector2 scrollPosition_; 
	private string  stateName = "state";
	private string  eventName = "event";
	private string 	fromState = "stateFrom";
	private string 	toState = "stateTo";
	
	private int clickCounter=0;
	private Vector2 lastClick=new Vector2();
		
	private string eventCondition = "";
	private string eventAction = "";
	private string filename="mainFSM";
	private string resourcesDirectory="/stateMachine/Resources";
	private string controllerName="appStateController";
	private Color  darkGray = new Color(.02f,.02f,.02f,1);	
	
	//private string attributeLabel="";
	//private string attributeLabel="";
	
	EventConnection eventConDrag;
	
					
	private string stateDiscription = "This state happens when...";
	
	
	private int screenBoxX  = Screen.width;
	private int screenBoxY  = Screen.height;
	
	private int scrollBoxX  = Screen.width  * 2;
	private int scrollBoxY  = Screen.height * 3;
	
	float hSbarValue=.5f;
	Vector2 scrollPosition = new Vector2(0,0);
	Rect virtualWindow = new Rect (0, 0, 1000, 1000);
	float maxX=0;
	float maxY=0;
	Vector2 atttributScroll=new Vector2(0,100);
	private static bool justRecompiled;
		
	//---------------------------------------------------------------------------------
	[MenuItem ("Window/Tools/FSMWindow")]
	private static void showEditor()
	{
	  
	   EditorWindow.GetWindow<editorFSM>(false,"State-Based Controller",true);
	}
	//---------------------------------------------------------------------------------
	
	
	//---------------------------------------------------------------------------------
	[MenuItem ("Window/Tools/FSMWindow",true)]
	private static bool showEditorValidator()
	{
		return true;
	}
	//---------------------------------------------------------------------------------
	
	
	
	//---------------------------------------------------------------------------------
	//for working in scene view
	void OnSelectionChange()
	{
	  Debug.Log("Selection Changed");
	}
	//---------------------------------------------------------------------------------
	
	
	//---------------------------------------------------------------------------------
	[ExecuteInEditMode]
	void OnEnable()
	{
		//myDelegate=DoWindow;
		Debug.Log("On enable");
		if(PlayerPrefs.HasKey("controllerName"))
		{
				controllerName = PlayerPrefs.GetString("controllerName");
				
		}
		if(PlayerPrefs.HasKey("target"))
			{
				string targetName = PlayerPrefs.GetString("target");
				GameObject o=GameObject.Find(targetName);
				
				if(o!=null)
				source = GameObject.Find(targetName);
				
			}
			if(PlayerPrefs.HasKey("resourceFilename"))
			{
				filename = PlayerPrefs.GetString("resourceFilename");
				
			}
		
	}
	//---------------------------------------------------------------------------------

	
		
	//----------------------------------------------------------------------------
	float getChunk(Rect frame, int l,float size)
	{
		
		return frame.y+15 +size*l;
	}
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------
	float getChunk(Rect frame, int l)
	{
		float lChunk=50;
		return frame.y+15 +lChunk*l;
	}
	//----------------------------------------------------------------------------
	
		

		
	//----------------------------------------------------------------------------
		public void Update()
		{
			if(justRecompiled)
			{
			justRecompiled=false;
				needUpdate=false;
				loadFSM(HelperFile.getTextFileFromResource(filename));
				
				if(needToAddControllerScript)
				{
					needToAddControllerScript=false;
					GameObject o=  (GameObject)source;
					o.AddComponent(controllerName);
				}
			}
			
			if(source!=null && Application.isPlaying)
			{
				GameObject sco = (GameObject)source;
				if(sco!=null)
				{
				    stateController sc= sco.GetComponent<stateController>();
					if(sc.myStateMachine!=null)
					{
						if(sc.myStateMachine.state!=null)
						{
							currentState=sc.myStateMachine.state.name;
							currentStateController = sc;
							Repaint();
						}
					}
				}
			}
			
		}
	//----------------------------------------------------------------------------
		
		
		
	//----------------------------------------------------------------------------
		public static void OnCompileScripts()
		{
			Debug.Log("OnCompileScripts");
		}	
	//----------------------------------------------------------------------------
		
		
		
	//----------------------------------------------------------------------------
	void attributeInputBox(Rect frame, float boarder)
	{
		
		HelperEditor.DrawColorBox(new Rect(frame.x-boarder,frame.y-boarder,frame.width+boarder*2,frame.height+boarder*2),Color.gray,"");
		HelperEditor.DrawColorBox(frame,new Color(.15f,.15f,.15f),"Attributes");
		float lChunk    = 25;
		float rightSide = frame.width-boarder*2;
		float l1        = frame.y+15;
		float width     = 70;
		
		//atttributScroll = EditorGUILayout.BeginScrollView(atttributScroll, true, true,  GUILayout.Width(frame.width),  GUILayout.Height(frame.height-30));  
		atttributScroll = GUI.BeginScrollView (new Rect (frame.x+5,l1+10,frame.width-10,200),
		                                       atttributScroll, new Rect (frame.x+5, 0, 0, lChunk*attributes.Count),false,false);
		for(int i=0;i<attributes.Count;i++)
		{
		AttributePair attr = (AttributePair)attributes[i];
			 
			attr.label = GUI.TextField(new Rect(15,i*lChunk,width,20),attr.label);
			
			GUI.Label(new Rect(20+width,i*lChunk,20,20),"=");
			if(!Application.isPlaying )
				attr.value = GUI.TextField(new Rect(15+width+25,i*lChunk,width/1.5f,20),attr.value);
			else
				if(currentStateController!=null)
				{   
					GUI.TextField(new Rect(15+width+25,i*lChunk,width/1.5f,20),currentStateController.myStateMachine.getAtributeValue(attr.label));
				}
			if(GUI.Button(new Rect(45+width+width/1.5f, i*lChunk,20,20),"-"))
			{
			    attributes.Remove(attr);
			}
		}
		GUI.EndScrollView();
		
		
		GUILayout.BeginArea(new Rect(frame.x+70,frame.y+frame.height-20,60,20));
		if(GUILayout.Button("NEW"))
		{
			attributes.Add(new AttributePair("",""));
		}
		GUILayout.EndArea();		
   }
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------	
	private void addState()
	{
			statePanel newState = new statePanel(new Rect(startPos.x-HelperConstants.StateWidth/2,startPos.y-HelperConstants.StateHeight/2,
		                                              HelperConstants.StateWidth,
			                                          HelperConstants.StateHeight),stateName);
		newState.stateDiscription = stateDiscription;
		newState.id = states.Count;
		states.Add(newState);
	}
	//----------------------------------------------------------------------------
	
	
	
	
	//----------------------------------------------------------------------------
	/// <summary>
	/// Save the editor window fields
	/// </summary>
	private void save()
	{
			PlayerPrefs.SetString("resourceFilename",filename);
			PlayerPrefs.SetString("controllerName",controllerName);
			if(source!=null)
			   PlayerPrefs.SetString("target",source.name);
			
			PlayerPrefs.Save();
	}
	//----------------------------------------------------------------------------
		
		
		
		
	//----------------------------------------------------------------------------
	void fileControlPanel(Vector2 point)
	{
	        HelperEditor.DrawColorBox(new Rect(point.x-7,point.y-7,204,274),Color.gray);
			HelperEditor.DrawColorBox(new Rect(point.x-5,point.y-5,200,270),new Color(.15f,.15f,.15f));
	   
			GUI.Label(new Rect(point.x,point.y,150,20),"Resource Directory");
			resourcesDirectory = GUI.TextField(new Rect(point.x,point.y+20,180,20),resourcesDirectory);
			
			GUI.Label(new Rect(point.x,point.y+45,200,20),"Resource Filename");
			filename = GUI.TextField(new Rect(point.x,point.y+65,150,20),filename);
			
			

			
			GUILayout.BeginArea(new Rect(point.x,point.y+90,80,50));
			if(GUILayout.Button("SAVE FSM"))
			{
				Debug.Log(("SAVE FSM "+ Application.dataPath + resourcesDirectory));
				save();
				HelperFile.saveToFile(Application.dataPath + resourcesDirectory+"/"+filename+".txt",getFSMString());

				//AssetDatabase.ImportAsset(Application.dataPath + resourcesDirectory+"/"+filename+".txt");
				//loadFSM(HelperFile.getTextFileFromResource(filename));
				Repaint();
			}//
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(point.x+100,point.y+90,80,50));
			if(GUILayout.Button("LOAD FSM"))
			{
				Debug.Log(("LOAD FSM "+ Application.dataPath + resourcesDirectory));
               save();
				//HelperFile.saveToFile(Application.dataPath + resourcesDirectory+"/"+filename+".txt",getFSMString());
				
				string filestring =HelperFile.getTextFileFromResource(filename);
				//filestring = EditorGUILayout.ObjectField(filestring,typeof(object), true);
				
				
				if(filestring.Length>3)
					loadFSM(filestring);

			}
			GUILayout.EndArea();		
			
		
			
			GUI.Label(new Rect(point.x,point.y+130,200,20),"Controller name");

			controllerName = GUI.TextField(new Rect(point.x,point.y+150,180,20),controllerName);
						
			GUILayout.BeginArea(new Rect(point.x,point.y+175,100,50));
			if(GUILayout.Button("Build Controller"))
			{
				if(controllerName=="stateController")controllerName=controllerName+"1";
				Debug.Log("Make Controller: "+Application.dataPath+"/"+controllerName);
				save();
				HelperFile.saveToFile(Application.dataPath+"/stateMachine/Controllers/"+controllerName+".cs",HelperFormater.makeFileUsing(controllerName,filename,states));

				//AssetDatabase.ImportAsset(Application.dataPath+"/"+controllerName+".cs");
				//loadFSM(HelperFile.getTextFileFromResource(filename));
				
				//if(currentStateController == null)
				if(source==null)
				{
				  Debug.Log("No controller selected");
				  GameObject o=new GameObject();
					o.name=controllerName;
					source = o;
					needToAddControllerScript=true;
					AssetDatabase.Refresh();//
				  //GameObject go = (GameObject)Instantiate(o);
					//o.AddComponent(controllerName);//
				}else
				{
				   GameObject so=(GameObject)source;
					object testO=so.GetComponent(controllerName);
					if(testO == null)
					{
						needToAddControllerScript=true;
						AssetDatabase.Refresh();//
					}
				}
				
				Repaint();
			}
			GUILayout.EndArea();
			string beforeSource="";
			if(source!=null)
			{
			 beforeSource = source.name;
			}
			GUI.Label(new Rect(point.x,point.y+210,200,20),"Target");
			GUILayout.BeginArea(new Rect(point.x+50,point.y+210,135,50));
			source = EditorGUILayout.ObjectField(source, typeof(GameObject), true);
			GUILayout.EndArea();
			if(source!=null)
			if(source.name!=beforeSource)
			{
				PlayerPrefs.SetString("target",source.name);
				PlayerPrefs.Save();
				GameObject o=(GameObject)source;
				stateController tempSC = o.GetComponent<stateController>();
				if(tempSC!=null)
				{
				    controllerName = getShortName( tempSC.ToString());
					PlayerPrefs.SetString("controllerName",controllerName);
					PlayerPrefs.Save();
				 }
			}
			//
			
			GUILayout.BeginArea(new Rect(point.x,point.y+240,100,50));
			if(GUILayout.Button("Clear Screen"))
			{   
			
				reset();
			}
			GUILayout.EndArea();
			
	}
    //-----------------------------------------------------------------------------
		
		private string getShortName(string name)
		{
		   string[] s=name.Split('(');
			return s[1].Replace(")","");
		   
		}
		
	//-----------------------------------------------------------------------------
	void buttonPanel()
	{
			
		
			fileControlPanel(new Vector2(10,20));
	

		
		//stateInputBox(new Rect(10,10, 200, 180),2);
		//eventInputBox(new Rect(10,200, 200, 300),2);
		
		attributeInputBox(new Rect(5,300, 200, 280),2);
		
	}
	//----------------------------------------------------------------------------

	
	
	//----------------------------------------------------------------------------
	void cleanEvents()
	{
		for(int i=events.Count-1;i>=0;i--)
		{
			if(events[i].markedForDeath)
			{
				events[i].selected=false;
				events.Remove(events[i]);
			}
			
		}
		int count=1;
		foreach(EventConnection ec in events)
		{
			ec.fromToCount = getNumberEvents(ec.from,ec.to,count);
			ec.id = count - 1;
			count++;
		}
	}
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------
	int getNumberEvents(statePanel from, statePanel to, int startIndex)
	{
	   int sum=0;
	   for(int i =startIndex;i<events.Count;i++)
	   {
	      EventConnection ec =(EventConnection)events[i]; 
	      if(ec.from == from && ec.to == to)
	      {
	        sum++;
	      }
	   }
	   return sum;
	}
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------
	void clean()
	{//
	
		dirty=false;
	    for(int i=states.Count-1;i>=0;i--)
	    {
	       if(states[i].markedForDeath)
	        {
				states[i].selected=false;
				deleteState(states[i]);
			}
			
		}

	}
	//---------------------------------------------------------------------------------
	
	

	//---------------------------------------------------------------------------------
	 bool mouseUp
	{
	get{
		Event currentEvent = Event.current;
		
		EventType currentEventType = currentEvent.type;
			//Debug.Log("mouse:"+currentEventType);
		return currentEventType==EventType.mouseUp;
		}
		
	}
	//---------------------------------------------------------------------------------
	
	//---------------------------------------------------------------------------------
	bool mouseDown
	{
		get{
			Event currentEvent = Event.current;
			
			EventType currentEventType = currentEvent.type;
			return currentEventType==EventType.mouseDown;
		}
		
	}
	//---------------------------------------------------------------------------------
	
	
	//---------------------------------------------------------------------------------
	bool mouseDrag
	{
		get{
			Event currentEvent = Event.current;
			
			EventType currentEventType = currentEvent.type;
			return currentEventType==EventType.mouseDrag;
		}
		
	}
	//---------------------------------------------------------------------------------
	
	//*********************************************************************************
	void OnGUI()
	{
		maxX=0;
		maxY=0;
		//	Debug.Log("scrollPosition ["+scrollPosition.x+", "+scrollPosition.y+"]");
		scrollPosition = GUI.BeginScrollView (
			new Rect (0, 0, position.width, position.height), 
			scrollPosition, 
			virtualWindow
			);	
			
			GUI.Label(new Rect(position.width/2-100,5,200,20),"Double click to create new state.");
			
			foreach(statePanel sp in states)
			{
			  if(sp.selected==true){ sp.showHighlight();}
			}
			
			BeginWindows ();
			int count=0;


		       foreach(statePanel state_ in states)
		       {	
				    state_.location = GUILayout.Window (count, state_.location, DoPanelWindow, count+": "+state_.stateName);
				    

				    

					if(state_.location.x+state_.location.width>maxX)
					{
						maxX=state_.location.x+state_.location.width + 10;
					}
					if(state_.location.y+state_.location.height>maxY)
					{
						maxY=state_.location.y+state_.location.height + 10;
					}
			
		
					state_.id = count;
					state_.show();
					count++;
					
               }
               
		virtualWindow.width=maxX;
		virtualWindow.height=maxY;
		EndWindows();
		
			
		drawLinks();	
			
			
		
		
		foreach(statePanel state_ in states)
		{
			if(state_.markedForDeath)
				dirty=true;
		}
		
		GUI.EndScrollView();
	
		buttonPanel      ();
		
		eventMouseMaker  ();	
				
						
	if(dirty)clean();
	cleanEvents();

		
	}
	//*********************************************************************************
	//---------------------------------------------------------------------------------

/*
	//----------------------------------------------------------------------------
	private EventConnection getConnection(Vector2 point)
		{
			//Debug.Log("scrollPosition ["+scrollPosition.x+", "+scrollPosition.y+"]  "+point.x+",  "+point.y);
			Vector2 pmod=new Vector2(point.x+scrollPosition.x,point.y+scrollPosition.y);
			foreach (EventConnection ec in events)
			{
				if(ec.holdsPoint(pmod))return ec;
			}
		return null;
		}
	//----------------------------------------------------------------------------
	*/
	
	//----------------------------------------------------------------------------
private void eventMouseMaker()
	{//...................................................................
		if(mouseUp)
		{
			
			Vector2 stopMousePos = Event.current.mousePosition;
			//currentPos = Event.current.mousePosition;
			
			bool clicked = Vector2.Distance(stopMousePos,startPos)<.5f;
			if(clicked)
				{
				clickCounter++;
				   selectEvent(startPos);
					Debug.Log("Clicked"+clickCounter+"  "+Vector2.Distance(stopMousePos,lastClick));
				if(selectedEvent==null && clickCounter>1 )
				{
					clickCounter=1;
					if(Vector2.Distance(stopMousePos,lastClick)<.9f)
					{
						clickCounter=0;
					  addState();
					  Repaint();
					  return;
				    }	
				}
				 if(clickCounter>1)clickCounter=1;
				 lastClick = new Vector2(stopMousePos.x,stopMousePos.y);
				//if(Vector2.Distance(stopMousePos,lastClick)<.9f)
				
			    }else
				  clickCounter=0;
				
			statePanel sp = getStateForPoint(stopMousePos);
			
			if(sp !=null  && startStateSelected!=null && eventConDrag !=null)
			{
				endStateSelected = sp;
				//Debug.Log("mouseUP  "+sp.stateName);
				EventConnection newEvent = new EventConnection(startStateSelected,endStateSelected);
				//newState.stateDiscription = stateDiscription;
				newEvent.eventName = eventName;
				newEvent.id = events.Count;
				newEvent.conditions=eventCondition;
				newEvent.actions = eventAction;
				events.Add(newEvent);
				
				
				
			}
			eventConDrag=null;
			
			//getConnection(stopMousePos);
			
			selectStateReset();
			Repaint();
		}
		//...................................................................
		if(mouseDown)
		{
			startPos=Event.current.mousePosition;
			Vector2 startMousePos = Event.current.mousePosition;
			
			statePanel sp = getStateHandleForPoint(startMousePos);
			if(sp !=null)
			{
				startStateSelected = sp;
				eventConDrag = new EventConnection(sp);

			}
			Repaint();
		}
		//...................................................................
		if(mouseDrag && startStateSelected!=null)
		{  
		    
			Vector2 mousePos = Event.current.mousePosition;
			
				Vector2 pmod=new Vector2(mousePos.x+scrollPosition.x,mousePos.y+scrollPosition.y);
				mousePos = pmod;
			//if(startStateSelected!=null && eventConDrag!=null)
			   if(eventConDrag!=null)
				{
					if(eventConDrag.to==null)
						eventConDrag.to = new statePanel(new Rect(mousePos.x,mousePos.y,5,5),"temp");
					else
					{
						eventConDrag.to.location.x=mousePos.x;
						eventConDrag.to.location.y=mousePos.y;
						

						selectState(mousePos);
						//}
						
						Repaint();
					}
				
			
			}
			
			
			
		}
}
	//----------------------------------------------------------------------------
	
	//----------------------------------------------------------------------------
	private void selectState(Vector2 point)
    {
       foreach(statePanel sp in states)
       {
          if(sp.stateHolds(point))
          {
             sp.selected=true;
			}else  sp.selected=false;
       }
    }
	//----------------------------------------------------------------------------
	
	//----------------------------------------------------------------------------
	private void selectEvent(Vector2 point)
	{
		selectedEvent=null;
			Vector2 pmod=new Vector2(point.x+scrollPosition.x,point.y+scrollPosition.y);
		foreach(EventConnection ec in events)
		{
				if(ec.holdsPoint(pmod))
			{
				ec.selected    =true;
				selectedEvent  = ec;
				eventName      = ec.eventName;
				eventCondition = ec.conditions;
				eventAction    = ec.actions;
				
				
			}
			else 
			{
			    ec.selected=false;
				
			}
		}
	}
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	private void resetEvent(Vector2 point)
	{
		foreach(EventConnection ec in events)
		{
			  ec.selected=false;
		}
		selectedEvent=null;
	}
	//----------------------------------------------------------------------------
	
	//----------------------------------------------------------------------------
	private void selectStateReset()
	{
		foreach(statePanel sp in states)
		{
			  sp.selected=false;
		}
		selectedState = null;
	}
	//----------------------------------------------------------------------------
	
	//----------------------------------------------------------------------------
	private statePanel getStateHandleForPoint(Vector2 point)
	{
		Vector2 pmod=new Vector2(point.x+scrollPosition.x,point.y+scrollPosition.y);
			
	   foreach(statePanel sp in states)
	   {
				if(sp.handleHolds(pmod))return sp;
	   }
	 return null;
	}
	//----------------------------------------------------------------------------
	
	//----------------------------------------------------------------------------
	private statePanel getStateForPoint(Vector2 point)
	{
			Vector2 pmod=new Vector2(point.x+scrollPosition.x,point.y+scrollPosition.y);
			
		foreach(statePanel sp in states)
		{
				if(sp.stateHolds(pmod))return sp;
		}
		return null;
	}	
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	void drawLinks()
	{
		//Debug.Log("test 1: "+events.Count);
	 
	
	   foreach(EventConnection ec in events)
	   {
	       
			Handles.BeginGUI();
			if(ec !=null)
			 DrawNodeCurve(ec.from.location,ec.to.location,ec);
			
			Handles.EndGUI();
		   
		   
		   //  BeginWindows ();
		
		//windowRect2 = GUILayout.Window (2, windowRect2, DoWindow, "State2");
		//windowRect = GUILayout.Window(1,
		//EndWindows();
		}
		
		if(eventConDrag!=null)
		{
			if(eventConDrag.to!=null)
			{

				Handles.BeginGUI();
			    DrawNodeCurve(eventConDrag.from.location,eventConDrag.to.location,eventConDrag);
			    Handles.EndGUI();

			}
		
		}
		
		foreach(EventConnection ec in events)
			{
				ec.showNotSelected();
			}
		foreach(EventConnection ec in events)
			{
				ec.showSelected();
			}

	}
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	public enum nodeLocation
	{
	  right,
	  bottom,
	  left,
	  top
	
	}
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------
	nodeLocation getNodeLocation(float angle)
	{
		if(angle>= -45  &&  angle <  45 ) return nodeLocation.right;
		if(angle>=  45  &&  angle <  135) return nodeLocation.top;
		if(angle>=  135 ||  angle < -135) return nodeLocation.left;
		if(angle>= -135 &&  angle < -45 ) return nodeLocation.bottom;
		
	    return nodeLocation.right;
	}
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	nodeLocation getNodeLocation(Rect start, Rect end)
		{  
	      return getNodeLocation(HelperGraphics.angle(start.center,end.center));
		}	
	//----------------------------------------------------------------------------
	

	//----------------------------------------------------------------------------	
	Vector3 getStartPos(Rect start, Rect end, nodeLocation currentNodeLocation, int repeatCount)
	{
	    float adj = 0;
	    if(start == end) adj = -60.0f;
	    float boxAdjust=2;
	
		if(currentNodeLocation == nodeLocation.right)
		{
				return new Vector3(start.x + start.width+boxAdjust, start.y + start.height / 3 +repeatCount*10 +adj, 0);
		
		}
		if(currentNodeLocation == nodeLocation.left)
		{
				return new Vector3(start.x-boxAdjust , start.y + start.height / 3 +repeatCount*10  +adj, 0);
			
		}
		if(currentNodeLocation == nodeLocation.top)
		{
				return new Vector3(start.x + start.width/3 +repeatCount*10  +adj, start.y-boxAdjust , 0);
			
		}
		if(currentNodeLocation == nodeLocation.bottom)
		{
				return new Vector3(start.x + start.width/3 +repeatCount*10  +adj, start.y  + start.height + boxAdjust, 0);
			
		}
		return new Vector3(start.x + start.width, start.y + start.height / 2, 0);
	}
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	//new Vector3(end.x, end.y + end.height / 2, 0);
	Vector3 getEndPos(Rect start, Rect end, nodeLocation currentNodeLocation, int repeatCount)
	{
	    float adj=10;
		float adj2 = 0;	
		if(start == end) adj2 = 60.0f;
		
		if(currentNodeLocation == nodeLocation.right)
		{
				return new Vector3(end.x-adj, end.y + end.height / 1.5f +repeatCount*10 +adj2, 0);
			
		}
		if(currentNodeLocation == nodeLocation.left)
		{
				return new Vector3(end.x +end.width+adj, end.y + end.height / 1.5f +repeatCount*10 +adj2, 0);
			
		}
		if(currentNodeLocation == nodeLocation.top)
		{
				return new Vector3(end.x + end.width / 1.5f +repeatCount*10+adj2, end.y  + end.height+adj, 0);
			
		}
		if(currentNodeLocation == nodeLocation.bottom)
		{
				return new Vector3(end.x +end.width/1.5f +repeatCount*10+adj2, end.y -adj, 0);
			
		}
		return new Vector3(end.x, end.y + end.height / 1.5f, 0);
	}
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------
	Vector3 getStartTan(nodeLocation currentNodeLocation, float chunk, Vector3 startPos)
	{
		if(currentNodeLocation == nodeLocation.right)
		{
			return startPos +   chunk*Vector3.right;
			
		}
		if(currentNodeLocation == nodeLocation.left)
		{
			return startPos +   chunk*Vector3.left;
			
		}
		if(currentNodeLocation == nodeLocation.top)
		{
			return startPos +   chunk*Vector3.down;
			
		}
		if(currentNodeLocation == nodeLocation.bottom)
		{
			return startPos +   chunk*Vector3.up;
			
		}
		return startPos +   chunk*Vector3.right;
	}
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------
	Vector3 getEndTan(nodeLocation currentNodeLocation, float chunk, Vector3 endPos)
	{
		if(currentNodeLocation == nodeLocation.right)
		{
			return endPos +   chunk*Vector3.left;
			
		}
		if(currentNodeLocation == nodeLocation.left)
		{
			return endPos +   chunk*Vector3.right;
			
		}
		if(currentNodeLocation == nodeLocation.top)
		{
			return endPos +   chunk*Vector3.up;
			
		}
		if(currentNodeLocation == nodeLocation.bottom)
		{
			return endPos +   chunk*Vector3.down;
			
		}
		return endPos +   chunk*Vector3.right;
	}
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	    void drawHandle(Vector3 point)
	    {  
	       float boarder = 1;
		   float size = 4;
		   Rect box = new Rect(point.x-size/2,point.y-size/2,size,size);
		   Rect box2 = new Rect(box.x-boarder,box.y-boarder,box.width+boarder*2,box.height+boarder*2);
		   
		   HelperEditor.DrawColorBox(box2,Color.gray);
		   HelperEditor.DrawColorBox(box,Color.black);
		
	     }
	//----------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------
		void DrawNodeCurve(Rect start, Rect end, EventConnection eventCon) {
		
			nodeLocation currentNodeLocation=getNodeLocation(start,end);
			
			Vector3 startPos =   getStartPos(start,end,currentNodeLocation,eventCon.fromToCount);
			Vector3 endPos   =   getEndPos(start,end,currentNodeLocation,eventCon.fromToCount);
			
			eventCon.fromPT  =   startPos;
			eventCon.toPT    =   endPos;			
			
			float chunk =  Vector3.Distance(startPos,endPos)/2.5f;		
			
			Vector3 startTan = getStartTan(currentNodeLocation,chunk,startPos);
			Vector3 endTan = getEndTan(currentNodeLocation,chunk,endPos);//endPos +  chunk*Vector3.left;;
			
			Color shadowCol = new Color(0, 0, 0, 0.06f);
			//Vector3 end3=new Vector3(endPos.x-(endPos.x- startPos.x)/45,endPos.y-(endPos.y- startPos.y)/45,0);
			
			for (int i = 0; i < 3; i++) 
				{// Draw a shadow
				//arrow = Resources.GetBuiltinResource<Texture>("/stateMachine/Resources/arrow.png");
				Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 3) * 5);
				}
			float arrowSize=17;	
			Rect arrowRect=new Rect(endPos.x-arrowSize/2,endPos.y-arrowSize/2,arrowSize,arrowSize);	
			if(currentNodeLocation==nodeLocation.bottom)
			    arrow = (Texture2D)Resources.Load("arrowDown",typeof(Texture2D));
			
			else
				if(currentNodeLocation==nodeLocation.top)
				arrow = (Texture2D)Resources.Load("arrowUp",typeof(Texture2D));
			else
				if(currentNodeLocation==nodeLocation.left)
					arrow = (Texture2D)Resources.Load("arrowLeft",typeof(Texture2D));	
			else
				
					arrow = (Texture2D)Resources.Load("arrowRight",typeof(Texture2D));					
							
					
			//GUIUtility.RotateAroundPivot(20,new Vector2(0,0));
			
			Handles.color = Color.white;
			
			
			//Handles.ConeCap (0,end3, Quaternion.AngleAxis(0,Vector3.down) ,11);	
			//Handles.ConeCap (0,endPos, Quaternion.AngleAxis(0,Vector3.down) ,7);
	
			Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.gray, null, 2);
			drawHandle(startPos);
			GUI.DrawTexture(arrowRect,arrow);
	}
	//----------------------------------------------------------------------------
	
	
	//---------------------------------------------------------------------------------
	statePanel getPanelAtClick(Vector2 clickedAt)
	    {
	       
	       foreach(statePanel state_ in states)
	       {
	          if(state_.location.Contains(clickedAt))return state_;
	       
	       }
	       return null;
	    
	    }
	//---------------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	void deleteState(statePanel target_)
	{
		states.Remove(target_);
	
	  
	   for(int i=events.Count-1;i>=0;i--)
	   {
			if(events[i].from == target_ || events[i].to == target_) 
			  {
			   EventConnection ec=(EventConnection)events[i];
				events.Remove(ec);
			  }
	   }
		for(int i=0;i<events.Count;i++)
		{
		   events[i].id = i;
		}
	   
	}
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	private string getLinkedEvents(statePanel state)
	{
	    List<string> links=new List<string>();
	    foreach(EventConnection ec in events)
	    {
	       if(ec.from == state)links.Add(ec.id.ToString());
	       
	    }
	    
	    string outString = "";
	    for(int i = 0 ; i < links.Count ; i++)
	    {
			if(i>0)outString = outString+":"+links[i];
			else outString = links[i];
			
	    }
	
		return outString;
	}
	//----------------------------------------------------------------------------
	
	//----------------------------------------------------------------------------
	private string getAttributeString()
	{
	string line="";
	for(int i=0;i<attributes.Count;i++)
		{
			if(i<attributes.Count-1)	
			 line=line+attributes[i].label+"="+attributes[i].value+";\n";
			 else
					line=line+attributes[i].label+"="+attributes[i].value+"\n";
		}
		return line + "|\n\n";
	}
	//----------------------------------------------------------------------------
	
	//----------------------------------------------------------------------------
	private string getStateString()
	{
	   string line1 = "//STATES   ID,   EVENTS\n";
	   int i=0;
	   foreach(statePanel sp in states)
	   {
			if(i<states.Count-1)
	           line1 = line1+sp.stateName+","+sp.id+","+getLinkedEvents(sp)+","+sp.location.x+","+sp.location.y+","+sp.stateDiscription+";\n";
	         else
			   line1 = line1+sp.stateName+","+sp.id+","+getLinkedEvents(sp)+","+sp.location.x+","+sp.location.y+","+sp.stateDiscription+"\n";
				i++;
	   }
		
		return line1+"|\n\n";
	}
	//----------------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------------
	private string getFSMString()
	{
	  
		return getStateString()+getAttributeString()+getEventsString();
	
	}
	//----------------------------------------------------------------------------
	
	
	//---------------------------------------------------------------------------
	private string getEventsString()
	{
	string line="";
	   for(int i=0;i<events.Count;i++)
	   {
			if(i<events.Count-1)
				line =line+events[i].eventName+","+events[i].id+","+events[i].to.id+","+events[i].conditions+","+events[i].actions+";\n";
	        else
				line =line+events[i].eventName+","+events[i].id+","+events[i].to.id+","+events[i].conditions+","+events[i].actions+"\n";
			}
	return line;
	}//
	
	
	//----------------------------------------------------------------------------
	void DoPanelWindow (int windowID) {
		if(GUI.Button (new Rect(180,0,20,15),"-"))
		{
			//Debug.Log("Delete "+windowID);
			states[windowID].markedForDeath=true;
			dirty=true;
		}//
		
		states[windowID].stateName=GUILayout.TextField(states[windowID].stateName);
			states[windowID].stateName = states[windowID].stateName.Replace(" ","");
			states[windowID].stateName = states[windowID].stateName.Replace(".","");
			states[windowID].stateName = states[windowID].stateName.Replace(",","");
			states[windowID].stateName = states[windowID].stateName.Replace("/","");
			states[windowID].stateName = states[windowID].stateName.Replace("\"","");
			states[windowID].stateName = states[windowID].stateName.Replace(";","");
			states[windowID].stateName = states[windowID].stateName.Replace(":","");
		states[windowID].stateDiscription=GUILayout.TextArea(states[windowID].stateDiscription);
		
			//Debug.Log("-+"+currentState+"  --"+states[windowID].stateName);
			if(Application.isPlaying &&  currentState == states[windowID].stateName)
			{
			    runCounter+=2;
				int adjRC = runCounter%((int)HelperConstants.StateWidth-10);
				int adjX = runCounter%((int)HelperConstants.StateWidth-25);
				int adjY = runCounter%((int)45);
				HelperEditor.DrawColorBox(new Rect(5,HelperConstants.StateHeight-15,5+adjRC,10),Color.green);
				
				//HelperEditor.DrawColorBox(new Rect(5+adjX-10,HelperConstants.StateHeight-20,25,15),Color.green);
			}else
			{
			 //runCounter=0;
		     // HelperEditor.DrawColorBox(new Rect(5,HelperConstants.StateHeight-10,HelperConstants.StateWidth-10,5),Color.gray);
		    }
		//GUI.Box(,)
		
		GUI.DragWindow ();
		//return null;		
	}
	//----------------------------------------------------------------------------
	
	private statePanel getStateFrom(int id)
	{
	   foreach(statePanel sp in states)
	   {
	      if(sp.eventsList.Contains(id))return sp;
	   }
	   return null;
	}
	
	private void loadFSM (string FSM)
	    {
	       reset();
			//string line = FSM.Replace(" ","");
			string line = HelperFormater.stripComments(FSM.Split('\n'));
	        string[] parts=line.Split('|');
	        
			string[] stateParts = parts[0].Split(';');
			string[] attributeParts = parts[1].Split(';');
			string[] eventParts = parts[2].Split(';');
						
			for(int i=0;i<stateParts.Length;i++)
			{
				states.Add(new statePanel(stateParts[i]));
			}
			
			
			for(int i=0;i<attributeParts.Length;i++)
			{
				string[] s=attributeParts[i].Split('=');
				if(s.Length>1)
					attributes.Add(new AttributePair(s[0],s[1]));
				
			}
			//name,id,to,cond,action		
			for(int i=0;i<eventParts.Length;i++)
			{
			   
				string[] s=eventParts[i].Split(',');
				if(s.Length>3)
				{
					//Debug.Log("EVENT="+eventParts[i]);
				int id_=int.Parse( s[1]);
				string name_=s[0];
				statePanel from_=getStateFrom(id_);
				statePanel to_ = states[int.Parse(s[2])];
				
				EventConnection ec = new EventConnection(from_,to_);
				ec.eventName = name_;
				ec.id = id_;
				ec.conditions = s[3];
				ec.actions = s[4];
				events.Add(ec);
				}
				
			}
			
	        
	
		}
		
			private void reset()
			{
			   //source = null;
			   states.Clear();
			   events.Clear();
			   attributes.Clear();
			   clickCounter = 0;
			   startStateSelected=null;
			   endStateSelected=null;
			   
			   Repaint();
			}	
		
		
	
	
	
	

}
}
