var o = new ActiveXObject("Scan_log_lib.Scanloglib");
//WSH.Echo(o.help);
/*
RegexString(�������� {get,set}) - ������� ���������
Position(�������� {get,set}) - ID ����� ��������� ������
TimeWork(�������� {get}) - ������ ����������� ����� ������ ������
FindString(string file) - ����� ������ ���������. ���������� ��������� ������. ����� ������������ �������� Position.
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

