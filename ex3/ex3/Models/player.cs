﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace ex3.Models
{
    public class Player
    {
        /// <summary>
        /// client
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// IsConnected
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="client"></param>
        public Player(TcpClient client)
        {
            this.Client = client;
            this.IsConnected = true;
        }
    }
}