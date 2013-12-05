using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using Interrial.PPRS.Dal.TypedListClasses;
using Interrial.PPRS.Dal.EntityClasses;
using Interrial.PPRS.Dal.FactoryClasses;
using Interrial.PPRS.Dal.CollectionClasses;
using Interrial.PPRS.Dal.HelperClasses;
using Interrial.PPRS.Dal;

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

namespace InterrailPPRS.Admin
{
    public partial class ADPCompare : PageBase
    {

        Dim Uploader, File;

        protected override void Page_Load(object sender, EventArgs e)
        {

            base.Page_Load(sender, e);

            GrantAccess("Super, Admin, User");

            // Create the FileUploader;

             Uploader = New FileUploader;

            string sGUID;
            string sPath;
            string sFileName;
            string ADPFile;

            sPath    =  "D:\\WebSites\\Interrail\\wwwroot\\Admin\\ADP Files\\";

            sGUID = Server.CreateObject("Scriptlet.TypeLib").GUID;
            sADPFile = Mid(sGUID,2, 36) + ".txt";

            string[] arFName = new string[14];

            arFName[0]  = "Facility";
            arFName[1]  = "File #"       '' !in DB;
            arFName[2]  = "Last Name";
            arFName[3]  = "First Name";
            arFName[4]  = "Address Line 1";
            arFName[5]  = "Address Line 2";
            arFName[6]  = "City";
            arFName[7]  = "State";
            arFName[8]  = "Zip Code";
            arFName[9]  = "Phone #";
            arFName[10] = "Gender"        '' !in DB;
            arFName[11] = "SSN";
            arFName[12] = "Date Of Birth";
            arFName[13] = "Hire Date";

            // This starts the upload process;
            Uploader.Upload();
        }

        public void ShowResults(){

           string sFileName = "N/A";
           if(Uploader.Files.Count = 0 ){
             Response.Write("File !uploaded.");
           }else{
             for( Each File In Uploader.Files.Items){
                 File.SaveToDisk sPath, sADPFile;
                 sFileName = File.FileName;
             } 


            SSNCompare();

           }
        }


        ////FacilityID, LastName, FirstName + MiddleInitial, Address1, Address2, City, State, Zip, EmployeePhone,;
        ////NOGENDER, SSN, BirthDate, HireDate;



        ////FACILITY, FILE, LASTNAME, FIRSTNAME, STREETLINE1, STREETLINE2, CITY, STATE, ZIPCODE, AREACODEPHONE,;
        ////GENDER, SOCIALSECURITY, BIRTHDATE, HIREDATE;




