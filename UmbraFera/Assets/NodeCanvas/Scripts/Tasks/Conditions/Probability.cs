using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class Probability : ConditionTask{

	    public BBFloat probability = new BBFloat{value = 0.5f};
	    public BBFloat maxValue = new BBFloat{value = 1};

	    protected override string conditionInfo{
	        get {return (probability.value/maxValue.value * 100) + "%";}
	    }

	    protected override bool OnCheck(){
	        return Random.Range(0f, maxValue.value) <= probability.value;
	    }
	}
}