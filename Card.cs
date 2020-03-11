﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Card_Game
{
    public class Card : IComparable<Card>
    {
       
        private Dictionary<CardsAttributes, int> attributes;
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public int this[CardsAttributes key]
        {
            get
            {
                return attributes[key];
            }
        }

        public Card(int id, string name, int[] attributes, string description)
        {
            this.ID = id;
            Name = name;
            Description = description;
            this.attributes = new Dictionary<CardsAttributes, int>
            {
                {CardsAttributes.Fluffiness, attributes[0]},
                {CardsAttributes.Madness, attributes[1]},
                {CardsAttributes.Gluttony, attributes[2]},
                {CardsAttributes.Laziness, attributes[3]},
            };
        }

        public int CompareTo(Card card)
        {
            if (ID > card.ID) 
                return 1;

            if (ID == card.ID) 
                return 0;

            return -1;
        }
    }
}
