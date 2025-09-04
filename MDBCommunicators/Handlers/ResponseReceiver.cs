namespace MDBCommunicators.Handlers
{
    public class ResponseReceiver : IHandler, IResponseReceiver
    {
        private readonly List<char> _identifiers;
        private readonly MessageStorage _storage = new MessageStorage();
        private const int MessageSleepMs = 5;
        private const int MessageTimeoutMs = 1000;

        public ResponseReceiver()
        {
            _identifiers = new List<char>
            {
                'p',
                'v',
                'h',
                'f',
                'l',
                'm',
                'r',
            };
        }

        public bool TryGetFirstMessageById(char messageId, out MDBInputMessage result)
        {
            for (int i = 0; i < MessageTimeoutMs; i += MessageSleepMs)
            {
                Thread.Sleep(MessageSleepMs);

                if (_storage.TryGetFirstMessageById(messageId, out result))
                    return true;
            }

            result = null;
            return false;
        }

        public bool TryConsume(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            char id = text[0];

            if (_identifiers.Contains(id) == false)
                return false;

            _storage.AddMessage(new MDBInputMessage(text, id));

            return true;
        }
    }
}