const app = require('express')();
const server = require('http').Server(app);
const { json } = require('express');
const websocket = require('ws');
const wss = new websocket.Server({server});
const sqlite3 = require('sqlite3').verbose();

let db = new sqlite3.Database('./database/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=> //แก้ไหมเข็คเอา
{
    if(err) throw err;
    {
        console.log('Connected to database.');
    }

    
    
    
   /* var splitStr = dataFromClient.data.split('#');
    var userID = splitStr[0];
    var password = splitStr[1];
    var name = splitStr[2];
    */

    //var sqlSelect = "SELECT * FROM UserData WHERE UserID='"+userID+"'AND Password='"+password+"' " //login
    //var sqlInsert ="INSERT INTO UserData (UserID,Password,Name,Money) VALUES ('"+userID+"','"+password+"','"+name+"','0')"
    //var sqlUpdate = "UPDATE UserData SET Money='200' WHERE UserID='"+userID+"' "

 

    /*db.all(sqlInsert, (err,rows)=>{
        if(err)
        {
            var callbackMsg = {
                eventName:"Register",
                data:"fail"
            }
            var toJsonStr = JSON.stringify(callbackMsg);
            
        }
        else
        {
            var callbackMsg = {
                eventName:"Register",
                data:"success"
            }
            var toJsonStr = JSON.stringify(callbackMsg);
            
        }
    });

    db.all(sqlSelect , (err,rows)=>{
        if(err)
        {
            console.log(err);
        }
        else
        {
            if(rows.length>0)
            {
                var callbackLogin = {
                    UserID:rows[0].UserID,
                    Name:rows[0].Name
                }
                var callbackMsg = {
                    eventName:"Login",
                    data:callbackLogin
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                
            }
            else
            {
                console.log("login fail");
            }
            var toJsonStr = JSON.stringify(callbackMsg);
               
        }
    });
    */
});
server.listen(process.env.PORT || 25500, ()=>{
    console.log("Server start at port "+server.address().port);
});
 
//var wsList = [];
var roomList = [];
/*
{
    roomName: ""
    wsList: []
}
*/
 
