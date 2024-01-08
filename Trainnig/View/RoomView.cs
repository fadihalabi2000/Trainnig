﻿using TrainnigApI.Model;

namespace TrainnigApI.View
{
    public class RoomView
    {
        public string? Name { get; set; }
        public int CenterId { get; set; }
        public Center? Center { get; set; }
        public int Capacity { get; set; }
    }
}
