// 
// SessionManager.cs
//  
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
// 
// Copyright (c) 2012 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using Raven.Client;
using Raven.Client.Extensions;
using Raven.Client.Embedded;
using System.IO;
using System;


namespace GestUAB
{
    public interface IRavenSessionProvider
    {
        IDocumentSession GetSession ();
    }

    public class RavenSessionProvider : IRavenSessionProvider
    {
        static IDocumentStore _documentStore;

        public static IDocumentStore DocumentStore {
            get { return (_documentStore ?? (_documentStore = CreateDocumentStore ())); }
        }

        static IDocumentStore CreateDocumentStore ()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, Conventions.RavenDataDirectory);
            path = Path.Combine(path, Conventions.RavenDatabase);
            var populate = !Directory.Exists(path);

            var documentStore = new EmbeddableDocumentStore()
            {
                DataDirectory = path
            }.Initialize();

            if (populate) documentStore.PopulateUsers();

            //documentStore.DatabaseCommands.EnsureDatabaseExists (Conventions.RavenDatabase);

            return documentStore;
        }

        public IDocumentSession GetSession ()
        {
            //return DocumentStore.OpenSession (Conventions.RavenDatabase);
            return DocumentStore.OpenSession ();
        }
    }
}

