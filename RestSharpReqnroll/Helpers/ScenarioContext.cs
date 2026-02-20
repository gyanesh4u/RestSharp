namespace RestSharpReqnroll.Helpers
{
    public class ScenarioContextHelper
    {
        private readonly Dictionary<string, object?> _context = new();

        public void Set(string key, object? value)
        {
            _context[key] = value;
        }

        public T? Get<T>(string key)
        {
            if (_context.TryGetValue(key, out var value))
            {
                return (T?)value;
            }
            throw new KeyNotFoundException($"Key '{key}' not found in context.");
        }

        public bool Contains(string key) => _context.ContainsKey(key);

        public void Clear() => _context.Clear();
    }
}
