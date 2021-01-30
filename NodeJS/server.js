var websocket = require('ws');


var callbackInitServer =()=>{
    console.log(" Server is running");
    }


var wss = new websocket.Server({port:49151},callbackInitServer );

var wsList =[];


wss.on("connection",(ws)=>{
    console.log("client connected.");
    wsList.push(ws);

ws.on("message ",(data)=>{
console.log("send from client :"+data)
Boardcast(data);
});

    ws.on("close ", ()=>{
        console.log("client disconnected.")
        wsList = ArrayRemove(wsList,ws);
    })
}

);
function ArrayRemove(array,value)
{
    return array.filter((eliment)=>{
    return element != value;
    })
}
function Boardcast(data)
{
    for (var i =0 ; i<wsList.length;i++)
    {
        wsList[i].send(data);
    }
}



/*var callbackTest =()=>{

}

function test(a,callback)
{
    callback();
}

test (5,callbackTest)*/

