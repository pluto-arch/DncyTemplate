using System.Collections;

namespace DncyTemplate.Job.Infra;

/// <summary>
///     定长队列
/// </summary>
public class FixLengthQueue : Queue
{
    private readonly int _length;

    public FixLengthQueue(int length)
    {
        _length = length;
    }

    /// <summary>
    ///     默认长度10
    /// </summary>
    public FixLengthQueue() : this(10)
    {
    }


    /// <inheritdoc />
    public override void Enqueue(object obj)
    {
        if (Count >= _length)
        {
            Dequeue();
        }

        base.Enqueue(obj);
    }
}