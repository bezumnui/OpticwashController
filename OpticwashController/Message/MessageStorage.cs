namespace OpticwashController.Message
{
    public class MessageStorage : IMessageStorage
    {
        private readonly Dictionary<InputMessage, DateTime> _messages = new Dictionary<InputMessage, DateTime>();
        private int _timoutMs;
        private int _sleepMs;
    
        public MessageStorage(int timoutMs = 1000, int sleepMs = 50)
        {
            _sleepMs = sleepMs;
            _timoutMs = timoutMs;
        }

        public void AddMessage(InputMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
        
            if (_messages.ContainsKey(message))
                throw new InvalidOperationException("Message already exists in storage");
        
            _messages.Add(message, DateTime.UtcNow);
        }
    
    
        public void ClearTimeoutMessages()
        {
            DateTime now = DateTime.UtcNow;
            List<InputMessage> toRemove = new List<InputMessage>();
        
            foreach (KeyValuePair<InputMessage, DateTime> pair in _messages)
            {
                if ((now - pair.Value).TotalMilliseconds > _timoutMs)
                    toRemove.Add(pair.Key);
            }

            foreach (InputMessage message in toRemove)
                _messages.Remove(message);
        
        }

        public InputMessage GetMessageByPacketLabel(int packetLabel)
        {
            for (int i = 0; i < _timoutMs; i += _sleepMs)
            {
                if (TryGetMessageByPacketLabel(packetLabel, out InputMessage message))
                    return message;
            
                Thread.Sleep(_sleepMs);
            }
        
            throw new TimeoutException("Timeout while waiting for message");
        }
    
        private bool TryGetMessageByPacketLabel(int packetLabel, out InputMessage message)
        {
            ClearTimeoutMessages();
        
            message = null;
        
            foreach (KeyValuePair<InputMessage, DateTime> pair in _messages)
            {
                if (pair.Key.PacketLabel == packetLabel)
                {
                    message = pair.Key;
                    break;
                }
            }

            if (message == null)
                return false;
        
            _messages.Remove(message);

            return true;
        }
    }
}