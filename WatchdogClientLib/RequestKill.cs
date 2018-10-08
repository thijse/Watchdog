namespace WatchdogClient
{
    public class RequestKill
    {
        private readonly uint _killDelay;
        private readonly int _noAllowedErrors;
        private int _noErrors;

        public RequestKill(int noAllowedErrors, uint killDelay)
        {
            _noAllowedErrors = noAllowedErrors;
            _killDelay = killDelay;
            _noErrors = 0;
        }

        public void Reset()
        {
            _noErrors = 0;
        }

        public void Error()
        {
            _noErrors++;
            if (_noErrors == _noAllowedErrors)
            {
                Heartbeat.Instance.RequestKill(_killDelay);
            }
        }
    }
}