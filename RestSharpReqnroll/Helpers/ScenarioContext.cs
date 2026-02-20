using Reqnroll;
using System.Linq;

namespace RestSharpReqnroll.Helpers
{
    public class ScenarioContextHelper
    {
        private readonly ScenarioContext _scenarioContext;

        public ScenarioContextHelper(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public void Set<T>(string key, T value)
        {
            _scenarioContext[key] = value!;
        }

        public T Get<T>(string key)
        {
            return _scenarioContext.TryGetValue(key, out var value)
                ? (T)value
                : default!;
        }

        public bool Contains(string key)
        {
            return _scenarioContext.ContainsKey(key);
        }

        public void Remove(string key)
        {
            if (_scenarioContext.ContainsKey(key))
                _scenarioContext.Remove(key);
        }

        // ✅ REQUIRED BY HOOKS
        public void Clear()
        {
            var keys = _scenarioContext.Keys.ToList();

            foreach (var key in keys)
            {
                _scenarioContext.Remove(key);
            }
        }
    }
}