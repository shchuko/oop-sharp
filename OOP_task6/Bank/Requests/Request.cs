using Bank.Accounts;

namespace Bank.Requests
{
    public abstract class Request {
        internal Request(IAccount account) {
            _account = account;
        }

        public Request SetNextRequest(Request nextRequest) {
            this._nextRequest = nextRequest;
            return nextRequest;
        }

        public void Execute() {
            ExecuteRequest();
            _nextRequest?.Execute();
        }

        public IAccount GetAccount() {
            return _account;
        }
        
        internal abstract void ExecuteRequest();
        
        private Request _nextRequest;
        private IAccount _account;
    }
}