/*****************************************************************************
NetFanatics Standard Data Entry Validation Routines

$Workfile: $    $Revision:  $

Overview:  This file contains code for client side data type validation

First in the file are functions names isXXX() the have no user interface
but simply retrun true or false to indicate validity.
You pass in a string only. These functions are intended to be called
from the ValidXXX() functions (see below).

Later in the file are functions named ValidXXX() that will inform the user
with popup message box if data is in valid, and will set teh focus
to the offeding input field. They also return true or false.
You pass in a reference to a form input, value required flag, and
displayable name for the field.

Written by Charles Karow July 1999
Modified by Christopher Karper, Jan 2000 for MileOne
Modified by Christopher Karper, Apr 2000 for DealByNet
Modified by Christopher Karper, Apr 2000 for MileOne
Modified by Christopher Karper, May 2000 for DealByNet
Modified by Dave King, Sept 2000 to consolidate versions.
Copyright 1999-2000 by NetFanatics, Inc. All rights reserved

Last changed: $Modtime: $ ($Date:  $ by $Author:  $)

Example usage:

1. Add this include to the <HEAD> section of your page:
<script LANGUAGE="JavaScript1.2" SRC="common/Validation.js"></script>

2. Create a function for form input validation. Include one call to a ValidXXX()
function for each input to be validated.

<script LANGUAGE="JavaScript1.2" SRC="common/Validation.js">
function ValidateForm(frm)
{
if (!ValidCurrency(txtPayment, true, "Amount of Payment")) {return false;}
if (!ValidDate(txtDate, true, "Date of Payment")) {return false;}
}
</script>

3. Call your validation function from the form's onSubmit event handler.
<FORM action="MyPage.aspx" method="post" onSubmit="return ValidateForm(this)">
... inputs ...
</FORM>

******************************************************************************
Function Prototypes
******************************************************************************
ValidTime(Field, bRequired, sFieldName)

ValidText(Field, bRequired, sFieldName)

ValidPositiveNumber(Field, bRequired, sFieldName)

ValidDate(Field, bRequired, sFieldName)

ValidSSN(Field, bRequired, sFieldName)

ValidPhoneNumber(Field, bRequired, sFieldName)

ValidPositiveInteger(Field, bRequired, sFieldName)

ValidInteger(Field, bRequired, sFieldName)

ValidPostalCode(Field, bRequired, sFieldName)

ValidEmail(Field, bRequired, sFieldName)

ValidCurrency(Field, bRequired, sFieldName)

ValidTime(Field, bRequired, sFieldName)

******************************************************************************/

// general purpose function to see if a suspected numeric input
// is a positive integer
function isPosInteger(inputVal) {
    inputStr = inputVal.toString()
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }
    return true
}

// general purpose function to see if a suspected numeric input
// is a positive or negative integer
function isInteger(inputVal) {
    inputStr = inputVal.toString()
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)
        if (i == 0 && oneChar == "-") {
            continue
        }
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }
    return true
}

// general purpose function to see if a suspected numeric input
// is a positive or negative number
function isNumber(inputVal) {
    oneDecimal = false
    inputStr = inputVal.toString()
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)
        if (i == 0 && oneChar == "-") {
            continue
        }
        if (oneChar == "." && !oneDecimal) {
            oneDecimal = true
            continue
        }
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }

    return true
}

// another general purpose function to see if a suspected numeric input
// is a positive or negative number
function isNumber2(inputValue) {
    if (isNaN(parseFloat(inputValue))) {
        alert("The value you entered is not a number.")
        return false
    }
    return true
}

// general purpose function to see if a suspected numeric input
// is a positive number
function isPosNumber(inputVal) {
    oneDecimal = false
    inputStr = inputVal.toString()
    for (var i = 0; i < inputStr.length; i++) {
        var oneChar = inputStr.charAt(i)

        if (oneChar == "." && !oneDecimal) {
            oneDecimal = true
            continue
        }
        if (oneChar < "0" || oneChar > "9") {
            return false
        }
    }

    return true
}

