﻿
using System.Collections.Generic;

namespace ObjDoctor.model
{
    public class Group
    {
        public string name;
        public List<FaceGroup> fg = new List<FaceGroup>();

        public Group()
        {
        }

        public Group(string name)
        {
            this.name = name;
        }
    }
}
// Copyright Jorge Castro Castillo March 2018.