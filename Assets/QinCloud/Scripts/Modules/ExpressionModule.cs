using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;

public class ExpressionModule : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Test()
    {
        ParameterExpression a = Expression.Parameter(typeof(float), "a");
        ParameterExpression b = Expression.Parameter(typeof(float), "b");
        BinaryExpression result = Expression.Add(a, b);
       
    }
}
