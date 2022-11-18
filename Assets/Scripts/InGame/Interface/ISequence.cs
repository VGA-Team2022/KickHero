interface ISequence 
{
    public void Initialize(System.Action<InGameCycle.EventEnum> action);
    public void OnUpdate();
}
