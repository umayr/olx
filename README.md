OLX-DS
===

A complete data scrapper utitly to fetch OLX ad details. **_(For learning purposes only)_**

This repo consists of three modules:
- Core 
- API
- Client

Core
-
It consists of main scrapper logic, its a .NET backed console application.

**_TODOS_**
- Implement Async model. **Currently scraps synchronously**
- Better flags
- Ability to fetch certain category
- Scrap more fields

API
-
RESTful api for clientside backed by .NET's WEB-API.

**_TODOS_**
- Token based authentication
- Search functionality

Client
-
Angular based dashboard to utilize API and display scrapped data.

**_TODOS_**
- Authentication
- Search ability
- Display statistics

Lisence
-
The MIT License (MIT)

Copyright (c) 2014 Umayr Shahid <umayrr@hotmail.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.