// function to see if input is a valid D&B DUNS number.
function isDUNS(strInput) {
    // D&B DUNS should be nn-nnn-nnnn.
    // Be sure there are two dashes.
    var dash1 = strInput.indexOf("-")
    var dash2 = strInput.lastIndexOf("-")
    if (dash1 == -1 || dash1 == dash2)
    { return false }

    // Extract the three parts of the DUNS.
    if (dash1 == 2 && dash2 == 6) {
        var DUNS1 = parseInt(strInput.substring(0, dash1), 10)
        var DUNS2 = parseInt(strInput.substring(dash1 + 2, dash2), 10)
        var DUNS3 = parseInt(strInput.substring(dash2 + 2, strInput.length), 10)
        if (isNaN(DUNS1) || isNaN(DUNS2) || isNaN(DUNS3)) {
            // There is a non-numeric character in one of the component values.
            alert("NaN: The D&B DUNS entry is not in an acceptable format.\n\nYou should enter the DUNS number as nn-nnn-nnnn.")
            return false
        }

    }
    else {
        // There are no dashes or they are in the wrong places.
        alert("Bad Dash: The D&B DUNS entry is not in an acceptable format.\n\nYou should enter the DUNS number as nn-nnn-nnnn.")
        return false
    }
    return true;
}

// function to see if input is a valid Tax ID number.
function isTaxID(strInput) {
    // Tax id should be nn-nnnnnnn.
    // Be sure there is a dash.
    var dash1 = strInput.indexOf("-")
    if (dash1 == -1)
    { return false }

    // Extract the two parts of the tax id.
    if (dash1 == 2) {
        var taxid1 = parseInt(strInput.substring(0, dash1), 10)
        var taxid2 = parseInt(strInput.substring(dash1 + 2, strInput.length), 10)
        if (isNaN(taxid1) || isNaN(taxid2)) {
            // There is a non-numeric character in one of the component values.
            alert("NaN: The Tax ID entry is not in an acceptable format.\n\nYou should enter the Tax ID number as nn-nnnnnnn.")
            return false
        }

    }
    else {
        // There are no dashes or they are in the wrong places.
        alert("Bad Dash: The Tax ID entry is not in an acceptable format.\n\nYou should enter the Tax ID number as nn-nnnnnnn.")
        return false
    }
    return true;
}

// function to see if input is valid US Social Security Number
function isSSN(strInput) {
    // SSN should be nnn-nn-nnnn

    // Be sure there are two dashes
    var dash1 = strInput.indexOf("-")
    var dash2 = strInput.lastIndexOf("-")
    if (dash1 == -1 || dash1 == dash2)
    { return false }

    // Extract the tree paqrts of the SSN
    if (dash1 == 3 && dash2 == 6) {
        var SSN1 = parseInt(strInput.substring(0, dash1), 10)
        var SSN2 = parseInt(strInput.substring(dash1 + 2, dash2), 10)
        var SSN3 = parseInt(strInput.substring(dash2 + 2, strInput.length), 10)
        if (isNaN(SSN1) || isNaN(SSN2) || isNaN(SSN3)) {
            // there is a non-numeric character in one of the component values
            alert("NaN: The SSN entry is not in an acceptable format.\n\nYou should enter SSN as nnn-nn-nnnn.")
            return false
        }

    }
    else {
        // there are no dashes or they are in the wrong places
        alert("Bad Dash: The SSN entry is not in an acceptable format.\n\nYou should enter SSN as nnn-nn-nnnn.")
        return false
    }
    return true;
}

// Replaces the first occurrance of chFind with chReplacement in strInput
// and returns the result.
//
// Used in isDate() and isPhone()
function replaceString(strInput, chFind, chReplacement) {
    var i = strInput.indexOf(chFind)
    var str = strInput.substring(0, i) + chReplacement + strInput.substring(i + 1, strInput.length)

    return str;
}