        public void SSNCompare(){

          int iRec;
          string sRecord;
          string[] arRecord;
          DataTableReader rsEmp;
          string sADPValues;
          string sFNames;
          string sDBValues;
          int i;


          Response.Write("<table width=100% border='0'>");
          Response.Write("  <tr><td width='5%' class='cellTopBottomBorder'>SSN</td><td width='40%'class=' cellTopBottomBorder'>ADP File</td><td width='15%' class='cellTopBottomBorder'>Field</td><td width='40%' class='cellTopBottomBorder'>Employee table</td></tr>");

            Dim oFS, objTS, strSQL;
            // Open the uploaded file;
             oFS = Server.CreateObject("Scripting.FileSystemObject");
             objTS = oFS.OpenTextFile(sPath + sADPFile, 1);

            // Skip First record (headings);
            sRecord = objTS.ReadLine;
            iRec = 0;

            while(!objTS.AtEndOfStream){

              sRecord = objTS.ReadLine;
              arRecord = Split(sRecord, vbTab);
              //;
              // if(Facility name is missing use previous name;
              //;
              if(iRec==0 ){
                 sOldFacility = arRecord[0];
              }else{
                if(arRecord[0] == ""){
                   arRecord[0] = sOldFacility;
                }
                sOldFacility = arRecord[0];
              }

              strSQL = " ";
              strSQL = strSQL + "Select Facility=(Select Left(Name, Len(Name)-4) From Facility f Where f.ID = FacilityID), FileNumber='N/A', LastName, ";
              strSQL = strSQL + "       FirstName = Case When MiddleInitial IS NULL Or MiddleInitial='' ){ FirstName }else{ FirstName + ' ' + MiddleInitial + '.' End, ";
              strSQL = strSQL + "       Address1, Address2, City, State, Zip, ";
              strSQL = strSQL + "       Phone=Replace(Replace(Replace(Replace(EmployeePhone,'(',''),')',''),'-',''),' ',''), ";
              strSQL = strSQL + "       Gender='N/A', SSN=Replace(SSN,'-',''), ";
              strSQL = strSQL + "       BirthDate=Convert(Char(10), BirthDate,101),";
              strSQL = strSQL + "       HireDate=Convert(Char(10), HireDate,101) ";
              strSQL = strSQL + "       From Employee ";
              strSQL = strSQL + "WHERE (SSN IS !NULL) AND (SSN != '') AND (Replace(SSN,'-','') = '" + arRecord[11] + "') ";
              strSQL = strSQL + "       AND Active=1 ";
              strSQL = strSQL + "       AND (HireDate <= GetDate()) AND (HireDate IS !NULL) ";


               rsEmp = getRS(strSQL);



              if(rsEmp.Read() ){

              }else{

                 sADPValues = "";
                 sFNames    = "";
                 sDBValues  = "";
                 i = 0;
                 int iDiff = 0;

                 for( Each f In rsEmp.Fields;

                   if((i==9) ){
                     arRecord[i] = FormatThePhone(arRecord[i]);
                   }

                   if((i==12 || i==13) ){
                     arRecord[i] = FormatTheDate(arRecord[i]);
                   }

                   if((Trim(UCase(rsEmp.[i])) != Trim(UCase(arRecord[i])) && i != 1 && i != 10 && i != 11) ){
                     sFNames = sFNames + arFName[i] + "<br>";
                     if(i==9){
                       sADPValues = sADPValues + ShowThePhone(arRecord[i])   + "<br>";
                       sDBValues  = sDBValues  + ShowThePhone(cStr(f.Value)) + "<br>";
                     }else{
                       sADPValues = sADPValues + arRecord[i]   + "<br>";
                       sDBValues  = sDBValues  + cStr(f.Value) + "<br>";
                     }
                     iDiff = iDiff + 1;
                   }

                   i=i+1;
                 } 


                 int iRow = iRow + 1;
                 if(iRow % 2 == 0){
                   lc = "reportEvenLine";
                 }else{
                   lc = "reportOddLine";
                 }

                 Response.Write("<tr class='" + lc + "'>");
                 Response.Write(" <td width='5%' valign='top'><b>"  + arRecord[11]  + "</b></td>");
                 if(iDiff==0){
                   Response.Write(" <td width='80%' colspan='3' class='lblColorBold' align='center'>Records match</td>");
                 }else{
                   Response.Write(" <td width='40%'>" + sADPValues    + "</td>");
                   Response.Write(" <td width='20%'>" + sFNames       + "</td>");
                   Response.Write(" <td width='40%'>" + sDBValues     + "</td>");
                 }
                 Response.Write("</tr>");

              }


              rsEmp.Close;

              iRec = iRec+1;

            } //End While

            Response.Write("</table>");

            objTs.Close;


            oFS.DeleteFile(sPath + sADPFile);
    
        }

        public FormatTheDate(int inDate)
          string smDate, sdDate, syDate;

          smDate = Right(Cstr(DatePart("m",    inDate) + 100), 2);
          sdDate = Right(Cstr(DatePart("d",    inDate) + 100), 2);
          syDate = Cstr(DatePart("yyyy", inDate));
          FormatTheDate= smDate + "/" + sdDate + "/" + syDate;

        }

        public FormatThePhone(inPhone)
           FormatPhone=Replace(Replace(Replace(Replace(inPhone,"(",""),")",""),"-","")," ","");
        }

        public ShowThePhone(inPhone)
           if((Len(inPhone) = 10) ){
              ShowThePhone = "(" + Left(inPhone,3) + ") " + Mid(InPhone, 4, 3) + "-" + Right(inPhone, 4);
           }else{
              ShowThePhone = inPhone;
           }
        }

        public ReadADPFile(){

            iRec = 0;

            Response.Write("<table width=100% border=1>");

            while(!objTS.AtEndOfStream){

              iRec = iRec+1;
              sRecord = objTS.ReadLine;
              arRecord = Split(sRecord, vbTab);

              Response.Write("<tr>");
                for( int i=0; i < Ubound(arRecord); i++){
                  if(iRec=1 ){
                     sOldFacility = arRecord(0);
                  }else{
                    if((arRecord(i) = "") ){
                       arRecord(i) = sOldFacility;
                    }
                    sOldFacility = arRecord(0);
                  }

                  Response.Write("<td>");
                  if(arRecord(i) == ""){
                    Response.Write("&nbsp;");
                  }else{
                    Response.Write(arRecord(i));
                  }
                  Response.Write("</td>");
                } 
              Response.Write("</tr>");
            } //End While

            Response.Write("</table>");

            objTs.Close;


            oFS.DeleteFile(sPath + sADPFile);
        }

    }
}