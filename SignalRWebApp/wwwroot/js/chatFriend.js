"use strict";


    //连接服务
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatOneFriend").build();

    // 禁止发送按钮，直到建立连接
    $("#sendButton").hide();

    // 建立连接
    connection.start().then(function () {
        // 连接成功则显示发送按钮
        $("#sendButton").show();
        console.log("连接成功！")
    }).catch(function (err) {
        // 连接失败则直接返回错误消息
        return console.error(err.toString());
    });

    // 发送消息
    $("#sendButton").click(function () {
        //从好友列表中，选择出在线好友，获得好友的userId
        var userId = $("#userId").val();
        debugger
        var message = $("#messageInput").text();

        sendMessage("3fcd8626-1dd8-48a2-bf73-3d161f776122", userId, message);

    });

    /*
    receiverId:接受用戶id
    sendId：发送用户id
    message：消息
    */
    function sendMessage(receiverId, sendId, message) {
        connection.invoke("SendMessage", receiverId, sendId, message).catch(function (err) {
            return console.error(err.toString());
        });
    }


    // 接收消息
    connection.on("ReceiveMessage", function (receiverId, message) {
        $("#content").append(`<p>${receiverId}</p><p>${message}</p><br>`);
        $("#content").animate({ scrollTop: 100000 });
    });

    //查看聊天记录
    var pageIndex = 1;
    $("#findMessage").click(function () {
        $("#historyMessage").fadeIn();
        $.post("/Home/GetMessages", { pageIndex: pageIndex, pageSize: 10 }, function (data) {
            $.each(data, function (i, e) {
                $("#historyMessage").append(`<p>${e.uesrName} ${e.createTime}</p><p>${e.content}</p><br>`);
            });
        })
    });




