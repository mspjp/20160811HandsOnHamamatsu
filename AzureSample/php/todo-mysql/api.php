<?php
//javascriptからajax非同期通信で呼ばれるapiプログラム

//アクセスはすべてjson、utf-8で返す
header("Content-Type: application/json; charset=utf-8");

//mysqlへの接続情報
$mysql_host = 'ja-cdbr-azure-west-a.cloudapp.net';
$mysql_user = 'b82cee9bd25c06';
$mysql_pass = 'ee67f77a';


//もしリクエストがhttp getメソッドなら
if($_SERVER["REQUEST_METHOD"] == "POST"){
    //httpステータスコードを200に
    http_response_code(200);
    $param = $_POST['param'];
    $eventType = $param['type'];
    //データベースサーバーへ接続
    $sql = mysql_connect($mysql_host,$mysql_user,$mysql_pass);
    //データベースを選択
    $todoDb = mysql_select_db('garitodo',$sql);
    mysql_set_charset('utf-8');
    
    //リクエストがどのタイプかによって処理を変更
    switch($eventType){
        case 'sync':
            sync($param);
        break;
        case 'add':
            add($param);
        break;
        case 'remove':
            remove($param);
        break;
    }
    print(mysql_error($sql));
    mysql_close($sql);
}else{
    //httpステータスコードを400に
    http_response_code(400);
}

//タスク一覧を取得する関数
function sync($param){
    $result = array();
    //selectクエリーを使用し、結果をarrayに入れてjsonとして返す
    $sqlresult = mysql_query('select * from todo');
    while($i =mysql_fetch_assoc($sqlresult)){
        array_push($result,array('task_name'=>$i['name'],'task_date'=>$i['date'],'task_id'=>$i['id']));
    }
    print(json_encode($result));
}

//タスクを追加する関数
function add($param){
    //insertクエリによってデータを追加
    $task = $param['task'];
    $name = $task['task_name'];
    $date = $task['task_date'];
    $result = mysql_query('insert into todo(name,date) values ("'.$name.'","'.$date.'")');
    print('{"message":"success"}');
}

//タスクを削除する関数
function remove($param){
    //idからdeleteクエリによって削除
    $task_id = $param['id'];
    $result = mysql_query("delete from todo where id = '".$task_id."'");
    print('{"message":"success"}');
}

?>