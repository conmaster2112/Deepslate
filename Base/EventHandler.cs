namespace ConMaster
{
    public delegate void BeforeEventHandler<T>(object sender, T args, ref bool cancel) where T : EventArgs;
    public class SequencedEventManager<T> where T : EventArgs
    {
        //public event ErrorEventHandler? OnError;
        public event BeforeEventHandler<T>? Before;
        public event EventHandler<T>? When;
        public event EventHandler<T>? After;
        public bool Invoke(object sender, T args)
        {
            bool cancel = false;
            Before?.Invoke(sender, args, ref cancel);
            if (cancel) return true;
            When?.Invoke(sender, args);
            After?.Invoke(sender, args);
            return false;
        }
    }
}
