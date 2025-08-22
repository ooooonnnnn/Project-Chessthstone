using System.Collections.Generic;

namespace MonoGameProject1.Extensions;

public static class LinkedListExtentions
{
	public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> node) 
		=> node.Next ?? node.List.First;
	
	public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> node) 
		=> node.Previous ?? node.List.Last;
}