using System;

namespace ElympicsRoomsAPI
{
    public class ElympicsRoomsAPIResponses
    {
        [Serializable]
        public enum RoomState
		{
            open,
            closed
		}

        [Serializable]
        public class Bet
        {
            public int amount;
            public string address;
        }

        [Serializable]
        public class PlayerInRoom
        {
            public string nickname;
            public string address;
            public bool is_you;
            public bool is_active;
        }

        [Serializable]
        public class ElympicsPlayer
        {
            public string nickname;
            public string address;
            public string elympics_id;
        }

        [Serializable]
        public class Room
        {
            public Bet bet;
            public string room_key;
            public RoomState state;
            public string time_created;
            public string time_updated;
            public PlayerInRoom[] players;
            public string matchmaker_queue_name;
        }
    }

}