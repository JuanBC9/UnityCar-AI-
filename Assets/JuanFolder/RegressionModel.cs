using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython.Hosting;

public class RegressionModel : MonoBehaviour
{
	void Start()
	{
		var engine = Python.CreateEngine();
		var scope = engine.CreateScope();

		// Add the python library path to the engine. Note that this will
		// not work for builds; you will need to manually place the python
		// library files in a place that your code can find it at runtime.
		var paths = engine.GetSearchPaths();
		paths.Add(Application.dataPath + "/JuanFolder/");
        paths.Add(@"C:\Users\Jbcse\AppData\Local\Programs\Python\Python37\Lib\site-packages\");
		engine.SetSearchPaths(paths);

		string code = @"
import PolynomialRegression
data = 'Data.txt'
maxScore = -1
bestDegree = 0

for i in range(0, 15):
	model = PolynomialRegression(i, data)
	model.train_Model()
	score = model.score()

	if(score > maxScore):
		maxScore = score
		bestDegree = i
";

		var source = engine.CreateScriptSourceFromString(code);
		source.Execute(scope);

		Debug.Log(scope.GetVariable<int>("bestDegree"));
	}
}