// date field validation
function isDate(inputValue) {
    var inputStr = inputValue.toString()

    if (isNaN(Date.parse(inputStr))) {
        return (false);
    }
    var datInput = new Date(Date.parse(inputStr))
    if (datInput.getFullYear() < 1920) {
        return (false);
    }
    // CMK - 22 MAY 2000 - Added check to disallow 5 or more digit years.
    if (datInput.getFullYear() > 9999) {
        return (false);
    }

    //cwbutler - 6/5/2000 - Added to handle "9/31/2000", etc
    //first thing - zap all leading zeros
    var sFixDate = inputStr;
    sFixDate = sFixDate.replace(/^0/, '');
    sFixDate = sFixDate.replace(/\/0+/, '/');
    sFixDate = sFixDate.replace(/\/0+/, '/');

    //second thing, get the date JavaScript THINKS it has
    // use getFullYear() instead of getYear() for Netscape and IE. modified 9/8/00 STang
    var sCompDate = (datInput.getMonth() + 1) + "/" + datInput.getDate() + "/" + datInput.getFullYear();

    //finally, make sure they're the same
    if (sCompDate != sFixDate) {
        //cuz if they're not, it's an error
        return (false);
    }

    return (true);
}

// Function to see  if input is valid US phone number
function isPhone(strInput) {
    //CMK - 20 Apr 2000 - Rewrote function to accept following formats:
    // (111) 111-1111 Parenthesis with dash
    // 111-111-1111 Dashes
    // 111 111 1111 Spaces
    // 1111111111 No separation characters
    // 111.111.1111 Dots

    var inputStr = strInput.toString();
    var strFormatMsg = "The phone entry is not in an acceptable format.\n\nPlease enter phone numbers in the following format ###-###-####.";

    //This RegExp removes all dashes, parens, spaces, and dots from the string.
    inputStr = inputStr.replace(/[\-\)\(\.\ ]/g, '');

    if (inputStr.length != 10) { return false; }

    var AreaCode = parseInt(inputStr.substr(0, 3), 10)
    var CO = parseInt(inputStr.substr(3, 3), 10)
    var Number = parseInt(inputStr.substr(6, 4), 10)

    if (isNaN(AreaCode) || isNaN(CO) || isNaN(Number)) { return false; }
    if (AreaCode < 100 || AreaCode > 999) { return false; }
    if (CO < 100 || CO > 999) { return false; }
    if (Number < 0 || Number > 9999) { return false; }

    return true;
}

// Check whether string s is empty.

function isEmpty(s) {
    return ((s == null) || (s.length == 0))
}

// Returns true if string s is empty or
// whitespace characters only.

function isWhitespace(s) {
    var i;

    // whitespace characters
    var whitespace = " \t\n\r";

    // Is s empty?
    if (isEmpty(s)) return true;

    // Search through string's characters one by one
    // until we find a non-whitespace character.
    // When we do, return false; if we don't, return true.

    for (i = 0; i < s.length; i++) {
        // Check that current character isn't whitespace.
        var c = s.charAt(i);

        if (whitespace.indexOf(c) == -1) return false;
    }

    // All characters are whitespace.
    return true;
}

// function to see if input is valid US or Canadian Postal Code
function isPostalCode(s) {
    var sPart1
    var sPart2
    var sPart3

    // OK if US Zip Code
    if (s.length == 5 && isPosInteger(s)) { return true; }
    if (s.length == 10) {

        // Is it an extended zip code
        sPart1 = s.substring(0, 5);
        sPart2 = s.substring(5, 6);
        sPart3 = s.substring(6, 10);

        if (isPosInteger(sPart1) == true && isPosInteger(sPart3) == true && sPart2 == "-") {
            return true;
        }
        else
        { return false; }
    }

    // OK if Canadian Postal Code
    //if (s.length == 6) {return true;}

    // Otherwise its not OK!
    return false;
}

