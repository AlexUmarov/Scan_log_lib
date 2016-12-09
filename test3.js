var o = new ActiveXObject("Scan_log_lib.Scanloglib");
//WSH.Echo(o.help);
/*
RegexString(свойство {get,set}) - искомое выражение
Position(свойство {get,set}) - ID блока найденной строки
TimeWork(свойство {get}) - вернет затраченное время работы метода
FindString(string file) - метод поиска выражений. Возвращает найденную строку. Может использовать свойство Position.
*/
WSH.Echo(o.help);
WSH.Echo(o.GetPositionEnd("1.txt"));
//o.Position=41004687;
//var filename="test.txt";
//var i=5
//while(""!=(sr = o.FindString(filename)) &&--i){
  //WSH.Echo("o.Position="+(o.Position)); 
  //WSH.Echo("o.FindString=("+sr+")");
//}

