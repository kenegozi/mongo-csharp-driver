﻿/* Copyright 2010 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Bson.DefaultSerializer;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB.DriverOnlineTests.Jira.CSharp110 {
    [TestFixture]
    public class CSharp110Tests {
        private class C {
            public ObjectId Id;
            public int X;
        }

        [Test]
        public void TestFind() {
            var server = MongoServer.Create("mongodb://localhost/?safe=true");
            var database = server["onlinetests"];
            var collection = database.GetCollection<C>("csharp110");

            collection.RemoveAll();
            var c = new C { X = 1 };
            collection.Insert(c);
            c = new C { X = 2 };
            collection.Insert(c);

            var query = Query.EQ("X", 2);
            foreach (var document in collection.Find(query)) {
                Assert.AreNotEqual(ObjectId.Empty, document.Id);
                Assert.AreEqual(2, document.X);
            }
        }
    }
}
