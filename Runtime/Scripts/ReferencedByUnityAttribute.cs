using System;

[AttributeUsage(AttributeTargets.Method)]
public class ReferencedByUnityAttribute : Attribute
{
	// Used for methods that are called by Unity via animation events or button callbacks
	// Remember, that if you rename such methods, all references will break so use extreme caution, or just don't do it
	// Also, resist the temptation to remove these methods, even when IDE shows no references
}
