using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkElement<T>
{
    public T value;
    public LinkElement<T> next;

    public LinkElement(T value, LinkElement<T> next)
    {
        this.value = value;
        this.next = next;
    }

    public LinkElement()
    {
        this.value = default(T);
        this.next = null;
    }
}
