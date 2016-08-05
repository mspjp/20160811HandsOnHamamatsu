//サーバーとなるjavascriptプログラム
var http = require('http');             //httpアクセスを制御するモジュール
var express = require('express');       //Webサイトとしてルーティングを制御するモジュール
var socketio = require('socket.io');    //WebSocketを使うためのモジュール
var app = express();
var server = http.createServer(app);
var io = socketio(server);
var port = process.env.PORT||1337;      //process.env.PORTはAzureWebAppで実行したときのポート番号
var usercount = 0;                      //現在のチャットルームの人数

//expressモジュールの機能でpublicフォルダ以下のファイルを静的にアクセス可能にする
//こうすることでWebサイトのルートにアクセスしたときpublicフォルダ内のhtmlファイルにアクセスできる
app.use(express.static('public'));

//socket.ioが初期化されたときこの中が実行される
io.on('connection', function(socket){
    
    //誰かユーザーがログインしたとき
    socket.on('login',function(name){
        
        //チャットルーム人数を増やし、ユーザーがログインしたことを全員に送信する
        usercount++;
        var data = {
            name:name,
            count:usercount
        };
        io.emit('broadcast-login',data);
        
    });
    
    //誰かが発言したとき
    socket.on('message',function(data){
        
        //発言を全員送信する 
        io.emit('broadcast-message',data);
        
    });
    
    //誰かユーザーがログアウトしたとき
    socket.on('logout',function(name){
        
        //チャットルームの人数を減らし、ユーザーがログアウトしたことを全員に送信する
        usercount--;
        var data = {
            name:name,
            count:usercount
        };
        io.emit('broadcast-logout',data);
        
    });
});

//httpサーバーを開始する
server.listen(port, function(){
    
});
