namespace ChoreoApp.Algorithms.MinCostMaxFlow;

public sealed class Edge(
    int fromNode,
    int toNode,
    int capacity,
    float cost)
{
    public int FromNode { get; } = fromNode;
    public int ToNode { get; } = toNode;
    public int Capacity { get; set; } = capacity;
    public float Cost { get; } = cost;
    public Edge? ReverseEdge { get; set; }
}
