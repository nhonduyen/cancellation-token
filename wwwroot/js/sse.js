"use strict";
if (typeof (EventSource)) {
    console.log("Support SSE");
    let source = new EventSource("/ServerSentEvent/GetTime");

    source.onmessage = (event) => {
        document.getElementById("sseResult").innerHTML = event.data;
        console.log(event.data);
    }
   
    source.onerror = (err) => {
        //console.log(err);
    };
    source.addEventListener("open", function (e) {
        console.log("Open");
    }, false);

    // read file
    let sourceFile = new EventSource("/ServerSentEvent/GetFile");
    sourceFile.onmessage = (event) => {
        document.getElementById("fileContent").innerHTML = event.data;
        console.log("File " + event.data);
    }
    document.getElementById("btnClosetime").addEventListener("click", function (){
        source.close();
        document.getElementById("sseResult").innerHTML = "Connection closed";
        console.log("Connection closed");
    });
    document.getElementById("btnClosefile").addEventListener("click", function (){
        sourceFile.close();
        document.getElementById("fileContent").innerHTML = "Connection closed";
        console.log("Connection closed");
    });
} else {
    console.log("Not Support SSE");
}