using System.Collections;
using System.Collections.Generic;

namespace Lab01.Domain.Tests;

public class NumberGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>();

    public NumberGenerator()
    {
        _data.Add(new object[] {5, 2, 7});
        _data.Add(new object[] {5, 5, 10});
        _data.Add(new object[] {10, 10, 20});
    }
    
    public IEnumerator<object[]> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}