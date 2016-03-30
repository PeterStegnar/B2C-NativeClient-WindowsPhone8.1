//----------------------------------------------------------------------------------------------
//    Copyright 2014 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//----------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

// The following using statements were added for this sample.
using System.Collections.Concurrent;
using TodoListService.Models;
using System.Security.Claims;
using System.Diagnostics;

namespace TodoListService.Controllers
{
    [Authorize]
    public class TodoListController : ApiController
    {
        private const string oidClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifie";

        //
        // To Do items list for all users.  Since the list is stored in memory, it will go away if the service is cycled.
        // A user's To Do list is keyed off of the ObjectIdentifier claim, which contains an immutable, unique identifier for the user.
        //
        static ConcurrentBag<TodoItem> todoBag = new ConcurrentBag<TodoItem>();

        // GET api/todolist
        public IEnumerable<TodoItem> Get()
        {
            Trace.TraceInformation("GET api/todolist");

            Claim subject = ClaimsPrincipal.Current.FindFirst(oidClaimType);
            Trace.TraceInformation("ObjectIdentifier:" + subject.Value);

            return from todo in todoBag
                   where todo.Owner == subject.Value
                   select todo;
        }

        // POST api/todolist
        public void Post(TodoItem todo)
        {
            Trace.TraceInformation("POST api/todolist");

            if (null != todo && !string.IsNullOrWhiteSpace(todo.Title))
            {
                Claim subject = ClaimsPrincipal.Current.FindFirst(oidClaimType);
                Trace.TraceInformation("ObjectIdentifier:" + subject.Value);

                todoBag.Add(new TodoItem { Title = todo.Title, Owner = subject.Value });
            }
        }
    }
}
