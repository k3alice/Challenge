<?php
require 'autoload.php';
use Abraham\TwitterOAuth\TwitterOAuth;
require_once('config.php');//設定ファイル読み込み

//各種APIキーを指定
$consumeKey = API_KEY;
$consumeSecret = API_SECRET;
$accessToken = ACCESS_TOKEN;
$accessTokenSecret = ACCESS_SECRET;

//単語をテキストファイルから取得
$lastword = file(__DIR__ . '/lastword.txt', FILE_IGNORE_NEW_LINES);
$noun = file(__DIR__ . '/noun.txt', FILE_IGNORE_NEW_LINES);
$prepo= file(__DIR__ . '/preposition.txt', FILE_IGNORE_NEW_LINES);

//名詞とパラメータをセットにして分割
$nouns = array();
foreach ($noun as $split) {
  $onetime = mb_convert_kana($split, 's','utf-8');
  $nouns[] = preg_split('/,/', $onetime);
}
$nounnum = sizeof($nouns);

//前置詞をセットにして分割
$prepos = array();
$preponum = array();
foreach ($prepo as $split) {
  $onetime = mb_convert_kana($split, 's','utf-8');
  $onetime = preg_split('/,/', $onetime);//分割
  $prepos[] = $onetime;//格納
  $preponum[] = sizeof($onetime);//要素数を取得
}

$contenue = true;
$usednoun = "";
$tweetmesages = "";

function Nwaka(&$tweetmesages,&$randnum,&$usednoun,&$contenue,&$count){
  extract($GLOBALS);
  $tweetmesages = $tweetmesages.$nouns[$randnum][0];
  $usednoun = $usednoun.$nouns[$randnum][0];

  switch ($nouns[$randnum][1]) {

    case "person":
      $tweetmesages = $tweetmesages.$prepos[0][rand(0,$preponum[0] - 1)];
      break;

    case "place":
      $tweetmesages = $tweetmesages.$prepos[1][rand(0,$preponum[1] - 1)];
      break;

    case "food":
      $tweetmesages = $tweetmesages.$prepos[2][rand(0,$preponum[2] - 1)];
      break;

    case "other":
      $tweetmesages = $tweetmesages.$prepos[3][rand(0,$preponum[3] - 1)];
      break;

    default:
      echo "失敗しました";
      break;
  }
}

function CreateMes(&$tweetmesages,&$randnum,&$usednoun,&$contenue,&$count){
  extract($GLOBALS);
  $usednoun = "";

  $tweetmesages = "#にわかちゃれんじ\n";

  $count = sizeof($tweetmesages);
  $randnum = rand(0, $nounnum - 1);
  Nwaka($tweetmesages,$randnum,$usednoun,$contenue,$count);

  while($contenue){
    $count = sizeof($tweetmesages);
    $randnum = rand(0, $nounnum);
    if ($count > 30 || $randnum == $nounnum || preg_match('/'.$nouns[$randnum][0].'/',$usednoun)) $contenue = false;
    else Nwaka($tweetmesages,$randnum,$usednoun,$contenue,$count);
  }
  $tweetmesages = $tweetmesages.$lastword[rand(0,sizeof($lastword) - 1)];
}

$contenue = true;
CreateMes($tweetmesages,$randnum,$usednoun,$contenue,$count);
$connection = new TwitterOAuth($consumeKey,$consumeSecret,$accessToken,$accessTokenSecret);
$result = $connection -> post("statuses/update",array("status" => $tweetmesages));


?>
