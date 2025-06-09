using Malshinon.DB;
using Malshinon.models;
using System;
using System.Collections.Generic;
using System.Data;
namespace Malshinon
{
    class Program
    {
        static void Main(string[] args)
        {
            MySQL db = new MySQL();
            db.OpenConnection();
        }

    }
}