using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
	public static bool Contains ( this LayerMask mask, int layer )
	{
		return mask == (mask | (1 << layer));
	}

	public static int Mod ( int x, int modulo )
	{
		return (x % modulo + modulo) % modulo;
	}
	
	
}
