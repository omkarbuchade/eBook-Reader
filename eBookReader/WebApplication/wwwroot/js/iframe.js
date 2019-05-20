//////////////////////////////////////////////////////////////
// iframe.js - This is the java script for project          //
// Author: Omkar Buchade                                    //
// Course: CSE686 - Internet Programming, Spring 2019       //
//////////////////////////////////////////////////////////////




function plus()      //function to increase size the iframe
{
    var x = document.getElementById("IFrame").width;
    document.getElementById("IFrame").width =1.2*x;

    var y = document.getElementById("IFrame").height;
    document.getElementById("IFrame").height=1.2*y;
}

function minus() //function to reduce size of the iframe
{
    var x = document.getElementById("IFrame").width;
    document.getElementById("IFrame").width =x/1.2;

    var y = document.getElementById("IFrame").height;
    document.getElementById("IFrame").height=y/1.2;
}