//
function isEmail(s) {
    // is s whitespace?
    if (isWhitespace(s)) return false;

    // there must be >= 1 character before @, so we
    // start looking at character position 1
    // (i.e. second character)
    var i = 1;
    var nLength = s.length;

    // look for @
    while ((i < nLength) && (s.charAt(i) != "@")) {
        i++
    }
    // The must be something after the @
    if ((i >= nLength) || (s.charAt(i) != "@")) return false;
    else i += 2;

    // look for .
    while ((i < nLength) && (s.charAt(i) != ".")) {
        i++
    }

    // there must be at least one character after the .
    if ((i >= nLength - 1) || (s.charAt(i) != ".")) return false;
    else return true;
}

//
// Function isTime()
// Returns true if the given string contains a valid 12 hour time.
//
function isTime(s) {
    // HH:MM AM or HH:MM am or HH:MM PM or HH:MM pm
    // HH = 1 - 12, MM = 0 - 59

    var aApP = "aApP"

    // Find colon and space -- be sure they're in the right places
    var iColon = s.indexOf(":")
    var iSpace = s.lastIndexOf(" ")

    // h:mm am or hh:mm am
    if ((iColon != 2 && iColon != 1) || iSpace != 5 && iSpace != 4) { return false; }

    // Get the hours, minutes and am/pm
    var hh = parseInt(s.substring(0, iColon), 10)
    var mm = parseInt(s.substring(iColon + 1, iSpace), 10)
    var ap = s.charAt(iSpace + 1)

    if (isNaN(hh) || isNaN(mm)) {
        // there is a non-numeric character in one of the component values
        return false
    }

    // Be sure values are in range
    if (hh < 1 || hh > 12) { return false; }
    if (mm < 0 || mm > 59) { return false; }
    if (aApP.indexOf(ap) == -1) return false;

    return true;
}

function isCurrency(s) {
    // Allow $ -- ignore it
    return isNumber(replaceString(s, "$", ""));
}


//*****************************************************************************
//  User interface functions (ValidXXX())
//*****************************************************************************

function ValidText(Field, bRequired, sFieldName) {
    if (bRequired && isWhitespace(Field.value))
    //if (bRequired && (Field.value == ""))
    {
        alert("Field '" + sFieldName + "' is required.");
        Field.focus();
        return false;
    }

    return true;
}

function ValidPositiveNumber(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (!isPosNumber(Field.value)) {
        alert("You must supply a valid number" + sName + ".");
        Field.focus();
        Field.select();
        return false;
    }

    return true;
}

