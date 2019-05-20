//////////////////////////////////////////////////////////////
// jsClass.js - This is the java script class for           //
//              contact page                                //
// Author: Omkar Buchade                                    //
// Course: CSE686 - Internet Programming, Spring 2019       //
//////////////////////////////////////////////////////////////

class RenderDivision {
    setDiv() {
        document.getElementById("renderDiv").innerHTML = "Contact";
    }
};

let obj = new RenderDivision();
obj.setDiv();
