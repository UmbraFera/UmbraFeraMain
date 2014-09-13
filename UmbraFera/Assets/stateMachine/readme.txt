State-Based Controller

Discription:  We built this asset to provide us keep a very light-weight controller that is the same for all our apps, easy to use and does not require,
   the GUI builder if we don't want to use it. This is not designed to allow a graphic way of building an app. It is just a way of graphically 
   designing the state flow and building the undelying framework for that design. We do not recommend this asset for none developers. Playmaker
   and many other assets are available to do that. We find graphically building logic is often much slower than writing the code and so those
   assets did not work well for our situation. Indeed the GUI builder came after the state controller when we got tired of transposing our state
   diagrams from Power Point Diagrams. But you can still do that if you choose since the state resource file is very human understandable.

   The state controller allows you to build state machine controllers for a wide range of applications. The controllers contain callbacks for each
   of the states that fire when the state in entered by the controller. The developer can then control the flow of the application simply by
   included method calls in these callbacks as well as by sending events as needed (such as button clicks) to the state machine. The conditions/attributes
   and actions greatly increase the flexibility of the controller to allow it to work for almost any type of problem. You can add multiple 
   conditions (seperated by a colon) and multiple actions (selerated by a colon.) The actions and conditions are based on the attributes in the
   state machine.

Ticker:  The state controller also includes a ticker which will fire a "tick" event to the state machine every x amount of second, where x is some float.
         To use the ticker, 
         1. click the controller game object that holds your controller script.
         2. In the inspector, make sure repeating is checked and ticker is check.
         3. Set Total Time in Seconds to be how many seconds you want between ticks
         4. Your state machine should have events labled "tick" with whatever conditions and actions you need for your game flow


Steps to make a new controller
1. Start a new scene
2. Make a blank gameObject that will be your controller
3. Rename your game object to something that makes sense for your domain
4. Select from the menu bar Window->Tools->FSMWindow
5. Drag your empty game object to the Target field on the left of the FSMWindow
6. Rename Resource Filename to something that makes sense for this controller. This is the resource text file that will define this state machine.
7. Add new veriables by clicking NEW in the Attributes panel. These can be ints, floats, strings, or booleans. You dont need to tell it what type
   it is, it will figure that out.
8. Design the state machine by double clicking in the FSMWindow to add states and dragging from the little nobs in the bottom left of the state
   to another state to add events.
    Note: you can change all values in states and events by clicking on the state field that w]you want to change or click on the event name to
          convert the event to edit mode.
    Node: Don't add spaces or special symbols to event or state names
9. Click SAVE FSM. Now your resource file will be loaded in the Resource directory of /Assets/stateMachine/
10. Rename "Controller name" to something that makes sense for this application
11. Click "Build Controller" to make a controller script that should attach to the GameObject you dragged into target. If it doesn't or you did
    not drag a GameObject you can attach it manually like you normally attach a script.
     Note: The controller.cs file is created in the stateMachine/Controllers directory
12. After you make your controller it is a good idea to move it out of the controllers directory to avoid over-writing it by accident after you have
    implemented your handling code.

Steps to edit a controller
1. Type in the name of the FSM resource file in the "Resource Filename" input field. Click "Load FSM"
2. Make your changes to the state machine by pointing clicking, dragging and editing.
3. Click "Save FSM"

Steps to observe a controller during runtime
1. Type in the name of the FSM resource file in the "Resource Filename" input field. Click "Load FSM"
2. Drag the game object that has the controller script into the "Target" input field
3. Run the program in the editor. You should see a green bar in the active state 

How to make a new state
1. Double click anywhere in the FSMWindow to create a new state. This state can be dragged anywhere you want it to be in the 
   window.
2. Click in the top entry field to change the name of the state. Please do not use spaces or special symbols
3. Click in the second field to provide a state discription. This is used in the controller to provide comments about that state


How to make a new event
1. Click and hold on the little nob on the bottom left corner of the start state panel. 
2. Drag to the destinatioin state panel. Let go when that panel lights up yellow.
3. Click the title of the event to change the displayed event to an editable panel.
4. Click in the top panel to change the name of the event. Again no spaces or special charactors
5.

How to add conditions to an event
1. Click on the title of the event to add conditions to
2. In the second field add your conditions
examples:
    state!=Texas
    child<=12
    child<=12:child>5
    child<=12:child>5:sex=boy

How to add actions to an event
1. Click on the title of the event to add conditions to.
2. In the third field add your actions
examples:
    age=20
    state=Florida
    old=true
    radius=3.1415

In your controller file (inherets from Abstract Class stateController)

To send an event to the state machine use: 
   eventToFSM = "stateName";

The current state: 
   myStateMachine.state   or currentState

To jump to a specific state bypassing the state graph:
   myStateMachine.jumpToState(int StateID); 
   Note: the state id is shown next to the state in the state diagram window (FSMWindow)
   
To get a state attribute:
  string yourAttribute = myStateMachine.getAtributeValue("attributeName");
  to convert it to a float:
      float.Parse(yourAttribute);
  to convert it to an int:
    int.Parse(yourAttribute);
  to convert it to a bool:
    bool.Parse(yourAttribute); 
example:
int counter =int.Parse( myStateMachine.getAtributeValue("counter"));   

        
to save an attribute value:
  myStateMachine.setAttribute("attributeName",yourAttribute); 
  Note:    yourAttribute can be a string, int, bool, long or float
example: 
myStateMachine.setAttribute("counter",counter);    
    
    
    
    
    
    
   


