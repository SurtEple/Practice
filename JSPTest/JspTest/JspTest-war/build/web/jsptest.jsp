<%-- 
    Document   : jsptest
    Created on : Feb 17, 2014, 1:51:46 PM
    Author     : Missy
--%>

<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <%! int i = 0; %> 
        <%! int a, b, c; %> 
        <%! int day = 5; %> 
        <%! int fontSize; %> 
        <title>JSP Page</title>
    </head>
    <body>
        <h1>Hello World!</h1>
        <%
            out.println("Your IP address is " + request.getRemoteAddr());
         %>

        <% if (day == 1 | day == 7) { %>
      <p> Today is weekend</p>
        <% } else { %>
      <p> Today is not weekend</p>
            <% } %>
            
            <% 
switch(day) {
case 0:
   out.println("It\'s Sunday.");
   break;
case 1:
   out.println("It\'s Monday.");
   break;
case 2:
   out.println("It\'s Tuesday.");
   break;
case 3:
   out.println("It\'s Wednesday.");
   break;
case 4:
   out.println("It\'s Thursday.");
   break;
case 5:
   out.println("It\'s Friday.");
   break;
default:
   out.println("It's Saturday.");
}
%>
    </body>
</html>