function ValidDate(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isDate(Field.value)) {
            alert("You must supply a valid date" + sName + " with a 4 digit year.");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidSSN(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isSSN(Field.value)) {
            alert("You must supply a valid Social Security Number" + sName + ". The format is NNN-NN-NNNN");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidDUNS(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isDUNS(Field.value)) {
            alert("You must supply a valid Dun & Bradstreet D-U-N-S number" + sName + ". The format is NN-NNN-NNNN.");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidTaxID(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isTaxID(Field.value)) {
            alert("You must supply a valid Tax ID or EIN number" + sName + ". The format is NN-NNNNNNN.");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidPhoneNumber(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isPhone(Field.value)) {
            //CMK - 13 Apr 2000 - changed error message to use more common dash format.
            alert("You must supply a valid phone number" + sName + ". The format should be ###-###-####");  //CMK - 13 Apr 2000
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidPositiveInteger(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isPosInteger(Field.value)) {
            alert("You must supply a valid number" + sName + ".");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidInteger(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isInteger(Field.value)) {
            alert("You must supply a valid number" + sName + ".");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidNumber(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isNumber(Field.value)) {
            alert("You must supply a valid number" + sName + ".");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidPostalCode(Field, bRequired, sFieldName) {
    var sName = sFieldName

    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }
    if (Field.value != "") {
        if (!isPostalCode(Field.value)) {
            alert("Invalid postal code entered " + sName + ".  Valid Format is '99999' or '99999-9999'" + ".");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}


function ValidEmail(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isEmail(Field.value)) {
            alert("You must supply a valid email address" + sName + ". <user>@<organization>.<domain>");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

function ValidCurrency(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (!isCurrency(Field.value)) {
        alert("You must supply a valid currency value" + sName + ".");
        Field.focus();
        Field.select();
        return false;
    }

    return true;
}

function ValidTime(Field, bRequired, sFieldName) {
    var sName = sFieldName
    if (sFieldName == "") { sName = "" } else { sName = " for " + sFieldName }

    if (bRequired && isWhitespace(Field.value)) {
        alert("A value is required" + sName + ".");
        Field.focus();
        return false;
    }

    if (Field.value != "") {
        if (!isTime(Field.value)) {
            alert("You must supply a valid time" + sName + ". (HH:MM AM)>");
            Field.focus();
            Field.select();
            return false;
        }
    }
    return true;
}

// function added for common Netscape and IE behavior for Select
function ValidSelect(Field, sFieldName) {
    if (Field.options[Field.selectedIndex].value == "") {
        alert("Field '" + sFieldName + "' is required.");
        Field.focus();
        return false;
    }
    return true;
}

function isSelectionMade(field, indexExcluded) {
    // Contact Method Edit

    var i
    var bSelection = false
    var nEntries = field.length
    for (i = 0; (i < nEntries); i++) {
        if (field[i].selected == true) {
            if (i != indexExcluded) {
                bSelection = true;
            }
        }
    }
    if (bSelection == false) {
        return false;
    }
    else {
        return true;
    }
}



function ValidUNCPath(Field, bRequired, sFieldName) {
    if (bRequired && UNCCheck(Field.value))
    //if (bRequired && (Field.value == ""))
    {
        alert("The field '" + sFieldName + "' must specify a UNC path (\\machine name\...)")
        //alert("Field '" + sFieldName + "' is required.");
        Field.focus();
        return false;
    }

    return true;
}


function UNCCheck(sPath) {
    // Simple UNC path validation
    // Checks only first two characters for "//"

    if ((sPath.charAt(0) == "/") && (sPath.charAt(1) == "/")) {
        return true
    }
    else {
        //alert("This field must specify a UNC path (\\machine name\...)")
        return false
    }

}

function CheckStringForForm(frm) {
    var key, r, i, j
    r = ""
    for (i = 0; i < frm.elements.length; i++) {
        if ((frm.elements[i].type == "checkbox") || (frm.elements[i].type == "radio")) {
            r = r + frm.elements[i].checked;
        }
        else {
            r = r + frm.elements[i].value;
        }
    }
    return (r);
}

function MaximumLength(strField, maxLength) {
    if (strField.length > maxLength) {
        alert("'DETERMING FACTORS' must be less than " + maxLength + " characters in length.")
        return false
    }
    return true
}

function IsValidTime(timeStr) {
    // Checks if time is in HH:MM:SS AM/PM format.
    // The seconds and AM/PM are optional.

    var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s?(AM|am|PM|pm))?$/;

    var matchArray = timeStr.match(timePat);
    if (matchArray == null) {
        alert("Enter Military Time Format '12:00:00'.");
        return false;
    }
    hour = matchArray[1];
    minute = matchArray[2];
    second = matchArray[4];
    ampm = matchArray[6];

    if (second == "") { second = null; }
    if (ampm == "") { ampm = null }

    if (hour < 0 || hour > 23) {
        alert("Hour must be between 0 and 23");
        return false;
    }

    if (ampm != null) {
        alert("You can't specify AM or PM for military time.");
        return false;
    }
    if (minute < 0 || minute > 59) {
        alert("Minute must be between 0 and 59.");
        return false;
    }
    if (second != null && (second < 0 || second > 59)) {
        alert("Second must be between 0 and 59.");
        return false;
    }
    return true;
}

var OriginalFormCheckSumValue = "";

// End of Validation.js

