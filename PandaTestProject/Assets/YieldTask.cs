using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class YieldTask : CustomYieldInstruction
{


	public YieldTask(Task task)
	{
		Task = task;
	}

	public override bool keepWaiting => !Task.IsCompleted;
	public Task Task { get; }
}