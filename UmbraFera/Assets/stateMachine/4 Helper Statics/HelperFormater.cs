using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FSM_NS{
public static class HelperFormater : object {



//----------------------------------------------------------------------------------------------------------------
   /// <summary>
   /// header section
   /// </summary>
   /// <returns>The header.</returns>
   /// <param name="controllerName">Controller name.</param>
	public static string sectionHeader(string controllerName)
		{   string line0="using FSM_NS;\n";
		string line1="using UnityEngine;\n";
		string line2="using System.Collections;\n\n";
		string line3="public class "+controllerName+": stateController ,stateControllerInterface \n{\n";
		
		return line0+line1+line2+line3;
		
		}
	//----------------------------------------------------------------------------------------------------------------
		
		
	//----------------------------------------------------------------------------------------------------------------
	
				
	
	//----------------------------------------------------------------------------------------------------------------		
	public static string sectionLoadFSM(string definationFile)
		{
			string line1="    string pushString = FSM_NS.HelperFile.getTextFileFromResource(\""+definationFile+"\");\n";
			string line2="    myStateMachine = new FSM_NS.FSM(pushString,this);\n";
			return line1+line2;
		}
	
	
	/// <summary>
	/// Sections  start and update.
	/// </summary>
	/// <returns>The start and update.</returns>
	/// <param name="definationFile">Defination file.</param>
	public static string sectionStartAndUpdate(string definationFile)
	{	
		string line1="void Start ()\n"+
		              "  {\n"+
		                   sectionLoadFSM(definationFile)+
					"\nmyStateMachine.jumpToState(startStateID);\n "+
		             "  }\n\n";
		string line2="void Update () \n  {\n//Your code here\n\n"+
				"superUpdate();\n"+"\n  }\n\n";
				
				
			string line3="\n\npublic override void tickFired(){\n\n}\n\n";		
		return line1+line2+line3;
		}
	//----------------------------------------------------------------------------------------------------------------
		
		
	//----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Section state call backs.
	/// </summary>
	/// <returns>The state call backs.</returns>
	/// <param name="states">States.</param>
	public static string sectionStateCallBacks(List<statePanel> states)
	{
	string line="";
	string devider="//.......................................................................\n";
	
		foreach (statePanel sp in states)
		{
				line=line+devider+"//"+sp.stateDiscription+
					"\nprivate void Entered_State_"+sp.stateName+" ()\n  {\n Debug.Log(\"Entered State "+sp.stateName +"\");  \n   //state entered handling code goes here\n\n  }\n"+devider +"\n\n\n";
					
				line=line+devider+"//"+sp.stateDiscription+
					"\nprivate void Left_State_"+sp.stateName+" ()\n  {\n Debug.Log(\"Left State "+sp.stateName +"\");  \n   //state left handling code goes here\n\n  }\n"+devider +"\n\n\n";
				
		}
		
		
		return line;
		}
	//----------------------------------------------------------------------------------------------------------------
	
	
	
	//----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Section end.
	/// </summary>
	/// <returns>The end.</returns>
	public static string sectionEnd()
	{
		return "\n\n  }\n\n";
	}
	//----------------------------------------------------------------------------------------------------------------
		
	
	//----------------------------------------------------------------------------------------------------------------	
	/// <summary>
	/// Makes the file
	/// </summary>
	/// <returns>The file using.</returns>
	/// <param name="controllerName">Controller name.</param>
	/// <param name="definitionFileName">Definition file name.</param>
	/// <param name="states">States.</param>
	public static string makeFileUsing(string controllerName, string definitionFileName, List<statePanel> states)
	{
		return sectionHeader(controllerName)+
			   sectionStartAndUpdate(definitionFileName)+
			   sectionStateCallBacks(states)+
			   sectionEnd();
	}
	//----------------------------------------------------------------------------------------------------------------
		

	//----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Strips the comments.
	/// </summary>
	/// <returns>The comments.</returns>
	/// <param name="lines">Lines.</param>
	public static string stripComments(string[] lines)	
	{
		string outstring="";
		foreach(string line in lines)
			{
				if( line.Length>0)
				{
					if(line.Length>1 && !(line[0]=='/' && line[1]=='/'))
					{
						outstring=outstring+line;
					}
					if(line.Length==1)outstring=outstring+line;
				}
				
			}
			return outstring;
	}
	//----------------------------------------------------------------------------------------------------------------
		

}
}