wss.on("connection", (ws)=>{
 
    //Lobby
    console.log("client connected.");
    //Reception
    ws.on("message", (data)=>{
        console.log("send from client :"+ data);
 
        //========== Convert jsonStr into jsonObj =======
 
        //toJsonObj = JSON.parse(data);
 
        // I change to line below for prevent confusion
        var toJsonObj = { 
            roomName:"",
            data:""
        }
        toJsonObj = JSON.parse(data);
        //===============================================
        if(toJsonObj.eventName =="Register")
        {
            toRegister = JSON.parse(toJsonObj.data);
            var sqlInsert ="INSERT INTO UserData (UserID,Password,Name,Money) VALUES ('"+toRegister.UserID+"','"+toRegister.Password+"','"+toRegister.Name+"','0')"

            db.all(sqlInsert, (err,rows)=>{
                if(err)
                {
                    var callbackMsg = {
                        eventName:"Register",
                        data:"fail"
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                }
                else
                {
                    var callbackMsg = {
                        eventName:"Register",
                        data:"success"
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                }
            });
        }
        else if(toJsonObj.eventName =="Chat")
        {
            Boardcast(ws,toJsonObj.data);
        }
        else if (toJsonObj.eventName == "Login")
        {
            toLogin = JSON.parse(toJsonObj.data);
            console.log(toLogin)
            var sqlSelect = "SELECT * FROM UserData WHERE UserID='"+toLogin.UserID+"'AND Password='"+toLogin.Password+"' " //login
            console.log("1")
            db.all(sqlSelect, (err,rows)=>{
                console.log("2")
                if(err)
                {
                    console.log("3")
                    var callbackMsg = {
                        eventName:"Login",
                        data:"fail"
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                }
                else
                {
                    console.log("4")
                    var callbackName = {
                        UserID:rows[0].UserID,
                        Name:rows[0].Name
                    }

                    var toJsonName = JSON.stringify(callbackName);

                    var callbackMsg = {
                        eventName:"Login",
                        data:toJsonName
                    }

                    
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                }
            });
        }
        
        else if(toJsonObj.eventName == "CreateRoom")//CreateRoom
        {
            //============= Find room with roomName from Client =========
            var isFoundRoom = false;
            for(var i = 0; i < roomList.length; i++)
            {
                if(roomList[i].roomName == toJsonObj.data)
                {
                    isFoundRoom = true;
                    break;
                }
            }

        
            //===========================================================
       
            if(isFoundRoom == true)// Found room
            {
                //Can't create room because roomName is exist.
                //========== Send callback message to Client ============
 
                //ws.send("CreateRoomFail"); 
 
                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"CreateRoom",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================
 
                console.log("client create room fail.");
            }

            else
            {
                //============ Create room and Add to roomList ==========
                var newRoom = {
                    roomName: toJsonObj.data,
                    wsList: []
                }
 
                newRoom.wsList.push(ws);
 
                roomList.push(newRoom);
                //=======================================================
 
                //========== Send callback message to Client ============
 
                //ws.send("CreateRoomSuccess");
 
                //I need to send roomName into client too. I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"CreateRoom",
                    data:toJsonObj.data
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================
                console.log("client create room success.");
            }
 
            //console.log("client request CreateRoom ["+toJsonObj.data+"]");
 
        }
        else if(toJsonObj.eventName == "JoinRoom")//JoinRoom
        {
            //============= Home work ================
            // Implementation JoinRoom event when have request from client.
 
            //================= Hint =================
            //roomList[i].wsList.push(ws);
            for(var i = 0; i < roomList.length; i++)
            {
                if(roomList[i].roomName == toJsonObj.data)
                {
                    roomList[i].wsList.push(ws);
 
                    var callbackMsg = {
                        eventName:"JoinRoom",
                        data:toJsonObj.data
                    }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
 
                    console.log("client join room dai la na ")
                    break;
                }
                if(i==roomList.length-1)
                {
                    var callbackMsg = {
                        eventName:"JoinRoom",
                        data:"fail"
                    }
 
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
 
                    console.log("client join room mai dai na ")
                    break;
                }
            }
 
            //============= Find room with roomName from Client =========
 
            console.log("client request JoinRoom");
            //========================================
        }
        else if(toJsonObj.eventName == "LeaveRoom")//LeaveRoom
        {
            //============ Find client in room for remove client out of room ================
            var isLeaveSuccess = false;//Set false to default.
            for(var i = 0; i < roomList.length; i++)//Loop in roomList
            {
                for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
                {
                    if(ws == roomList[i].wsList[j])//If founded client.
                    {
                        roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.
 
                        if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                        {
                            roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                        }
                        isLeaveSuccess = true;
                        break;
                    }
                }
            }
            //===============================================================================
 
            if(isLeaveSuccess)
            {
                //========== Send callback message to Client ============
 
                //ws.send("LeaveRoomSuccess");
 
                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"LeaveRoom",
                    data:"success"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================
 
                console.log("leave room success");
            }
            else
            {
                //========== Send callback message to Client ============
 
                //ws.send("LeaveRoomFail");
 
                //I will change to json string like a client side. Please see below
                var callbackMsg = {
                    eventName:"LeaveRoom",
                    data:"fail"
                }
                var toJsonStr = JSON.stringify(callbackMsg);
                ws.send(toJsonStr);
                //=======================================================
 
                console.log("leave room fail");
            }
        }
    });
 
 
    /*wsList.push(ws);
 
    ws.on("message", (data)=>{
        console.log("send from client :"+ data);
        Boardcast(data);
    });
    */
    ws.on("close", ()=>{
        console.log("client disconnected.");
 
        //============ Find client in room for remove client out of room ================
        for(var i = 0; i < roomList.length; i++)//Loop in roomList
        {
            for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
            {
                if(ws == roomList[i].wsList[j])//If founded client.
                {
                    roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.
 
                    if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                    {
                        roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                    }
                    break;
                }
            }
        }
        //===============================================================================
    });
});
 
function Boardcast(ws, message)
{
    var selectionRoomIndex = -1;

    for(var i = 0; i < roomList.length; i++){
        for(var j = 0; j < roomList[i].wsList.length; j++){
            if(ws == roomList[i].wsList[j]){
                selectionRoomIndex = i;
                break;
            }
        }
    }

    for(var i = 0; i < roomList[selectionRoomIndex].wsList.length; i++)
    {
        var callbackMsg = {
            eventName:"SendMessage",
            data:message
        }

        roomList[selectionRoomIndex].wsList[i].send(JSON.stringify(callbackMsg));
    }
}